using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfBeds { get; set; }

        [Precision(18, 2)] // Precision attribute to specify the precision and scale of the decimal type.
        public decimal PricePerNight { get; set; }


        //Navigation properites
        public ICollection<Room> Rooms { get; set; }
    }
}