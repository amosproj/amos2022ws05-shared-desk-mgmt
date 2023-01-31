using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Helper;
using Deskstar.Models;
using Microsoft.EntityFrameworkCore;
namespace Deskstar.Usecases;

public interface IBookingUsecases
{
  public List<Booking> GetFilteredBookings(Guid userId, int n, int skip, string direction, DateTime start, DateTime end);
  public List<ExtendedBooking> GetRecentBookings(Guid userId);
  public Booking CreateBooking(Guid userId, BookingRequest bookingRequest);
  int CountValidBookings(Guid userId, string direction, DateTime start, DateTime end);
  public Booking DeleteBooking(Guid userId, Guid bookingId);
  public Booking UpdateBooking(Guid userId, Guid bookingId, UpdateBookingRequest updateBookingRequest);
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

    var mapBookingsToRecentBookings = withReferences.Select(b => new ExtendedBooking
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

  public Booking CreateBooking(Guid userId, BookingRequest bookingRequest)
  {
    // check if desk exists
    var desk = _context.Desks.FirstOrDefault(d => d.DeskId == bookingRequest.DeskId);
    if (desk == null)
    {
      // throw an exception that deks was not found with error code 404
      throw new ArgumentException("Desk not found");
    }
    if (desk.IsMarkedForDeletion)
    {
      throw new ArgumentException("Desk is marked for deletion");
    }

    // check if user exists
    var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
    if (user == null)
    {
      throw new ArgumentException("User not found");
    }

    // check if desk availabe
    var bookings = _context.Bookings.Where(b => b.DeskId == bookingRequest.DeskId);
    var timeSlotAvailable = bookings.All(b => b.StartTime >= bookingRequest.EndTime || b.EndTime <= bookingRequest.StartTime);
    if (!timeSlotAvailable)
    {
      throw new ArgumentException("Time slot not available");
    }
    var booking = new Booking
    {
      BookingId = Guid.NewGuid(),
      UserId = userId,
      DeskId = bookingRequest.DeskId,
      StartTime = bookingRequest.StartTime,
      EndTime = bookingRequest.EndTime,
      Timestamp = DateTime.Now
    };

    _context.Bookings.Add(booking);
    _context.SaveChanges();
    var body = $"Hello {user.FirstName},</br> " +
               $"you have successfully booked desk {booking.Desk.DeskName}.</br> " +
               $"Your booked timeslot is from {booking.StartTime} till {booking.EndTime}.</br>" +
               "Have a great day in the office!</br>";
    EmailHelper.SendEmail(_logger, user.MailAddress, $"Booking of desk {booking.Desk.DeskName} successful!", body);
    return booking;
  }

  public int CountValidBookings(Guid userId, string direction, DateTime start, DateTime end)
  {
    var allBookingsFromUser = _context.Bookings.Where(booking => booking.UserId == userId);
    var filteredEnd = allBookingsFromUser.Where(b => b.StartTime < end);
    var filteredStart = filteredEnd.Where(b => b.StartTime >= start);
    var sortedBookings = direction.ToUpper() == "ASC" ? filteredStart.OrderBy(bookings => bookings.StartTime) : filteredStart.OrderByDescending(b => b.StartTime);

    return sortedBookings.Count();
  }
  public Booking DeleteBooking(Guid userId, Guid bookingId)
  {
    var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
    if (user == null)
    {
      throw new ArgumentException("User not found");
    }

    var booking = _context.Bookings.Include(b => b.Desk).ThenInclude(b => b.user).FirstOrDefault(b => b.BookingId == bookingId);
    if (booking == null)
    {
      throw new ArgumentException("Booking not found");
    }

    if (booking.UserId != userId)
    {
      throw new ArgumentException("You are not allowed to delete this booking");
    }

    _context.Bookings.Remove(booking);
    _context.SaveChanges();

    var body = $"Hello {user.FirstName},</br> " +
               $"you have successfully canceled your booking of desk {booking.Desk.DeskName} " +
               $"form {booking.StartTime} till {booking.EndTime}.</br>" +
               "Have a great day in the office!</br>";
    EmailHelper.SendEmail(_logger, user.MailAddress, $"Cancellation of booking of desk {booking.Desk.DeskName} successful!", body);
    return booking;
  }

  public Booking UpdateBooking(Guid userId, Guid bookingId, UpdateBookingRequest updateBookingRequest)
  {
    var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
    if (user == null)
    {
      throw new ArgumentException("User not found");
    }

    var booking = _context.Bookings.Include(b => b.Desk).ThenInclude(b => b.user).FirstOrDefault(b => b.BookingId == bookingId);
    if (booking == null)
    {
      throw new ArgumentException("Booking not found");
    }

    if (booking.UserId != userId)
    {
      throw new ArgumentException("You are not allowed to update this booking");
    }

    var bookings = _context.Bookings.Where(b => b.DeskId == booking.DeskId && b.BookingId != bookingId);
    var timeSlotAvailable = bookings.All(b => b.StartTime >= updateBookingRequest.EndTime || b.EndTime <= updateBookingRequest.StartTime);
    if (!timeSlotAvailable)
    {
      throw new ArgumentException("Time slot not available");
    }

    booking.StartTime = updateBookingRequest.StartTime;
    booking.EndTime = updateBookingRequest.EndTime;
    booking.Timestamp = DateTime.Now;

    _context.Bookings.Update(booking);
    _context.SaveChanges();

    var body = $"Hello {user.FirstName},</br> " +
               $"you have successfully edit your booking of desk {booking.Desk.DeskName}.</br> " +
               $"Your new timeslot is from {booking.StartTime} till {booking.EndTime}.</br>" +
               "Have a great day in the office!</br>";
    EmailHelper.SendEmail(_logger, user.MailAddress, $"Booking of desk {booking.Desk.DeskName} successful!", body);
    return booking;
  }
}
