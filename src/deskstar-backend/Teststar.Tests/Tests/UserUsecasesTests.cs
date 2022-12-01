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
    public void ReadSpecificUser_ShouldThrowArgumentException_WhenInvalidUserIdIsGiven()
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
        Assert.Throws<ArgumentException>(() => usecases.ReadSpecificUser(invalidId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void ReadSpecificUser_ShouldReturnUser_WhenValidUserIdIsGiven()
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
            BuildingName = "Gebäude1",
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