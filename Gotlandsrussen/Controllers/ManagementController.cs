using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Gotlandsrussen.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public ManagementController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet("GetAllFutureBookings")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetAllFutureBookings()
        {
            return Ok(await _bookingRepository.GetAllFutureBookings());
        }

        [HttpGet("GetBookingsGroupedByWeek")]
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

        [HttpGet("GetBookingsGroupedByMonth")]
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
    }
}
