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
        var result = subject.GetRecentBookings(mogDB.Users.First(u=>u.MailAddress==address).UserId);


        //assert
        Assert.That(result,Is.Empty);
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
        Assert.That(result,Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(1));
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
        var booking=new Booking
        {
            UserId =  user.UserId,
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