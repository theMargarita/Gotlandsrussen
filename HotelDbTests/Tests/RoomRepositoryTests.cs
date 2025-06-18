using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussenTESTS.TestSetup;
using Microsoft.EntityFrameworkCore;

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

        [TestMethod]
        public async Task GetAvailableRoomByDateAndGuests_GetsTheRightAmmountOfAvailableRooms_ReturnsAvailableRooms()
        {
            // Arrange
            var fromDate = new DateOnly(2025, 6, 10);
            var toDate = new DateOnly(2025, 6, 15);
            int adults = 2;
            int children = 1;

            // Act
            var result = await _repository.GetAvailableRoomByDateAndGuests(fromDate, toDate, adults, children);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(15, result.Count); // There is 15 rooms available in the SeedData.
        }

        [TestMethod]
        public async Task GetAvailableRoomsAsync_IfNoBookings_ReturnsAllRooms()
        {
            //Arrange
            var existingBookings = _context.Bookings.ToList();
            _context.Bookings.RemoveRange(existingBookings);
            await _context.SaveChangesAsync();  

            var existingRooms = _context.Rooms.ToList();
            _context.Rooms.RemoveRange(existingRooms);
            await _context.SaveChangesAsync();  

            var rooms = MockDataSetup.GetRooms();
            _context.Rooms.AddRange(rooms);
            await _context.SaveChangesAsync();  

            var startDate = new DateOnly(2025, 05, 15);
            var endDate = new DateOnly(2025, 05, 17);

            //Act
            var result = await _repository.GetAvailableRoomsAsync(startDate, endDate);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public async Task GetAvailableRoomsAsync_WhenARoomIsBooked_DoesNotReturnTheBookedRoom()
        {
            //Arrange
            var startDate = new DateOnly(2025, 06, 10);
            var endDate = new DateOnly(2025, 06, 11);

            //Act
            var result = await _repository.GetAvailableRoomsAsync(startDate, endDate);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(23, result.Count);
        }

        [TestMethod]
        public async Task GetAvailableRoomsAsync_WhenRoomIsBookedAfterCallingMethod_ReturnsOneLessRoom()
        {
            // Arrange
            var startDate = new DateOnly(2025, 02, 10);
            var endDate = new DateOnly(2025, 02, 15);

            var testRoomType = new RoomType { Name = "Testtyp" };
            _context.RoomTypes.Add(testRoomType);
            await _context.SaveChangesAsync();

            var testRoom = new Room { Name = "Testrum", RoomType = testRoomType};
            _context.Rooms.Add(testRoom);
            await _context.SaveChangesAsync();

            var initialResult = await _repository.GetAvailableRoomsAsync(startDate, endDate);
            var initialCount = initialResult.Count;

            var booking = new Booking
            {
                FromDate = startDate,
                ToDate = endDate,
                IsCancelled = false
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var bookingRoom = new BookingRoom
            {
                BookingId = booking.Id,
                RoomId = testRoom.Id
            };
            _context.BookingRooms.Add(bookingRoom);
            await _context.SaveChangesAsync();

            // Act
            var afterBookingResult = await _repository.GetAvailableRoomsAsync(startDate, endDate);

            // Assert
            Assert.IsNotNull(afterBookingResult);
            Assert.AreEqual(initialCount - 1, afterBookingResult.Count);
        }

        [TestMethod]
        public async Task GetAvailableRoomsAsync_WhenBookingIsCancelled_RoomIsAvailable()
        {
            // Arrange
            var booking = _context.Bookings.ToList()[0];
            booking.IsCancelled = false;
            await _context.SaveChangesAsync();

            var initialResult = await _repository.GetAvailableRoomsAsync(booking.FromDate, booking.ToDate);
            var initialCount = initialResult.Count();

            booking.IsCancelled = true;
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAvailableRoomsAsync(booking.FromDate, booking.ToDate);

            // Assert
            Assert.AreEqual(initialCount + 1, result.Count);
        }

        [TestMethod]
        public async Task GetAvailableRoomsAsync_WhenCallingMethod_ReturnsCorrectDto()
        {
            var testRoomType = new RoomType { Name = "Testrumstyp", NumberOfBeds = 2, PricePerNight = 900 };
            var testRoom = new Room { Name = "Testrum", RoomType = testRoomType };
            _context.RoomTypes.Add(testRoomType);
            _context.Rooms.Add(testRoom);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAvailableRoomsAsync(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 3));
            var dto = result.FirstOrDefault(r => r.RoomName == "Testrum");

            Assert.IsNotNull(dto);
            Assert.AreEqual(testRoom.Id, dto.Id);
            Assert.AreEqual("Testrum", dto.RoomName);
            Assert.AreEqual("Testrumstyp", dto.RoomTypeName);
            Assert.AreEqual(2, dto.NumberOfBeds);
            Assert.AreEqual(900, dto.PricePerNight);
        }

        [TestMethod]
        public async Task GetRoomById_WhenCallingMethodd_ReturnsCorrectId()
        {
            //Arrange
            var roomId = 1;

            //Act
            var result = _repository.GetRoomById(roomId);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod] 
        public async Task GetCleanRooms_ShouldReturnOnlyCleanedRooms()
        {
            // Arrange
            var existingRooms = _context.Rooms.ToList();
            _context.Rooms.RemoveRange(existingRooms);
            await _context.SaveChangesAsync();

            // Lägg till testdata
            var roomType = new RoomType { Name = "Single", NumberOfBeds = 1, PricePerNight = 500m };
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();

            var rooms = new List<Room>
            {
                new Room { Name = "101", RoomTypeId = roomType.Id, IsCleaned = true },
                new Room { Name = "102", RoomTypeId = roomType.Id, IsCleaned = false },
                new Room { Name = "103", RoomTypeId = roomType.Id, IsCleaned = true }
            };

            _context.Rooms.AddRange(rooms);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetCleanRooms();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); 
            Assert.IsTrue(result.All(r => r.RoomName == "101" || r.RoomName == "103"));
        }


    }
}

