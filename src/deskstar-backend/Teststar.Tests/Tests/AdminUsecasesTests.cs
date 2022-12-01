using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class AdminUsecasesTests
{

    [Test]
    public void ApproveUser_ShouldApproveUser_WhenValidUserIdIsGiven()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act
        var result = adminUsecases.ApproveUser(admin.UserId, user.UserId.ToString());

        //assert
        Assert.That(result == user.UserId);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void ApproveUser_ShouldThrowArgumentException_WhenInvalidUserIdIsGiven()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentException>(() => adminUsecases.ApproveUser(admin.UserId, "invalidguid"));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void ApproveUser_ShouldThrowArgumentException_WhenThereIsNoUserWithGivenId()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentException>(() => adminUsecases.ApproveUser(admin.UserId, (new Guid()).ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void ApproveUser_ShouldThrowArgumentException_WhenValidUserIsGivenButAdminIsFromDifferentCompany()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUserInDifferentCompany(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentException>(() => adminUsecases.ApproveUser(admin.UserId, user.UserId.ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void DeclineUser_ShouldThrowArgumentException_WhenValidUserIsGivenButAdminIsFromDifferentCompany()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUserInDifferentCompany(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentException>(() => adminUsecases.DeclineUser(admin.UserId, user.UserId.ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void DeclineUser_ShouldThrowArgumentException_WhenUserIsAlreadyApproved()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = true;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentException>(() => adminUsecases.DeclineUser(admin.UserId, user.UserId.ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void DeclineUser_ShouldThrowArgumentException_WhenNoUserExistsWithGivenId()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentException>(() => adminUsecases.DeclineUser(admin.UserId, (new Guid()).ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void DeclineUser_ShouldRemoveUser_WhenValidUserIdIsGiven()
    {
        //setup
        using var db = new DataContext();
        User user, admin;
        bool isUserApproved = false;

        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<AdminUsecases>>();
        var adminUsecases = new AdminUsecases(logger.Object, db);

        //act
        var result = adminUsecases.DeclineUser(admin.UserId, user.UserId.ToString());

        //assert
        var userWasRemoved = !db.Users.Contains(user);
        Assert.That(userWasRemoved);
        Assert.That(result == user.UserId);

        //cleanup
        db.Database.EnsureDeleted();
    }

    private void SetupSingleUser(DataContext db, bool userApproved, out User user, out User admin)
    {
        var hasher = new PasswordHasher<User>();

        var companyId = Guid.NewGuid();
        var company = new Company
        {
            CompanyId = companyId,
            CompanyName = "testLLC"
        };
        user = new User
        {
            UserId = Guid.NewGuid(),
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = companyId,
            IsApproved = userApproved
        };
        user.Password = hasher.HashPassword(user, "testpw");
        admin = new User
        {
            UserId = Guid.NewGuid(),
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = companyId,
            IsApproved = false,
            IsCompanyAdmin = true
        };
        admin.Password = hasher.HashPassword(admin, "testpw");
        db.Companies.Add(company);
        db.Users.Add(user);
        db.Users.Add(admin);
        db.SaveChanges();
    }
    private void SetupSingleUserInDifferentCompany(DataContext db, bool isApproved, out User user, out User admin)
    {
        var hasher = new PasswordHasher<User>();

        var companyId = Guid.NewGuid();
        var company = new Company
        {
            CompanyId = companyId,
            CompanyName = "testLLC"
        };
        var companyId2 = Guid.NewGuid();
        var company2 = new Company
        {
            CompanyId = companyId2,
            CompanyName = "testLLC2"
        };
        user = new User
        {
            UserId = Guid.NewGuid(),
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = companyId2,
            IsApproved = isApproved
        };
        user.Password = hasher.HashPassword(user, "testpw");
        admin = new User
        {
            UserId = Guid.NewGuid(),
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = companyId,
            IsApproved = false,
            IsCompanyAdmin = true
        };
        admin.Password = hasher.HashPassword(admin, "testpw");
        db.Companies.Add(company);
        db.Companies.Add(company2);
        db.Users.Add(user);
        db.Users.Add(admin);
        db.SaveChanges();
    }
}