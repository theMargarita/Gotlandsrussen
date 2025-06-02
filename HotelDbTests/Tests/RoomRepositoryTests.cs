using Gotlandsrussen.Data;
using Gotlandsrussen.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGotlandsrussenTESTS.Tests
{
    [TestClass]
    public class RoomRepositoryTests
    {
        private RoomRepository _repository;
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

            _repository = new RoomRepository(_context);
        }

        //Skriv tester här nedan
        //OBS: Inför varje nytt test skapas en ny databas med samma seed data som vår vanliga databas. Alltså samma HotelDbContext.
        //Det är en kopia av databasen som endast ligger i minnet.
        //Varje nytt test ger en ny fräsch DbContext. Det sparas alltså inget mellan testerna.

    }
}
