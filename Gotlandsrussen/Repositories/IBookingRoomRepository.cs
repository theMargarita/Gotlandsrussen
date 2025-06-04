using Gotlandsrussen.Models;

namespace GotlandsrussenAPI.Repositories
{
    public  interface IBookingRoomRepository
    {
        public Task<BookingRoom> AddBookingRooms(BookingRoom bookingRoom);
    }
}