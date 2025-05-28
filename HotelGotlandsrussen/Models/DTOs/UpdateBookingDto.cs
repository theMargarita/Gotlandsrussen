using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGotlandsrussen.Models.DTOs
{
    public class UpdateBookingDto
    {
        public int Id { get; set; }
        public DateOnly BookedFromDate { get; set; }
        public DateOnly BookedToDate { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool Breakfast { get; set; }
    }
}
