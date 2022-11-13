using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Controllers;

[ApiController]
[Route("/bookings")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingUsecases _bookingUsecases;
    private readonly IConfiguration _configuration;
    private readonly IAuthUsecases _authUsecases;
    
    public BookingController(ILogger<BookingController> logger, IBookingUsecases bookingUsecases,
        IConfiguration configuration, IAuthUsecases authUsecases)
    {
        _logger = logger;
        _bookingUsecases = bookingUsecases;
        _configuration = configuration;
        _authUsecases = authUsecases;
    }

    [HttpGet("recent")]
    [Authorize]
    public IActionResult RecentBookings()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        // TODO get user id when it's in token
        var mailAddress = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;
        return Ok(JsonSerializer.Serialize(_bookingUsecases.GetRecentBookings(mailAddress)));
    }
}