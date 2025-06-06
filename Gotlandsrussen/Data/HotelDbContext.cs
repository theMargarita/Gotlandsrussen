﻿using Gotlandsrussen.Models;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<BookingRoom> BookingRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData.Seed(modelBuilder);

        }
    }
}

    