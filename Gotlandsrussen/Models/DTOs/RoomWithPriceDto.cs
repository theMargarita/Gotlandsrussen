namespace Gotlandsrussen.Models.DTOs
{
    public class RoomWithPriceDto
    {
        public string RoomType { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
    }
}
