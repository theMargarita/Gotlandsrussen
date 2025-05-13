namespace Gotlandsrussen.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }

        //Navigation properties
        public RoomType RoomType { get; set; }
        public ICollection<Booking> Booking { get; set; }
    }
    
    //public enum RoomTypes
    //{
    //    Singel = 1,
    //    Double = 2,
    //    Suite = 4,
    //    Family = 6,
    //    HoneymoonSuite = 1//heart shaped bed
    //}

}