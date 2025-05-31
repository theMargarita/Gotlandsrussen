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

        public async Task<Guest> AddGuest(Guest guest)  //Kim
        {
            var addedGuest = _context.Guests.Add(guest);
            _context.SaveChanges();
            return addedGuest.Entity;
        }

        public async Task<ICollection<Guest>> GetAllGuests()   //Lina
        {
            var getAllGuests = _context.Guests.ToList();
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
