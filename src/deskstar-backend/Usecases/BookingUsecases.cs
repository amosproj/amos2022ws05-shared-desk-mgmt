using Deskstar.DataAccess;
using Deskstar.Entities;

namespace Deskstar.Usecases;

public interface IBookingUsecases
{
    public List<Booking> GetRecentBookings(String mailAddress);
}

public class BookingUsecases : IBookingUsecases
{
    private readonly ILogger<BookingUsecases> _logger;
    private readonly DataContext _context;

    public BookingUsecases(ILogger<BookingUsecases> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<Booking> GetRecentBookings(String mailAddress)
    {
        Guid user = _getUser(mailAddress).UserId;
        Booking booking = _context.Bookings.Single(b => b.UserId == user);
        List<Booking> bookings = new List<Booking>();
        bookings.Add(booking);
        return bookings;
    }

    private User _getUser(String mail)
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