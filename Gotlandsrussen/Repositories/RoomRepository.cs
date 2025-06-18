using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
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
                   br.Booking.FromDate <= toDate &&
                   br.Booking.ToDate >= fromDate))
            .Select(r => new RoomDto
            {
                Id = r.Id,
                RoomName = r.Name,
                RoomTypeName= r.RoomType.Name,
                NumberOfBeds = r.RoomType.NumberOfBeds,
                PricePerNight = r.RoomType.PricePerNight
            })
            .ToListAsync();

            return availableRooms;
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
        public async Task<Room> GetRoomById(int roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<ICollection<RoomDto>> GetCleanRooms()
        {
            return await _context.Rooms
                .Where(r => r.IsCleaned)
                .Include(r => r.RoomType)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomName = r.Name,
                    RoomTypeName = r.RoomType.Name,
                    NumberOfBeds = r.RoomType.NumberOfBeds,
                    PricePerNight = r.RoomType.PricePerNight,
                    IsCleaned = r.IsCleaned  
                }).ToListAsync();
        }
    }
}
