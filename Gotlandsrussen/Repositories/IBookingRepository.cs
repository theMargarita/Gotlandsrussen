using Gotlandsrussen.Models.DTOs;

namespace Gotlandsrussen.Repositories
{
    public interface IBookingRepository
    {
        public Task<ICollection<BookingDto>> GetAllFutureBookings();
    }
}
