using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;

namespace Deskstar.Usecases;

public interface IBookingUsecases
{
    public List<RecentBooking> GetRecentBookings(string mailAddress);
}

public class BookingUsecases : IBookingUsecases
{
    private readonly DataContext _context;
    private readonly ILogger<BookingUsecases> _logger;

    public BookingUsecases(ILogger<BookingUsecases> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<RecentBooking> GetRecentBookings(string mailAddress)
    {
        var userId = _getUser(mailAddress).UserId;
        var bookings = _context.Bookings
            .Where(b => b.UserId == userId && b.StartTime >= DateTime.Now)
            .OrderBy(b => b.StartTime)
            .Take(10);
        var mapBookingsToRecentBookings = bookings.Select(b => new RecentBooking
        {
            DeskName = b.Desk.DeskName,
            EndTime = b.EndTime,
            StartTime = b.StartTime,
            Timestamp = b.Timestamp,
            BuildingName = b.Desk.Room.Floor.Building.BuildingName,
            FloorName = b.Desk.Room.Floor.FloorName,
            RoomName = b.Desk.Room.RoomName
        });
        
        return mapBookingsToRecentBookings.ToList();
    }

    private User _getUser(string mail)
    {
        try
        {
            return _context.Users.Single(u => u.MailAddress == mail);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return User.Null;
        }
    }
}