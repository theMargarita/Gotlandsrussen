using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using HotelGotlandsrussen.Models.DTOs;

namespace Gotlandsrussen.Repositories
{
    public interface IBookingRepository
    {
        public Task<ICollection<BookingDto>> GetAllFutureBookings();
        public Task<ICollection<BookingDto>> GetBookingHistory();

        public Task<Booking> GetById(int id);

        public Task<ICollection<RoomDto>> GetAvailableRoomsAsync(DateOnly startDate, DateOnly endDate);

        public Task Update(Booking booking);

        public Task<Booking?> UpdateBookingAsync(UpdateBookingDto updatedBooking);

    }
}
