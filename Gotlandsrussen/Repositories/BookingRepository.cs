using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using HotelGotlandsrussen.Models.DTOs;
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

        public async Task<ICollection<BookingDto>> GetAllFutureBookings()  //Florent
        {
            return await _context.Bookings
                .Where(b => b.FromDate >= DateOnly.FromDateTime(DateTime.Today)
                    && b.IsCancelled == false)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    GuestName = b.Guest.LastName + ", " + b.Guest.FirstName,
                    RoomNames = b.BookingRooms.Select(br => br.Room.Name).ToList(),
                    BookedFromDate = b.FromDate,
                    BookedToDate = b.ToDate,
                    NumberOfAdults = b.NumberOfAdults,
                    NumberOfChildren = b.NumberOfChildren,
                }).ToListAsync();
        }

        public async Task<Booking?> GetById(int id)   //Kim
        {
            return await _context.Bookings
                .Include(b => b.BookingRooms)
                .ThenInclude(br => br.Room)
                .ThenInclude(r => r.RoomType)
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task Update(Booking booking)    //Lina
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<BookingDto>> GetBookingHistory()   //Margarita
        {
            return await _context.Bookings
                .Where(b => b.FromDate <= DateOnly.FromDateTime(DateTime.Today))
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    GuestName = b.Guest.LastName + ", " + b.Guest.FirstName,
                    RoomNames = b.BookingRooms.Select(br => br.Room.Name).ToList(),
                    BookedFromDate = b.FromDate,
                    BookedToDate = b.ToDate,
                    NumberOfAdults = b.NumberOfAdults,
                    NumberOfChildren = b.NumberOfChildren,
                }).ToListAsync();
        }

        public async Task<Booking?> UpdateBookingAsync(UpdateBookingDto updatedBooking)  //Florent
        {
            var booking = await _context.Bookings.FindAsync(updatedBooking.Id);
            if (booking == null) return null;

            booking.FromDate = updatedBooking.FromDate;
            booking.ToDate = updatedBooking.ToDate;
            booking.NumberOfAdults = updatedBooking.NumberOfAdults;
            booking.NumberOfChildren = updatedBooking.NumberOfChildren;
            booking.Breakfast = updatedBooking.Breakfast;

            await _context.SaveChangesAsync();
            return booking;
        }

    }
}
