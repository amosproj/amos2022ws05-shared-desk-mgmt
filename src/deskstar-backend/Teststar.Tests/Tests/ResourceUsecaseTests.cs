using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class ResourceUsecaseTests
{
    [Test]
    public void GetBuildings_WhenNoBuildingFound_ShouldReturnATupleTrueEmptyList()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var hasher = new PasswordHasher<User>();
        var company = new Company
        {
            CompanyId = companyId,
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            UserId = userId,
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = company.CompanyId,
            IsApproved = true
        };
        user.Password = hasher.HashPassword(user, "testpw");
        db.Add(company);
        db.Add(user);
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var (noFound, result) = usecases.GetBuildings(userId);

        //assert
        Assert.True(noFound);
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetBuildings_WhenOneBuildingFound_ShouldReturnATupleFalseListWithOneEntry()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        SetupMockData(db, userId: userId);
        
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var (noFound, result) = usecases.GetBuildings(userId);

        //assert
        Assert.False(noFound);
        Assert.That(result, Has.Count.EqualTo(1));

        //cleanup
        db.Database.EnsureDeleted();
    }
    
    [Test]
    public void GetFloors_WhenNoFloorFound_ShouldReturnATupleTrueEmptyList()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var hasher = new PasswordHasher<User>();
        var company = new Company
        {
            CompanyId = companyId,
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            UserId = userId,
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = company.CompanyId,
            IsApproved = true
        };
        user.Password = hasher.HashPassword(user, "testpw");
        db.Add(company);
        db.Add(user);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var (noFound, result) = usecases.GetBuildings(userId);

        //assert
        Assert.True(noFound);
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }
    
    [Test]
    public void GetFloors_WhenOneFloorFound_ShouldReturnATupleFalseListWithOneEntry()
    {
        //setup 
        using var db = new DataContext();

        var buildingId = Guid.NewGuid();
        SetupMockData(db, buildingId: buildingId);
        
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var (noFound, result) = usecases.GetFloors(buildingId);

        //assert
        Assert.False(noFound);
        Assert.That(result, Has.Count.EqualTo(1));

        //cleanup
        db.Database.EnsureDeleted();
    }
    
    [Test]
    public void GetDesks_WhenNoDeskFound_ShouldReturnATupleTrueEmptyList()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        var hasher = new PasswordHasher<User>();
        var company = new Company
        {
            CompanyId = companyId,
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            UserId = userId,
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = company.CompanyId,
            IsApproved = true
        };
        var building = new Building
        {
            BuildingId = buildingId,
            BuildingName = "Gebäude1",
            Location = "Location1",
            CompanyId = company.CompanyId
        };
        user.Password = hasher.HashPassword(user, "testpw");
        db.Add(company);
        db.Add(user);
        db.Add(building);
        
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var (noFound, result) = usecases.GetDesks(buildingId, DateTime.Now, DateTime.Now);

        //assert
        Assert.True(noFound);
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }
    
    [Test]
    public void GetDesks_WhenOneDeskFound_ShouldReturnATupleFalseListWithOneEntry()
    {
        //setup 
        using var db = new DataContext();

        var roomId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now;
        SetupMockData(db, roomId: roomId);
        
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var (noFound, result) = usecases.GetDesks(roomId, start, end);

        //assert
        Assert.False(noFound);
        Assert.That(result, Has.Count.EqualTo(1));

        //cleanup
        db.Database.EnsureDeleted();
    }
    
    [Test]
    public void GetDesk_WhenDeskNotFound_ShouldReturnNull()
    {
        //setup 
        using var db = new DataContext();

        var deskId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now;
        SetupMockData(db, deskId: deskId);
        
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var  result = usecases.GetDesk(Guid.NewGuid(), start, end);

        //assert
        Assert.That(result,Is.Null);

        //cleanup
        db.Database.EnsureDeleted();
    }
    
    [Test]
    public void GetDesk_WhenDeskIsFound_ShouldCurrentsDeskObject()
    {
        //setup 
        using var db = new DataContext();

        var deskId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now;
        SetupMockData(db, deskId: deskId);
        
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db);


        //act
        var  result = usecases.GetDesk(deskId, start, end);

        //assert
        Assert.That(result,Is.Not.Null);

        //cleanup
        db.Database.EnsureDeleted();
    }

    private void SetupMockData(DataContext moqDb, Guid companyId = new(), Guid userId = new(), Guid buildingId = new(),
        Guid floorId = new(), Guid roomId = new(), Guid deskTypeId = new(), Guid deskId = new())
    {
        if (companyId.ToString() == "00000000-0000-0000-0000-000000000000") companyId = Guid.NewGuid();
        if (userId.ToString() == "00000000-0000-0000-0000-000000000000") userId = Guid.NewGuid();
        if (buildingId.ToString() == "00000000-0000-0000-0000-000000000000") buildingId = Guid.NewGuid();
        if (floorId.ToString() == "00000000-0000-0000-0000-000000000000") floorId = Guid.NewGuid();
        if (roomId.ToString() == "00000000-0000-0000-0000-000000000000") roomId = Guid.NewGuid();
        if (deskTypeId.ToString() == "00000000-0000-0000-0000-000000000000") deskTypeId = Guid.NewGuid();
        if (deskId.ToString() == "00000000-0000-0000-0000-000000000000") deskId = Guid.NewGuid();
        var hasher = new PasswordHasher<User>();
        var company = new Company
        {
            CompanyId = companyId,
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            UserId = userId,
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = company.CompanyId,
            IsApproved = true
        };
        user.Password = hasher.HashPassword(user, "testpw");
        var building = new Building
        {
            BuildingId = buildingId,
            BuildingName = "Gebäude1",
            Location = "Location1",
            CompanyId = company.CompanyId
        };
        var floor = new Floor
        {
            FloorId = floorId,
            FloorName = "Stockwerk1",
            BuildingId = building.BuildingId
        };
        var room = new Room
        {
            RoomId = roomId,
            FloorId = floor.FloorId,
            RoomName = "Raum1"
        };
        var deskTyp = new DeskType
        {
            DeskTypeId = deskTypeId,
            CompanyId = company.CompanyId,
            DeskTypeName = "Typ1"
        };
        var desk = new Desk
        {
            DeskId = deskId,
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