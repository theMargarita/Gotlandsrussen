namespace Gotlandsrussen.Models
{
    public class Breakfast
    {
        public int Id { get; set; }
        public decimal PricePerAdult { get; set; } = 100m;
        public decimal PricePerChild { get; set; } = 50m;
    }

}
