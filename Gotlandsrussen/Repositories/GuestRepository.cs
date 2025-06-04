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

        public async Task<ICollection<Guest>> GetAllGuests() 
        {
            var getAllGuests = await _context.Guests.ToListAsync();
            return getAllGuests;
        }

        public async Task DeleteGuest(int guestId)
        {
            var guest = await _context.Guests.FindAsync(guestId);
            
            if (guest == null)
            {
                throw new KeyNotFoundException("Guest not found");
            }

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
        }

    }
}
