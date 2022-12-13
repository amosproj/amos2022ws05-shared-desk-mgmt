using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Deskstar.Usecases;
using Deskstar.Models;
using Deskstar.Core;

namespace Deskstar.Controllers;

[ApiController]
[Route("/resources")]
[Produces("application/json")]
public class ResourcesController : ControllerBase
{
    private readonly IResourceUsecases _resourceUsecases;
    private readonly IUserUsecases _userUsecases;
    private readonly ILogger<ResourcesController> _logger;

    public ResourcesController(ILogger<ResourcesController> logger, IResourceUsecases resourceUsecases, IUserUsecases userUsecases)
    {
        _logger = logger;
        _resourceUsecases = resourceUsecases;
        _userUsecases = userUsecases;
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
        var userId = RequestInteractions.ExtractIdFromRequest(Request);
        List<CurrentBuilding> buildings;
        try
        {
            buildings = _resourceUsecases.GetBuildings(userId);
        }
        catch (ArgumentException e)
        {
            return Problem(statusCode: 500, detail: e.Message);
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
    [HttpGet("buildings/{buildingId}/floors")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentFloor>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetFloorsByBuildingId(string buildingId)
    {
        List<CurrentFloor> floor;
        try
        {
            floor = _resourceUsecases.GetFloors(new Guid(buildingId));
        }
        catch (ArgumentException e)
        {
            return Problem(statusCode: 500, detail: e.Message);
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
        List<CurrentRoom> rooms;
        try
        {
            rooms = _resourceUsecases.GetRooms(new Guid(floorId));
        }
        catch (ArgumentException e)
        {
            return Problem(statusCode: 500, detail: e.Message);
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
    ///     GET /resources/rooms/3de7afbf-0289-4ba6-bada-a34353c5548a/desks?start=1669021730904&end=1669121730904
    ///     with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the desks list</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("rooms/{roomId}/desks")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentDesk>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDesksByRoomId(string roomId, long start = 0, long end = 0)
    {
        var startDateTime = start == 0 ? DateTime.MinValue : DateTimeOffset.FromUnixTimeMilliseconds(start).DateTime;
        var endDateTime = end == 0 ? DateTime.MaxValue : DateTimeOffset.FromUnixTimeMilliseconds(end).DateTime;
        List<CurrentDesk> desks;
        try
        {
            desks = _resourceUsecases.GetDesks(new Guid(roomId), startDateTime, endDateTime);
        }
        catch (ArgumentException e)
        {
            return Problem(statusCode: 500, detail: e.Message);
        }

        return Ok(desks.ToList());
    }

    /// <summary>
    /// Returns details of Desks.
    /// </summary>
    /// <returns>A List of Desks in JSON Format by RoomId (can be empty) </returns>
    /// <remarks>
    /// Sample request:
    ///     GET /resources/desks/3de7afbf-0289-4ba6-bada-a34353c5548a?start=1669021730904&end=1669121730904
    ///     with JWT Token
    /// </remarks>
    ///
    /// <response code="200">Returns the buildings list</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("desks/{deskId}")]
    [Authorize]
    [ProducesResponseType(typeof(List<CurrentDesk>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetDeskDetailsByDeskId(string deskId, long start = 0, long end = 0)
    {
        DateTime startDateTime;
        DateTime endDateTime;
        try
        {
            startDateTime = start == 0 ? DateTime.Now : DateTimeOffset.FromUnixTimeMilliseconds(start).DateTime;

            endDateTime = end == 0 ? DateTime.MaxValue : DateTimeOffset.FromUnixTimeMilliseconds(end).DateTime;
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

        CurrentDesk desk;
        try
        {
            desk = _resourceUsecases.GetDesk(new Guid(deskId), startDateTime, endDateTime);
        }
        catch (ArgumentException e)
        {
            return Problem(statusCode: 500, detail: e.Message);
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
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("desks")]
    [Authorize(Policy="Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult CreateDesk(CreateDeskDto desk)
    {
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            _resourceUsecases.CreateDesk(desk.DeskName, desk.DeskTypId, desk.RoomId);
            return Ok();
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return Problem(detail: e.Message, statusCode: 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
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

    /// <summary>
    /// Creates a new DeskType.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST /resources/desktypes with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="201"></response>
    /// <response code="400"></response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("desktypes")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult CreateDeskType(CurrentDesk newDesk)
    {
        return Problem(statusCode: 501);
    }
    /// <summary>
    /// Return a list of desk types
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     Get /resources/desktypes with JWT-Admin Token
    /// </remarks>
    ///
    /// <response code="200">List<DeskTypDTO></response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("desktypes")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult ReadDeskTypes()
    {
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;
            var deskTypes = _resourceUsecases.GetDeskTypes(companyId);
            return Ok(deskTypes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
    }
}