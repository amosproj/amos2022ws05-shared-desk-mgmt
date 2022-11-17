using Deskstar.DataAccess;
using Deskstar.Usecases;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class BookingUsecasesTest
{
    [Test]
    public void CheckGetRecentBookings_ValidMailAddress()
    {
        //setup
        using (var mogDB = new DataContext())
        {
            mogDB.SaveChanges();

            //arrange
            var logger = new Mock<ILogger<BookingUsecases>>();
            var subject = new BookingUsecases(logger.Object, mogDB);

            //act
            var address = "testmail@exmaple.com";
            var result = subject.GetRecentBookings(address);


            //assert
            Assert.That(result,Is.Not.Null);
        }
    }
}