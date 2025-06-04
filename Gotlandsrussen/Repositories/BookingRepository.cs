using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using HotelGotlandsrussen.Models.DTOs;
using HotelGotlandsrussenLIBRARY.Models.DTOs;
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

        public async Task<Booking?> GetById(int id)
        {
            return await _context.Bookings
                .Include(b => b.BookingRooms)
                .ThenInclude(br => br.Room)
                .ThenInclude(r => r.RoomType)
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<BookingDto>> GetBookingHistory()
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

        public async Task<Booking?> UpdateBookingAsync(UpdateBookingDto updatedBooking)
        {
            var booking = await _context.Bookings
               .Include(b => b.BookingRooms)
                 .ThenInclude(br => br.Room)
                 .ThenInclude(r => r.RoomType)
                 .FirstOrDefaultAsync(b => b.Id == updatedBooking.Id);

            if (booking == null)
                return null;

            if (updatedBooking.NumberOfAdults + updatedBooking.NumberOfChildren == 0)
                throw new InvalidOperationException("Bokningen måste innehålla minst en person.");

            if (updatedBooking.FromDate < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException("Startdatumet har redan passerat.");

            var roomIds = booking.BookingRooms.Select(br => br.RoomId).ToList();

            bool hasConflict = await _context.BookingRooms
                 .Include(br => br.Booking)
                 .AnyAsync(br =>
                     roomIds.Contains(br.RoomId) &&
                     br.Booking.Id != booking.Id &&
                     !br.Booking.IsCancelled &&
                     updatedBooking.FromDate < br.Booking.ToDate &&
                     updatedBooking.ToDate > br.Booking.FromDate
                 );

            if (hasConflict)
                throw new InvalidOperationException("Vald tid krockar med en annan bokning.");


            int totalBeds = await _context.Rooms
                .Where(r => roomIds.Contains(r.Id))
                .SumAsync(r => r.RoomType.NumberOfBeds);

            int totalGuests = updatedBooking.NumberOfAdults + updatedBooking.NumberOfChildren;

            if (totalGuests > totalBeds)
                throw new InvalidOperationException("För många gäster för det valda rummets kapacitet.");


            booking.FromDate = updatedBooking.FromDate;
            booking.ToDate = updatedBooking.ToDate;
            booking.NumberOfAdults = updatedBooking.NumberOfAdults;
            booking.NumberOfChildren = updatedBooking.NumberOfChildren;
            booking.Breakfast = updatedBooking.Breakfast;

            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            var createBooking =  await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return createBooking.Entity;
        }

        public async Task DeleteBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingRooms)
                .FirstOrDefaultAsync(b => b.Id == id);
            
            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
}
