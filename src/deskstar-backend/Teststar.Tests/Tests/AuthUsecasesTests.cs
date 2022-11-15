using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class AuthUsecasesTests
{
    [Test]
    public void CheckCredentials_ValidMailAndPassword()
    {
        //setup
        using (var mogDB = new DataContext())
        {
            AddOneCompany_AddOneUser(mogDB);

            //arrange
            var logger = new Mock<ILogger<AuthUsecases>>();
            var subject = new AuthUsecases(logger.Object, mogDB);

            var mail = "test@mail.de";
            var pw = "testpw";

            //act
            var result = subject.checkCredentials(mail, pw);


            //assert
            Assert.That(result);
        }
    }

    [Test]
    public void CreateToken_ValidMailAndPassword()
    {
        using (var mogDB = new DataContext())
        {
            AddOneCompany_AddOneUser(mogDB);

            //arrange
            var logger = new Mock<ILogger<AuthUsecases>>();
            var subject = new AuthUsecases(logger.Object, mogDB);
            var mogSettings = new Dictionary<string, string>
            {
                { "Jwt:Issuer", "https://deskstar.com/" },
                { "Jwt:Audience", "https://deskstar.com/" },
                { "Jwt:Key", "thisIsATopSecretForTesting" }
            };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(mogSettings).Build();


            var mail = "test@mail.de";

            //act
            var result = subject.createToken(configuration, mail);


            //assert
            Assert.That(result, Is.Not.Null);
        }
    }

    [Test]
    public void RegisterUser_NewUser()
    {
        using (var mogDB = new DataContext())
        {
            var company = new Company
            {
                CompanyId = new Guid(),
                CompanyName = "gehmalbierholn"
            };

            mogDB.Companies.Add(company);
            mogDB.SaveChanges();

            //arrange
            var logger = new Mock<ILogger<AuthUsecases>>();
            var subject = new AuthUsecases(logger.Object, mogDB);

            var user = new RegisterUser();
            user.MailAddress = "test@mail.de";
            user.FirstName = "testF";
            user.LastName = "testL";
            user.Password = "testpw";
            user.CompanyId = company.CompanyId;

            //act
            var result = subject.registerUser(user);

            //assert
            Assert.That(result);
        }
    }

    [Test]
    public void RegisterUser_ExistingMail()
    {
        using (var mogDB = new DataContext())
        {
            AddOneCompany_AddOneUser(mogDB);

            //arrange
            var logger = new Mock<ILogger<AuthUsecases>>();
            var subject = new AuthUsecases(logger.Object, mogDB);

            var user = new RegisterUser();
            user.MailAddress = "test@mail.de";
            user.FirstName = "testF";
            user.LastName = "testL";
            user.Password = "testpw";

            //act
            var result = subject.registerUser(user);

            //assert
            Assert.That(result, Is.False);
        }
    }

    [Test]
    public void RegisterUser_NonExistingCompany()
    {
        using (var mogDB = new DataContext())
        {
            var company = new Company
            {
                CompanyId = new Guid(),
                CompanyName = "gehmalbierholn"
            };

            mogDB.Companies.Add(company);
            mogDB.SaveChanges();

            //arrange
            var logger = new Mock<ILogger<AuthUsecases>>();
            var subject = new AuthUsecases(logger.Object, mogDB);

            var user = new RegisterUser();
            user.MailAddress = "test@mail.de";
            user.FirstName = "testF";
            user.LastName = "testL";
            user.Password = "testpw";

            //act
            var result = subject.registerUser(user);

            //assert
            Assert.That(result, Is.False);
        }
    }

    private void AddOneCompany_AddOneUser(DataContext mogDB)
    {
        var company = new Company
        {
            CompanyId = new Guid(),
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            MailAddress = "test@mail.de",
            Password = "testpw",
            FirstName = "testF",
            LastName = "testL",
            Company = company
        };
        mogDB.Companies.Add(company);
        mogDB.Users.Add(user);
        mogDB.SaveChanges();
    }
}