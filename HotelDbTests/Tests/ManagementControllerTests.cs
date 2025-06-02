using Gotlandsrussen.Controllers;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using HotelGotlandsrussen.Models.DTOs;
using HotelGotlandsrussenTESTS.TestSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelGotlandsrussenTESTS.Tests
{
    [TestClass]
    public class ManagementControllerTests
    {
        private Mock<IBookingRepository>? _mockBookingRepository;
        private Mock<IRoomRepository>? _mockRoomRepository;
        private ManagementController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            _controller = new ManagementController(_mockBookingRepository.Object, _mockRoomRepository.Object);
        }

        // Börja test här

        [TestMethod]
        public async Task GetAllFutureBookings_CallingMethod_ReturnsOkWithBookings()
        {
            //Arrange
            var bookings = MockDataSetup.GetBookingDtos();

            _mockBookingRepository.Setup(repo => repo.GetAllFutureBookings())
                .ReturnsAsync(bookings);

            //Act
            var result = await _controller.GetAllFutureBookings();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBookings = okResult.Value as ICollection<BookingDto>;
            Assert.IsNotNull(returnedBookings);
            Assert.AreEqual(3, returnedBookings.Count);

            var bookingList = returnedBookings.ToList();
            Assert.AreEqual(1, bookingList[0].Id);
            Assert.AreEqual("Andersson, Alice", bookingList[0].GuestName);
            Assert.AreEqual(new DateOnly(2025, 6, 10), bookingList[0].BookedFromDate);

            _mockBookingRepository.Verify(repo => repo.GetAllFutureBookings(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllFutureBookings_MethodReturnsEmptyList_ReturnsOk()
        {
            //Arrange
            var emptyBookingList = new List<BookingDto>();

            _mockBookingRepository.Setup(repo => repo.GetAllFutureBookings())
                .ReturnsAsync(emptyBookingList);

            //Act
            var result = await _controller.GetAllFutureBookings();

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBookings = okResult.Value as ICollection<BookingDto>;
            Assert.IsNotNull(returnedBookings);
            Assert.AreEqual(0, returnedBookings.Count);

            _mockBookingRepository.Verify(repo => repo.GetAllFutureBookings(), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPrice_IfBookingNotExists_ReturnsNotFouund()
        {
            //Arrange
            var bookings = MockDataSetup.GetBookings();

            _mockBookingRepository.Setup(repo => repo.GetById(100))
                .ReturnsAsync((Booking)null);

            //Act
            var result = await _controller.GetTotalPrice(100);

            //Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        [DataRow(1, 4200.0, "Complex booking")]
        [DataRow(2, 6250.0, "Normal booking")]
        [DataRow(3, 6000.0, "Normal booking without breakfast")]
        public async Task GetTotalPrice_DifferentBookingScenarios_ReturnsCorrectTotalPrice
            (int bookingId, double expectedTotalPrice, string message)
        {
            //Arrange
            var booking = MockDataSetup.GetBookingsWithRelations(bookingId);

            _mockBookingRepository.Setup(repo => repo.GetById(bookingId))
                .ReturnsAsync(booking);

            //Act
            var result = await _controller.GetTotalPrice(bookingId);

            //Assert
            var okResult = result.Result as OkObjectResult;
            var summary = okResult.Value as TotalPriceDto;

            Assert.IsNotNull(summary, message);
            Assert.AreEqual((decimal)expectedTotalPrice, summary.TotalPrice, message);
        }

        [TestMethod]
        public async Task UpdateBooking_UpdatesExistingBooking_ReturnsOkWithUpdatedBooking()
        {
            // Arrange
            var existingBooking = MockDataSetup.GetBookings()[0];

            var mockBookingDto = MockDataSetup.GetUpdateBookingDtos()[0];

            var updatedBooking = new Booking
            {
                Id = existingBooking.Id,
                GuestId = existingBooking.GuestId,
                FromDate = mockBookingDto.FromDate,
                ToDate = mockBookingDto.ToDate,
                NumberOfAdults = mockBookingDto.NumberOfAdults,
                NumberOfChildren = mockBookingDto.NumberOfChildren,
                Breakfast = mockBookingDto.Breakfast
            };

            _mockBookingRepository
                .Setup(repo => repo.UpdateBookingAsync(mockBookingDto))
                .ReturnsAsync(updatedBooking);

            // Act
            var result = await _controller.UpdateBooking(mockBookingDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected a OkObjectResult");

            var returned = okResult.Value as Booking;
            Assert.IsNotNull(returned, "Expected a Booking");

            Assert.AreEqual(mockBookingDto.Id, returned.Id);
            Assert.AreEqual(mockBookingDto.FromDate, returned.FromDate);
            Assert.AreEqual(mockBookingDto.ToDate, returned.ToDate);
            Assert.AreEqual(mockBookingDto.NumberOfAdults, returned.NumberOfAdults);
            Assert.AreEqual(mockBookingDto.NumberOfChildren, returned.NumberOfChildren);
            Assert.AreEqual(mockBookingDto.Breakfast, returned.Breakfast);

            _mockBookingRepository.Verify(repo => repo.UpdateBookingAsync(mockBookingDto), Times.Once);
        }

        [TestMethod]
        public async Task UpdateBooking_DoesNotUpdateIfBookingDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var mockBookingDto = MockDataSetup.GetUpdateBookingDtos()[1];

            _mockBookingRepository
                .Setup(repo => repo.UpdateBookingAsync(It.Is<UpdateBookingDto>(dto =>
                    dto.Id == mockBookingDto.Id)))
                .ReturnsAsync((Booking?)null); // Simulerar att bokningen inte finns

            // Act
            var result = await _controller.UpdateBooking(mockBookingDto);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult, "Expected NotFoundObjectResult");

            string? resultString = notFoundResult.Value?.ToString();
            Assert.IsTrue(resultString!.Contains("Booking not found")); // Change True => False for debugg
            
            _mockBookingRepository.Verify(repo => repo.UpdateBookingAsync(mockBookingDto), Times.Once);
        }

    }

}
