using Gotlandsrussen.Controllers;
using Gotlandsrussen.Repositories;
using Moq;

namespace HotelGotlandsrussenTESTS.Tests
{
    [TestClass]
    public class ManagementControllerTests
    {
        
        private Mock<IBookingRepository>? _mockBookingRepository;
        private Mock<IRoomRepository>? _mockRoomRepository;

        [TestInitialize]
        public void Setup()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            ManagementController _controller = new ManagementController(_mockBookingRepository.Object, _mockRoomRepository.Object);
        }

        // Börja test här
    } 
}
