using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;
using Microsoft.EntityFrameworkCore;
namespace Deskstar.Usecases;

public interface IBookingUsecases
{
    public List<Booking> GetFilteredBookings(Guid userId, int n, int skip, string direction, DateTime start, DateTime end);
    public List<ExtendedBooking> GetRecentBookings(Guid userId);
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

    public List<Booking> GetFilteredBookings(Guid userId, int n, int skip, string direction, DateTime start, DateTime end)
    {
        var allBookingsFromUser = _context.Bookings.Where(booking => booking.UserId == userId);
        var filteredEnd = allBookingsFromUser.Where(b => b.StartTime < end);
        var filteredStart = filteredEnd.Where(b => b.StartTime >= start);
        var sortedBookings = direction.ToUpper() == "ASC" ? filteredStart.OrderBy(bookings => bookings.StartTime) : filteredStart.OrderByDescending(b => b.StartTime);
        var skipped = sortedBookings.Skip(skip);
        var takeN = skipped.Take(n);
        var withReferences = takeN.Include(b => b.Desk)
        .ThenInclude(d => d.Room)
        .ThenInclude(r => r.Floor)
        .ThenInclude(b => b.Building);

        return withReferences.ToList();
    }

    public List<ExtendedBooking> GetRecentBookings(Guid userId)
    {
        var bookings = _context.Bookings
            .Where(b => b.UserId == userId && b.StartTime >= DateTime.Now)
            .OrderBy(b => b.StartTime)
            .Take(10);

        var withReferences = bookings.Include(b => b.Desk)
               .ThenInclude(d => d.Room)
               .ThenInclude(r => r.Floor)
               .ThenInclude(b => b.Building);

        var mapBookingsToRecentBookings = bookings.Select(b => new ExtendedBooking
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
}