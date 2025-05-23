using System.Globalization;

namespace Gotlandsrussen.Models.DTOs
{
    public class YearWeekBookingsDto
    {
        public int Year { get; set; }
        public int Week { get; set; }
        public List<BookingDto> Bookings { get; set; } = new();
    }
}
