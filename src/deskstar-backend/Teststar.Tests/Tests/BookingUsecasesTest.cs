﻿using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class BookingUsecasesTest
{

    [Test]
    public void GetFilteredBookings_WhenDefaultConfigIsUsed_ShouldReturnASingleBooking()
    {
        //setup 
        using var db = new DataContext();

        var userID = Guid.NewGuid();
        var deskID = Guid.NewGuid();
        setupMockData(db, userID: userID, deskID: deskID);

        var bID = Guid.NewGuid();
        var bStart = DateTime.Now.Add(TimeSpan.FromHours(1));
        var bEnd = DateTime.Now.Add(TimeSpan.FromHours(2));
        var booking = new Booking
        {
            BookingId = bID,
            DeskId = deskID,
            UserId = userID,
            Timestamp = DateTime.Now,
            StartTime = bStart,
            EndTime = bEnd
        };
        db.Add(booking);
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);
        
        //act
        var result = usecases.GetFilteredBookings(userID, int.MaxValue, 0, "DESC", DateTime.Now, DateTime.MaxValue);

        //assert
        Assert.That(result.Count == 1);

        //cleanup
        db.Database.EnsureDeleted();

    }

    [Test]
    [TestCase(1, 0, 1)]
    [TestCase(2, 0, 2)]
    [TestCase(1, 1, 1)]
    [TestCase(1, 2, 0)]
    public void GetFilteredBookings_WhenQueryForNandSkip_ShouldReturnTheExpectedAmountOfBookings(int n, int skip, int expectedBookings)
    {
        //setup 
        using var db = new DataContext();

        var userID = Guid.NewGuid();
        var deskID = Guid.NewGuid();
        setupMockData(db, userID: userID, deskID: deskID);

        var fbID = Guid.NewGuid();
        var sbID = Guid.NewGuid();
        setupTwoBookings(db, userID, deskID, fbID, sbID);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        //act
        var result = usecases.GetFilteredBookings(userID, n, skip, "", DateTime.Now, DateTime.MaxValue);

        //assert
        Assert.That(result.Count == expectedBookings);

        //cleanup
        db.Database.EnsureDeleted();

    }


    [Test]
    public void GetFilteredBookings_WhenQueryForDirectionASC_ShouldReturnTheBookingsInAscendingOrder()
    {
        //setup 
        using var db = new DataContext();

        var userID = Guid.NewGuid();
        var deskID = Guid.NewGuid();
        setupMockData(db, userID: userID, deskID: deskID);

        var fbID = Guid.NewGuid();
        var sbID = Guid.NewGuid();
        setupTwoBookings(db, userID, deskID, fbID, sbID);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        //act
        var result = usecases.GetFilteredBookings(userID, 2, 0, "ASC", DateTime.Now, DateTime.MaxValue);

        //assert
        Assert.That(result[0].BookingId == fbID);
        Assert.That(result[1].BookingId == sbID);

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void GetFilteredBookings_WhenQueryForDirectionDESC_ShouldReturnTheBookingsInDescendingOrder()
    {
        //setup 
        using var db = new DataContext();

        var userID = Guid.NewGuid();
        var deskID = Guid.NewGuid();
        setupMockData(db, userID: userID, deskID: deskID);

        var fbID = Guid.NewGuid();
        var sbID = Guid.NewGuid();
        setupTwoBookings(db, userID, deskID, fbID, sbID);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        //act
        var result = usecases.GetFilteredBookings(userID, 2, 0, "DESC", DateTime.Now, DateTime.MaxValue);

        //assert
        Assert.That(result[0].BookingId == sbID);
        Assert.That(result[1].BookingId == fbID);

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void GetFilteredBookings_WhenStartIsAfterBookingStartTime_ShouldNotReturnTheBookingsWhereStartTimeBeforeStart()
    {
        //setup 
        using var db = new DataContext();

        var userID = Guid.NewGuid();
        var deskID = Guid.NewGuid();
        setupMockData(db, userID: userID, deskID: deskID);

        var fbID = Guid.NewGuid();
        var sbID = Guid.NewGuid();
        var (firstBooking, secondBooking) = setupTwoBookings(db, userID, deskID, fbID, sbID);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);
        var start = firstBooking.StartTime.Add(TimeSpan.FromTicks(1));

        //act
        var result = usecases.GetFilteredBookings(userID, 2, 0, "DESC", start, DateTime.MaxValue);

        //assert
        Assert.That(result.Count == 1);
        Assert.That(!result.Contains(firstBooking));
        Assert.That(result.Contains(secondBooking));

        //cleanup
        db.Database.EnsureDeleted();
    }
    [Test]
    public void GetFilteredBookings_WhenEndIsBeforeBookingStartTime_ShouldNotReturnTheBookingsWhereStartTimeAfterEnd()
    {
        //setup 
        using var db = new DataContext();

        var userID = Guid.NewGuid();
        var deskID = Guid.NewGuid();
        setupMockData(db, userID: userID, deskID: deskID);

        var fbID = Guid.NewGuid();
        var sbID = Guid.NewGuid();
        var (firstBooking, secondBooking) = setupTwoBookings(db, userID, deskID, fbID, sbID);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);
        var end = secondBooking.StartTime.Subtract(TimeSpan.FromTicks(1));

        //act
        var result = usecases.GetFilteredBookings(userID, 2, 0, "DESC", DateTime.Now, end);

        //assert
        Assert.That(result.Count == 1);
        Assert.That(result.Contains(firstBooking));
        Assert.That(!result.Contains(secondBooking));

        //cleanup
        db.Database.EnsureDeleted();
    }

    public void CheckGetRecentBookings_ValidMailAddress_NoBookings()
    {
        //setup
        using var mogDB = new DataContext();
        AddOneCompany_AddOneUser(mogDB, new PasswordHasher<User>());

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var subject = new BookingUsecases(logger.Object, mogDB);

        //act
        const string address = "test@mail.de";
        var result = subject.GetRecentBookings(mogDB.Users.First(u => u.MailAddress == address).UserId);


        //assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void CheckGetRecentBookings_ValidMailAddress_1Booking()
    {
        //setup
        using var mogDB = new DataContext();
        FillDatabaseWithEverything(mogDB, new PasswordHasher<User>());
        const string address = "test@example.de";
        var userId = mogDB.Users.First(u => u.MailAddress == address).UserId;

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var subject = new BookingUsecases(logger.Object, mogDB);

        //act

        var result = subject.GetRecentBookings(userId);


        //assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(1));
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
    private (Booking, Booking) setupTwoBookings(DataContext db, Guid userID, Guid deskID, Guid fbID, Guid sbID)
    {
        var fbStart = DateTime.Now.Add(TimeSpan.FromHours(1));
        var fbEnd = DateTime.Now.Add(TimeSpan.FromHours(2));
        var firstBooking = new Booking
        {
            BookingId = fbID,
            DeskId = deskID,
            UserId = userID,
            Timestamp = DateTime.Now,
            StartTime = fbStart,
            EndTime = fbEnd
        };

        var sbStart = DateTime.Now.Add(TimeSpan.FromDays(1));
        var sbEnd = DateTime.Now.Add(TimeSpan.FromDays(1));
        var secondBooking = new Booking
        {
            BookingId = sbID,
            DeskId = deskID,
            UserId = userID,
            Timestamp = DateTime.Now,
            StartTime = sbStart,
            EndTime = sbEnd
        };
        db.Add(firstBooking);
        db.Add(secondBooking);
        db.SaveChanges();
        return (firstBooking, secondBooking);
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
            UserId = new Guid(),
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

    private void FillDatabaseWithEverything(DataContext mogDB, PasswordHasher<User> hasher)
    {
        var company = new Company
        {
            CompanyId = Guid.NewGuid(),
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            UserId = Guid.NewGuid(),
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            CompanyId = company.CompanyId,
            IsApproved = true
        };
        user.Password = hasher.HashPassword(user, "testpw");
        var building = new Building
        {
            BuildingId = Guid.NewGuid(),
            BuildingName = "Gebäude1",
            Location = "Location1",
            CompanyId = company.CompanyId
        };
        var floor = new Floor
        {
            FloorId = Guid.NewGuid(),
            FloorName = "Stockwerk1",
            BuildingId = building.BuildingId
        };
        var room = new Room
        {
            RoomId = Guid.NewGuid(),
            FloorId = floor.FloorId,
            RoomName = "Raum1"
        };
        var deskTyp = new DeskType
        {
            DeskTypeId = Guid.NewGuid(),
            CompanyId = company.CompanyId,
            DeskTypeName = "Typ1"
        };
        var desk = new Desk
        {
            DeskId = Guid.NewGuid(),
            DeskName = "Desk1",
            DeskTypeId = deskTyp.DeskTypeId,
            RoomId = room.RoomId
        };
        var booking = new Booking
        {
            UserId = user.UserId,
            DeskId = desk.DeskId,
            Timestamp = DateTime.Now,
            StartTime = DateTime.Now.Add(TimeSpan.FromHours(1)),
            EndTime = DateTime.Now.Add(TimeSpan.FromHours(2))
        };
        mogDB.Companies.Add(company);
        mogDB.Users.Add(user);
        mogDB.Buildings.Add(building);
        mogDB.Floors.Add(floor);
        mogDB.Rooms.Add(room);
        mogDB.DeskTypes.Add(deskTyp);
        mogDB.Desks.Add(desk);
        mogDB.Bookings.Add(booking);

        mogDB.SaveChanges();
    }
}