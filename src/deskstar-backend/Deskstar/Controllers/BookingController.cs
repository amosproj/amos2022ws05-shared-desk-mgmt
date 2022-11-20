using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Controllers;

[ApiController]
[Route("/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingUsecases _bookingUsecases;
    private readonly ILogger<BookingController> _logger;

    public BookingController(ILogger<BookingController> logger, IBookingUsecases bookingUsecases)
    {
        _logger = logger;
        _bookingUsecases = bookingUsecases;
    }

    [HttpGet("recent")]
    [Authorize]
    public IActionResult RecentBookings()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var userId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);
        try
        {
            var bookings = _bookingUsecases.GetRecentBookings(userId);
            return Ok(JsonSerializer.Serialize(bookings));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }

        return null;
    }
}