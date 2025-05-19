using Gotlandsrussen.Data;
using Gotlandsrussen.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]/GetAllFutureBookings")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public ManagementController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAllFutureBookings")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetAllFutureBookings()
        {
            return Ok(await _context.Bookings
                //.Include(b => b.BookingRooms)
                //.ThenInclude(br => br.Room)
                //.Include(b => b.Guest)
                .Where(b => b.BookedFromDate >= DateOnly.FromDateTime(DateTime.Today)
                    && b.BookingIsCancelled == false)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    GuestName = b.Guest.LastName + ", " + b.Guest.FirstName,
                    RoomNames = b.BookingRooms.Select(br => br.Room.RoomName).ToList(),
                    BookedFromDate = b.BookedFromDate,
                    BookedToDate = b.BookedToDate,
                    NumberOfAdults = b.NumberOfAdults,
                    NumberOfChildren = b.NumberOfChildren,
                }).ToListAsync());
        }
    }
}
