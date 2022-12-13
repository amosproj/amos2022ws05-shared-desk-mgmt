﻿using Deskstar.Core;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Deskstar.Controllers;

[ApiController]
[Route("/bookings")]
[Produces("application/json")]
public class BookingController : ControllerBase
{
    private readonly IBookingUsecases _bookingUsecases;
    private readonly ILogger<BookingController> _logger;
    private readonly IAutoMapperConfiguration _autoMapperConfiguration;

    public BookingController(ILogger<BookingController> logger, IBookingUsecases bookingUsecases, IAutoMapperConfiguration autoMapperConfiguration)
    {
        _logger = logger;
        _bookingUsecases = bookingUsecases;
        _autoMapperConfiguration = autoMapperConfiguration;
    }


    /// <summary>
    /// Returns a list of paginated bookings ranging from a start to an end timestamp.
    /// </summary>
    /// <returns>A List of Bookings in JSON Format (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     Get /bookings/range?n=100&skip=50&direction=DESC&from=1669021730904&end=1669121730904 with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the booking list</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("range")]
    [Authorize]
    [ProducesResponseType(typeof(List<RecentBooking>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetBookingsByDirection(int n = int.MaxValue, int skip = 0, string direction = "DESC", long start = 0, long end = 0)
    {
        var userId = RequestInteractions.ExtractIdFromRequest(Request);

        DateTime startDateTime;
        DateTime endDateTime;

        try
        {
            if (start == 0)
                startDateTime = DateTime.Now;
            else
                startDateTime = DateTimeOffset.FromUnixTimeMilliseconds(start).DateTime;

            if (end == 0)
                endDateTime = DateTime.MaxValue;
            else
                endDateTime = DateTimeOffset.FromUnixTimeMilliseconds(end).DateTime;

            if (n < 0)
                throw new FormatException("n must be greater than zero");
            if (skip < 0)
                throw new FormatException("skip must be zero or greater");
            if (direction.ToUpper() != "ASC" && direction.ToUpper() != "DESC")
                throw new FormatException("direction must either be 'ASC' or 'DESC'");

            if (start > end)
            {
                var swap = end;
                end = start;
                start = swap;
            }

        }
        catch (FormatException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        try
        {
            var bookings = _bookingUsecases.GetFilteredBookings(userId, n, skip, direction, startDateTime, endDateTime);

            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var mapped = bookings.Select((b) => mapper.Map<Entities.Booking, RecentBooking>(b)).ToList();

            return Ok(mapped);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
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
    [ProducesResponseType(typeof(List<RecentBooking>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public IActionResult RecentBookings()
    {
        var userId = RequestInteractions.ExtractIdFromRequest(Request);
        try
        {
            var bookings = _bookingUsecases.GetRecentBookings(userId);
            return Ok(bookings);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
        return NotFound();
    }
}
