using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests;

public class AuthUsecasesTests
{



    [Test]
    public void CheckCredentials_ValidMailAndPassword()
    {
        //setup
        using (var moqDB = new DataContext())
        {
            var company = new Company()
            {
                CompanyId = new Guid(),
                CompanyName = "gehmalbierholn"
            };
            var user = new User()
            {
                MailAddress = "test@mail.de",
                Password = "testpw",
                FirstName = "testF",
                LastName = "testL",
                Company = company
            };
            moqDB.Companies.Add(company);
            moqDB.Users.Add(user);
            moqDB.SaveChanges();

            //arrange
            var logger = new Mock<ILogger<AuthUsecases>>();
            var subject = new AuthUsecases(logger.Object,moqDB);

            var mail = "test@mail.de";
            var pw = "testpw";

            //act
            var result = subject.checkCredentials(mail,pw);


            //assert
            Assert.That(result);

        }
    }
   
}