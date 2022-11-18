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
            CompanyId = new Guid(),
            CompanyName = "gehmalbierholn"
        };
        var user = new User
        {
            UserId = new Guid(),
            MailAddress = "test@example.de",
            FirstName = "testF",
            LastName = "testL",
            Company = company,
            IsApproved = true
        };
        user.Password = hasher.HashPassword(user, "testpw");
        var building = new Building
        {
            BuildingId = new Guid(),
            BuildingName = "Gebäude1",
            Location = "Location1",
            CompanyId = company.CompanyId
        };
        var floor = new Floor
        {
            FloorId = new Guid(),
            FloorName = "Stockwerk1",
            BuildingId = building.BuildingId
        };
        var room = new Room
        {
            RoomId = new Guid(),
            FloorId = floor.FloorId,
            RoomName = "Raum1"
        };
        var deskTyp = new DeskType
        {
            DeskTypeId = new Guid(),
            Company = company,
            DeskTypeName = "Typ1"
        };
        var desk = new Desk
        {
            DeskId = new Guid(),
            DeskName = "Desk1",
            DeskTypeId = deskTyp.DeskTypeId
        };
        var booking=new Booking
        {
            UserId =  user.UserId,
            DeskId = desk.DeskId,
            Timestamp = DateTime.Now,
            StartTime = DateTime.Now.Add(TimeSpan.FromHours(1)),
            EndTime = DateTime.Now.Add(TimeSpan.FromHours(2)),
            Desk = desk,
            User=user
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