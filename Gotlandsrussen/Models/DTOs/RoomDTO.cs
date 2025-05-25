using System.Text.Json.Serialization;

namespace Gotlandsrussen.Models.DTOs
{
    public class RoomDTO
    {
        public string Name { get; set; }
        public int NumberOfBeds { get; set; }
        [JsonIgnore]
        public List<GetAvailableDateDTO> GetAvailableDate { get;set;}

    }
}