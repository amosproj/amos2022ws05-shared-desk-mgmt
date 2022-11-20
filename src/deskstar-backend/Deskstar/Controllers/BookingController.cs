using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Deskstar.Entities;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Controllers;

[ApiController]
[Route("/bookings")]
[Produces("application/json")]
public class BookingController : ControllerBase
{
    private readonly IBookingUsecases _bookingUsecases;
    private readonly ILogger<BookingController> _logger;

    public BookingController(ILogger<BookingController> logger, IBookingUsecases bookingUsecases)
    {
        _logger = logger;
        _bookingUsecases = bookingUsecases;
    }
    
    /// <summary>
    /// Offers next ten Bookings for Token-User
    /// </summary>
    /// <returns>A List of Bookings in JSON Format (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     Get /bookings/recent with JWT Token
    /// </remarks>
    /// 
    /// <response code="200">Returns the booking list</response>
    /// <response code="404">User not found</response>
    [HttpGet("recent")]
    [Authorize]
    [ProducesResponseType(typeof(RecentBooking),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
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
        return NotFound();
    }
}