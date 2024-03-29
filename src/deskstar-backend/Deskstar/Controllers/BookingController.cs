﻿/**
 * Program
 *
 * Version 1.0
 *
 * 2023-01-03
 *
 * MIT License
 */

using Deskstar.Core;
using Deskstar.Entities;
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
  private readonly IAutoMapperConfiguration _autoMapperConfiguration;
  private readonly IBookingUsecases _bookingUsecases;
  private readonly ILogger<BookingController> _logger;

  public BookingController(ILogger<BookingController> logger, IBookingUsecases bookingUsecases,
    IAutoMapperConfiguration autoMapperConfiguration)
  {
    _logger = logger;
    _bookingUsecases = bookingUsecases;
    _autoMapperConfiguration = autoMapperConfiguration;
  }


  /// <summary>
  ///   Returns a paginated bookings ranging from a start to an end timestamp.
  /// </summary>
  /// <returns>A List of Bookings and the total amount of bookings for the user in JSON Format (can be empty) </returns>
  /// <remarks>
  ///   Sample request:
  ///   Get /bookings?n=100&skip=50&direction=DESC&from=1669021730904&end=1669121730904 with JWT Token
  /// </remarks>
  /// <response code="200">Returns the booking list and amounts of bookings of user</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet]
  [Authorize]
  [ProducesResponseType(typeof(List<ExtendedBooking>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetBookingsByDirection(int n = int.MaxValue, int skip = 0, string direction = "DESC",
    long start = 0, long end = 0)
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
      var amountOfBookings = _bookingUsecases.CountValidBookings(userId, direction, startDateTime, endDateTime);

      var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
      var mapped = bookings.Select(b => mapper.Map<Booking, ExtendedBooking>(b)).ToList();

      var paginated = new PaginatedBookingsDto
      {
        AmountOfBookings = amountOfBookings,
        Bookings = mapped
      };

      return Ok(paginated);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }


  /// <summary>
  ///   Offers next ten Bookings for Token-User
  /// </summary>
  /// <returns>A List of Bookings in JSON Format (can be empty) </returns>
  /// <remarks>
  ///   Sample request:
  ///   Get /bookings/recent with JWT Token
  /// </remarks>
  /// <response code="200">Returns the booking list</response>
  /// <response code="404">User not found</response>
  [HttpGet("recent")]
  [Authorize]
  [ProducesResponseType(typeof(List<ExtendedBooking>), StatusCodes.Status200OK)]
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


  /// <summary>
  ///   Creates a new Booking for Token-User
  /// </summary>
  /// <returns>Created Booking in JSON Format</returns>
  /// <remarks>
  ///   Sample request:
  ///   Post /bookings with JWT Token
  /// </remarks>
  /// <response code="201">Returns the created booking</response>
  /// <response code="404">User not found</response>
  /// <response code="404">Desk not found</response>
  /// <response code="409">Desk is not available at that time</response>
  /// <response code="400">Bad Request</response>
  [HttpPost]
  [Authorize]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [Produces("application/json")]
  public IActionResult CreateBooking([FromBody] BookingRequest bookingRequest)
  {
    if (bookingRequest.StartTime.Equals(DateTime.MinValue) || bookingRequest.EndTime.Equals(DateTime.MinValue) ||
        bookingRequest.DeskId.Equals(Guid.Empty)) return BadRequest("Required fields are missing");

    var userId = RequestInteractions.ExtractIdFromRequest(Request);

    //ToDo: require Frontend to Use Universaltime
    bookingRequest.StartTime = bookingRequest.StartTime.ToLocalTime();
    bookingRequest.EndTime = bookingRequest.EndTime.ToLocalTime();

    try
    {
      _bookingUsecases.CreateBooking(userId, bookingRequest);
      return Ok();
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return e.Message switch
      {
        "User not found" => NotFound(e.Message),
        "Desk not found" => NotFound(e.Message),
        "Time slot not available" => Conflict(e.Message),
        _ => Problem(statusCode: 500)
      };
    }
  }

  /// <summary>
  ///   Deletes a Booking for Token-User
  /// </summary>
  /// <returns>Deleted Booking in JSON Format</returns>
  /// <remarks>
  ///   Sample request:
  ///   Delete /bookings/{bookingId} with JWT Token
  /// </remarks>
  /// <response code="200">Returns the deleted booking</response>
  /// <response code="404">User not found</response>
  /// <response code="404">Booking not found</response>
  /// <response code="403">User is not allowed to delete this booking</response>
  /// <response code="400">Bad Request</response>
  [HttpDelete("{bookingId}")]
  [Authorize]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [Produces("application/json")]
  public IActionResult DeleteBooking(string bookingId)
  {
    var userId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      var booking = _bookingUsecases.DeleteBooking(userId, new Guid(bookingId));
      return Ok(booking);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return e.Message switch
      {
        "User not found" => NotFound(e.Message),
        "Booking not found" => NotFound(e.Message),
        "You are not allowed to delete this booking" => Forbid(e.Message),
        _ => Problem(statusCode: 500)
      };
    }
  }

  /// <summary>
  ///   Updates a Booking for Token-User
  /// </summary>
  /// <returns>Updated Booking in JSON Format</returns>
  /// <remarks>
  ///   Sample request:
  ///   Put /bookings/{bookingId} with JWT Token
  /// </remarks>
  /// <response code="200">Returns the updated booking</response>
  /// <response code="404">User not found</response>
  /// <response code="404">Booking not found</response>
  /// <response code="403">User is not allowed to update this booking</response>
  /// <response code="409">Desk is not available at that time</response>
  /// <response code="400">Bad Request</response>
  [HttpPut("{bookingId}")]
  [Authorize]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [Produces("application/json")]
  public IActionResult UpdateBooking(string bookingId, [FromBody] UpdateBookingRequest updateBookingRequest)
  {
    if (updateBookingRequest.StartTime.Equals(DateTime.MinValue) ||
        updateBookingRequest.EndTime.Equals(DateTime.MinValue)) return BadRequest("Required fields are missing");

    var userId = RequestInteractions.ExtractIdFromRequest(Request);

    // ToDo: require Frontend to Use Universaltime
    updateBookingRequest.StartTime = updateBookingRequest.StartTime.ToLocalTime();
    updateBookingRequest.EndTime = updateBookingRequest.EndTime.ToLocalTime();

    try
    {
      var booking = _bookingUsecases.UpdateBooking(userId, new Guid(bookingId), updateBookingRequest);
      return Ok();
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return e.Message switch
      {
        "User not found" => NotFound(e.Message),
        "Booking not found" => NotFound(e.Message),
        "You are not allowed to update this booking" => Forbid(e.Message),
        "Time slot not available" => Conflict(e.Message),
        _ => Problem(statusCode: 500)
      };
    }
  }
}
