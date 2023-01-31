using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Deskstar.Usecases;
using Deskstar.Models;
using Deskstar.Core;
using Deskstar.Entities;
using Deskstar.Core.Exceptions;

namespace Deskstar.Controllers;

[ApiController]
[Route("/resources")]
[Produces("application/json")]
public class ResourceController : ControllerBase
{
  private readonly IResourceUsecases _resourceUsecases;
  private readonly IUserUsecases _userUsecases;
  private readonly ILogger<ResourceController> _logger;

  private readonly AutoMapper.IMapper _mapper;

  public ResourceController(ILogger<ResourceController> logger, IResourceUsecases resourceUsecases,
    IUserUsecases userUsecases, IAutoMapperConfiguration autoMapperConfiguration)
  {
    _logger = logger;
    _resourceUsecases = resourceUsecases;
    _userUsecases = userUsecases;
    _mapper = autoMapperConfiguration.GetConfiguration().CreateMapper();
  }
  /// <summary>
  /// Updates a Desk.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     PUT /resources/desks/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPut("desks/{deskId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult UpdateDesk(string deskId, UpdateDeskDto dto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var deskGuid = new Guid(deskId);
      var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;
      Guid? roomId = dto.RoomId == null ? null : new Guid(dto.RoomId);
      Guid? deskTypeId = dto.DeskTypeId == null ? null : new Guid(dto.DeskTypeId);

