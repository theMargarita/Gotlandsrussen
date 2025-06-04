using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Repositories;
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

    }
}
