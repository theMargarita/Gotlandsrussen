using Gotlandsrussen.Data;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult>AddBreakfast([FromQuery] AddBreakfastRequestDto request)
        {
            if (request.BookingId == null || request.BookingId <= 0)
            {
                return NotFound("BookingId was not found");
            }

            var result = await _bookingRepository.AddBreakfast(request);

            return Ok(result.Value);
        }
    }
}
