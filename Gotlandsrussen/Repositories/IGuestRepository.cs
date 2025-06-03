using Gotlandsrussen.Models;

namespace Gotlandsrussen.Repositories
{
    public interface IGuestRepository
    {
        Task<ICollection<Guest>> GetAllGuests();
        Task<Guest> AddGuest(Guest guest);
        Task DeleteGuest(int guestId);
    }
}