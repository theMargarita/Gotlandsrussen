using Gotlandsrussen.Data;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public GuestController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpPut("AddBreakfast")]
        public async Task<ActionResult>AddBreakfast(int bookingId)
        {
            if (bookingId == null || bookingId <= 0)
            {
                return NotFound("Booking not found");
            }

            return Ok(await _bookingRepository.AddBreakfast(bookingId));
        }
    }
}
