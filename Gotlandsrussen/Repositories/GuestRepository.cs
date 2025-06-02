using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _context;
        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Guest> AddGuest(Guest guest)  //Kim
        {
            var addedGuest = _context.Guests.Add(guest);
            await _context.SaveChangesAsync();
            return addedGuest.Entity;
        }

        public async Task<ICollection<Guest>> GetAllGuests()   //Lina
        {
            var getAllGuests = await _context.Guests.ToListAsync();
            return getAllGuests;
        }

        public async Task<Guest?> GetById(int id)   //Margarita
        {
            throw new NotImplementedException();
        }

        public async Task<Guest> UpdateGuest(Guest guest)    //Florent
        {
            throw new NotImplementedException();
        }
    }
}
