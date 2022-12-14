using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class AuthUseCasesTests
{
    [Test]
    public void CheckCredentials_ValidMailAndPassword()
    {
        //setup
        using var mogDB = new DataContext();
        AddOneCompany_AddOneUser(mogDB, new PasswordHasher<User>());

        //arrange
        var logger = new Mock<ILogger<AuthUsecases>>();
        var subject = new AuthUsecases(logger.Object, mogDB);

        const string mail = "test@mail.de";
        const string pw = "testpw";

        //act
        var result = subject.CheckCredentials(mail, pw);


        //assert
        Assert.That(result.Message, Is.EqualTo(LoginReturn.Ok));

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    [Test]
    public void CheckCredentials_ValidMailAndPassword_butNotApproved()
    {
        //setup
        using var mogDB = new DataContext();
        var company = new Company
        {
            CompanyId = new Guid(),
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            MailAddress = "test@mail.de",
            FirstName = "testF",
            LastName = "testL",
            Company = company
        };
        user.Password = new PasswordHasher<User>().HashPassword(user, "testpw");

        mogDB.Companies.Add(company);
        mogDB.Users.Add(user);
        mogDB.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<AuthUsecases>>();
        var subject = new AuthUsecases(logger.Object, mogDB);

        const string mail = "test@mail.de";
        const string pw = "testpw";

        //act
        var result = subject.CheckCredentials(mail, pw);


        //assert
        Assert.That(result.Message, Is.EqualTo(LoginReturn.NotYetApproved));

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    [Test]
    public void CheckCredentials_NonValidMailAndValidPassword()
    {
        //setup
        using var mogDB = new DataContext();
        AddOneCompany_AddOneUser(mogDB, new PasswordHasher<User>());

        //arrange
        var logger = new Mock<ILogger<AuthUsecases>>();
        var subject = new AuthUsecases(logger.Object, mogDB);

        const string mail = "teest@mail.de";
        const string pw = "testpw";

        //act
        var result = subject.CheckCredentials(mail, pw);


        //assert
        Assert.That(result.Message, Is.EqualTo(LoginReturn.CreditialsWrong));

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    [Test]
    public void CheckCredentials_ValidMail_NonValidPassword()
    {
        //setup
        using var mogDB = new DataContext();
        AddOneCompany_AddOneUser(mogDB, new PasswordHasher<User>());

        //arrange
        var logger = new Mock<ILogger<AuthUsecases>>();
        var subject = new AuthUsecases(logger.Object, mogDB);

        const string mail = "test@mail.de";
        const string pw = "testp";

        //act
        var result = subject.CheckCredentials(mail, pw);


        //assert
        Assert.That(result.Message, Is.EqualTo(LoginReturn.CreditialsWrong));

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    [Test]
    public void CreateToken_ValidMailAndPassword()
    {
        using var mogDB = new DataContext();
        AddOneCompany_AddOneUser(mogDB, new PasswordHasher<User>());

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


        const string mail = "test@mail.de";

        //act
        var result = subject.CreateToken(configuration, mail);


        //assert
        Assert.That(result, Is.Not.Null);

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    [Test]
    public void RegisterUser_NewUser()
    {
        using var mogDB = new DataContext();
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
        var result = subject.RegisterUser(user);

        //assert
        Assert.That(result.Message, Is.EqualTo(RegisterReturn.Ok));

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    [Test]
    public void RegisterUser_ExistingMail()
    {
        using var mogDB = new DataContext();
        AddOneCompany_AddOneUser(mogDB, new PasswordHasher<User>());

        //arrange
        var logger = new Mock<ILogger<AuthUsecases>>();
        var subject = new AuthUsecases(logger.Object, mogDB);
        var hasher = new PasswordHasher<User>();

        var user = new RegisterUser
        {
            MailAddress = "test@mail.de",
            FirstName = "testF",
            LastName = "testL",
            Password = "password",
            CompanyId = mogDB.Companies.First(c => c.CompanyName == "gehmalbierholn").CompanyId
        };

        //act
        var result = subject.RegisterUser(user);

        //assert
        Assert.That(result.Message, Is.EqualTo(RegisterReturn.MailAddressInUse));

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    [Test]
    public void RegisterUser_NonExistingCompany()
    {
        using var mogDB = new DataContext();
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

        var user = new RegisterUser
        {
            MailAddress = "test@mail.de",
            FirstName = "testF",
            LastName = "testL",
            Password = "testpw"
        };

        //act
        var result = subject.RegisterUser(user);

        //assert
        Assert.That(result.Message, Is.EqualTo(RegisterReturn.CompanyNotFound));

        //cleanup
        mogDB.Database.EnsureDeleted();
    }

    private void AddOneCompany_AddOneUser(DataContext mogDB, PasswordHasher<User> hasher)
    {
        var company = new Company
        {
            CompanyId = new Guid(),
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            MailAddress = "test@mail.de",
            FirstName = "testF",
            LastName = "testL",
            Company = company,
            IsApproved = true
        };
        user.Password = hasher.HashPassword(user, "testpw");
        mogDB.Companies.Add(company);
        mogDB.Users.Add(user);
        mogDB.SaveChanges();
    }
}