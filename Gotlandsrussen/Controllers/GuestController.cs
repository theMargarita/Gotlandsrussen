using Gotlandsrussen.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly HotelDbContext _context;
        public GuestController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddBreakfast")]
        public async Task<IActionResult> AddBreakfast(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null || bookingId <= 0)
            {
                return NotFound("Booking not found");
            }
            if (bookingId <= 0)
            {
                return BadRequest("Invalid bookingId");
            }

            // add breakfast to booking.
            booking.Breakfast = true;
            
            _context.Bookings.Update(booking);

            return Ok("Breakfast was added!");
        }
    }
}
