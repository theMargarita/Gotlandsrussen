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

        public async Task<Guest> AddGuest(Guest guest)
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
    }
}
