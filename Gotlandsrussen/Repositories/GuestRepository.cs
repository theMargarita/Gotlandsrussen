using Gotlandsrussen.Data;
using Gotlandsrussen.Models;

namespace Gotlandsrussen.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _context;
        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public Task<Guest> AddGuest(Guest guest)
        {
            var addedGuest = _context.Guests.Add(guest);
            _context.SaveChanges();
            return 
        }

        public Task<ICollection<Guest>> GetAllGuests()
        {
            throw new NotImplementedException();
        }

        public Task<Guest?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Guest> UpdateGuest(Guest guest)
        {
            throw new NotImplementedException();
        }
    }
}
