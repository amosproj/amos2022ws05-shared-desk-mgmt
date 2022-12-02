using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Deskstar.Usecases;
using Deskstar.Models;

namespace Deskstar.Controllers;

[ApiController]
[Route("/resources")]
[Produces("text/plain")]
public class ResourcesController : ControllerBase
{
    private readonly IResourceUsecases _resourceUsecases;
    private readonly ILogger<ResourcesController> _logger;

    public ResourcesController(ILogger<ResourcesController> logger, IResourceUsecases resourceUsecases)
    {
        _logger = logger;
        _resourceUsecases = resourceUsecases;
    }

    /// <summary>
    /// Returns a list of Buildings.
    /// </summary>
    /// <returns>A List of Buildings in JSON Format (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/buildings with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("buildings")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentBuilding>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetAllBuildings()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var userId =
            new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);
        var buildings = _resourceUsecases.GetBuildings(userId);
        if (buildings.Count == 0)
        {
            return Problem(statusCode: 500);
        }

        return Ok(buildings.ToList());
    }

    /// <summary>
    /// Creates a new Building.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST /resources/buildings with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="205"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("buildings")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult CreateBuilding(string buildingId)
    {
        return Problem(statusCode: 501);
    }
    
    /// <summary>
    /// Deletes a Building.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     DELETE /resources/buildings/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="205"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpDelete("buildings/{buildingId}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status205ResetContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult DeleteBuilding(string buildingId)
    {
        return Problem(statusCode: 501);
    }


    /// <summary>
    /// Returns a list of Floors.
    /// </summary>
    /// <returns>A List of Floors in JSON Format by BuildingId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/buildings/3de7afbf-0289-4ba6-bada-a34353c5548a/floors with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the floor list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("buildings/{buildingId}/floor")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentFloor>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetFloorsByBuildingId(string buildingId)
    {
        var floor = _resourceUsecases.GetFloors(new Guid(buildingId));
        if (floor.Count == 0)
        {
            return Problem(statusCode: 500);
        }

        return Ok(floor.ToList());
    }
    
    /// <summary>
    /// Creates a new Floor.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST /resources/floors with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="201"></response>
    /// <response code="409"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("floors")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult CreateFloor(CurrentFloor newFloor)
    {
        return Problem(statusCode: 501);
    }
    
    /// <summary>
    /// Deletes a Floor.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     DELETE /resources/floors/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="205"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpDelete("floors/{floorId}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status205ResetContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult DeleteFloor(string floorId)
    {
        return Problem(statusCode: 501);
    }

    /// <summary>
    /// Returns a list of Rooms.
    /// </summary>
    /// <returns>A List of Rooms in JSON Format by FloorId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/floors/3de7afbf-0289-4ba6-bada-a34353c5548a/rooms with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the rooms list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("floors/{floorId}/rooms")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetRoomsByFloorId(string floorId)
    {
        var rooms = _resourceUsecases.GetRooms(new Guid(floorId));
        if (rooms.Count == 0)
        {
            return Problem(statusCode: 500);
        }

        return Ok(rooms.ToList());
    }
    
    /// <summary>
    /// Creates a new Room.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST /resources/rooms with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="201"></response>
    /// <response code="409"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("rooms")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult CreateRoom(CurrentRoom newRoom)
    {
        return Problem(statusCode: 501);
    }
    
    /// <summary>
    /// Deletes a Room.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     DELETE /resources/rooms/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="205"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpDelete("rooms/{roomId}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status205ResetContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult DeleteRoom(string roomId)
    {
        return Problem(statusCode: 501);
    }

    /// <summary>
    /// Returns a list of Desks.
    /// </summary>
    /// <returns>A List of Desks in JSON Format by RoomId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/rooms/3de7afbf-0289-4ba6-bada-a34353c5548a/desks with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the desks list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("rooms/{roomId}/desks")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDesksByRoomId(string roomId)
    {
        var desks = _resourceUsecases.GetDesks(new Guid(roomId));
        if (desks.Count == 0)
        {
            return Problem(statusCode: 500);
        }

        return Ok(desks.ToList());
    }

    /// <summary>
    /// Returns details of Desks.
    /// </summary>
    /// <returns>A List of Desks in JSON Format by RoomId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/desks/3de7afbf-0289-4ba6-bada-a34353c5548a?start=1669021730904&end=1669121730904 with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("desks/{deskId}")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDeskDetailsByDeskId(string deskId, long start = 0, long end = 0)
    {
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
            if (start > end)
            {
                (endDateTime, startDateTime) = (startDateTime, endDateTime);
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

        var desk = _resourceUsecases.GetDesk(new Guid(deskId), startDateTime, endDateTime);
        if (desk == null)
        {
            return Problem(statusCode: 500);
        }

        return Ok(desk);
    }
    
    /// <summary>
    /// Creates a new Desk.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST /resources/desks with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="201"></response>
    /// <response code="409"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("desks")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult CreateDesk(CurrentDesk newDesk)
    {
        return Problem(statusCode: 501);
    }
    
    /// <summary>
    /// Deletes a Desk.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     DELETE /resources/desks/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="205"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpDelete("desks/{deskId}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status205ResetContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult DeleteDesk(string deskId)
    {
        return Problem(statusCode: 501);
    }
}