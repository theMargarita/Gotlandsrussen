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

        //Utan includes så blir relationshämtningarna null... Varför?
        public async Task<Booking?> GetById(int id)
        {
            return await _context.Bookings
                .Include(b => b.BookingRooms)
                .ThenInclude(br => br.Room)
                .ThenInclude(r => r.RoomType)
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<ICollection<RoomDto>> GetAvailableRoomsAsync(DateOnly startDate, DateOnly endDate)
        {
            var bookedRoomIds = await _context.BookingRooms
                .Include(br => br.Booking)
                .Where(br =>
                    !br.Booking.IsCancelled &&
                    (startDate < br.Booking.ToDate &&
                     endDate > br.Booking.FromDate))
                .Select(br => br.RoomId)
                .Distinct()
                .ToListAsync();

            var availableRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => !bookedRoomIds.Contains(r.Id))
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomName = r.Name,
                    RoomTypeName = r.RoomType.Name,
                    NumberOfBeds = r.RoomType.NumberOfBeds,
                    PricePerNight = r.RoomType.PricePerNight
                })
                .ToListAsync();

            return availableRooms;
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
            var booking = await _context.Bookings.FindAsync(updatedBooking.Id);
            if (booking == null) return null;

            booking.FromDate = updatedBooking.BookedFromDate;
            booking.ToDate = updatedBooking.BookedToDate;
            booking.NumberOfAdults = updatedBooking.NumberOfAdults;
            booking.NumberOfChildren = updatedBooking.NumberOfChildren;
            booking.Breakfast = updatedBooking.Breakfast;

            await _context.SaveChangesAsync();
            return booking;
        }

    }
}
