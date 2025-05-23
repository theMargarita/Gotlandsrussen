namespace Gotlandsrussen.Models.DTOs
{
    public class AddBreakfastRequestDto
    {
        public int BookingId { get; set; }
        public bool Breakfast { get; set; } = false;
    }
}
