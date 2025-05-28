using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using Gotlandsrussen.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGotlandsrussenTESTS.Tests
{
    [TestClass]
    public class BookingTests
    {

        [TestMethod]
        public void GetById_GetABookingById_ReturnsCorrectBookingAndNotNull()
        {
            //arrange
            var mockRepo = new Mock<IBookingRepository>();

            mockRepo.Setup(gbi => gbi
            .GetById(1))
                .ReturnsAsync(new Booking { Id = 1 });

            var repo = mockRepo.Object;

            //act
            var result = repo.GetById(1);

            //assert
            //Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }
    }
}
