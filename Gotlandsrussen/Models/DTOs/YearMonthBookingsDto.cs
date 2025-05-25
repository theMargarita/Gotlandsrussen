namespace Gotlandsrussen.Models.DTOs
{
    public class YearMonthBookingsDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<BookingDto> Bookings { get; set; } = new();
    }
}
