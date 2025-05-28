namespace Gotlandsrussen.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public DateOnly FromDate { get; set; } // Booked room from this date
        public DateOnly ToDate { get; set; } // Booked room To this date
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool IsCancelled { get; set; } = false; // if gest wants to cancel the booking. 
        public bool Breakfast { get; set; } = false;

        //Nagivation properties
        public Guest Guest { get; set; }
        public ICollection<BookingRoom> BookingRooms { get; set; }
    }
}