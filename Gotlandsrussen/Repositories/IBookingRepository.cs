using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using HotelGotlandsrussen.Models.DTOs;

namespace Gotlandsrussen.Repositories
{
    public interface IBookingRepository
    {
        public Task<ICollection<BookingDto>> GetAllFutureBookings(); //Kim
        public Task<ICollection<BookingDto>> GetBookingHistory(); //Margo

        public Task<Booking> GetById(int id); //done

        public Task Update(Booking booking); //Lina

        public Task<Booking?> UpdateBookingAsync(UpdateBookingDto updatedBooking); // Florent

    }
}
