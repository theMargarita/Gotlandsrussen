using Gotlandsrussen.Controllers;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussenTESTS.TestSetup;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;

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
        public async Task GetBookingById_ExistingId_ReturnsBookingWithCorrectData()
        {
            // Arrange
            var mockBooking = MockDataSetup.GetBookings()[0];

            _mockBookingRepository?
                .Setup(repo => repo.GetById(mockBooking.Id))
                .ReturnsAsync(mockBooking);

            // Act
            var result = await _controller.GetBookingById(mockBooking.Id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult");

            var returnedBooking = okResult.Value as Booking;
            Assert.IsNotNull(returnedBooking, "Expected Booking object as Value");

            Assert.AreEqual(mockBooking.Id, returnedBooking.Id);
        }

        [TestMethod]
        public async Task GetBookingById_ShouldReturnNotFound_WhenBookingDoesNotExist()
        {
            // Arrange
            _mockBookingRepository?
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await _controller.GetBookingById(99);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            string? resultString = notFoundResult.Value?.ToString();
            Assert.IsTrue(resultString!.Contains("Booking not found"));
        }

        [TestMethod]
        public async Task CreateGuest_CreateValidGuest_ReturnsCreatedGuest()
        {
            // Arrange
            var request = new CreateGuestRequestDto
            {
                FirstName = "Lisa",
                LastName = "Lindberg",
                Email = "lisa@example.com",
                Phone = "0701234567"
            };

            var mockGuests = MockDataSetup.GetGuests();

            // Gets the next available guest ID in mockData.
            int nextId = mockGuests.Max(g => g.Id) + 1;
            _mockGuestRepository?
                .Setup(repo => repo.AddGuest(It.IsAny<Guest>()))
                .ReturnsAsync((Guest guest) =>
                {
                    guest.Id = nextId;
                    return guest;
                });

            // Act
            var result = await _controller.CreateGuest(request);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult, "Expected a CreatedAtActionResult");

            var returnedGuest = createdResult.Value as Guest;
            Assert.IsNotNull(returnedGuest, "Expected a Guest");

            Assert.AreEqual(nextId, returnedGuest.Id); 
            Assert.AreEqual(request.FirstName, returnedGuest.FirstName);
            Assert.AreEqual(request.LastName, returnedGuest.LastName);
            Assert.AreEqual(request.Email, returnedGuest.Email);
            Assert.AreEqual(request.Phone, returnedGuest.Phone);

            _mockGuestRepository?.Verify(repo => repo.AddGuest(It.IsAny<Guest>()), Times.Once);
        }




    }
}
