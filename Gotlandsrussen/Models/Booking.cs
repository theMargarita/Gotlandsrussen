namespace Gotlandsrussen.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }


        //Nagivation properties
        public Guest Guest { get; set; } 
        public ICollection<Room> Room { get; set; }

    }
}