      var desk = _resourceUsecases.UpdateDesk(companyId, deskGuid, dto.DeskName, roomId, deskTypeId);
      var updatedDesk = _mapper.Map<Desk, UpdateDeskResponseObject>(desk);
      return Ok(updatedDesk);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Updates a Desk Type.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     PUT /resources/desktypes/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPut("desktypes/{desktypeId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult UpdateDeskType(string deskTypeId, UpdateDeskTypeDto dto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var deskTypeGuid = new Guid(deskTypeId);
      var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;

      var deskType = _resourceUsecases.UpdateDeskType(companyId, deskTypeGuid, dto.DeskTypeName);
      var updatedDeskType = _mapper.Map<DeskType, UpdateDeskTypeResponseObject>(deskType);
      return Ok(updatedDeskType);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Updates a Building.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     PUT /resources/buildings/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [HttpPut("buildings/{buildingId}")]
  [Authorize(Policy = "Admin")]
  [Produces("application/json")]
  public IActionResult UpdateBuilding(string buildingId, UpdateBuildingDto dto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var buildingGuid = new Guid(buildingId);
      var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;

      var building = _resourceUsecases.UpdateBuilding(companyId, buildingGuid, dto.BuildingName, dto.Location);
      var updatedBuilding = _mapper.Map<Building, UpdateBuildingResponseObject>(building);
      return Ok(updatedBuilding);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Updates a Floor.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     PUT /resources/floors/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [HttpPut("floors/{floorId}")]
  [Authorize(Policy = "Admin")]
  [Produces("application/json")]
  public IActionResult UpdateFloor(string floorId, UpdateFloorDto dto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var floorGuid = new Guid(floorId);
      var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;
      Guid? buildingGuid = dto.BuildingId == null ? null : new Guid(dto.BuildingId);

      var floor = _resourceUsecases.UpdateFloor(companyId, floorGuid, dto.FloorName, buildingGuid);
      var updatedFloor = _mapper.Map<Floor, UpdateFloorResponseObject>(floor);
      return Ok(updatedFloor);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Updates a Room.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     PUT /resources/rooms/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [HttpPut("rooms/{roomId}")]
  [Authorize(Policy = "Admin")]
  [Produces("application/json")]
  public IActionResult UpdateRoom(string roomId, UpdateRoomDto dto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var roomGuid = new Guid(roomId);
      var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;
      Guid? floorId = dto.FloorId == null ? null : new Guid(dto.FloorId);

      var room = _resourceUsecases.UpdateRoom(companyId, roomGuid, dto.RoomName, floorId);
      var updatedRoom = _mapper.Map<Room, UpdateRoomResponseObject>(room);
      return Ok(updatedRoom);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
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
  /// <response code="400">Bad Request</response>
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
  /// <response code="200">CreateBuildingResponseObject</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("buildings")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult CreateBuilding(CreateBuildingDto buildingDto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;
      var building = _resourceUsecases.CreateBuilding(buildingDto.BuildingName, buildingDto.Location, companyId);
      var resultBuilding = _mapper.Map<Building, CreateBuildingResponseObject>(building);
      return Ok(resultBuilding);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Restores a Building.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     POST /resources/buildings/restore/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("buildings/restore/{buildingId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult RestoreBuilding(string buildingId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.RestoreBuilding(adminId, buildingId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Deletes a Building.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     DELETE /resources/buildings/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpDelete("buildings/{buildingId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult DeleteBuilding(string buildingId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.DeleteBuilding(adminId, buildingId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
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
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("buildings/{buildingId}/floors")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentFloor>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetFloorsByBuildingId(string buildingId)
  {
    var callerId = RequestInteractions.ExtractIdFromRequest(Request);
    List<CurrentFloor> floor;
    try
    {
      floor = _resourceUsecases.GetFloors(callerId, buildingId);
    }
    catch (ArgumentException e)
    {
      return Problem(statusCode: 500, detail: e.Message);
    }

    return Ok(floor.ToList());
  }

  /// <summary>
  /// Returns a list of Floors.
  /// </summary>
  /// <returns>A List of Floors in JSON Format </returns>
  /// <remarks>
  /// Sample request:
  ///     GET /resources/floors with JWT Token
  /// </remarks>
  ///
  /// <response code="200">Returns the floor list</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("floors")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentFloor>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetAllFloors()
  {
    var callerId = RequestInteractions.ExtractIdFromRequest(Request);
    List<CurrentFloor> floor;
    try
    {
      floor = _resourceUsecases.GetFloors(callerId, "");
    }
    catch (ArgumentException e)
    {
      return Problem(statusCode: 500, detail: e.Message);
    }

    return Ok(floor.ToList());
  }

  /// <summary>
  /// Returns a list of all Floors.
  /// </summary>
  /// <returns>A List of Floors in JSON Format (can be empty) </returns>
  /// <remarks>
  /// Sample request:
  ///     GET /resources/floors with JWT Token
  /// </remarks>
  ///
  /// <response code="200">Returns the floor list</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("floors")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentFloor>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetAllFloors()
  {
    List<CurrentFloor> floor;
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      floor = _resourceUsecases.GetAllFloors(adminId);
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
  /// <response code="200">CreateFloorResponseObject</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("floors")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult CreateFloor(CreateFloorDto floorDto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var buildingId = new Guid(floorDto.BuildingId);
      var floor = _resourceUsecases.CreateFloor(floorDto.FloorName, buildingId);
      var resultFloor = _mapper.Map<Floor, CreateFloorResponseObject>(floor);
      return Ok(resultFloor);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Deletes a Floor.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     DELETE /resources/floors/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpDelete("floors/{floorId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult DeleteFloor(string floorId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.DeleteFloor(adminId, floorId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Restores a floor.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     POST /resources/floors/restore/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("floors/restore/{floorID}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult RestoreFloor(string floorID)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.RestoreFloor(adminId, floorID);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
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
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("floors/{floorId}/rooms")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetRoomsByFloorId(string floorId)
  {
    var callerId = RequestInteractions.ExtractIdFromRequest(Request);
    List<CurrentRoom> rooms;
    try
    {
      rooms = _resourceUsecases.GetRooms(callerId, floorId);
    }
    catch (ArgumentException e)
    {
      return Problem(statusCode: 500, detail: e.Message);
    }

    return Ok(rooms.ToList());
  }

  /// <summary>
  /// Returns a list of Rooms.
  /// </summary>
  /// <returns>A List of Rooms in JSON Format </returns>
  /// <remarks>
  /// Sample request:
  ///     GET /resources/rooms with JWT Token
  /// </remarks>
  ///
  /// <response code="200">Returns the rooms list</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("rooms")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetAllRooms()
  {
    var callerId = RequestInteractions.ExtractIdFromRequest(Request);
    List<CurrentRoom> rooms;
    try
    {
      rooms = _resourceUsecases.GetRooms(callerId, "");
    }
    catch (ArgumentException e)
    {
      return Problem(statusCode: 500, detail: e.Message);
    }

    return Ok(rooms.ToList());
  }

  /// <summary>
  /// Returns a list of all Rooms.
  /// </summary>
  /// <returns>A List of Rooms in JSON Format (can be empty) </returns>
  /// <remarks>
  /// Sample request:
  ///     GET /resources/rooms with JWT Token
  /// </remarks>
  ///
  /// <response code="200">Returns the rooms list</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("rooms")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentRoom>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetAllRooms()
  {
    List<CurrentRoom> rooms;
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      rooms = _resourceUsecases.GetAllRooms(adminId);
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
  /// <response code="200">CreateRoomResponseObject</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("rooms")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult CreateRoom(CreateRoomDto roomDto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var floorId = new Guid(roomDto.FloorId);
      var room = _resourceUsecases.CreateRoom(roomDto.RoomName, floorId);
      var resultRoom = _mapper.Map<Room, CreateRoomResponseObject>(room);
      return Ok(resultRoom);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Deletes a Room.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     DELETE /resources/rooms/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpDelete("rooms/{roomId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult DeleteRoom(string roomId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.DeleteRoom(adminId, roomId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Restores a room.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     POST /resources/rooms/restore/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("rooms/restore/{roomId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult RestoreRoom(string roomId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.RestoreRoom(adminId, roomId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
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
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("rooms/{roomId}/desks")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentDesk>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetDesksByRoomId(string roomId, long start = 0, long end = 0)
  {
    var callerId = RequestInteractions.ExtractIdFromRequest(Request);
    var startDateTime = start == 0 ? DateTime.MinValue : DateTimeOffset.FromUnixTimeMilliseconds(start).DateTime;
    var endDateTime = end == 0 ? DateTime.MaxValue : DateTimeOffset.FromUnixTimeMilliseconds(end).DateTime;
    List<CurrentDesk> desks;
    try
    {
      desks = _resourceUsecases.GetDesks(callerId, roomId, startDateTime, endDateTime);
    }
    catch (ArgumentException e)
    {
      return Problem(statusCode: 500, detail: e.Message);
    }

    return Ok(desks.ToList());
  }

  /// <summary>
  /// Returns a list of Desks.
  /// </summary>
  /// <returns>A List of Desks in JSON Format </returns>
  /// <remarks>
  /// Sample request:
  ///     GET /resources/desks
  ///     with JWT Token
  /// </remarks>
  ///
  /// <response code="200">Returns the desks list</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("desks")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentDesk>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetAllDesks()
  {
    var callerId = RequestInteractions.ExtractIdFromRequest(Request);
    List<CurrentDesk> desks;
    try
    {
      desks = _resourceUsecases.GetDesks(callerId, "", DateTime.MinValue, DateTime.MaxValue);
    }
    catch (ArgumentException e)
    {
      return Problem(statusCode: 500, detail: e.Message);
    }

    return Ok(desks.ToList());
  }

  /// <summary>
  /// Returns a list of all Desks.
  /// </summary>
  /// <returns>A List of Desks in JSON Format by RoomId (can be empty) </returns>
  /// <remarks>
  /// Sample request:
  ///     GET /resources/desks with JWT Token
  /// </remarks>
  ///
  /// <response code="200">Returns the desks list</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("desks")]
  [Authorize]
  [ProducesResponseType(typeof(List<CurrentDesk>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetAllDesks()
  {
    List<CurrentDesk> desks;
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      desks = _resourceUsecases.GetAllDesks(adminId);
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
  /// <response code="200">CreateDeskResponseObject</response>
  /// <response code="400">Bad Request</response>
  /// <response code="404">Not Found</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("desks")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult CreateDesk(CreateDeskDto deskDto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);

    try
    {
      var deskTypeId = new Guid(deskDto.DeskTypeId);
      var roomId = new Guid(deskDto.RoomId);
      var desk = _resourceUsecases.CreateDesk(deskDto.DeskName, deskTypeId, roomId);
      var resultDesk = _mapper.Map<Desk, CreateDeskResponseObject>(desk);
      return Ok(resultDesk);
    }
    catch (EntityNotFoundException e)
    {
      _logger.LogError(e, e.Message);
      return NotFound(e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
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
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpDelete("desks/{deskId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult DeleteDesk(string deskId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.DeleteDesk(adminId, deskId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Restores a Desk.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     POST /resources/desks/restore/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("desks/restore/{deskId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult RestoreDesk(string deskId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.RestoreDesk(adminId, deskId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Creates a new DeskType.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     POST /resources/desktypes with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">CreateDeskTypeResponseObject</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("desktypes")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult CreateDeskType(CreateDeskTypeDto deskTypeDto)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      var companyId = _userUsecases.ReadSpecificUser(adminId).CompanyId;
      var deskType = _resourceUsecases.CreateDeskType(deskTypeDto.DeskTypeName, companyId);
      var resultDeskType = _mapper.Map<DeskType, CreateDeskTypeResponseObject>(deskType);
      return Ok(resultDeskType);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Return a list of desk types
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     Get /resources/desktypes with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">List<DeskTypeDto></response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet("desktypes")]
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
      var entities = _resourceUsecases.GetDeskTypes(companyId);
      var deskTypes = entities.Select<DeskType, DeskTypeDto>(desktype => _mapper.Map<DeskType, DeskTypeDto>(desktype))
        .ToList();
      return Ok(deskTypes);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Deletes a desk type.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     DELETE /resources/desktypes/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpDelete("desktypes/{deskTypeId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult DeleteDeskType(string deskTypeId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.DeleteDeskType(adminId, deskTypeId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }

  /// <summary>
  /// Restores a deskType.
  /// </summary>
  /// <remarks>
  /// Sample request:
  ///     POST /resources/desktypes/restore/3de7afbf-0289-4ba6-bada-a34353c5548a with JWT-Admin Token
  /// </remarks>
  ///
  /// <response code="200">Ok</response>
  /// <response code="400">Bad Request</response>
  /// <response code="500">Internal Server Error</response>
  [HttpPost("desktypes/restore/{deskTypeId}")]
  [Authorize(Policy = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult RestoreDeskType(string deskTypeId)
  {
    var adminId = RequestInteractions.ExtractIdFromRequest(Request);
    try
    {
      _resourceUsecases.RestoreDeskType(adminId, deskTypeId);
      return Ok();
    }
    catch (EntityNotFoundException e)
    {
      return Problem(statusCode: 404, detail: e.Message);
    }
    catch (Exception e) when (e is ArgumentInvalidException or ArgumentNullException or FormatException
                                or OverflowException)
    {
      _logger.LogError(e, e.Message);
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      _logger.LogError(e, e.Message);
      return Problem(statusCode: 500);
    }
  }
}