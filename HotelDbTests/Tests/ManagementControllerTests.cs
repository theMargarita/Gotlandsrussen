using Gotlandsrussen.Controllers;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
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
        private ManagementController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            _controller = new ManagementController(_mockBookingRepository.Object, _mockRoomRepository.Object);
        }

        // Börja test här

        [TestMethod]
        public async Task GetAllFutureBookings_ReturnsOkWithBookings()
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
        public async Task GetAllFutureBookings_ReturnsOkWithEmptyList()
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
        public async Task GetBookingsGroupedByWeek_ReturnsOkWithExpectedBookings()
        {
            //Arrange
            var expectedBookings = MockDataSetup.GetBookingDtos();
            _mockBookingRepository.Setup(repo => repo.GetAllFutureBookings()).ReturnsAsync(expectedBookings);

            //Act
            var result = await _controller.GetBookingsGroupedByWeek();
            var okResult = result.Result as OkObjectResult; //result gets the actionresult - asokobjectresult returns like a Ok(something); return


            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBookingDates = okResult.Value as ICollection<BookingDto>; //has the values from the bookingList of expectedBookings & return collection of bookingdto else null 
            Assert.IsNotNull(returnedBookingDates);
            Assert.AreEqual(3, returnedBookingDates.Count);


            var expectedFirst = expectedBookings.First();
            var bookingList = returnedBookingDates.ToList();

            Assert.AreEqual(1, bookingList[0].Id);
            Assert.AreEqual(expectedFirst.BookedFromDate, bookingList[0].BookedFromDate);
            Assert.AreEqual(expectedFirst.BookedToDate, bookingList[0].BookedToDate);

            _mockBookingRepository.Verify(repo => repo.GetAllFutureBookings(), Times.Once);
        }
    }
}

