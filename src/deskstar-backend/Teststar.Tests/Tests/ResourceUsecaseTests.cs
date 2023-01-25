using Deskstar.Core.Exceptions;
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
    public void GetBuildings_WhenNoBuildingFound_ShouldReturnAEmptyList()
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetBuildings(userId);

        //assert
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetBuildings_WhenOneBuildingFound_ShouldReturnAException()
    {
        //setup
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        SetupMockData(db, userId: userId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var callId = Guid.NewGuid();

        //act
        try
        {
            usecases.GetBuildings(callId);

            //assert
            Assert.Fail("No exception thrown");
        }
        catch (Exception e)
        {
            Assert.That(e.Message, Is.EqualTo($"There is no User with id '{callId}'"));
        }

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetFloors_WhenNoFloorFound_ShouldReturnAEmptyList()
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetBuildings(userId);

        //assert
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetFloors_WhenBuildingNotExsits_ShouldReturnAException()
    {
        //setup
        using var db = new DataContext();

        var buildingId = Guid.NewGuid();
        SetupMockData(db, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var callId = Guid.NewGuid();

        //act
        try
        {
            usecases.GetFloors(callId);

            //assert
            Assert.Fail("No exception thrown");
        }
        catch (Exception e)
        {
            Assert.That(e.Message, Is.EqualTo($"There is no Floor with id '{callId}'"));
        }

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDesks_WhenNoDeskFound_ShouldReturnAEmptyList()
    {
        //setup
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        var floorId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
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
        db.Add(company);
        db.Add(user);
        db.Add(building);
        db.Add(floor);
        db.Add(room);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, DateTime.Now, DateTime.Now);

        //assert
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDesks_WhenOneDeskFound_ShouldReturnAException()
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var callId = Guid.NewGuid();

        //act
        try
        {
            usecases.GetDesks(callId, start, end);

            //assert
            Assert.Fail("No exception thrown");
        }
        catch (Exception e)
        {
            Assert.That(e.Message, Is.EqualTo($"There is no Room with id '{callId}'"));
        }

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDesks_WhenDeskIsFound_StartBevoreEndIn_ShouldReturnACurrentsDeskList()
    {
        //setup
        using var db = new DataContext();

        var roomId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        var start = DateTime.Now.AddHours(-1);
        var end = DateTime.Now.AddHours(1);
        SetupMockData(db, roomId: roomId, userId: userId, deskId: deskId);
        var booking = new Booking
        {
            BookingId = Guid.NewGuid(),
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = DateTime.Now,
            EndTime = end
        };
        db.Add(booking);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, start, end);

        //assert

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0], Is.Not.Null);
            Assert.That(result[0].DeskId, Has.Length.EqualTo(36));
            Assert.That(result[0].DeskName, Is.EqualTo("Desk1"));
            Assert.That(result[0].RoomId, Has.Length.EqualTo(36));
            Assert.That(result[0].RoomName, Is.EqualTo("Raum1"));
            Assert.That(result[0].FloorId, Has.Length.EqualTo(36));
            Assert.That(result[0].FloorName, Is.EqualTo("Stockwerk1"));
            Assert.That(result[0].BuildingId, Has.Length.EqualTo(36));
            Assert.That(result[0].BuildingName, Is.EqualTo("Gebäude1"));
            Assert.That(result[0].DeskTyp, Is.EqualTo("Typ1"));
            Assert.That(result[0].Location, Is.EqualTo("Location1"));
            Assert.That(result[0].Bookings, Is.Not.Null);
            Assert.That(result[0].Bookings, Has.Count.EqualTo(1));
            Assert.That(result[0].Bookings[0], Is.Not.Null);
        });


        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDesks_WhenDeskIsFound_StartInEndBevore_ShouldReturnACurrentsDeskList()
    {
        //setup
        using var db = new DataContext();

        var roomId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        var start = DateTime.Now.AddHours(1);
        var end = DateTime.Now.AddHours(2);
        SetupMockData(db, roomId: roomId, userId: userId, deskId: deskId);
        var booking = new Booking
        {
            BookingId = Guid.NewGuid(),
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = DateTime.Now,
            EndTime = end.AddMinutes(-1)
        };
        db.Add(booking);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, start, end);

        //assert

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0], Is.Not.Null);
            Assert.That(result[0].DeskId, Has.Length.EqualTo(36));
            Assert.That(result[0].DeskName, Is.EqualTo("Desk1"));
            Assert.That(result[0].RoomId, Has.Length.EqualTo(36));
            Assert.That(result[0].RoomName, Is.EqualTo("Raum1"));
            Assert.That(result[0].FloorId, Has.Length.EqualTo(36));
            Assert.That(result[0].FloorName, Is.EqualTo("Stockwerk1"));
            Assert.That(result[0].BuildingId, Has.Length.EqualTo(36));
            Assert.That(result[0].BuildingName, Is.EqualTo("Gebäude1"));
            Assert.That(result[0].DeskTyp, Is.EqualTo("Typ1"));
            Assert.That(result[0].Location, Is.EqualTo("Location1"));
            Assert.That(result[0].Bookings, Is.Not.Null);
            Assert.That(result[0].Bookings, Has.Count.EqualTo(1));
            Assert.That(result[0].Bookings[0], Is.Not.Null);
        });


        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDesks_WhenDeskIsFound_SameStartAndEndTime_ShouldReturnACurrentsDeskList()
    {
        //setup
        using var db = new DataContext();

        var roomId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now.AddHours(1);
        SetupMockData(db, roomId: roomId, userId: userId, deskId: deskId);
        var booking = new Booking
        {
            BookingId = Guid.NewGuid(),
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = start,
            EndTime = end
        };
        db.Add(booking);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, start, end);

        //assert

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0], Is.Not.Null);
            Assert.That(result[0].DeskId, Has.Length.EqualTo(36));
            Assert.That(result[0].DeskName, Is.EqualTo("Desk1"));
            Assert.That(result[0].RoomId, Has.Length.EqualTo(36));
            Assert.That(result[0].RoomName, Is.EqualTo("Raum1"));
            Assert.That(result[0].FloorId, Has.Length.EqualTo(36));
            Assert.That(result[0].FloorName, Is.EqualTo("Stockwerk1"));
            Assert.That(result[0].BuildingId, Has.Length.EqualTo(36));
            Assert.That(result[0].BuildingName, Is.EqualTo("Gebäude1"));
            Assert.That(result[0].DeskTyp, Is.EqualTo("Typ1"));
            Assert.That(result[0].Location, Is.EqualTo("Location1"));
            Assert.That(result[0].Bookings, Is.Not.Null);
            Assert.That(result[0].Bookings, Has.Count.EqualTo(1));
            Assert.That(result[0].Bookings[0], Is.Not.Null);
        });


        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDesk_WhenDeskNotFound_ShouldThrowAnException()
    {
        //setup
        using var db = new DataContext();

        var deskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now;
        SetupMockData(db, deskId: deskId, userId: userId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var callId = Guid.NewGuid();

        //act
        try
        {
            usecases.GetDesk(callId, start, end);

            //assert
            Assert.Fail("No exception thrown");
        }
        catch (Exception e)
        {
            Assert.That(e.Message, Is.EqualTo($"There is no Desk with id '{callId}'"));
        }

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDesk_WhenDeskIsFound_ShouldReturnCurrentDeskObject()
    {
        //setup
        using var db = new DataContext();

        var deskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = new DateTime().AddHours(1);
        SetupMockData(db, deskId: deskId, userId: userId);
        var booking = new Booking
        {
            BookingId = Guid.NewGuid(),
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = start,
            EndTime = end
        };
        db.Add(booking);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesk(deskId, start, end);

        //assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.DeskId, Is.EqualTo(deskId.ToString()));
            Assert.That(result.DeskName, Is.EqualTo("Desk1"));
            Assert.That(result.RoomId, Has.Length.EqualTo(36));
            Assert.That(result.RoomName, Is.EqualTo("Raum1"));
            Assert.That(result.FloorId, Has.Length.EqualTo(36));
            Assert.That(result.FloorName, Is.EqualTo("Stockwerk1"));
            Assert.That(result.BuildingId, Has.Length.EqualTo(36));
            Assert.That(result.BuildingName, Is.EqualTo("Gebäude1"));
            Assert.That(result.DeskTyp, Is.EqualTo("Typ1"));
            Assert.That(result.Location, Is.EqualTo("Location1"));
            Assert.That(result.Bookings, Is.Not.Null);
            Assert.That(result.Bookings, Has.Count.EqualTo(1));
            Assert.That(result.Bookings[0], Is.Not.Null);
        });
        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetDeskTypes_WhenDeskTypeIsFound_ShouldReturnDeskTypes()
    {
        //setup
        using var db = new DataContext();

        var companyId = Guid.NewGuid();
        var deskTypeId = Guid.NewGuid();
        SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDeskTypes(companyId);

        //assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result[0].DeskTypeId == deskTypeId);
        Assert.That(result[0].CompanyId == companyId);

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void GetDeskTypes_WhenCompanyHasNoDeskTypes_ShouldReturnEmptyList()
    {
        //setup
        using var db = new DataContext();

        var companyId = Guid.NewGuid();
        var deskTypeId = Guid.NewGuid();
        SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDeskTypes(new Guid());

        //assert
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDesk_WhenValidDeskTypeAndRoomId_ShouldAddDeskAndReturnItsGuid()
    {
        //setup
        using var db = new DataContext();

        var companyId = Guid.NewGuid();
        var deskTypeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId, roomId: roomId);

        db.SaveChanges();

        //arrange
        var deskName = "validDeskName";
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.CreateDesk(deskName, deskTypeId, roomId);

        //assert
        Assert.That(result != null);

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDesk_WhenDuplicatedNameIsProvided_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();

        var companyId = Guid.NewGuid();
        var deskTypeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId, roomId: roomId);

        db.SaveChanges();

        //arrange
        var duplicated = "Desk1";
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDesk(duplicated, deskTypeId, roomId));


        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDesk_WhenNoNameIsProvided_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();

        var companyId = Guid.NewGuid();
        var deskTypeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId, roomId: roomId);

        db.SaveChanges();

        //arrange
        var noDeskName = "";
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDesk(noDeskName, deskTypeId, roomId));


        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDesk_WhenInvalidDeskType_ShouldThrowEntityNotFoundException()
    {
        //setup
        using var db = new DataContext();

        var companyId = Guid.NewGuid();
        var invalidDeskTypeId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, roomId: roomId);

        db.SaveChanges();

        //arrange
        var deskName = "validDeskName";
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

        //act+assert
        Assert.Throws<EntityNotFoundException>(() => usecases.CreateDesk(deskName, invalidDeskTypeId, roomId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDesk_WhenInvalidRoomId_ShouldThrowEntityNotFoundException()
    {
        //setup
        using var db = new DataContext();

        var companyId = Guid.NewGuid();
        var deskTypeId = Guid.NewGuid();
        var invalidRoomId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, deskTypeId: deskTypeId);

        db.SaveChanges();

        //arrange
        var deskName = "validDeskName";
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

        //act+assert
        Assert.Throws<EntityNotFoundException>(() => usecases.CreateDesk(deskName, deskTypeId, invalidRoomId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDeskType_WhenInvalidCompanyIdIsProvided_ShouldThrowEntitiyNotFoundException()
    {
        //setup
        using var db = new DataContext();
        SetupMockData(db);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var deskTypeName = "validDeskName";
        var invalidCompanyId = Guid.NewGuid();

        //act+assert
        Assert.Throws<EntityNotFoundException>(() => usecases.CreateDeskType(deskTypeName, invalidCompanyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDeskType_WhenNoDeskTypeNameIsProvided_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var noDeskTypeName = "";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDeskType(noDeskTypeName, companyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDeskType_WhenDeskTypeAlreadyExists_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var duplicateName = "Typ1";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDeskType(duplicateName, companyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateDeskType_WhenValidArgumentsProvided_ShouldCreateDeskType()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var deskTypeName = "ValidName";

        //act+assert
        Assert.DoesNotThrow(() => usecases.CreateDeskType(deskTypeName, companyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateRoom_WhenInvalidFloorIdIsProvided_ShouldThrowEntitiyNotFoundException()
    {
        //setup
        using var db = new DataContext();
        SetupMockData(db);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var roomName = "validRoomName";
        var invalidFloorId = Guid.NewGuid();

        //act+assert
        Assert.Throws<EntityNotFoundException>(() => usecases.CreateRoom(roomName, invalidFloorId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateRoom_WhenNoRoomNameIsProvided_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        var floorId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, floorId: floorId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var noRoomName = "";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateRoom(noRoomName, floorId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateRoom_WhenRoomNameAlreadyExists_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        var floorId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, floorId: floorId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var duplicateName = "Raum1";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateRoom(duplicateName, floorId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateRoom_WhenValidArgumentsProvided_ShouldCreateRoom()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        var floorId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, floorId: floorId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var roomName = "ValidName";

        //act+assert
        Assert.DoesNotThrow(() => usecases.CreateRoom(roomName, floorId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateFloor_WhenInvalidBuildingIdIsProvided_ShouldThrowEntitiyNotFoundException()
    {
        //setup
        using var db = new DataContext();
        SetupMockData(db);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var floorName = "validName";
        var invalidBuildingId = Guid.NewGuid();

        //act+assert
        Assert.Throws<EntityNotFoundException>(() => usecases.CreateFloor(floorName, invalidBuildingId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateFloor_WhenNoFloorNameIsProvided_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var noFloorName = "";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateFloor(noFloorName, buildingId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateFloor_WhenFloorNameAlreadyExists_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var duplicateName = "Stockwerk1";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateFloor(duplicateName, buildingId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateFloor_WhenValidArgumentsProvided_ShouldCreateFloor()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var floorName = "ValidName";

        //act+assert
        Assert.DoesNotThrow(() => usecases.CreateFloor(floorName, buildingId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateBuilding_WhenInvalidCompanyIdIsProvided_ShouldThrowEntitiyNotFoundException()
    {
        //setup
        using var db = new DataContext();
        SetupMockData(db);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var buildingName = "validName";
        var location = "validLocation";
        var invalidCompanyId = Guid.NewGuid();

        //act+assert
        Assert.Throws<EntityNotFoundException>(() => usecases.CreateBuilding(buildingName, location, invalidCompanyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateBuilding_WhenNoLocationIsProvided_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var buildingName = "validName";
        var noLocation = "";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateBuilding(buildingName, noLocation, companyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateBuilding_WhenNoBuildingNameIsProvided_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var noBuildingName = "";
        var location = "validLocation";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateBuilding(noBuildingName, location, companyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateBuilding_WhenBuildingNameAlreadyExists_ShouldThrowArgumentInvalidException()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var duplicateName = "Gebäude1";
        var location = "validLocation";

        //act+assert
        Assert.Throws<ArgumentInvalidException>(() => usecases.CreateBuilding(duplicateName, location, companyId));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void CreateBuilding_WhenValidArgumentsProvided_ShouldCreateBuilding()
    {
        //setup
        using var db = new DataContext();
        var companyId = Guid.NewGuid();
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var buildingName = "validName";
        var location = "validLocation";

        //act+assert
        Assert.DoesNotThrow(() => usecases.CreateBuilding(buildingName, location, companyId));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void DeleteBuilding_WhenValidBuildingIsProvided_ShouldUpdateBuilding()
    {
      //setup
      using var db = new DataContext();
      var buildingId = Guid.NewGuid();
      var adminId = Guid.NewGuid();

      SetupMockData(db, buildingId: buildingId, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

      //act
      resourceUsecases.DeleteBuilding(adminId, buildingId.ToString());

      //assert
      Assert.That(db.Buildings.First(b => b.BuildingId == buildingId).IsMarkedForDeletion);
      db.Floors.Where(f => f.BuildingId == buildingId).ToList().ForEach(f => Assert.That(f.IsMarkedForDeletion));
    }

    [Test]
    public void DeleteBuilding_WhenNonValidBuildingIsProvided_ShouldThrowEntityNotFoundException()
    {
      //setup
      using var db = new DataContext();
      var adminId = Guid.NewGuid();

      SetupMockData(db, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


      //act & assert
      Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteBuilding(adminId, new Guid().ToString()));
    }

    [Test]
    public void DeleteFloor_WhenValidFloorIsProvided_ShouldUpdateFloor()
    {
      //setup
      using var db = new DataContext();
      var floorId = Guid.NewGuid();
      var adminId = Guid.NewGuid();

      SetupMockData(db, floorId: floorId, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

      //act
      resourceUsecases.DeleteFloor(adminId, floorId.ToString());

      //assert
      Assert.That(db.Floors.First(f => f.FloorId == floorId).IsMarkedForDeletion);
      db.Rooms.Where(r => r.FloorId == floorId).ToList().ForEach(r => Assert.That(r.IsMarkedForDeletion));
    }

    [Test]
    public void DeleteFloor_WhenNonValidFloorIsProvided_ShouldThrowEntityNotFoundException()
    {
      //setup
      using var db = new DataContext();
      var adminId = Guid.NewGuid();

      SetupMockData(db, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


      //act & assert
      Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteFloor(adminId, new Guid().ToString()));
    }

    [Test]
    public void DeleteRoom_WhenValidRoomIsProvided_ShouldUpdateRoom()
    {
      //setup
      using var db = new DataContext();
      var roomId = Guid.NewGuid();
      var adminId = Guid.NewGuid();

      SetupMockData(db, roomId: roomId, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

      //act
      resourceUsecases.DeleteRoom(adminId, roomId.ToString());

      //assert
      Assert.That(db.Rooms.First(r=> r.RoomId == roomId).IsMarkedForDeletion);
      db.Desks.Where(d => d.RoomId == roomId).ToList().ForEach(d => Assert.That(d.IsMarkedForDeletion));
    }

    [Test]
    public void DeleteRoom_WhenNonValidRoomIsProvided_ShouldThrowEntityNotFoundException()
    {
      //setup
      using var db = new DataContext();
      var adminId = Guid.NewGuid();

      SetupMockData(db, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


      //act & assert
      Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteRoom(adminId, new Guid().ToString()));
    }

    [Test]
    public void DeleteDesk_WhenValidDeskIsProvided_ShouldUpdateDesk()
    {
      //setup
      using var db = new DataContext();
      var deskId = Guid.NewGuid();
      var adminId = Guid.NewGuid();

      SetupMockData(db, deskId: deskId, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

      //act
      resourceUsecases.DeleteDesk(adminId, deskId.ToString());

      //assert
      Assert.That(db.Desks.First(d=> d.DeskId == deskId).IsMarkedForDeletion);
    }

    [Test]
    public void DeleteDesk_WhenNonValidDeskIsProvided_ShouldThrowEntityNotFoundException()
    {
      //setup
      using var db = new DataContext();
      var adminId = Guid.NewGuid();

      SetupMockData(db, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


      //act & assert
      Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteDesk(adminId, new Guid().ToString()));
    }

    [Test]
    public void DeleteDeskType_WhenValidDeskTypeIsProvided_ShouldUpdateDeskType()
    {
      //setup
      using var db = new DataContext();
      var deskTypeId = Guid.NewGuid();
      var adminId = Guid.NewGuid();
      var deskId = Guid.NewGuid();

      SetupMockData(db, deskTypeId: deskTypeId, userId: adminId, deskId:deskId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
      resourceUsecases.DeleteDesk(adminId,deskId.ToString());
      //act
      resourceUsecases.DeleteDeskType(adminId, deskTypeId.ToString());

      //assert
      Assert.That(db.DeskTypes.First(d=> d.DeskTypeId == deskTypeId).IsMarkedForDeletion);
    }

    [Test]
    public void DeleteDeskType_WhenNonValidDeskTypeIsProvided_ShouldThrowEntityNotFoundException()
    {
      //setup
      using var db = new DataContext();
      var adminId = Guid.NewGuid();

      SetupMockData(db, userId: adminId);

      //arrange
      var logger = new Mock<ILogger<ResourceUsecases>>();
      var resourceUsecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


      //act & assert
      Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteDeskType(adminId, new Guid().ToString()));
    }

    private UserUsecases SetupUserUsecases(DataContext db)
    {
        var logger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(logger.Object, db);

        return userUsecases;
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
