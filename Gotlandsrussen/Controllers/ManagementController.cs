using Gotlandsrussen.Data;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Http;
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

        [HttpGet(Name = "GetBookings")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookings()
        {
            return Ok(await _bookingRepository.GetAllFutureBookings());
        }
    }
}
