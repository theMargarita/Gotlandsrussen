using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Gotlandsrussen.Utilities;
using GotlandsrussenAPI.Repositories;
using HotelGotlandsrussen.Models.DTOs;
using HotelGotlandsrussenLIBRARY.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRoomRepository _bookingRoomRepository;

        public ManagementController(IBookingRepository bookingRepository, IRoomRepository roomRepository, IGuestRepository guestRepository, IBookingRoomRepository bookingRoomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _bookingRoomRepository = bookingRoomRepository;
        }

        [HttpGet("GetAllFutureBookings")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetAllFutureBookings()
        {
            return Ok(await _bookingRepository.GetAllFutureBookings());
        }

        [HttpGet("GetBookingsGroupedByWeek")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingsGroupedByWeek()
        {
            var bookings = await _bookingRepository.GetAllFutureBookings();

            var grouped = bookings
                .GroupBy(b => b.BookedFromDate.GetIsoYearAndWeek())
                .Select(g => new YearWeekBookingsDto
                {
                    Year = g.Key.Year,
                    Week = g.Key.Week,
                    Bookings = g.ToList()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Week)
                .ToList();

            return Ok(grouped);
        }

        [HttpGet("GetBookingsGroupedByMonth")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingsGroupedByMonth()
        {
            var bookings = await _bookingRepository.GetAllFutureBookings();

            var grouped = bookings
                .GroupBy(b => new { b.BookedFromDate.Year, b.BookedFromDate.Month })
                .Select(g => new YearMonthBookingsDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Bookings = g.ToList()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();

            return Ok(grouped);
        }

        [HttpGet("GetBookingById")]
        public async Task<ActionResult<Booking>> GetBookingById([FromQuery]int id)
        {
            var booking = await _bookingRepository.GetById(id);

            if (booking == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            return Ok(booking);
        }

        [HttpGet("GetTotalPrice")]
        public async Task<ActionResult<TotalPriceDto>> GetTotalPrice([FromQuery]int BookingId)
        {
            var booking = await _bookingRepository.GetById(BookingId);
            if (booking == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            var rooms = booking.BookingRooms.Select(br => new RoomTypeWithPriceDto
            {
                RoomType = br.Room.RoomType.Name,
                PricePerNight = br.Room.RoomType.PricePerNight
            }).ToList();
            int numberOfNights = (booking.ToDate.ToDateTime(TimeOnly.MinValue) - booking.FromDate.ToDateTime(TimeOnly.MinValue)).Days;
            int numberOfGuests = booking.NumberOfAdults + booking.NumberOfChildren;
            int numberOfBreakfasts = booking.Breakfast ? numberOfNights * numberOfGuests : 0;
            decimal breakfastPrice = 50m;
            decimal totalPrice = rooms.Sum(r => r.PricePerNight * numberOfNights) + (numberOfBreakfasts * breakfastPrice);

            var summary = new TotalPriceDto
            {
                BookingId = booking.Id,
                Rooms = rooms,
                NumberOfNights = numberOfNights,
                NumberOfGuests = numberOfGuests,
                NumberOfBreakfasts = numberOfBreakfasts,
                BreakfastPrice = breakfastPrice,
                TotalPrice = totalPrice
            };

            return Ok(summary);
        }

        [HttpGet("GetAvailableRoomByDateAndGuests")]
        public async Task<ActionResult<ICollection<RoomDto>>> GetAvailableRoomByDateAndGuests([FromQuery]DateOnly fromDate, DateOnly toDate, int adults, int children)
        {
            var getDate = await _roomRepository.GetAvailableRoomByDateAndGuests(fromDate, toDate, adults, children);

            if (getDate == null)
            {
                return BadRequest(new { errorMessage = "Date incorrectly typed" });
            }

            var today = DateOnly.FromDateTime(DateTime.Now);

            if (fromDate < today)
            {
                return BadRequest(new { errorMessgae = "Cannot get past date" });
            }
            if (fromDate > toDate)
            {
                return BadRequest(new { errorMessage = "Cannot book date before start date" });
            }
            if (toDate == today)
            {
                return BadRequest(new { errorMessage = "Cannot book for the same day" });
            }

            return Ok(getDate);
        }

        [HttpGet("GetBookingHistory")]
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingHistory()
        {
            return Ok(await _bookingRepository.GetBookingHistory());
        }

        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromQuery] UpdateBookingDto updatedBooking)
        {
            try
            {
                var result = await _bookingRepository.UpdateBookingAsync(updatedBooking);

                if (result == null)
                {
                    return NotFound(new { errorMessage = "Booking not found" });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { errorMessage = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorMessage = "Ett oväntat fel uppstod." });
            }
        }

        [HttpPost("CreateBooking")] //margarita 
        public async Task<ActionResult<CreateBookingDto>> CreateBooking([FromQuery]List<int> roomId, int guestId, DateOnly fromDate, DateOnly toDate, int adults, int children, bool breakfast)
        {

        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromQuery] int guestId, DateOnly fromDate, DateOnly toDate, int adults, int children, bool breakfast)
        {
            var newBooking = await _bookingRepository.CreateBooking(guestId, fromDate, toDate, adults, children, breakfast);
            if (adults == 0)
            {
                return BadRequest(new { errorMessage = "Must add atleast one adult" });
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            if (fromDate < today)
            {
                return BadRequest(new { errorMessgae = "Cannot get past date" });
            }
            if (fromDate == toDate)
            {
                return BadRequest(new { errorMessgae = "Cannot book for the same day" });
            }
            if (fromDate > toDate)
            {
                return BadRequest(new { errorMessage = "Cannot book date before start date" });
            }
            if (adults < 0 || children < 0)
            {
                return BadRequest("Number of adults and children cannot be negative.");
            }

            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(fromDate, toDate);
            if (availableRooms == null || !availableRooms.Any())
            {
                return BadRequest(new { Message = "No available rooms on this period of time" });
            }

            var checkGuestId = await _guestRepository.GetAllGuests();
            if (!checkGuestId.Any(g => g.Id == guestId))
            {
                return NotFound("Guest not found");
            }

            bool allRoomsAvailable = roomId.All(id => availableRooms.Any(r => r.Id == id));

            if (!allRoomsAvailable)
            {
                return BadRequest(new { errorMessage = "All rooms are not available" });
            }

            bool hasDouble = roomId.GroupBy(r => r).Any(g => g.Count() > 1);

            if(hasDouble ==  true)
            {
                return BadRequest(new { Message = "Cannot double book"});
            }

            var booking = new Booking
            {
                GuestId = guestId,
                FromDate = fromDate,
                ToDate = toDate,
                NumberOfAdults = adults,
                NumberOfChildren = children,
                Breakfast = breakfast,

            };

            booking = await _bookingRepository.CreateBooking(booking);

            var roomsToBook = availableRooms.Where(r => roomId.Contains(r.Id)).ToList();

            var bookingRooms = roomsToBook.Select(r => new BookingRoom
            {
                BookingId = booking.Id,
                RoomId = r.Id
            }).ToList();

            foreach (var bookingRoom in bookingRooms)
            {
                await _bookingRoomRepository.AddBookingRooms(bookingRoom);
            }

            var newBooking = new CreateBookingDto
            {
                BookingId = booking.Id,
                GuestId = guestId,
                FromDate = fromDate,
                ToDate = toDate,
                NumberOfAdults = adults,
                NumberOfChildren = children,
                Breakfast = breakfast,
                RoomIds = roomId
            };


            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, new { newBooking  });
        }

        [HttpDelete("DeleteBooking")]
        public async Task<IActionResult> DeleteBooking([FromQuery] int bookingId)
        {
            if (bookingId <= 0)
            {
                return BadRequest("Invalid booking ID");
            }

            try
            {
                await _bookingRepository.DeleteBooking(bookingId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Booking not found");
            }
        }
    }
}
