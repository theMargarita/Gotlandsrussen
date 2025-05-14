namespace Gotlandsrussen.Models
{
    public class BreakfastOption
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public bool IsIncluded { get; set; } = false;

        // Navigation
        public Booking Booking { get; set; }
    }
}
