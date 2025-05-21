namespace Gotlandsrussen.Models.DTOs
{
    public class BookingRoomDTO
    {
        //public int Id { get; set; }
        public DateOnly BookedFromDate { get; set; }
        public DateOnly BookedToDate { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public List<string> RoomNames { get; set; }
        public List<int> NumberOfBed { get; set; }

    }
}
