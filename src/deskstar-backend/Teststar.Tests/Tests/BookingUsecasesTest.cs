using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;
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

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        SetupMockData(db, userId: userId, deskId: deskId);

        var bId = Guid.NewGuid();
        var bStart = DateTime.Now.Add(TimeSpan.FromHours(1));
        var bEnd = DateTime.Now.Add(TimeSpan.FromHours(2));
        var booking = new Booking
        {
            BookingId = bId,
            DeskId = deskId,
            UserId = userId,
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
        var result = usecases.GetFilteredBookings(userId, int.MaxValue, 0, "DESC", DateTime.Now, DateTime.MaxValue);

        //assert
        Assert.That(result, Has.Count.EqualTo(1));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    [TestCase(1, 0, 1)]
    [TestCase(2, 0, 2)]
    [TestCase(1, 1, 1)]
    [TestCase(1, 2, 0)]
    public void GetFilteredBookings_WhenQueryForNandSkip_ShouldReturnTheExpectedAmountOfBookings(int n, int skip,
        int expectedBookings)
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        SetupMockData(db, userId: userId, deskId: deskId);

        var fbId = Guid.NewGuid();
        var sbId = Guid.NewGuid();
        SetupTwoBookings(db, userId, deskId, fbId, sbId);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        //act
        var result = usecases.GetFilteredBookings(userId, n, skip, "", DateTime.Now, DateTime.MaxValue);

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

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        SetupMockData(db, userId: userId, deskId: deskId);

        var fbId = Guid.NewGuid();
        var sbId = Guid.NewGuid();
        SetupTwoBookings(db, userId, deskId, fbId, sbId);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        //act
        var result = usecases.GetFilteredBookings(userId, 2, 0, "ASC", DateTime.Now, DateTime.MaxValue);

        //assert
        Assert.That(result[0].BookingId == fbId);
        Assert.That(result[1].BookingId == sbId);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetFilteredBookings_WhenQueryForDirectionDESC_ShouldReturnTheBookingsInDescendingOrder()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        SetupMockData(db, userId: userId, deskId: deskId);

        var fbId = Guid.NewGuid();
        var sbId = Guid.NewGuid();
        SetupTwoBookings(db, userId, deskId, fbId, sbId);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        //act
        var result = usecases.GetFilteredBookings(userId, 2, 0, "DESC", DateTime.Now, DateTime.MaxValue);

        //assert
        Assert.That(result[0].BookingId, Is.EqualTo(sbId));
        Assert.That(result[1].BookingId, Is.EqualTo(fbId));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void
        GetFilteredBookings_WhenStartIsAfterBookingStartTime_ShouldNotReturnTheBookingsWhereStartTimeBeforeStart()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        SetupMockData(db, userId: userId, deskId: deskId);

        var fbId = Guid.NewGuid();
        var sbId = Guid.NewGuid();
        var (firstBooking, secondBooking) = SetupTwoBookings(db, userId, deskId, fbId, sbId);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);
        var start = firstBooking.StartTime.Add(TimeSpan.FromTicks(1));

        //act
        var result = usecases.GetFilteredBookings(userId, 2, 0, "DESC", start, DateTime.MaxValue);

        //assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result, Does.Not.Contain(firstBooking));
        Assert.That(result, Does.Contain(secondBooking));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetFilteredBookings_WhenEndIsBeforeBookingStartTime_ShouldNotReturnTheBookingsWhereStartTimeAfterEnd()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        SetupMockData(db, userId: userId, deskId: deskId);

        var fbId = Guid.NewGuid();
        var sbId = Guid.NewGuid();
        var (firstBooking, secondBooking) = SetupTwoBookings(db, userId, deskId, fbId, sbId);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);
        var end = secondBooking.StartTime.Subtract(TimeSpan.FromTicks(1));

        //act
        var result = usecases.GetFilteredBookings(userId, 2, 0, "DESC", DateTime.Now, end);

        //assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result, Does.Contain(firstBooking));
        Assert.That(result, Does.Not.Contain(secondBooking));

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void CreateBooking_WhenDesksNotExists_ShouldThrowAnArgumentException()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        SetupMockData(db, userId: userId);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        var bookingRequest = new BookingRequest
        {
            DeskId = Guid.NewGuid(),
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        };
        //act
        try
        {
            var result = usecases.CreateBooking(Guid.NewGuid(), bookingRequest);

            //assert
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("Desk not found"));
        }

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void CreateBooking_WhenUserNotExists_ShouldThrowAnArgumentException()
    {
        //setup 
        using var db = new DataContext();

        var deskId = Guid.NewGuid();
        SetupMockData(db, deskId: deskId);

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        var bookingRequest = new BookingRequest
        {
            DeskId = deskId,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        };
        //act
        try
        {
            var result = usecases.CreateBooking(Guid.NewGuid(), bookingRequest);

            //assert
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("User not found"));
        }

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void CreateBooking_WhenTimeslotBooked_ShouldThrowAnArgumentException()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();

        SetupMockData(db, userId: userId, deskId: deskId);
        var fbStart = DateTime.Now.Add(TimeSpan.FromHours(1));
        var fbEnd = DateTime.Now.Add(TimeSpan.FromHours(2));
        var firstBooking = new Booking
        {
            BookingId = bookingId,
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = fbStart,
            EndTime = fbEnd
        };
        db.Add(firstBooking);
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        var bookingRequest = new BookingRequest
        {
            DeskId = deskId,
            StartTime = fbStart,
            EndTime = fbEnd
        };
        //act
        try
        {
            var result = usecases.CreateBooking(userId, bookingRequest);

            //assert
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("Time slot not available"));
        }

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void CreateBooking_WhenAvailable_ShouldReturnABooking()
    {
        //setup 
        using var db = new DataContext();

        var userId = Guid.NewGuid();
        var deskId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();

        SetupMockData(db, userId: userId, deskId: deskId);
        var firstBooking = new Booking
        {
            BookingId = bookingId,
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        };
        db.Add(firstBooking);
        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var usecases = new BookingUsecases(logger.Object, db);

        var fbStart = DateTime.Now.Add(TimeSpan.FromHours(1));
        var fbEnd = DateTime.Now.Add(TimeSpan.FromHours(2));

        var bookingRequest = new BookingRequest
        {
            DeskId = deskId,
            StartTime = fbStart,
            EndTime = fbEnd
        };
        //act
        var result = usecases.CreateBooking(userId, bookingRequest);

        //assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.DeskId, Is.EqualTo(deskId));
        Assert.That(result.StartTime, Is.EqualTo(fbStart));
        Assert.That(result.EndTime, Is.EqualTo(fbEnd));


        //cleanup
        db.Database.EnsureDeleted();
    }
    
    public void CheckGetRecentBookings_ValidMailAddress_NoBookings()
    {
        //setup
        using var mogDb = new DataContext();
        AddOneCompany_AddOneUser(mogDb, new PasswordHasher<User>());

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var subject = new BookingUsecases(logger.Object, mogDb);

        //act
        const string address = "test@mail.de";
        var result = subject.GetRecentBookings(mogDb.Users.First(u => u.MailAddress == address).UserId);


        //assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void CheckGetRecentBookings_ValidMailAddress_1Booking()
    {
        //setup
        using var mogDb = new DataContext();
        FillDatabaseWithEverything(mogDb, new PasswordHasher<User>());
        const string address = "test@example.de";
        var userId = mogDb.Users.First(u => u.MailAddress == address).UserId;

        //arrange
        var logger = new Mock<ILogger<BookingUsecases>>();
        var subject = new BookingUsecases(logger.Object, mogDb);

        //act

        var result = subject.GetRecentBookings(userId);


        //assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(1));
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

    private (Booking, Booking) SetupTwoBookings(DataContext db, Guid userId, Guid deskId, Guid fbId, Guid sbId)
    {
        var fbStart = DateTime.Now.Add(TimeSpan.FromHours(1));
        var fbEnd = DateTime.Now.Add(TimeSpan.FromHours(2));
        var firstBooking = new Booking
        {
            BookingId = fbId,
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = fbStart,
            EndTime = fbEnd
        };

        var sbStart = DateTime.Now.Add(TimeSpan.FromDays(1));
        var sbEnd = DateTime.Now.Add(TimeSpan.FromDays(1));
        var secondBooking = new Booking
        {
            BookingId = sbId,
            DeskId = deskId,
            UserId = userId,
            Timestamp = DateTime.Now,
            StartTime = sbStart,
            EndTime = sbEnd
        };
        db.Add(firstBooking);
        db.Add(secondBooking);
        db.SaveChanges();
        return (firstBooking, secondBooking);
    }

    private void AddOneCompany_AddOneUser(DataContext mogDb, PasswordHasher<User> hasher)
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
        mogDb.Companies.Add(company);
        mogDb.Users.Add(user);
        mogDb.SaveChanges();
    }

    private void FillDatabaseWithEverything(DataContext mogDb, PasswordHasher<User> hasher)
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
        mogDb.Companies.Add(company);
        mogDb.Users.Add(user);
        mogDb.Buildings.Add(building);
        mogDb.Floors.Add(floor);
        mogDb.Rooms.Add(room);
        mogDb.DeskTypes.Add(deskTyp);
        mogDb.Desks.Add(desk);
        mogDb.Bookings.Add(booking);

        mogDb.SaveChanges();
    }
}