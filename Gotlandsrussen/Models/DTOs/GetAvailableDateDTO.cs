using System.Text.Json.Serialization;

namespace Gotlandsrussen.Models.DTOs
{
    public class GetAvailableDateDTO
    {
        public DateOnly BookedFromDate { get; set; }
        public DateOnly BookedToDate { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }

    }
}
