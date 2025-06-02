using Gotlandsrussen.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGotlandsrussenLIBRARY.Models.DTOs
{
    public class CreateBookingDto
    {
        public int GuestId { get; set; }
        public DateOnly FromDate { get; set; } 
        public DateOnly ToDate { get; set; } 
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool Breakfast { get; set; } = false; 
        public List<RoomDto>? Rooms { get; set; }

    }
}
