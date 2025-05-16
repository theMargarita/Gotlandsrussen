namespace Gotlandsrussen.Models.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public Guest Guest { get; set; }
        public List<string> Rooms { get; set; }
        public DateOnly BookedFromDate { get; set; } 
        public DateOnly BookedToDate { get; set; } 
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool BookingIsCancelled { get; set; } = false;
    }
}
