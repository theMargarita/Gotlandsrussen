﻿using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Gotlandsrussen.Utilities;
using HotelGotlandsrussen.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public ManagementController(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        [HttpGet("GetAllFutureBookings")] // lina
        public async Task<ActionResult<ICollection<BookingDto>>> GetAllFutureBookings()
        {
            return Ok(await _bookingRepository.GetAllFutureBookings());
        }

        [HttpGet("GetBookingsGroupedByWeek")] // Margarita
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingsGroupedByWeek()
        {
            var bookings = await _bookingRepository.GetAllFutureBookings();

            var grouped = bookings
                .GroupBy(b => b.BookedFromDate.GetIsoYearAndWeek())
                .Select(g => new YearWeekBookingsDto
                {
                    Year = g.Key.Year,
                    Week = g.Key.Week,
                    Bookings = g.ToList()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Week)
                .ToList();

            return Ok(grouped);
        }

        [HttpGet("GetBookingsGroupedByMonth")] // Florent
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingsGroupedByMonth()
        {
            var bookings = await _bookingRepository.GetAllFutureBookings();

            var grouped = bookings
                .GroupBy(b => new { b.BookedFromDate.Year, b.BookedFromDate.Month })
                .Select(g => new YearMonthBookingsDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Bookings = g.ToList()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();

            return Ok(grouped);
        }

        [HttpGet("GetBookingById/{id}")] // kim
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _bookingRepository.GetById(id);

            if (booking == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            return Ok(booking);
        }

        [HttpGet("GetTotalPrice/{BookingId}")] // lina
        public async Task<ActionResult<TotalPriceDto>> GetTotalPrice(int BookingId)
        {
            var booking = await _bookingRepository.GetById(BookingId);
            if (booking == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            var rooms = booking.BookingRooms.Select(br => new RoomTypeWithPriceDto
            {
                RoomType = br.Room.RoomType.Name,
                PricePerNight = br.Room.RoomType.PricePerNight
            }).ToList();
            int numberOfNights = (booking.ToDate.ToDateTime(TimeOnly.MinValue) - booking.FromDate.ToDateTime(TimeOnly.MinValue)).Days;
            int numberOfGuests = booking.NumberOfAdults + booking.NumberOfChildren;
            int numberOfBreakfasts = numberOfNights * numberOfGuests;
            decimal breakfastPrice = 50m;
            decimal totalPrice = rooms.Sum(r => r.PricePerNight * numberOfNights) + (numberOfBreakfasts * breakfastPrice);

            var summary = new TotalPriceDto
            {
                BookingId = booking.Id,
                Rooms = rooms,
                NumberOfNights = numberOfNights,
                NumberOfGuests = numberOfGuests,
                NumberOfBreakfasts = numberOfBreakfasts,
                BreakfastPrice = breakfastPrice,
                TotalPrice =totalPrice
            };

            return Ok(summary);
        }


        //som receptionist vill jag kunna söka lediga rum baserat på datum och antal gäster
        [HttpGet("{fromDate}/{toDate}/{adults}/{children}", Name = "GetAvailableRoomByDateAndGuests")] // Margarita
        public async Task<ActionResult<ICollection<RoomDto>>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children)
        {
            var getDate = await _roomRepository.GetAvailableRoomByDateAndGuests(fromDate, toDate, adults, children);

            if (getDate == null)
            {
                return BadRequest(new { errorMessage = "Date incorrectly typed" });
            }

            var today = DateOnly.FromDateTime(DateTime.Now);

            if (fromDate < today)
            {
                return BadRequest(new { errorMessgae = "Cannot get past date" });
            }
            if(fromDate > toDate)
            {
                return BadRequest(new { errorMessage = "Cannot book date before start date" });
            }
            if (toDate == today)
            {
                return BadRequest(new { errorMessage = "Cannot book for the same day" });
            }

            return Ok(getDate);
        }

        [HttpGet("GetBookingHistory")] // Florent
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingHistory()
        {
            return Ok(await _bookingRepository.GetBookingHistory());
        }


        [HttpPut("UpdateBooking")] // kim
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingDto updatedBooking)
        {
            var result = await _bookingRepository.UpdateBookingAsync(updatedBooking);

            if (result == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            return Ok(result);
        }

    }
}
