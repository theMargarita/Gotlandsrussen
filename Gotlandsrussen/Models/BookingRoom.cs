namespace Gotlandsrussen.Models
{
    public class BookingRoom
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
