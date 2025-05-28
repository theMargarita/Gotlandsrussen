using Gotlandsrussen.Models;

namespace Gotlandsrussen.Repositories
{
    public interface IGuestRepository
    {
        Task<Guest?> GetById(int id); 
        Task<ICollection<Guest>> GetAllGuests(); //Florent 
        Task<Guest> AddGuest(Guest guest); //MArgo
        Task<Guest> UpdateGuest(Guest guest);
    }
}