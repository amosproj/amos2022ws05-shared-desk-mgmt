using Deskstar.Core.Exceptions;
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
    Assert.That(result.Message, Is.EqualTo(LoginReturn.CredentialsWrong));

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
    Assert.That(result.Message, Is.EqualTo(LoginReturn.CredentialsWrong));

    //cleanup
    mogDB.Database.EnsureDeleted();
  }

  [Test]
  public void CheckCredentials_DeletedUser()
  {
    //setup
    using var mogDB = new DataContext();
    AddOneCompany_AddOneUser(mogDB, new PasswordHasher<User>());

    //arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, mogDB);

    const string mail = "test@mail.de";
    const string pw = "testpw";

    mogDB.Users.Single(u => u.MailAddress.Equals(mail)).IsMarkedForDeletion = true;

    //act
    var result = subject.CheckCredentials(mail, pw);


    //assert
    Assert.That(result.Message, Is.EqualTo(LoginReturn.Deleted));

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

  [Test]
  public void RegisterAdmin_WhenAllValuesValid_ShouldCreateAdminAndCompany()
  {
    // setup
    using var context = new DataContext();
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var mailAddress = "test@mail.de";
    var firstName = "testF";
    var lastName = "testL";
    var password = "testpw";
    var companyName = "company";

    // act
    var admin = subject.RegisterAdmin(firstName, lastName, mailAddress, password, companyName);

    // assert
    Assert.NotNull(admin);
    Assert.NotNull(admin.UserId);
    Assert.NotNull(admin.Company);
    Assert.NotNull(admin.Company.CompanyId);

    Assert.True(admin.IsCompanyAdmin);

    Assert.That(admin.FirstName, Is.EqualTo(firstName));
    Assert.That(admin.LastName, Is.EqualTo(lastName));
    Assert.That(admin.Company.CompanyName, Is.EqualTo(companyName));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  [TestCase("a")]
  [TestCase("a.de")]
  [TestCase("a@e.")]
  [TestCase("@e.de")]
  public void RegisterAdmin_WhenMailIsInvalid_ShouldCreateAdminAndCompany(string invalidMailAddress)
  {
    // setup
    using var context = new DataContext();
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var firstName = "testF";
    var lastName = "testL";
    var password = "testpw";
    var companyName = "company";

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      subject.RegisterAdmin(firstName, lastName, invalidMailAddress, password, companyName));

    Assert.NotNull(ex);
    Assert.That(ex.Message, Is.EqualTo($"E-Mail '{invalidMailAddress}' is not valid"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void RegisterAdmin_WhenMailIsInvalid_ShouldCreateAdminAndCompany()
  {
    // setup
    using var context = new DataContext();
    var companyName = "company";
    var companyId = Guid.NewGuid();
    var company = new Company { CompanyId = companyId, CompanyName = companyName };

    context.Companies.Add(company);
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var firstName = "testF";
    var lastName = "testL";
    var mailAddress = "mail@add.ress";
    var password = "testpw";

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      subject.RegisterAdmin(firstName, lastName, mailAddress, password, companyName));

    Assert.NotNull(ex);
    Assert.That(ex.Message, Is.EqualTo($"Company name '{companyName}' already in use"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void RegisterAdmin_WhenCalledWithEmptyFirstName_ShouldThrowInvalidArgumentException()
  {
    // setup
    using var context = new DataContext();
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var firstName = string.Empty;
    var lastName = "notEmpty";
    var mailAddress = "notEmpty";
    var password = "notEmpty";
    var companyName = "notEmpty";


    // act + assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      subject.RegisterAdmin(firstName, lastName, mailAddress, password, companyName));

    Assert.NotNull(ex);
    Assert.That(ex.Message, Is.EqualTo($"'{nameof(firstName)}' is not set"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void RegisterAdmin_WhenCalledWithEmptyLastName_ShouldThrowInvalidArgumentException()
  {
    // setup
    using var context = new DataContext();
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var firstName = "notEmpty";
    var lastName = string.Empty;
    var mailAddress = "notEmpty";
    var password = "notEmpty";
    var companyName = "notEmpty";


    // act + assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      subject.RegisterAdmin(firstName, lastName, mailAddress, password, companyName));

    Assert.NotNull(ex);
    Assert.That(ex.Message, Is.EqualTo($"'{nameof(lastName)}' is not set"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void RegisterAdmin_WhenCalledWithEmptyMailAddress_ShouldThrowInvalidArgumentException()
  {
    // setup
    using var context = new DataContext();
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var firstName = "notEmpty";
    var lastName = "notEmpty";
    var mailAddress = string.Empty;
    var password = "notEmpty";
    var companyName = "notEmpty";


    // act + assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      subject.RegisterAdmin(firstName, lastName, mailAddress, password, companyName));

    Assert.NotNull(ex);
    Assert.That(ex.Message, Is.EqualTo($"'{nameof(mailAddress)}' is not set"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void RegisterAdmin_WhenCalledWithEmptyPassword_ShouldThrowInvalidArgumentException()
  {
    // setup
    using var context = new DataContext();
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var firstName = "notEmpty";
    var lastName = "notEmpty";
    var mailAddress = "notEmpty";
    var password = string.Empty;
    var companyName = "notEmpty";


    // act + assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      subject.RegisterAdmin(firstName, lastName, mailAddress, password, companyName));

    Assert.NotNull(ex);
    Assert.That(ex.Message, Is.EqualTo($"'{nameof(password)}' is not set"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void RegisterAdmin_WhenCalledWithEmptyCompanyName_ShouldThrowInvalidArgumentException()
  {
    // setup
    using var context = new DataContext();
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<AuthUsecases>>();
    var subject = new AuthUsecases(logger.Object, context);

    var firstName = "notEmpty";
    var lastName = "notEmpty";
    var mailAddress = "notEmpty";
    var password = "notEmpty";
    var companyName = string.Empty;


    // act + assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      subject.RegisterAdmin(firstName, lastName, mailAddress, password, companyName));

    Assert.NotNull(ex);
    Assert.That(ex.Message, Is.EqualTo($"'{nameof(companyName)}' is not set"));

    // cleanup
    context.Database.EnsureDeleted();
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
