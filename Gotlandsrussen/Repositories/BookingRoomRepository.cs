using Gotlandsrussen.Data;
using Gotlandsrussen.Models;

namespace GotlandsrussenAPI.Repositories
{
    public class BookingRoomRepository : IBookingRoomRepository
    {
        private readonly HotelDbContext _context;
        public BookingRoomRepository(HotelDbContext context)
        {
            _context = context;
        }
        public async Task<BookingRoom> AddBookingRooms(BookingRoom bookingRoom)
        {
            var booking = await _context.BookingRooms.AddAsync(bookingRoom);
            await _context.SaveChangesAsync();

            return booking.Entity;
        }
    }
}
