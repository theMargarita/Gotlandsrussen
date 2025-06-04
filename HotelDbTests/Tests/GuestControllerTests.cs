using Gotlandsrussen.Controllers;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussenTESTS.TestSetup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using System.Collections;

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


        // BookingId is not valid 
        [TestMethod]
        public async Task AddBreakfast_ShouldReturnNotFound_WhenBookingIdIsInvalid()
        {
            // Arrange
            var request = new AddBreakfastRequestDto { BookingId = 0 };
            _mockBookingRepository
                .Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await _controller.AddBreakfast(request);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("BookingId was not found", notFoundResult.Value);
        }

        // Breakfast already added
        [TestMethod]
        public async Task AddBreakfast_ShouldReturnConflict_WhenBreakfastAlreadyAdded()
        {
            // Arrange
            var request = new AddBreakfastRequestDto { BookingId = 1 };
            var booking = new Booking { Id = 1, Breakfast = true };

            _mockBookingRepository
                .Setup(r => r.GetById(1))
                .ReturnsAsync(booking);

            // Act
            var result = await _controller.AddBreakfast(request);

            // Assert
            var conflict = result.Result as ConflictObjectResult;
            Assert.IsNotNull(conflict);
            Assert.AreEqual(409, conflict.StatusCode);
            Assert.AreEqual("Breakfast is already added to the booking.", conflict.Value);
        }

        // When breakfast is added correct
        [TestMethod]
        public async Task AddBreakfast_ShouldReturnOk_WhenBreakfastIsSuccessfullyAdded()
        {
            // Arrange
            var request = new AddBreakfastRequestDto { BookingId = 3 };
            var booking = new Booking { Id = 3, Breakfast = false };

            _mockBookingRepository
                .Setup(r => r.GetById(3))
                .ReturnsAsync(booking);

            _mockBookingRepository
                .Setup(r => r.Update(It.IsAny<Booking>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddBreakfast(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var dto = okResult.Value as AddBreakfastDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(3, dto.BookingId);
            Assert.IsTrue(dto.Breakfast);
            Assert.AreEqual("Breakfast has been added to the booking.", dto.Message);

            _mockBookingRepository.Verify(r => r.Update(It.Is<Booking>(b => b.Id == 3 && b.Breakfast == true)), Times.Once);
        }


        // Return correct list of guests
        [TestMethod]
        public async Task GetAllGuests_ShouldReturnListOfGuests_WhenGuestsExist()
        {
            // Arrange
            var mockGuests = MockDataSetup.GetGuests();
            _mockGuestRepository
                .Setup(repo => repo.GetAllGuests())
                .ReturnsAsync(mockGuests);

            // Act
            var result = await _controller.GetAllGuests();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count); // Antal från MockDataSetup
            Assert.AreEqual("Alice", result.First().FirstName);

            _mockGuestRepository.Verify(repo => repo.GetAllGuests(), Times.Once);
        }

        // Return empty list of guests
        [TestMethod]
        public async Task GetAllGuests_ShouldReturnEmptyList_WhenNoGuestsExist()
        {
            // Arrange
            _mockGuestRepository
                .Setup(repo => repo.GetAllGuests())
                .ReturnsAsync(new List<Guest>());

            // Act
            var result = await _controller.GetAllGuests();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);

            _mockGuestRepository.Verify(repo => repo.GetAllGuests(), Times.Once);
        }

    }
}
