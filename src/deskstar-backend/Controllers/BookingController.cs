using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deskstar.Controllers;

[ApiController]
[Route("/bookings")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingUsecases _bookingUsecases;
    private readonly IConfiguration _configuration;


    public BookingController(ILogger<BookingController> logger, IBookingUsecases bookingUsecases, IConfiguration configuration)
    {
        _logger = logger;
        _bookingUsecases = bookingUsecases;
        _configuration = configuration;
    }
    
    [HttpGet("recent")]
    [Authorize]
    public IActionResult RecentBookings()
    {
        return Ok();
    }
}