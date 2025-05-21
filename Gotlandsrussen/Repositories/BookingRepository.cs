using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
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

        public async Task<ActionResult<Booking>> AddBreakfast(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            // add breakfast to booking.
            booking.Breakfast = true;

            await _context.SaveChangesAsync();

            return booking;
        }
    }
}
