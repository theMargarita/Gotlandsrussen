using Gotlandsrussen.Controllers;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussenTESTS.TestSetup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections;

namespace HotelGotlandsrussenTESTS.Tests
{
    

    [TestClass]
    public class GuestControllerTest
    {
        private Mock<IBookingRepository>? _mockBookingRepository;
        private Mock<IRoomRepository>? _mockRoomRepository;
        private Mock<IGuestRepository>? _mockGuestRepository;
        private GuestController _controller;

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
        public async Task GetAvailableRooms_StartDateBeforeToday_ReturnsBadRequest()
        {
            // Arrange
            var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2));
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2));
        
            // Act
            var result = await _controller.GetAvailableRooms(startDate, endDate);
        
            // Assert
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public async Task GetAvailableRooms_StartDateEqualToEndDate_ReturnsBadRequest()
        {
            // Arrange
            var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3));
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3));

            // Act
            var result = await _controller.GetAvailableRooms(startDate, endDate);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public async Task GetAvailableRooms_StartDateAfterEndDate_ReturnsBadRequest()
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2));

            var result = await _controller.GetAvailableRooms(startDate, endDate);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public async Task GetAvailableRooms_OkDates_ReturnsOkWithRooms()
        {
            // Arrange
            var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));

            var rooms = MockDataSetup.GetRoomDtos();

            _mockRoomRepository.Setup(r => r.GetAvailableRoomsAsync(startDate, endDate))
                .ReturnsAsync(rooms);

            // Act
            var result = await _controller.GetAvailableRooms(startDate, endDate);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedRooms = okResult.Value as ICollection<RoomDto>;
            Assert.IsNotNull(returnedRooms);
            Assert.AreEqual(2, returnedRooms.Count);

            _mockRoomRepository.Verify(r => r.GetAvailableRoomsAsync(startDate, endDate), Times.Once);
        }

        [TestMethod]
        public async Task GetAvailableRooms_NoRoomsAvailable_ReturnsEmptyListWithOk()
        {
            // Arrange
            var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3));

            _mockRoomRepository.Setup(r => r.GetAvailableRoomsAsync(startDate, endDate))
                .ReturnsAsync(new List<RoomDto>());

            // Act
            var result = await _controller.GetAvailableRooms(startDate, endDate);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedRooms = okResult.Value as ICollection<RoomDto>;
            Assert.IsNotNull(returnedRooms);
            Assert.AreEqual(0, returnedRooms.Count);
        }
    }
}
