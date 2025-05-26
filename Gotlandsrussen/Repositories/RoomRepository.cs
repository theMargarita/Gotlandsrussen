using Gotlandsrussen.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Repositories
{
    public class RoomRepository : IRoomRepository
    {

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
                RoomName = r.RoomName,
                Name = r.RoomType.Name,
                NumberOfBeds = r.RoomType.NumberOfBeds,
            })
            .ToListAsync();

            return availableRooms;
        }

    }
}
