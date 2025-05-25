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


        //som receptionist vill jag kunna söka lediga rum baserat på datum och antal gäster
        [HttpGet("{fromDate}/{toDate}/{adults}/{children}", Name = "GetAvailableRoomByDateAndGuests")]
        public async Task<ActionResult<ICollection<RoomDTO>>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children)
        {
            var getDate = await _bookingRepository.GetAvailableRoomByDateAndGuests(fromDate, toDate, adults, children);

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

            if (getDate == null)
            {
                return BadRequest(new { errorMessage = "Invalid or typo" });
            } 

            return Ok(getDate);
        }
    }
}
