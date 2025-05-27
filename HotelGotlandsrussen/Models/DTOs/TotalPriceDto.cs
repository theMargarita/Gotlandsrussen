namespace Gotlandsrussen.Models.DTOs
{
    public class TotalPriceDto
    {
        public int BookingId { get; set; }
        public List<RoomTypeWithPriceDto> Rooms { get; set; } = new();
        public int NumberOfNights { get; set; }
        public int NumberOfGuests { get; set; }
        public int NumberOfBreakfasts { get; set; }
        public decimal BreakfastPrice { get; set; } 
        public decimal TotalPrice { get; set; }
    }
}
