using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gotlandsrussen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Startdatum måste vara före slutdatum.");

            var rooms = await _bookingRepository.GetAvailableRoomsAsync(startDate, endDate);
            return Ok(rooms);
        }
    }
}
