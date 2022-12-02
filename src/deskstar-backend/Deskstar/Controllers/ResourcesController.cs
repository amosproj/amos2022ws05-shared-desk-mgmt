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
    public IActionResult GetBuildings()
    {
        return Problem(statusCode: 501);
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
    public IActionResult GetFloorsByBuildingId(Guid buildingId)
    {
        return Problem(statusCode: 501);
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
    [HttpGet("buildings/{buildingId}/floor")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetRoomsByFloorId(Guid FloorId)
    {
        return Problem(statusCode: 501);
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
    [HttpGet("buildings/{buildingId}/floor")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDesksByRoomId(Guid RoomId)
    {
        return Problem(statusCode: 501);
    }

    /// <summary>
    /// Returns details of Desks.
    /// </summary>
    /// <returns>A List of Desks in JSON Format by RoomId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/desks/{deskId}?from=&to= with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("/resources/desks/{deskId}")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDeskDetailsByRoomId(Guid RoomId)
    {
        return Problem(statusCode: 501);
    }
}