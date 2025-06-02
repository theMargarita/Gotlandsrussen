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
        public DateOnly FromDate { get; set; } // Booked room from this date
        public DateOnly ToDate { get; set; } // Booked room To this date
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool Breakfast { get; set; } = false; //not sure about this one
        public List<RoomDto>? Rooms { get; set; }

    }
}
