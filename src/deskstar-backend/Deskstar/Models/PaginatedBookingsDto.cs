namespace Deskstar.Models;

public class PaginatedBookingsDto
{
  public int AmountOfBookings { get; set; }
  public List<ExtendedBooking> Bookings { get; set; } = null!;
}
