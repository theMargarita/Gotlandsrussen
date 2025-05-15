using Gotlandsrussen.Models;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Data
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // RoomTypes
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { Id = 1, Name = "Single", NumberOfBeds = 1, PricePerNight = 500m },
                new RoomType { Id = 2, Name = "Double", NumberOfBeds = 2, PricePerNight = 900m },
                new RoomType { Id = 3, Name = "Family", NumberOfBeds = 4, PricePerNight = 1500m },
                new RoomType { Id = 4, Name = "Suite", NumberOfBeds = 6, PricePerNight = 3000m }
            );

            // Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomName = "101", RoomTypeId = 1 },
                new Room { Id = 2, RoomName = "102", RoomTypeId = 2 },
                new Room { Id = 3, RoomName = "103", RoomTypeId = 3 },
                new Room { Id = 4, RoomName = "104", RoomTypeId = 4 },
                new Room { Id = 5, RoomName = "105", RoomTypeId = 3 }

            );

            // Guests
            modelBuilder.Entity<Guest>().HasData(
                new Guest { Id = 1, FirstName = "Alice", LastName = "Andersson", Email = "alice@example.com", Phone = "0701234567" },
                new Guest { Id = 2, FirstName = "Bob", LastName = "Bengtsson", Email = "bob@example.com", Phone = "0702345678" },
                new Guest { Id = 3, FirstName = "Tom", LastName = "Larsson", Email = "tom@example.com", Phone = "0702345679" },
                new Guest { Id = 4, FirstName = "Lisa", LastName = "Helgesson", Email = "lisa@example.com", Phone = "0702345680" },
                new Guest { Id = 5, FirstName = "Kalle", LastName = "Spongtsson", Email = "kalle@example.com", Phone = "0702345681" }
            );

            // Bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    GuestId = 1,
                    BookedFromDate = new DateOnly(2025, 6, 10),
                    BookedToDate = new DateOnly(2025, 6, 15),
                    NumberOfAdults = 2,
                    NumberOfChildren = 0,
                    BookingIsCancelled = false
                },
                new Booking
                {
                    Id = 2,
                    GuestId = 2,
                    BookedFromDate = new DateOnly(2025, 7, 1),
                    BookedToDate = new DateOnly(2025, 7, 5),
                    NumberOfAdults = 2,
                    NumberOfChildren = 2,
                    BookingIsCancelled = false
                },
                new Booking
                {
                    Id = 3,
                    GuestId = 3,
                    BookedFromDate = new DateOnly(2025, 8, 20),
                    BookedToDate = new DateOnly(2025, 8, 25),
                    NumberOfAdults = 1,
                    NumberOfChildren = 0,
                    BookingIsCancelled = true
                },
                new Booking
                {
                    Id = 4,
                    GuestId = 4,
                    BookedFromDate = new DateOnly(2025, 9, 5),
                    BookedToDate = new DateOnly(2025, 9, 10),
                    NumberOfAdults = 2,
                    NumberOfChildren = 1,
                    BookingIsCancelled = false
                },
                new Booking
                {
                    Id = 5,
                    GuestId = 5,
                    BookedFromDate = new DateOnly(2025, 10, 15),
                    BookedToDate = new DateOnly(2025, 10, 18),
                    NumberOfAdults = 1,
                    NumberOfChildren = 1,
                    BookingIsCancelled = false
                }

            );

            // BreakfastOptions
            modelBuilder.Entity<BreakfastOption>().HasData(
                new BreakfastOption { Id = 1, BookingId = 1, IsIncluded = true },
                new BreakfastOption { Id = 2, BookingId = 2, IsIncluded = false },
                new BreakfastOption { Id = 3, BookingId = 3, IsIncluded = true },
                new BreakfastOption { Id = 4, BookingId = 4, IsIncluded = false },
                new BreakfastOption { Id = 5, BookingId = 5, IsIncluded = true }
            );

            // BookingRooms
            modelBuilder.Entity<BookingRoom>().HasData(
                new BookingRoom {Id = 1, BookingId = 1, RoomId = 2 }, // Alice -> Room 102
                new BookingRoom {Id = 2, BookingId = 2, RoomId = 3 },  // Bob -> Room 103
                new BookingRoom {Id = 3, BookingId = 3, RoomId = 4 },  // Tom -> Room 104
                new BookingRoom {Id = 4, BookingId = 4, RoomId = 1 },  // Lisa -> Room 101
                new BookingRoom {Id = 5, BookingId = 5, RoomId = 5 }   // Kalle -> Room 105

            );

        }
    }
}
