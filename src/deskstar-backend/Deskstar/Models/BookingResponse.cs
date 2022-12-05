using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public enum BookingReturn
{
    Ok,
    UserNotFound,
    DeskNotFound,
    TimeSlotNotAvailable,
}

public class BookingResponse
{
    [Required]
    public BookingReturn Message { get; set; }
    public ExtendedBooking? Booking { get; set; }
}