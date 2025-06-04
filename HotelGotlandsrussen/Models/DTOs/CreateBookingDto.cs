using Gotlandsrussen.Models.DTOs;

namespace HotelGotlandsrussenLIBRARY.Models.DTOs
{
    public class CreateBookingDto
    {
        public int BookingId { get; set; }
        public int GuestId { get; set; }
        public DateOnly FromDate { get; set; } 
        public DateOnly ToDate { get; set; } 
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool Breakfast { get; set; } = false; 
        public List<int>? RoomIds { get; set; }

    }
}
