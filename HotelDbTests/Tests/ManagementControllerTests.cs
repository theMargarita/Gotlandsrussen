using Gotlandsrussen.Controllers;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using GotlandsrussenAPI.Repositories;
using HotelGotlandsrussen.Models.DTOs;
using HotelGotlandsrussenLIBRARY.Models.DTOs;
using HotelGotlandsrussenTESTS.TestSetup;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace HotelGotlandsrussenTESTS.Tests
{
    [TestClass]
    public class ManagementControllerTests
    {
        private Mock<IBookingRepository>? _mockBookingRepository;
        private Mock<IRoomRepository>? _mockRoomRepository;
        private Mock<IGuestRepository>? _mockGuestRepository;
        private Mock<IBookingRoomRepository>? _mockBookingRoomRepository;
        private ManagementController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            _mockGuestRepository = new Mock<IGuestRepository>();
            _mockBookingRoomRepository = new Mock<IBookingRoomRepository>();

            _controller = new ManagementController(_mockBookingRepository.Object, _mockRoomRepository.Object, _mockGuestRepository.Object, _mockBookingRoomRepository.Object);
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
            Assert.AreEqual(5, returnedBookings.Count);

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

        [TestMethod]
        public async Task GetBookingsGroupedByWeek_ReturnsOkWithExpectedBookings()
        {
            //Arrange
            var expectedBookings = MockDataSetup.GetBookingDtos();
            _mockBookingRepository.Setup(repo => repo.GetAllFutureBookings()).ReturnsAsync(expectedBookings);

            //Act
            var result = await _controller.GetBookingsGroupedByWeek();
            var okResult = result.Result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBookingDates = okResult.Value as ICollection<YearWeekBookingsDto>;
            Assert.IsNotNull(returnedBookingDates);
            Assert.AreEqual(3, returnedBookingDates.Count);

            var expectedFirst = returnedBookingDates.First();
            Assert.AreEqual(2025, expectedFirst.Year); //chekcs year
            Assert.IsTrue(expectedFirst.Week > 0); //checks week
            Assert.IsTrue(expectedFirst.Bookings.Count > 0); //checks if there exists bookings
            _mockBookingRepository.Verify(repo => repo.GetAllFutureBookings(), Times.Once);
        }

        [TestMethod]
        public void GetAvailableRoomByDateAndGuests_ReturnsExpectedAvailableRooms()
        {
            //Arrange
            var bookings = MockDataSetup.GetBookingDtos();
            _mockBookingRepository?.Setup(repo => repo.GetAllFutureBookings()).ReturnsAsync(bookings);
            var expectedRooms = MockDataSetup.GetExpectedAvailableRoomsDto();

            var fromDate = new DateOnly(2025, 6, 10);
            var toDate = new DateOnly(2025, 6, 11);
            var adults = 2;
            var children = 0;

            _mockRoomRepository?.Setup(repo => repo.GetAvailableRoomByDateAndGuests(fromDate, toDate, adults, children)).ReturnsAsync(expectedRooms);

            //Act
            var result = _controller?.GetAvailableRoomByDateAndGuests(fromDate, toDate, adults, children).Result;

            //Assert
            var okResult = result?.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedRooms = okResult.Value as ICollection<RoomDto>;
            Assert.IsNotNull(returnedRooms);
            Assert.AreEqual(2, returnedRooms.Count);

            var room = returnedRooms.First();
            Assert.AreEqual(1, room.Id);
            Assert.AreEqual("101", room.RoomName);
            Assert.AreEqual("Single", room.RoomTypeName);
            Assert.AreEqual(1, room.NumberOfBeds);
            Assert.AreEqual(500m, room.PricePerNight); 

            _mockRoomRepository?.Verify(repo => repo.GetAvailableRoomByDateAndGuests(fromDate, toDate, adults, children), Times.Once);
        }

        [TestMethod]
        public void CreateBooking_WithInput_ReturnsCreatedBooking()
        {
            //Arrange
            var roomId = new List<int> { 1, 2 }; //inparametern för createbooking
            int guestId = 1;
            var fromDate = new DateOnly(2025, 06, 10);
            var toDate = new DateOnly(2025, 06, 11);
            int adults = 1;
            int children = 0;
            var breakfast = false;

            var getGuests = MockDataSetup.GetGuests();
            var expectedRooms = MockDataSetup.GetExpectedAvailableRoomsDto();

            Booking booking = new Booking
            {
                GuestId = guestId,
                FromDate = fromDate,
                ToDate = toDate,
                NumberOfAdults = adults,
                NumberOfChildren = children,
                Breakfast = breakfast
            };

            _mockGuestRepository?.Setup(repo => repo.GetAllGuests()).ReturnsAsync(getGuests);

            _mockRoomRepository?.Setup(repo => repo.GetAvailableRoomsAsync(fromDate, toDate)).ReturnsAsync(expectedRooms);

            _mockBookingRepository?.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(booking);

            _mockBookingRoomRepository?.Setup(repo => repo.AddBookingRooms(It.IsAny<BookingRoom>()));

            //Act
            var result = _controller?.CreateBooking(roomId, guestId, fromDate, toDate, adults, children, breakfast).Result;

            //Assert
            var okResult = result?.Result as CreatedAtActionResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.AreEqual(201, okResult.StatusCode);

            var createdResult = okResult.Value as CreateBookingDto;
            Assert.IsNotNull(createdResult);

            Assert.AreEqual(booking.Id, createdResult.BookingId);
            Assert.AreEqual(guestId, createdResult.GuestId);
            Assert.AreEqual(fromDate, createdResult.FromDate);
            Assert.AreEqual(toDate, createdResult.ToDate);
            Assert.AreEqual(adults, createdResult.NumberOfAdults);
            Assert.AreEqual(children, createdResult.NumberOfChildren);
            Assert.AreEqual(breakfast, booking.Breakfast);

            //check roomids 
            Assert.AreEqual(2, createdResult.RoomIds?.Count);
            Assert.IsTrue(createdResult.RoomIds?.Contains(1));
            Assert.IsTrue(createdResult.RoomIds?.Contains(2));

            _mockBookingRepository?.Verify(repo => repo.CreateBooking(It.IsAny<Booking>()), Times.Once);

            _mockBookingRoomRepository?.Verify(repo => repo.AddBookingRooms(It.IsAny<BookingRoom>()), Times.Exactly(roomId.Count));
        }

        [TestMethod] //dont forget this
        public void GetBookingHistory_WhenDataExists_ReturnsExpectedBookingList()
        {
            //Arrange
            var booking = MockDataSetup.GetBookingDtos(); //mock data database
            _mockBookingRepository?.Setup(repo => repo.GetBookingHistory()).ReturnsAsync(booking);


            //Act
            var result = _controller.GetBookingHistory().Result;

            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBookings = okResult.Value as ICollection<BookingDto>;
            Assert.IsNotNull(returnedBookings);
            Assert.AreEqual(5, returnedBookings.Count); //should be 5

            var bookingList = returnedBookings.ToList();
            Assert.AreEqual(5, bookingList[4].Id);
            Assert.AreEqual(new DateOnly(2025, 06, 01), bookingList[4].BookedFromDate);

            _mockBookingRepository?.Verify(repo => repo.GetBookingHistory(), Times.Once);

        }

        [TestMethod]
        public async Task GetBookingsGroupedByMonth_ShouldReturnGroupedBookings_WhenDataExists()
        {
            // Arrange
            var mockBookings = MockDataSetup.GetBookingDtos(); // Bokningar med olika månader
            _mockBookingRepository
                .Setup(repo => repo.GetAllFutureBookings())
                .ReturnsAsync(mockBookings);

            // Act
            var result = await _controller.GetBookingsGroupedByMonth();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var grouped = okResult.Value as ICollection<YearMonthBookingsDto>;
            Assert.IsNotNull(grouped);
            Assert.IsTrue(grouped.Count >= 1);

            var firstGroup = grouped.First();
            Assert.IsTrue(firstGroup.Year >= 2025);
            Assert.IsTrue(firstGroup.Month >= 1 && firstGroup.Month <= 12);
            Assert.IsTrue(firstGroup.Bookings.Count > 0);

            _mockBookingRepository.Verify(repo => repo.GetAllFutureBookings(), Times.Once);
        }

        [TestMethod]
        public async Task GetBookingsGroupedByMonth_ShouldReturnEmptyList_WhenNoBookingsExist()
        {
            // Arrange
            _mockBookingRepository
                .Setup(repo => repo.GetAllFutureBookings())
                .ReturnsAsync(new List<BookingDto>());

            // Act
            var result = await _controller.GetBookingsGroupedByMonth();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var grouped = okResult.Value as ICollection<YearMonthBookingsDto>;
            Assert.IsNotNull(grouped);
            Assert.AreEqual(0, grouped.Count);

            _mockBookingRepository.Verify(repo => repo.GetAllFutureBookings(), Times.Once);
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
        public async Task DeleteBooking_DeletesExistingBooking_ReturnsOk()
        {
            // Arrange
            var excistingBooking = MockDataSetup.GetBookings()[0];

            _mockBookingRepository
            .Setup(repo => repo.DeleteBooking(excistingBooking.Id))
            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBooking(excistingBooking.Id);
            var deletedBooking = result as NoContentResult;

            // Assert
            Assert.AreEqual(204, deletedBooking.StatusCode);
        }

        [TestMethod]
        public async Task GetCleanRooms_ReturnsOkWithOnlyCleanedRooms()
        {
            // Arrange
            var mockRooms = new List<RoomDto>
            {
                new RoomDto { Id = 1, RoomName = "101", RoomTypeName = "Single", NumberOfBeds = 1, PricePerNight = 500 },
                new RoomDto { Id = 3, RoomName = "103", RoomTypeName = "Single", NumberOfBeds = 1, PricePerNight = 500 }
            };

            _mockRoomRepository!
                .Setup(repo => repo.GetCleanRooms())
                .ReturnsAsync(mockRooms);

            // Act
            var result = await _controller!.GetCleanRooms();
            var okResult = result.Result as OkObjectResult; // FIX: cast from .Result

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedRooms = okResult.Value as ICollection<RoomDto>;
            Assert.IsNotNull(returnedRooms);
            Assert.AreEqual(2, returnedRooms.Count);
            Assert.IsTrue(returnedRooms.Any(r => r.RoomName == "101"));
            Assert.IsTrue(returnedRooms.Any(r => r.RoomName == "103"));

            _mockRoomRepository.Verify(repo => repo.GetCleanRooms(), Times.Once);
        }

    }
}