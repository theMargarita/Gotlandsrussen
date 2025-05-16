using Gotlandsrussen.Models;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Data
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {

            // Guests
            modelBuilder.Entity<Guest>().HasData(
                new Guest { Id = 1, FirstName = "Alice", LastName = "Andersson", Email = "alice@example.com", Phone = "0701234567" },
                new Guest { Id = 2, FirstName = "Bob", LastName = "Bengtsson", Email = "bob@example.com", Phone = "0702345678" },
                new Guest { Id = 3, FirstName = "Tom", LastName = "Larsson", Email = "tom@example.com", Phone = "0702345679" },
                new Guest { Id = 4, FirstName = "Lisa", LastName = "Helgesson", Email = "lisa@example.com", Phone = "0702345680" },
                new Guest { Id = 5, FirstName = "Kalle", LastName = "Spongtsson", Email = "kalle@example.com", Phone = "0702345681" },
                new Guest { Id = 6, FirstName = "Emma", LastName = "Nilsson", Email = "emma@example.com", Phone = "0702345682" },
                new Guest { Id = 7, FirstName = "Oscar", LastName = "Johansson", Email = "oscar@example.com", Phone = "0702345683" },
                new Guest { Id = 8, FirstName = "Sara", LastName = "Karlsson", Email = "sara@example.com", Phone = "0702345684" },
                new Guest { Id = 9, FirstName = "Erik", LastName = "Svensson", Email = "erik@example.com", Phone = "0702345685" },
                new Guest { Id = 10, FirstName = "Maja", LastName = "Gustafsson", Email = "maja@example.com", Phone = "0702345686" },
                new Guest { Id = 11, FirstName = "Johan", LastName = "Lindberg", Email = "johan@example.com", Phone = "0702345687" },
                new Guest { Id = 12, FirstName = "Elin", LastName = "Persson", Email = "elin@example.com", Phone = "0702345688" },
                new Guest { Id = 13, FirstName = "Andreas", LastName = "Eriksson", Email = "andreas@example.com", Phone = "0702345689" },
                new Guest { Id = 14, FirstName = "Nora", LastName = "Bergström", Email = "nora@example.com", Phone = "0702345690" },
                new Guest { Id = 15, FirstName = "Viktor", LastName = "Holm", Email = "viktor@example.com", Phone = "0702345691" }
            );


            // RoomTypes
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { Id = 1, Name = "Single", NumberOfBeds = 1, PricePerNight = 500m },
                new RoomType { Id = 2, Name = "Double", NumberOfBeds = 2, PricePerNight = 900m },
                new RoomType { Id = 3, Name = "Family", NumberOfBeds = 4, PricePerNight = 1500m },
                new RoomType { Id = 4, Name = "Suite", NumberOfBeds = 6, PricePerNight = 3000m },
                new RoomType { Id = 5, Name = "Stables", NumberOfBeds = 12, PricePerNight = 5500m }
            );

            // Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomName = "101", RoomTypeId = 1 },
                new Room { Id = 2, RoomName = "102", RoomTypeId = 1 },
                new Room { Id = 3, RoomName = "103", RoomTypeId = 1 },
                new Room { Id = 4, RoomName = "104", RoomTypeId = 1 },
                new Room { Id = 5, RoomName = "105", RoomTypeId = 1 },
                new Room { Id = 6, RoomName = "106", RoomTypeId = 2 },
                new Room { Id = 7, RoomName = "107", RoomTypeId = 2 },
                new Room { Id = 8, RoomName = "108", RoomTypeId = 2 },
                new Room { Id = 9, RoomName = "109", RoomTypeId = 2 },
                new Room { Id = 10, RoomName = "110", RoomTypeId = 2 },
                new Room { Id = 11, RoomName = "111", RoomTypeId = 3 },
                new Room { Id = 12, RoomName = "112", RoomTypeId = 3 },
                new Room { Id = 13, RoomName = "113", RoomTypeId = 3 },
                new Room { Id = 14, RoomName = "114", RoomTypeId = 3 },
                new Room { Id = 15, RoomName = "115", RoomTypeId = 3 },
                new Room { Id = 16, RoomName = "116", RoomTypeId = 3 },
                new Room { Id = 17, RoomName = "117", RoomTypeId = 3 },
                new Room { Id = 18, RoomName = "118", RoomTypeId = 3 },
                new Room { Id = 19, RoomName = "119", RoomTypeId = 3 },
                new Room { Id = 20, RoomName = "120", RoomTypeId = 3 },
                new Room { Id = 21, RoomName = "121", RoomTypeId = 4 },
                new Room { Id = 22, RoomName = "122", RoomTypeId = 4 },
                new Room { Id = 23, RoomName = "123", RoomTypeId = 4 },
                new Room { Id = 24, RoomName = "124", RoomTypeId = 4 },
                new Room { Id = 25, RoomName = "125", RoomTypeId = 5 }
            );


            // Bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, GuestId = 1, BookedFromDate = new DateOnly(2025, 6, 10), BookedToDate = new DateOnly(2025, 6, 11), NumberOfAdults = 1, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 2, GuestId = 2, BookedFromDate = new DateOnly(2025, 6, 10), BookedToDate = new DateOnly(2025, 6, 15), NumberOfAdults = 1, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 3, GuestId = 3, BookedFromDate = new DateOnly(2025, 6, 11), BookedToDate = new DateOnly(2025, 6, 13), NumberOfAdults = 1, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 4, GuestId = 4, BookedFromDate = new DateOnly(2025, 6, 12), BookedToDate = new DateOnly(2025, 6, 13), NumberOfAdults = 2, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 5, GuestId = 5, BookedFromDate = new DateOnly(2025, 6, 15), BookedToDate = new DateOnly(2025, 6, 17), NumberOfAdults = 2, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 6, GuestId = 6, BookedFromDate = new DateOnly(2025, 6, 15), BookedToDate = new DateOnly(2025, 6, 16), NumberOfAdults = 2, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 7, GuestId = 7, BookedFromDate = new DateOnly(2025, 6, 16), BookedToDate = new DateOnly(2025, 6, 18), NumberOfAdults = 2, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 8, GuestId = 8, BookedFromDate = new DateOnly(2025, 7, 01), BookedToDate = new DateOnly(2025, 7, 02), NumberOfAdults = 2, NumberOfChildren = 2, BookingIsCancelled = false },
                new Booking { Id = 9, GuestId = 9, BookedFromDate = new DateOnly(2025, 7, 01), BookedToDate = new DateOnly(2025, 7, 03), NumberOfAdults = 2, NumberOfChildren = 1, BookingIsCancelled = false },
                new Booking { Id = 10, GuestId = 10, BookedFromDate = new DateOnly(2025, 7, 01), BookedToDate = new DateOnly(2025, 7, 05), NumberOfAdults = 2, NumberOfChildren = 2, BookingIsCancelled = false },
                new Booking { Id = 11, GuestId = 11, BookedFromDate = new DateOnly(2025, 7, 05), BookedToDate = new DateOnly(2025, 7, 08), NumberOfAdults = 2, NumberOfChildren = 2, BookingIsCancelled = false },
                new Booking { Id = 12, GuestId = 12, BookedFromDate = new DateOnly(2025, 7, 10), BookedToDate = new DateOnly(2025, 7, 15), NumberOfAdults = 2, NumberOfChildren = 1, BookingIsCancelled = false },
                new Booking { Id = 13, GuestId = 13, BookedFromDate = new DateOnly(2025, 7, 17), BookedToDate = new DateOnly(2025, 7, 19), NumberOfAdults = 2, NumberOfChildren = 2, BookingIsCancelled = false },
                new Booking { Id = 14, GuestId = 14, BookedFromDate = new DateOnly(2025, 7, 25), BookedToDate = new DateOnly(2025, 7, 26), NumberOfAdults = 2, NumberOfChildren = 3, BookingIsCancelled = false },
                new Booking { Id = 15, GuestId = 15, BookedFromDate = new DateOnly(2025, 8, 02), BookedToDate = new DateOnly(2025, 8, 03), NumberOfAdults = 2, NumberOfChildren = 4, BookingIsCancelled = false },
                new Booking { Id = 16, GuestId = 1, BookedFromDate = new DateOnly(2025, 8, 03), BookedToDate = new DateOnly(2025, 8, 05), NumberOfAdults = 2, NumberOfChildren = 4, BookingIsCancelled = false },
                new Booking { Id = 17, GuestId = 2, BookedFromDate = new DateOnly(2025, 8, 05), BookedToDate = new DateOnly(2025, 8, 07), NumberOfAdults = 4, NumberOfChildren = 0, BookingIsCancelled = false },
                new Booking { Id = 18, GuestId = 3, BookedFromDate = new DateOnly(2025, 8, 10), BookedToDate = new DateOnly(2025, 8, 13), NumberOfAdults = 3, NumberOfChildren = 3, BookingIsCancelled = false },
                new Booking { Id = 19, GuestId = 4, BookedFromDate = new DateOnly(2025, 8, 12), BookedToDate = new DateOnly(2025, 8, 13), NumberOfAdults = 4, NumberOfChildren = 8, BookingIsCancelled = false },
                new Booking { Id = 20, GuestId = 5, BookedFromDate = new DateOnly(2025, 8, 15), BookedToDate = new DateOnly(2025, 8, 20), NumberOfAdults = 2, NumberOfChildren = 5, BookingIsCancelled = false }
            );

            // BookingRooms
            modelBuilder.Entity<BookingRoom>().HasData(
                new BookingRoom { Id = 1, BookingId = 1, RoomId = 1 }, 
                new BookingRoom { Id = 2, BookingId = 2, RoomId = 2 }, 
                new BookingRoom { Id = 3, BookingId = 3, RoomId = 3 }, 
                new BookingRoom { Id = 4, BookingId = 4, RoomId = 6 }, 
                new BookingRoom { Id = 5, BookingId = 5, RoomId = 7 },   
                new BookingRoom { Id = 6, BookingId = 6, RoomId = 8 },   
                new BookingRoom { Id = 7, BookingId = 7, RoomId = 9 },   
                new BookingRoom { Id = 8, BookingId = 8, RoomId = 11 },  
                new BookingRoom { Id = 9, BookingId = 9, RoomId = 12 },  
                new BookingRoom { Id = 10, BookingId = 10, RoomId = 13 },
                new BookingRoom { Id = 11, BookingId = 11, RoomId = 14 },
                new BookingRoom { Id = 12, BookingId = 12, RoomId = 15 },
                new BookingRoom { Id = 13, BookingId = 13, RoomId = 16 },
                new BookingRoom { Id = 14, BookingId = 14, RoomId = 21 },
                new BookingRoom { Id = 15, BookingId = 15, RoomId = 22 },
                new BookingRoom { Id = 16, BookingId = 16, RoomId = 23 },
                new BookingRoom { Id = 17, BookingId = 17, RoomId = 24 },
                new BookingRoom { Id = 18, BookingId = 18, RoomId = 21 },
                new BookingRoom { Id = 19, BookingId = 19, RoomId = 25 },
                new BookingRoom { Id = 20, BookingId = 20, RoomId = 25 } 
            );

            // Services
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "AdultBreakfast", Price = 100m },
                new Service { Id = 2, Name = "ChildBreakfast", Price = 50m }
            );

            // BookingServices
            modelBuilder.Entity<BookingService>().HasData(
                new BookingService { Id = 1, BookingId = 1, ServiceId = 1, Quantity = 1 },
                new BookingService { Id = 2, BookingId = 4, ServiceId = 1, Quantity = 2 },
                new BookingService { Id = 3, BookingId = 7, ServiceId = 1, Quantity = 2 },
                new BookingService { Id = 4, BookingId = 10, ServiceId = 1, Quantity = 2 },
                new BookingService { Id = 5, BookingId = 10, ServiceId = 2, Quantity = 2 },
                new BookingService { Id = 6, BookingId = 12, ServiceId = 1, Quantity = 2 },
                new BookingService { Id = 7, BookingId = 12, ServiceId = 2, Quantity = 1 },
                new BookingService { Id = 8, BookingId = 15, ServiceId = 1, Quantity = 2 },
                new BookingService { Id = 9, BookingId = 15, ServiceId = 2, Quantity = 4 },
                new BookingService { Id = 10, BookingId = 16, ServiceId = 1, Quantity = 2 },
                new BookingService { Id = 11, BookingId = 16, ServiceId = 2, Quantity = 4 },
                new BookingService { Id = 12, BookingId = 2, ServiceId = 1, Quantity = 1 },
                new BookingService { Id = 13, BookingId = 3, ServiceId = 1, Quantity = 1 },
                new BookingService { Id = 14, BookingId = 20, ServiceId = 1, Quantity = 2 },
                new BookingService { Id = 15, BookingId = 20, ServiceId = 2, Quantity = 5 }
            );
        }
    }
}
