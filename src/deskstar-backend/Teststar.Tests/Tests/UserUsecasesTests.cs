using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class UserUsecasesTests
{
    [Test]
    public void ReadSpecificUser_WhenInvalidUserIdIsGiven_ShouldThrowEntityNotFoundException()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        setupMockData(db, userID: userId);

        //arrange
        var invalidId = Guid.NewGuid();
        var logger = new Mock<ILogger<UserUsecases>>();
        var usecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<EntityNotFoundException>(() => usecases.ReadSpecificUser(invalidId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void ReadSpecificUser_WhenValidUserIdIsGiven_ShouldReturnUser()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        setupMockData(db, userID: userId);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var usecases = new UserUsecases(logger.Object, db);

        //act
        var result = usecases.ReadSpecificUser(userId);

        //assert
        Assert.That(result.UserId == userId);

        //cleanup
        db.Database.EnsureDeleted();

    }

    [Test]
    public void ApproveUser_WhenValidUserIdIsGiven_ShouldApproveUser()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act
        var result = userUsecases.ApproveUser(admin.UserId, user.UserId.ToString());

        //assert
        Assert.That(result == user.UserId);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void ApproveUser_WhenInvalidUserIdIsGiven_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentInvalidException>(() => userUsecases.ApproveUser(admin.UserId, "invalidguid"));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void ApproveUser_WhenThereIsNoUserWithGivenId_ShouldThrowEntityNotFoundException()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<EntityNotFoundException>(() => userUsecases.ApproveUser(admin.UserId, (new Guid()).ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void ApproveUser_WhenValidUserIsGivenButAdminIsFromDifferentCompany_ShouldThrowInsufficientPermissionException()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUserInDifferentCompany(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<InsufficientPermissionException>(() => userUsecases.ApproveUser(admin.UserId, user.UserId.ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void DeclineUser_WhenValidUserIsGivenButAdminIsFromDifferentCompany_ShouldThrowInsufficientPermissionException()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUserInDifferentCompany(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<InsufficientPermissionException>(() => userUsecases.DeclineUser(admin.UserId, user.UserId.ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void DeclineUser_WhenUserIsAlreadyApproved_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = true;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<ArgumentInvalidException>(() => userUsecases.DeclineUser(admin.UserId, user.UserId.ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void DeclineUser_WhenNoUserExistsWithGivenId_ShouldThrowEntityNotFoundException()
    {
        //setup
        using var db = new DataContext();
        bool isUserApproved = false;
        User user, admin;
        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<EntityNotFoundException>(() => userUsecases.DeclineUser(admin.UserId, (new Guid()).ToString()));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void DeclineUser_WhenValidUserIdIsGiven_ShouldRemoveUser()
    {
        //setup
        using var db = new DataContext();
        User user, admin;
        bool isUserApproved = false;

        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act
        var result = userUsecases.DeclineUser(admin.UserId, user.UserId.ToString());

        //assert
        var userWasRemoved = !db.Users.Contains(user);
        Assert.That(userWasRemoved);
        Assert.That(result == user.UserId);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void UpdateUser_WhenValidUserIsProvided_ShouldUpdateUser()
    {
        //setup
        using var db = new DataContext();
        User user, admin;
        bool isUserApproved = true;

        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act
        var result = userUsecases.UpdateUser(user);

        //assert
        Assert.That(result == user.UserId);

    }
    [Test]
    public void UpdateUser_WhenInvalidUserIsProvided_ShouldThrowEntityNotFoundException()
    {
        //setup
        using var db = new DataContext();
        User user, admin;
        bool isUserApproved = true;

        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<EntityNotFoundException>(() => userUsecases.UpdateUser(new User()));

    }
    [Test]
    public void ReadAllUsers_WhenValidUserIdIsProvided_ShouldReturnListOfAllUsersInTheSameCompany()
    {
        //setup
        using var db = new DataContext();
        User user, admin;
        bool isUserApproved = true;

        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act 
        var result = userUsecases.ReadAllUsers(admin.UserId);

        //assert
        Assert.That(result.Count != 0);
        Assert.That(result.Contains(user));

    }
    [Test]
    public void ReadAllUsers_WhenInvalidUserIdIsProvided_ShouldThrowEntityNotFoundException()
    {
        //setup
        using var db = new DataContext();
        User user, admin;
        bool isUserApproved = true;

        SetupSingleUser(db, isUserApproved, out user, out admin);

        //arrange
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        //act + assert
        Assert.Throws<EntityNotFoundException>(() => userUsecases.ReadAllUsers(new Guid()));

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
    private void setupMockData(DataContext moqDb, Guid companyID = new Guid(), Guid userID = new Guid(), Guid buildingID = new Guid(), Guid floorID = new Guid(), Guid roomID = new Guid(), Guid deskTypeID = new Guid(), Guid deskID = new Guid())
    {
        if (companyID.ToString() == "00000000-0000-0000-0000-000000000000")
        {
            companyID = Guid.NewGuid();
        }
        if (userID.ToString() == "00000000-0000-0000-0000-000000000000")
        {
            userID = Guid.NewGuid();
        }
        if (buildingID.ToString() == "00000000-0000-0000-0000-000000000000")
        {
            buildingID = Guid.NewGuid();
        }
        if (floorID.ToString() == "00000000-0000-0000-0000-000000000000")
        {
            floorID = Guid.NewGuid();
        }
        if (roomID.ToString() == "00000000-0000-0000-0000-000000000000")
        {
            roomID = Guid.NewGuid();
        }
        if (deskTypeID.ToString() == "00000000-0000-0000-0000-000000000000")
        {
            deskTypeID = Guid.NewGuid();
        }
        if (deskID.ToString() == "00000000-0000-0000-0000-000000000000")
        {
            deskID = Guid.NewGuid();
        }
        var hasher = new PasswordHasher<User>();
        var company = new Company
        {
            CompanyId = companyID,
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            UserId = userID,
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = company.CompanyId,
            IsApproved = true
        };
        user.Password = hasher.HashPassword(user, "testpw");
        var building = new Building
        {
            BuildingId = buildingID,
            BuildingName = "Geb??ude1",
            Location = "Location1",
            CompanyId = company.CompanyId
        };
        var floor = new Floor
        {
            FloorId = floorID,
            FloorName = "Stockwerk1",
            BuildingId = building.BuildingId
        };
        var room = new Room
        {
            RoomId = roomID,
            FloorId = floor.FloorId,
            RoomName = "Raum1"
        };
        var deskTyp = new DeskType
        {
            DeskTypeId = deskTypeID,
            CompanyId = company.CompanyId,
            DeskTypeName = "Typ1"
        };
        var desk = new Desk
        {
            DeskId = deskID,
            DeskName = "Desk1",
            DeskTypeId = deskTyp.DeskTypeId,
            RoomId = room.RoomId
        };
        moqDb.Companies.Add(company);
        moqDb.Users.Add(user);
        moqDb.Buildings.Add(building);
        moqDb.Floors.Add(floor);
        moqDb.Rooms.Add(room);
        moqDb.DeskTypes.Add(deskTyp);
        moqDb.Desks.Add(desk);

        moqDb.SaveChanges();

    }
}