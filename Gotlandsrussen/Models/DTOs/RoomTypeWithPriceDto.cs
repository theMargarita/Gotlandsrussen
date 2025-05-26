namespace Gotlandsrussen.Models.DTOs
{
    public class RoomTypeWithPriceDto
    {
        public string RoomType { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
    }
}
