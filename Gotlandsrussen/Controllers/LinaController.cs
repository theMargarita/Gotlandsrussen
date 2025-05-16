using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinaController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public LinaController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAllFutureBookings")]
        public async Task<ActionResult<ICollection<Booking>>> GetAllFutureBookings()
        {
            return Ok(await _context.Bookings
                .Include(b => b.BookingRooms)
                .ThenInclude(br => br.Room)
                .Include(b => b.Guest).ToListAsync());
        }
    }
}
