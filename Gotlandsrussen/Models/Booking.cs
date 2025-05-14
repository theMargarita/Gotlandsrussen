namespace Gotlandsrussen.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public DateOnly BookedFromDate { get; set; } // Booked room from this date
        public DateOnly BookedToDate { get; set; } // Booked room To this date
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool Breakfast { get; set; }
        public bool BookingIsCancelled { get; set; } = false; // if gest wants to cancel the booking. 

        //Nagivation properties
        public Guest Guest { get; set; } 
        public ICollection<Room> Room { get; set; }
    }
}