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

        //I testet nedan, för GetAllGuests, har vi 15 gäster inlagda i seed datan, vilket jag matar in i min Assert.

        [TestMethod]
        public async Task GetAllGuests_WhenCallingMethod_MethodReturnsAllGuests()
        {
            //Arrange
            var allGuests = await _context.Guests.ToListAsync();

            //Act
            var result = await _repository.GetAllGuests();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(15, result.Count);
        }

        
    }
}
