using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;

namespace HotelGotlandsrussenTESTS.TestSetup
{
    public static class MockDataSetup //test with moq
    {
        public static List<Guest> GetGuests()
        {
            return new List<Guest>
            {
                new Guest { Id = 1, FirstName = "Alice", LastName = "Andersson", Email = "alice@example.com", Phone = "0701234567" },
                new Guest { Id = 2, FirstName = "Bob", LastName = "Bengtsson", Email = "bob@example.com", Phone = "0702345678" },
                new Guest { Id = 3, FirstName = "Tom", LastName = "Larsson", Email = "tom@example.com", Phone = "0702345679" }
            };
        }

        public static List<RoomType> GetRoomTypes()
        {
            return new List<RoomType>
            {
                new RoomType { Id = 1, Name = "Single", NumberOfBeds = 1, PricePerNight = 500m },
                new RoomType { Id = 2, Name = "Double", NumberOfBeds = 2, PricePerNight = 900m },
                new RoomType { Id = 3, Name = "Family", NumberOfBeds = 4, PricePerNight = 1500m }
            };
        }

        public static List<Room> GetRooms()
        {
            return new List<Room>
            {
                new Room { Id = 1, Name = "101", RoomTypeId = 1 },
                new Room { Id = 2, Name = "102", RoomTypeId = 1 },
                new Room { Id = 3, Name = "103", RoomTypeId = 1 }
            };
        }

        public static List<Booking> GetBookings()
        {
            return new List<Booking>
            {
                new Booking { Id = 1, GuestId = 1, FromDate = new DateOnly(2025, 6, 10), ToDate = new DateOnly(2025, 6, 11), NumberOfAdults = 1, NumberOfChildren = 0, IsCancelled = false, Breakfast = false },
                new Booking { Id = 2, GuestId = 2, FromDate = new DateOnly(2025, 6, 10), ToDate = new DateOnly(2025, 6, 15), NumberOfAdults = 1, NumberOfChildren = 0, IsCancelled = false, Breakfast = true },
                new Booking { Id = 3, GuestId = 3, FromDate = new DateOnly(2025, 6, 11), ToDate = new DateOnly(2025, 6, 13), NumberOfAdults = 1, NumberOfChildren = 0, IsCancelled = false, Breakfast = false }
            };
        }

        public static List<BookingRoom> GetBookingRooms()
        {
            return new List<BookingRoom>
            {
                new BookingRoom { Id = 1, BookingId = 1, RoomId = 1 },
                new BookingRoom { Id = 2, BookingId = 2, RoomId = 2 },
                new BookingRoom { Id = 3, BookingId = 3, RoomId = 3 }
            };
        }

        public static List<BookingDto> GetBookingDtos()
        {
            return new List<BookingDto>
            {
                new BookingDto { Id = 1, GuestName = "Andersson, Alice", BookedFromDate = new DateOnly(2025, 6, 10), BookedToDate = new DateOnly(2025, 6, 11), NumberOfAdults = 1, NumberOfChildren = 0 },
                new BookingDto { Id = 2, GuestName = "Bengtsson, Bob", BookedFromDate = new DateOnly(2025, 6, 10), BookedToDate = new DateOnly(2025, 6, 15), NumberOfAdults = 1, NumberOfChildren = 0 },
                new BookingDto { Id = 3, GuestName = "Larsson, Tom", BookedFromDate = new DateOnly(2025, 6, 11), BookedToDate = new DateOnly(2025, 6, 13), NumberOfAdults = 1, NumberOfChildren = 0 }
            };
        }

        public static List<TotalPriceDto> GetTotalPriceDtos()
        {
            return new List<TotalPriceDto>
            {
                new TotalPriceDto
                {
                    BookingId = 1,
                    Rooms = new List<RoomTypeWithPriceDto>
                    {
                        new RoomTypeWithPriceDto { RoomType = "Single", PricePerNight = 500m },
                        new RoomTypeWithPriceDto { RoomType = "Double", PricePerNight = 750m }
                    },
                    NumberOfNights = 3,
                    NumberOfGuests = 2,
                    NumberOfBreakfasts = 6,   // 3 nätter * 2 gäster
                    BreakfastPrice = 50m,
                    TotalPrice = (500m + 750m) * 3 + 6 * 50m // 3750 + 300 = 4050
                },
                new TotalPriceDto
                {
                    BookingId = 2,
                    Rooms = new List<RoomTypeWithPriceDto>
                    {
                        new RoomTypeWithPriceDto { RoomType = "Suite", PricePerNight = 1200m }
                    },
                    NumberOfNights = 5,
                    NumberOfGuests = 1,
                    NumberOfBreakfasts = 5,   // 5 nätter * 1 gäst
                    BreakfastPrice = 50m,
                    TotalPrice = 1200m * 5 + 5 * 50m // 6000 + 250 = 6250
                },
                new TotalPriceDto
                {
                    BookingId = 3,
                    Rooms = new List<RoomTypeWithPriceDto>
                    {
                        new RoomTypeWithPriceDto { RoomType = "Double", PricePerNight = 750m },
                        new RoomTypeWithPriceDto { RoomType = "Single", PricePerNight = 500m }
                    },
                    NumberOfNights = 2,
                    NumberOfGuests = 4,
                    NumberOfBreakfasts = 8,  // 2 nätter * 4 gäster
                    BreakfastPrice = 50m,
                    TotalPrice = (750m + 500m) * 2 + 8 * 50m // 2500 + 400 = 2900
                }
            };
        }
    }
}
