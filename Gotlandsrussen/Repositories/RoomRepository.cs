using Gotlandsrussen.Models.DTOs;

namespace Gotlandsrussen.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        public Task<ICollection<RoomDTO>> GetAvailableRoomByDateAndGuests(DateOnly fromDate, DateOnly toDate, int adults, int children)
        {
            throw new NotImplementedException();
        }
    }
}
