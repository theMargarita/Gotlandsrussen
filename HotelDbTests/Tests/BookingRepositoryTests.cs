using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussen.Models.DTOs;
using HotelGotlandsrussenTESTS.TestSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGotlandsrussenTESTS.Tests
{
    [TestClass]
    public class BookingRepositoryTests
    {
        private BookingRepository _repository;
        private HotelDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unik databas för varje test
                .Options;

            _context = new HotelDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new BookingRepository(_context);
        }

        //Skriv tester här nedan
        //OBS: Inför varje nytt test skapas en ny databas med samma seed data som vår vanliga databas. Alltså samma HotelDbContext.
        //Det är en kopia av databasen som endast ligger i minnet.
        //Varje nytt test ger en ny fräsch DbContext. Det sparas alltså inget mellan testerna.

        [TestMethod]
        public async Task GetById_GettingABookingById_ReturnsMatchingBooking()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _repository.GetById(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task Update_ChangingNumberOfAdults_UpdatesNumberOfAdultsInBooking()
        {
            //Arrange
            var booking = new Booking { NumberOfAdults = 4 };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var originalNumberOfAdults = booking.NumberOfAdults;
            var bookingId = booking.Id;

            var bookingToUpdate = await _context.Bookings.FindAsync(bookingId);
            bookingToUpdate.NumberOfAdults = 2;

            //Act
            await _repository.Update(bookingToUpdate);

            //Assert
            var updatedBooking = await _context.Bookings.FindAsync(bookingId);

            Assert.IsNotNull(updatedBooking);
            Assert.AreNotEqual(originalNumberOfAdults, updatedBooking.NumberOfAdults);
            Assert.AreEqual(2, updatedBooking.NumberOfAdults);
        }

        [TestMethod]
        public async Task UpdateBookingAsync_ShouldReturnNull_WhenBookingDoesNotExist()
        {
            // Arrange
            var updatedBooking = new UpdateBookingDto
            {
                Id = 999, // obefintlig
                FromDate = new DateOnly(2025, 6, 10),
                ToDate = new DateOnly(2025, 6, 12),
                NumberOfAdults = 2,
                NumberOfChildren = 0,
                Breakfast = true
            };

            // Act
            var result = await _repository.UpdateBookingAsync(updatedBooking);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateBookingAsync_ShouldThrow_WhenDateConflictExists()
        {
            // Arrange: skapa 2 bokningar i samma rum, olika ID men överlappande datum
            var roomType = new RoomType { Name = "Standard", NumberOfBeds = 2, PricePerNight = 500 };
            var room = new Room { Name = "101", RoomType = roomType };
            _context.RoomTypes.Add(roomType);
            _context.Rooms.Add(room);

            var existingBooking = new Booking
            {
                FromDate = new DateOnly(2025, 6, 10),
                ToDate = new DateOnly(2025, 6, 15),
                IsCancelled = false,
                NumberOfAdults = 1,
                NumberOfChildren = 0,
                Breakfast = false,
                BookingRooms = new List<BookingRoom>
        {
            new BookingRoom { Room = room }
        }
            };

            _context.Bookings.Add(existingBooking);
            await _context.SaveChangesAsync();

            var conflictingBooking = new Booking
            {
                FromDate = new DateOnly(2025, 6, 5),
                ToDate = new DateOnly(2025, 6, 8),
                IsCancelled = false,
                NumberOfAdults = 1,
                NumberOfChildren = 0,
                Breakfast = false,
                BookingRooms = new List<BookingRoom>
        {
            new BookingRoom { Room = room }
        }
            };

            _context.Bookings.Add(conflictingBooking);
            await _context.SaveChangesAsync();

            var dto = new UpdateBookingDto
            {
                Id = conflictingBooking.Id,
                FromDate = new DateOnly(2025, 6, 12), // överlappar med existingBooking
                ToDate = new DateOnly(2025, 6, 14),
                NumberOfAdults = 1,
                NumberOfChildren = 0,
                Breakfast = true
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _repository.UpdateBookingAsync(dto));
        }

        [TestMethod]
        public async Task UpdateBookingAsync_ShouldThrow_WhenGuestCountExceedsCapacity()
        {
            // Arrange
            var roomType = new RoomType { Name = "Tiny", NumberOfBeds = 2 };
            var room = new Room { Name = "Mini", RoomType = roomType };
            _context.RoomTypes.Add(roomType);
            _context.Rooms.Add(room);

            var booking = new Booking
            {
                FromDate = new DateOnly(2025, 6, 1),
                ToDate = new DateOnly(2025, 6, 5),
                IsCancelled = false,
                NumberOfAdults = 1,
                NumberOfChildren = 0,
                Breakfast = false,
                BookingRooms = new List<BookingRoom>
        {
            new BookingRoom { Room = room }
        }
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var dto = new UpdateBookingDto
            {
                Id = booking.Id,
                FromDate = booking.FromDate,
                ToDate = booking.ToDate,
                NumberOfAdults = 3, // Totalt 3 gäster, men bara 2 sängar
                NumberOfChildren = 0,
                Breakfast = false
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _repository.UpdateBookingAsync(dto));
        }

        [TestMethod]
        public async Task UpdateBookingAsync_ShouldUpdateBooking_WhenAllValid()
        {
            // Arrange
            var roomType = new RoomType { Name = "Deluxe", NumberOfBeds = 4 };
            var room = new Room { Name = "RoomX", RoomType = roomType };
            _context.RoomTypes.Add(roomType);
            _context.Rooms.Add(room);

            var booking = new Booking
            {
                FromDate = new DateOnly(2025, 6, 5),
                ToDate = new DateOnly(2025, 6, 10),
                IsCancelled = false,
                NumberOfAdults = 1,
                NumberOfChildren = 1,
                Breakfast = false,
                BookingRooms = new List<BookingRoom>
        {
            new BookingRoom { Room = room }
        }
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var dto = new UpdateBookingDto
            {
                Id = booking.Id,
                FromDate = new DateOnly(2025, 6, 6),
                ToDate = new DateOnly(2025, 6, 12),
                NumberOfAdults = 2,
                NumberOfChildren = 1,
                Breakfast = true
            };

            // Act
            var result = await _repository.UpdateBookingAsync(dto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dto.FromDate, result.FromDate);
            Assert.AreEqual(dto.ToDate, result.ToDate);
            Assert.AreEqual(dto.NumberOfAdults, result.NumberOfAdults);
            Assert.AreEqual(dto.NumberOfChildren, result.NumberOfChildren);
            Assert.AreEqual(dto.Breakfast, result.Breakfast);
        }

        [TestMethod]
        public async Task DeleteBooking_DeletesExcistingBookingsAndCreatesANewBeforeRemoveIt_ReturnsNoBooking()
        {
            // Arrange
            // Clear existing bookings.
            var excistingBookings = _context.Bookings.ToList();
            _context.Bookings.RemoveRange(excistingBookings);
            await _context.SaveChangesAsync();

            // Add a booking to delete from the MockData database.
            var booking = MockDataSetup.GetBookings()[0];
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Act
            // Checks if bookingToDelete Excist and then Delete the booking with that id.
            var bookingToDelete = await _repository.GetById(booking.Id);
            Assert.IsNotNull(bookingToDelete, "Booking should exist before deletion");

            await _repository.DeleteBooking(booking.Id);
            var DeletetBooking = await _repository.GetById(booking.Id);

            // Assert
            Assert.IsNull(DeletetBooking, "Booking should not exist after deletion.");
        }

        [TestMethod]
        public async Task CreateBooking_ShouldCreatABookingWithAnExistingGuestId_ShouldReturnCorrectValueOfNewBooking()
        {
            //Arrange
            var existingBookings = _context.Bookings.ToList();
            _context.RemoveRange(existingBookings);
            await _context.SaveChangesAsync();

            var getBooking = MockDataSetup.GetBookings()[0];

            //Act
            var addBooking = _repository.CreateBooking(getBooking);
            var result = _context.Bookings.FirstOrDefault();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.GuestId);
            Assert.AreEqual(new DateOnly(2025, 6, 10), result.FromDate);
            Assert.AreEqual(new DateOnly(2025, 6, 11), result.ToDate);
            Assert.AreEqual(1, result.NumberOfAdults);
            Assert.AreEqual(0, result.NumberOfChildren);
            Assert.AreEqual(false, result.Breakfast);
        }
    }
}
