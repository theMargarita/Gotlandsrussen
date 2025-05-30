using Gotlandsrussen.Controllers;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelGotlandsrussenTESTS.Tests
{
    [TestClass]
    public class ManagementControllerTests
    {
        [TestMethod]
        
        public async Task GetAllFutureBookings_ReturnsOkWithBookings()
        {
            //Arrange
            var mockBookingRepo = new Mock<IBookingRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();

            var controller = new ManagementController(mockBookingRepo.Object, mockRoomRepo.Object);



        }
        
        private Mock<IBookingRepository>? _mockBookingRepository;
        private Mock<IRoomRepository>? _mockRoomRepository;

        [TestInitialize]
        public void Setup()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            ManagementController _controller = new ManagementController(_mockBookingRepository.Object, _mockRoomRepository.Object);
        }

        [HttpGet("GetAllFutureBookings")] // lina
        public async Task<ActionResult<ICollection<BookingDto>>> GetAllFutureBookings()
        {
            return Ok(await _bookingRepository.GetAllFutureBookings());
        }
    }
        // Börja test här
    } 
}
