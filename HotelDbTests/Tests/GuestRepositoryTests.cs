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
    public class GuestRepositoryTests
    {
        private GuestRepository _repository;
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

            _repository = new GuestRepository(_context);
        }

        //Skriv tester här nedan.
        //OBS: Inför varje nytt test skapas en ny databas med samma seed data som vår vanliga databas. Alltså samma HotelDbContext.
        //Det är en kopia av databasen som endast ligger i minnet.
        //Varje nytt test ger en ny fräsch DbContext. Det sparas alltså inget mellan testerna.

        //I testet nedan, för GetAllGuests, väljer jag att rensa bort alla gäster ur fejkdatabasen, sen lägga till
        //tre stycken nya från vår MockDataSetup. Detta för att koppla bort beroendet från seed datan. Men jag vet
        //inte om det går att göra för alla test.

        [TestMethod]
        public async Task GetAllGuests_ClearGuestsAndAddingThreeNewGuests_ReturnsThreeGuests()
        {
            //Arrange - jag tar bort seed datan för att isolera mitt test
            var existingGuests = _context.Guests.ToList();
            _context.Guests.RemoveRange(existingGuests);
            await _context.SaveChangesAsync();

            //Lägger till tre nya gäster från MockDataSetup
            var newGuests = MockDataSetup.GetGuests();
            _context.Guests.AddRange(newGuests);
            await _context.SaveChangesAsync();

            //Act - jag kör metoden som ska testas
            var result = await _repository.GetAllGuests();

            //Assert - kontrollerar att svaren är som förväntat
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public async Task AddGuest_ClearGuestsAndAddingANewGuest_ReturnsANewGuest()
        {
            // Arrange
            var existingGuests = _context.Guests.ToList();
            _context.Guests.RemoveRange(existingGuests);
            await _context.SaveChangesAsync();

            var newGuest = MockDataSetup.GetGuests()[0];

            // Act
            var result = await _repository.AddGuest(newGuest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Alice", result.FirstName);
            Assert.AreEqual("Andersson", result.LastName);
            Assert.AreEqual("alice@example.com", result.Email);
            Assert.AreEqual("0701234567", result.Phone);
        }
        [TestMethod]
        public async Task DeleteGuest_DeletesAGuestFromDatabaseById_ReturnsNoGuestWithMatchingId()
        {
            // Arrange
            var existingGuests = _context.Guests
                .FirstOrDefault(g => g.Id == 1);

            _context.Guests.Remove(existingGuests);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllGuests();
            Assert.IsNotNull(result);

            var deletedGuest = result.FirstOrDefault(g => g.Id == 1);

            // Assert
            Assert.IsNull(deletedGuest, "Expected no guest with ID 1 after deletion.");
        }
    }
}
