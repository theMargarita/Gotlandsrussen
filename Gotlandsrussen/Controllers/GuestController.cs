using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]/AddBreakfast")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        [HttpPost( Name = "AddBreakfast")]
        public IActionResult AddBreakfast([FromHeader] int bookingId)
        {
            return Ok(new { message = "Breakfast added successfully." });
        }
    }
}
