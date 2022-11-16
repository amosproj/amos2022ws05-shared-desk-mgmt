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

    [HttpGet("range")]
    //[Authorize]
    /*
    n -> Amount of Bookings
    skip -> Amount of Bookings we want to skip
    direction / sort -> Sort direction depending on the start time of the bookings
    from -> from that time (when not specified, everything until the end time)
    end -> end or until this the bookings (when not specified, infinite into future)
    */
    public IActionResult GetBookingsByDirection()
    {
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
            if (start > end)
                throw new FormatException("from must be before end");
            if (direction.ToUpper() != "ASC" && direction.ToUpper() != "DESC")
                throw new FormatException("direction must either be 'ASC' or 'DESC'");

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
        var bookings = _bookingUsecases.GetFilteredBookings(n,skip,direction,start,end);
        return Ok(bookings);
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
        try
        {
            var bookings = _bookingUsecases.GetRecentBookings(mailAddress);
            return Ok(JsonSerializer.Serialize(bookings));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }

        return null;
    }
}