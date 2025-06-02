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
            _context.Database.EnsureCreated();

            _repository = new BookingRepository(_context);
        }

        //Skriv tester här nedan

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

    }
}
