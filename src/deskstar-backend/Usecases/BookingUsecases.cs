using Deskstar.DataAccess;
using Deskstar.Entities;

namespace Deskstar.Usecases;

public interface IBookingUsecases
{
    public List<Booking> GetRecentBookings();
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

    public List<Booking> GetRecentBookings()
    {
        return null;
    }
}