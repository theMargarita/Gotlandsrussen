namespace Gotlandsrussen.Models.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string RoomTypeName { get; set; }
        public int NumberOfBeds { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
