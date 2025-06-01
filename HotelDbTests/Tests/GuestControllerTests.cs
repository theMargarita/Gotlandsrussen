using Gotlandsrussen.Controllers;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussenTESTS.TestSetup;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace HotelGotlandsrussenTESTS.Tests
{


    [TestClass]
    public class GuestControllerTest
    {
        private Mock<IBookingRepository>? _mockBookingRepository;
        private Mock<IRoomRepository>? _mockRoomRepository;
        private Mock<IGuestRepository>? _mockGuestRepository;
        private GuestController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockGuestRepository = new Mock<IGuestRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            _controller = new GuestController(_mockBookingRepository.Object, _mockGuestRepository.Object, _mockRoomRepository.Object);

        }

        // Börja test här

        [TestMethod]
        public async Task GetBookingById_ShouldReturnCorrectBooking_ReturnsTrue()
        {
            // Arrange
            var mockData = MockDataSetup.GetBookings();
            var mockDataObject = MockDataSetup.GetBookings()[0];

            // Setup the mock repository to return the mock data when GetById is called
            _mockBookingRepository.Setup(gb => gb.GetById(1))
                .ReturnsAsync(mockDataObject);

            // Act
            var result = await _controller.GetBookingById(1);
            var resultData = result.Value;

            // Assert
            //var okReult = result.Result as OkObjectResult;

            Assert.IsNotNull(resultData, "Result is not OkObjectResult");
            Assert.IsNotNull(result, "result är null");
            Assert.IsNotNull(result.Value, "result.Value är null");
            Assert.AreEqual(mockData[0].Id, resultData.Id);
            //Assert.AreEqual(mockData[0].Id, result.Value.Id);

        }


    }
}
