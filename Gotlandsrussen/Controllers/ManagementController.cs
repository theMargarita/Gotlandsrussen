using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly HotelDbContext _context;

        public ManagementController(HotelDbContext context)
        {
            _context = context;
        }


        //som receptionist vill jag kunna söka lediga rum baserat på datum och antal gäster
        [HttpGet("{fromDate}/{toDate}/{adults}/{children}", Name = "GetAvailableRoomByDateAndGuests")]
        public async Task<ActionResult<ICollection<BookingRoom>>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children)
        {
            int totalGuests = adults + children;

            //get date for the toom type
            var getDate = await _context.Bookings.Where(b => b.BookedFromDate == fromDate && b.BookedToDate == toDate).SelectMany(b => b.BookingRooms.Select(br => br.Room.RoomType.Name))
                .ToListAsync();

            if (getDate == null)
            {
                return BadRequest(new { errorMessage = "Date incorrectly typed" });
            }


            var dateNow = DateOnly.FromDateTime(DateTime.Now);

            if(fromDate != dateNow)
            {
                return BadRequest(new { errorMessgae = "Cannot get past date" });
            }
            if(toDate == dateNow)
            {
                return BadRequest(new { errorMessage = "Cannot book for the same day" });
            }

            //avaible room counted with the amout of guests
            var availableRooms = await _context.Rooms.Where(r => r.RoomType.NumberOfBeds >= totalGuests).GroupBy(r => r.RoomType).Select(g => g.Key).ToListAsync();

            if (availableRooms == null)
            {
                return BadRequest(new { errorMessage = "Invalid or typo" });
            }


            return Ok(availableRooms);
        }
    }
}
