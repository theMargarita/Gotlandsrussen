﻿using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;

        public GuestController(IBookingRepository bookingRepository, IGuestRepository guestRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
        }      


        [HttpPut("AddBreakfast")]
        public async Task<ActionResult<AddBreakfastDto>> AddBreakfast([FromQuery] AddBreakfastRequestDto request)
        {
            var booking = await _bookingRepository.GetById(request.BookingId);

            if (request.BookingId == null || request.BookingId <= 0)
            {
                return NotFound("BookingId was not found");
            }

            // check if breakfast is booked already.
            if (booking.Breakfast == true)
            {
                return Conflict("Breakfast is already added to the booking.");
            }

            // check if breakfast is null
            if (booking.Breakfast == null)
            {
                return NotFound("Breakfast was empty.");
            }

            var addBreakfast = new AddBreakfastDto
            {
                BookingId = request.BookingId,
                Breakfast = true,
                Message = "Breakfast has been added to the booking."
            };

            // save changes to the database
            booking.Breakfast = addBreakfast.Breakfast;
            await _bookingRepository.Update(booking);

            return Ok(addBreakfast);
        }

        [HttpGet("AvailableRooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            if (startDate < DateOnly.FromDateTime(DateTime.Today))
                return BadRequest("Startdatum har redan passerat.");

            if (startDate >= endDate)
                return BadRequest("Startdatum måste vara före slutdatum.");

            var rooms = await _roomRepository.GetAvailableRoomsAsync(startDate, endDate);
            return Ok(rooms);
        }

        [HttpPut("CancelBooking")]
        public async Task<IActionResult> CancelBooking([FromQuery]int bookingId)
        {
            var bookingToCancel = await _bookingRepository.GetById(bookingId);

            if (bookingToCancel == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            if (bookingToCancel.IsCancelled == true)
            {
                return Ok(new { message = "Booking is already cancelled" });
            }

            bookingToCancel.IsCancelled = true;
            await _bookingRepository.Update(bookingToCancel);
            return Ok(new { message = "Booking is cancelled" });
        }

        [HttpGet("GetAllGuests")]
        public async Task<ICollection<Guest>> GetAllGuests()
        {
            var getAllGest = await _guestRepository.GetAllGuests();
            return getAllGest.ToList();
        }

        [HttpPost("CreateGuest")]
        public async Task<ActionResult<CreateGuestRequestDto>> CreateGuest([FromQuery] CreateGuestRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("No data have been added.");
            }

            var guest = new Guest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone
            };

            var existingGuest = await _guestRepository.GetAllGuests() ?? new List<Guest>();

            // Check if the guest already exists
            if (existingGuest.Any(g => g.Email == guest.Email))
            {
                return Conflict("Guest with this email address already exists.");
            }
            else if (existingGuest.Any(g => g.Phone == guest.Phone))
            {
                return Conflict("Guest with this phone number already exists.");
            }

            await _guestRepository.AddGuest(guest);
            return CreatedAtAction(nameof(GetAllGuests), new { id = guest.Id }, guest);
        }
        
        [HttpDelete("DeleteGuest")]
        public async Task<IActionResult> DeleteGuest([FromQuery]int guestId)
        {
            if (guestId <= 0)
            {
                return BadRequest("Invalid guest ID");
            }

            try
            {
                await _guestRepository.DeleteGuest(guestId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Guest not found");
            }
        }
    }
}