using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Gotlandsrussen.Utilities;
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

        public ManagementController(IBookingRepository bookingRepository, IRoomRepository roomRepository, IGuestRepository guestRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
        }

        [HttpGet("GetAllFutureBookings")] // lina
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

        [HttpGet("GetBookingsGroupedByMonth")] // Florent
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

        [HttpGet("GetBookingById/{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _bookingRepository.GetById(id);

            if (booking == null)
            {
                return NotFound(new { errorMessage = "Booking not found" });
            }

            return Ok(booking);
        }

        [HttpGet("GetTotalPrice/{BookingId}")] // lina
        public async Task<ActionResult<TotalPriceDto>> GetTotalPrice(int BookingId)
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
        public async Task<ActionResult<ICollection<RoomDto>>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children)
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

        [HttpGet("GetBookingHistory")] // Florent
        public async Task<ActionResult<ICollection<BookingDto>>> GetBookingHistory()
        {
            return Ok(await _bookingRepository.GetBookingHistory());
        }

        [HttpPut("UpdateBooking")]
        public async Task<ActionResult> UpdateBooking([FromBody] UpdateBookingDto updatedBooking)
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

        //[HttpGet("GetAllGuests")]
        //public async Task<ActionResult<ICollection<Guest>>> GetAllGuests()
        //{
        //    var guests = await _guestRepository.GetAllGuests();
        //    return Ok(guests);
        //}

        [HttpPost("CreateBooking")] //margarita 
        public async Task<ActionResult<CreateBookingDto>> CreateBooking(int guestId,List<int> roomId, DateOnly fromDate, DateOnly toDate, int adults, int children, bool breakfast)
        {
            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(fromDate, toDate);

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
            if (availableRooms != null)
            {
                return BadRequest(new { Message = "No available rooms on this period of time" });
            }
            var checkGuestId = await _guestRepository.GetAllGuests();
            if (!checkGuestId.Any())
            {
                return NotFound("Guest not found");
            }


            bool allExist = roomId.All(id => availableRooms.Any(r => r.Id == id));

            if (!allExist)
            {
                return BadRequest(new { errorMessage = "All rooms are not available" });
            }

            int numberOfGuests = adults + children;

            //foreach (var room in roomId)
            //{
            //    avail
            //}

            //foreach (int room in roomId)
            //{
            //    int numberOfBeds = availableRooms.Where(a => a.Id == room)

            //}
            //var getRoom = List<Room>
            foreach(int room in roomId)
            {
                await _
            }

            await _bookingRepository.CreateBooking(new Booking
            {
                GuestId = guestId,
                FromDate = fromDate,
                ToDate = toDate,
                NumberOfAdults = adults,
                NumberOfChildren = children,
                Breakfast = breakfast
            });



            //await _bookingRepository.CreateBooking

            //return CreatedAtAction(nameof(GetBookingById), new { id = newBooking.Id }, newBooking);
        }
    }
}
