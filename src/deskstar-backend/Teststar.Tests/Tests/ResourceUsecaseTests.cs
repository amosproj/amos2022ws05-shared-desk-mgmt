﻿using Deskstar.DataAccess;
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
        var usecases = new ResourceUsecases(logger.Object, db);


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
        var usecases = new ResourceUsecases(logger.Object, db);
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
        var usecases = new ResourceUsecases(logger.Object, db);


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
        var usecases = new ResourceUsecases(logger.Object, db);
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
        var usecases = new ResourceUsecases(logger.Object, db);


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
        var usecases = new ResourceUsecases(logger.Object, db);
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
        var usecases = new ResourceUsecases(logger.Object, db);


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
        var usecases = new ResourceUsecases(logger.Object, db);


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
        var usecases = new ResourceUsecases(logger.Object, db);


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
        var usecases = new ResourceUsecases(logger.Object, db);
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
        var usecases = new ResourceUsecases(logger.Object, db);


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