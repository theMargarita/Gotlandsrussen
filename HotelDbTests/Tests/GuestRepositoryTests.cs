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
            _context.Database.EnsureCreated();

            _repository = new GuestRepository(_context);
        }

        //Skriv tester här nedan

        [TestMethod]
        public async Task GetAllGuests_WhenCallingMethod_MethodReturnsAllGuests()
        {
            //Arrange
            var guests = MockDataSetup.GetGuests();
            _context.AddRange(guests);
            await _context.SaveChangesAsync();

            //Act
            var result = await _repository.GetAllGuests();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }


        //Det är denna metod som ska testas 
        //public async Task<ICollection<Guest>> GetAllGuests()   //Lina
        //{
        //    var getAllGuests = _context.Guests.ToList();
        //    return getAllGuests;
        //}

    }
}
