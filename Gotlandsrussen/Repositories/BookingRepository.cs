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


        public async Task<ICollection<RoomDTO>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children)
        {
            var availableRooms = await _context.Rooms
            .Where(r => r.RoomType.NumberOfBeds >= (adults + children))
            .Where(r => !_context.BookingRooms
                .Any(br => br.RoomId == r.Id &&
                   br.Booking.BookedFromDate <= toDate &&
                   br.Booking.BookedToDate >= fromDate))
            .Select(r => new RoomDTO
            {
                Name = r.RoomType.Name,
                NumberOfBeds = r.RoomType.NumberOfBeds
            })
            .ToListAsync();

            var response = new List<GetAvailableDateDTO>()
            {
                 new GetAvailableDateDTO
                 {
                     BookedFromDate = fromDate,
                     BookedToDate = toDate,
                     NumberOfAdults = adults,
                     NumberOfChildren = children,
                 }
            }.ToList();

            return availableRooms;
        }

    }
}
