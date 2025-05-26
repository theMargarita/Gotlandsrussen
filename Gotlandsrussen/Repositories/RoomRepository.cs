using Gotlandsrussen.Data;
using Gotlandsrussen.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _context;

        public RoomRepository(HotelDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<RoomDto>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children)
        {
            var availableRooms = await _context.Rooms
            .Where(r => r.RoomType.NumberOfBeds >= (adults + children))
            .Where(r => !_context.BookingRooms
                .Any(br => br.RoomId == r.Id &&
                   br.Booking.BookedFromDate <= toDate &&
                   br.Booking.BookedToDate >= fromDate))
            .Select(r => new RoomDto
            {
                Id = r.Id,
                RoomName = r.RoomName,
                RoomTypeName= r.RoomType.Name,
                NumberOfBeds = r.RoomType.NumberOfBeds,
                PricePerNight = r.RoomType.PricePerNight
            })
            .ToListAsync();

            return availableRooms;
        }

    }
}
