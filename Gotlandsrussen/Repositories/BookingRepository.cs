using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<BookingDto>> GetAllFutureBookings()
        {
            return await _context.Bookings
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
                }).ToListAsync();
        }

        public async Task<ActionResult<AddBreakfastResponseDto>> AddBreakfast(AddBreakfastRequestDto request)
        {
            var booking = await _context.Bookings.FindAsync(request.BookingId);

            if (booking == null)
            {
                return new AddBreakfastResponseDto
                {
                    BookingId = request.BookingId,
                    Breakfast = false,
                    Message = "Booking was not found"
                };
            }

            booking.Breakfast = true;
            await _context.SaveChangesAsync();

            var response = new AddBreakfastResponseDto
            {
                BookingId = booking.Id,
                Breakfast = booking.Breakfast,
                Message = "Breakfast has been added successfully"
            };

            return response;
        }

        public async Task<ICollection<YearWeekBookingsDto>> GetBookingsGroupedByWeek()
        {
            var bookings = await GetAllFutureBookings();

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

            return grouped;
        }

        public async Task<ICollection<YearMonthBookingsDto>> GetBookingsGroupedByMonth()
        {
            var bookings = await GetAllFutureBookings();

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

            return grouped;
        }

        public async Task<Booking?> GetById(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }
       
    }
}
