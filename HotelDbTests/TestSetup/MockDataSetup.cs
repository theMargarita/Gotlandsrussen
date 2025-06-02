using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussen.Models.DTOs;

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
        
        public static List<UpdateBookingDto> GetUpdateBookingDtos()
        {
            return new List<UpdateBookingDto>
            {
                new UpdateBookingDto { Id = 1, FromDate = new DateOnly(2025, 6, 11), ToDate = new DateOnly(2025, 6, 12), NumberOfAdults = 2, NumberOfChildren = 1, Breakfast = true },
                new UpdateBookingDto { Id = 99, FromDate = new DateOnly(2025, 6, 12), ToDate = new DateOnly(2025, 6, 16), NumberOfAdults = 1, NumberOfChildren = 0, Breakfast = true },
                new UpdateBookingDto { Id = 999, FromDate = new DateOnly(2025, 6, 13), ToDate = new DateOnly(2025, 6, 14), NumberOfAdults = 1, NumberOfChildren = 0, Breakfast = false }
            };
        }

        public static List<RoomDto> GetRoomDtos()
        {
            return new List<RoomDto>
            {
                new RoomDto { Id = 1, RoomName = "Room A", RoomTypeName = "Single", NumberOfBeds = 1, PricePerNight = 500m },
                new RoomDto { Id = 2, RoomName = "Room B", RoomTypeName = "Double", NumberOfBeds = 2, PricePerNight = 750m }
            };
        }

        public static Booking? GetBookingsWithRelations(int id)
        {
            return id switch
            {
                1 => new Booking
                {
                    Id = 1,
                    FromDate = new DateOnly(2025, 6, 10),
                    ToDate = new DateOnly(2025, 6, 13), // 3 nätter
                    NumberOfAdults = 2,
                    NumberOfChildren = 1, // 3 gäster
                    IsCancelled = false,
                    Breakfast = true,
                    BookingRooms = new List<BookingRoom>
                    {
                        new BookingRoom
                        {
                            Room = new Room
                            {
                                RoomType = new RoomType
                                {
                                    Name = "Single",
                                    PricePerNight = 500m
                                }
                            }
                        },
                        new BookingRoom
                        {
                            Room = new Room
                            {
                                RoomType = new RoomType
                                {
                                    Name = "Double",
                                    PricePerNight = 750m
                                }
                            }
                        }
                    }
                },

                2 => new Booking
                {
                    Id = 2,
                    FromDate = new DateOnly(2025, 7, 1),
                    ToDate = new DateOnly(2025, 7, 6), // 5 nätter
                    NumberOfAdults = 1,
                    NumberOfChildren = 0,
                    IsCancelled = false,
                    Breakfast = true,
                    BookingRooms = new List<BookingRoom>
                    {
                        new BookingRoom
                        {
                            Room = new Room
                            {
                                RoomType = new RoomType
                                {
                                    Name = "Suite",
                                    PricePerNight = 1200m
                                }
                            }
                        }
                    }
                },
                3 => new Booking
                {
                    Id = 3,
                    FromDate = new DateOnly(2025, 7, 1),
                    ToDate = new DateOnly(2025, 7, 6), // 5 nätter
                    NumberOfAdults = 1,
                    NumberOfChildren = 0,
                    IsCancelled = false,
                    Breakfast = false,
                    BookingRooms = new List<BookingRoom>
                    {
                        new BookingRoom
                        {
                            Room = new Room
                            {
                                RoomType = new RoomType
                                {
                                    Name = "Suite",
                                    PricePerNight = 1200m
                                }
                            }
                        }
                    }
                }
            };
        }

    }
}
