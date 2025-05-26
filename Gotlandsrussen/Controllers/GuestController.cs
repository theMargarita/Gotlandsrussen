using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public GuestController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpPut("AddBreakfast")]
        public async Task<ActionResult<AddBreakfastDto>>AddBreakfast([FromQuery] AddBreakfastRequestDto request)
        {
            var booking = await _bookingRepository.GetById(request.BookingId);

            if (request.BookingId == null || request.BookingId <= 0)
            {
                return NotFound("BookingId was not found");
            }

            // check if breakfast is booked already.
            if (booking.Breakfast == true)
            {
                return BadRequest("Breakfast is already added to the booking.");
            }

            // check if breakfast is null
            if (booking.Breakfast == null)
            {
                return BadRequest("Breakfast is null.");
            }

            var addBreakfast = new AddBreakfastDto
            {
                BookingId = request.BookingId,
                Breakfast = true,
                Message = "Breakfast has been added to the booking."
            };

            // save changes to the database
            booking.Breakfast = addBreakfast.Breakfast;

            return Ok(addBreakfast);
        }

        //Denna metod hamnar både i guestcontroller och managementcontroller eftersom den behöver kallas på i båda.
        //Detta pga hur vi har delat upp våra controllers. Inte så snyggt. Fråga Petter.
        [HttpGet("GetBookingById/{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _bookingRepository.GetById(id);

            if (booking == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            return Ok(booking);
        }

        [HttpGet("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Startdatum måste vara före slutdatum.");

            var rooms = await _bookingRepository.GetAvailableRoomsAsync(startDate, endDate);
            return Ok(rooms);
        }

    }


}
