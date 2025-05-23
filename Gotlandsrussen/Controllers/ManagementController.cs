using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
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
            return Ok(await _bookingRepository.GetBookingsGroupedByWeek());
        }

        [HttpGet("GetBookingsGroupedByMonth")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingsGroupedByMonth()
        {
            return Ok(await _bookingRepository.GetBookingsGroupedByMonth());
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
