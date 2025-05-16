namespace Gotlandsrussen.Models
{
    public class BookingService
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int Quantity { get; set; }
    }
}
