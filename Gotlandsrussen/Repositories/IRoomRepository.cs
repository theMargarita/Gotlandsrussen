using Gotlandsrussen.Models.DTOs;

namespace Gotlandsrussen.Repositories
{
    public interface IRoomRepository
    {
        public Task<ICollection<RoomDto>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children); //lina


        public Task<ICollection<RoomDto>> GetAvailableRoomsAsync(DateOnly startDate, DateOnly endDate); //kim

    }
}
