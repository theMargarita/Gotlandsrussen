using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gotlandsrussen.Repositories
{
    public interface IBookingRepository
    {
        public Task<ICollection<BookingDto>> GetAllFutureBookings();
        public Task<ActionResult<AddBreakfastResponseDto>> AddBreakfast(AddBreakfastRequestDto request);
        public Task<ICollection<YearWeekBookingsDto>> GetBookingsGroupedByWeek();
        public Task<ICollection<YearMonthBookingsDto>> GetBookingsGroupedByMonth();
        public Task<Booking> GetById(int id);
        public Task<TotalPriceDto> GetTotalPrice(int BookingId);
    }
}
