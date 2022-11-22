using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
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

    [HttpGet("range")]
    [Authorize]
    /*
    n -> Amount of Bookings
    skip -> Amount of Bookings we want to skip
    direction / sort -> Sort direction depending on the start time of the bookings
    from -> from that time (when not specified, everything until the end time)
    end -> end or until this the bookings (when not specified, infinite into future)
    */
    public IActionResult GetBookingsByDirection()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var userId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);

        var queryParams = Request.Query.Select(element => element.Key);
        int n;
        string direction;
        int skip;
        DateTime start;
        DateTime end;
        try
        {
            if (queryParams.Contains("n"))
                int.TryParse(Request.Query["n"], out n);
            else
                n = int.MaxValue;

            if (queryParams.Contains("skip"))
                int.TryParse(Request.Query["skip"], out skip);
            else
                skip = 0;

            if (queryParams.Contains("direction"))
                direction = Request.Query["direction"];
            else
                direction = "DESC";

            if (queryParams.Contains("start"))
                DateTime.TryParse(Request.Query["start"], out start);
            else
                start = DateTime.Now;

            if (queryParams.Contains("end"))
                DateTime.TryParse(Request.Query["end"], out end);
            else
                end = DateTime.MaxValue;

            if (n <= 0)
                throw new FormatException("n must be greater or equal than 0");
            if (skip < 0)
                throw new FormatException("skip must not be greater negativ");
            if (direction.ToUpper() != "ASC" && direction.ToUpper() != "DESC")
                throw new FormatException("direction must either be 'ASC' or 'DESC'");

            if (start > end)
            {
                var swap = end;
                end = start;
                start = swap;
            }

        }
        catch (ArgumentNullException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (FormatException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (OverflowException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        var bookings = _bookingUsecases.GetFilteredBookings(userId, n, skip, direction, start, end);
        var mapped = bookings.Select(b => new RecentBooking()
        {
            Timestamp = b.Timestamp,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            BuildingName = b.Desk.Room.Floor.Building.BuildingName,
            FloorName = b.Desk.Room.Floor.FloorName,
            RoomName = b.Desk.Room.RoomName
        });
        return Ok(bookings);
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