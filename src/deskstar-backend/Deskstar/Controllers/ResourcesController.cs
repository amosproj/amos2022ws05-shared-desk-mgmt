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
        var userId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);
        var buildings = _resourceUsecases.GetBuildings(userId);
        if (buildings.Count==0)
        {
            return Problem(statusCode: 500);
        }
        return Ok(buildings.ToList());
    }

    /// <summary>
    /// Returns a list of Floors.
    /// </summary>
    /// <returns>A List of Floors in JSON Format by BuildingId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/buildings/{buildingId}/floors with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("buildings/{buildingId}/floor")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentFloor>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetFloorsByBuildingId(string buildingId)
    {
        var floor = _resourceUsecases.GetFloors(new Guid(buildingId));
        if (floor.Count==0)
        {
            return Problem(statusCode: 500);
        }
        return Ok(floor.ToList());
    }

    /// <summary>
    /// Returns a list of Rooms.
    /// </summary>
    /// <returns>A List of Rooms in JSON Format by FloorId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/floors/{floorId}/rooms with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("floors/{floorId}/rooms")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetRoomsByFloorId(string floorId)
    {
        var rooms = _resourceUsecases.GetRooms(new Guid(floorId));
        if (rooms.Count==0)
        {
            return Problem(statusCode: 500);
        }
        return Ok(rooms.ToList());
    }

    /// <summary>
    /// Returns a list of Desks.
    /// </summary>
    /// <returns>A List of Desks in JSON Format by RoomId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/rooms/{roomId}/desks with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("rooms/{roomId}/desks")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDesksByRoomId(string roomId)
    {
        var desks = _resourceUsecases.GetDesks(new Guid(roomId));
        if (desks.Count==0)
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
    ///     GET /resources/desks/{deskId}?from=2&to=5 with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("desks/{deskId}")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDeskDetailsByDeskId(string deskId)
    {
        return Problem(statusCode: 501);
    }
}