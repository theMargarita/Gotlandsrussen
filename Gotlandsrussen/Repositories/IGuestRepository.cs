using Gotlandsrussen.Models;

namespace Gotlandsrussen.Repositories
{
    public interface IGuestRepository
    {
        Task<Guest?> GetById(int id); 
        Task<ICollection<Guest>> GetAllGuests();
        Task<Guest> AddGuest(Guest guest);
        Task<Guest> UpdateGuest(Guest guest);
    }
}