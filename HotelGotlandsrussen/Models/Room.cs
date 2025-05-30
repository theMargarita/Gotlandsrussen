﻿namespace Gotlandsrussen.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int RoomTypeId { get; set; } // Foreign key to RoomType

        //Navigation properties
        public RoomType RoomType { get; set; }
        public ICollection<BookingRoom> BookingRooms { get; set; }
    }
}