namespace Gotlandsrussen.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfBeds { get; set; }

        //Navigation properites
        public ICollection<Room> Rooms { get; set; }
    }
}