using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Helper;
using Deskstar.Models;
using Microsoft.EntityFrameworkCore;

namespace Deskstar.Usecases;

public interface IResourceUsecases
{
  public List<DeskType> GetDeskTypes(Guid companyId);
  public List<CurrentBuilding> GetBuildings(Guid userId);
  public List<CurrentFloor> GetFloors(Guid callerId, string buildingId);
  public List<CurrentFloor> GetAllFloors(Guid userId);
  public List<CurrentRoom> GetRooms(Guid callerId, string floorId);

  public List<CurrentRoom> GetAllRooms(Guid userId);
  public List<CurrentDesk> GetDesks(Guid callerId, string roomId, DateTime start, DateTime end);
  public List<CurrentDesk> GetAllDesks(Guid userId);
  public CurrentDesk GetDesk(Guid deskId, DateTime startDateTime, DateTime endDateTime);

  public Desk CreateDesk(string deskName, Guid deskTypeId, Guid roomId);
  public DeskType CreateDeskType(string deskTypeName, Guid companyId);
  public Room CreateRoom(string roomName, Guid floorId);
  public Floor CreateFloor(string floorName, Guid buildingId);
  public Building CreateBuilding(string buildingName, string location, Guid companyId);

  public void DeleteDesk(Guid adminId, string deskId);
  public void DeleteDeskType(Guid adminId, string deskId);
  public void DeleteRoom(Guid adminId, string roomId);
  public void DeleteFloor(Guid adminId, string floorId);
  public void DeleteBuilding(Guid adminId, string buildingId);

  public Building UpdateBuilding(Guid companyId, Guid buildingGuid, string? buildingName, string? location);
  public Floor UpdateFloor(Guid companyId, Guid floorId, string? floorName, Guid? buildingId);
  public Room UpdateRoom(Guid companyId, Guid roomId, string? roomName, Guid? floorId);
  public Desk UpdateDesk(Guid companyId, Guid deskId, string? deskName, Guid? roomId, Guid? deskTypeId);
  public DeskType UpdateDeskType(Guid companyId, Guid deskTypeId, string? deskTypeName);
  public void RestoreDesk(Guid adminId, string deskId);
  public void RestoreDeskType(Guid adminId, string deskTypeId);
  public void RestoreRoom(Guid adminId, string roomId);
  public void RestoreFloor(Guid adminId, string floorId);
  public void RestoreBuilding(Guid adminId, string buildingId);
}

public class ResourceUsecases : IResourceUsecases
{
  private readonly DataContext _context;
  private readonly ILogger<ResourceUsecases> _logger;

  public ResourceUsecases(ILogger<ResourceUsecases> logger, DataContext context)
  {
    _logger = logger;
    _context = context;
  }
  public Building UpdateBuilding(Guid companyId, Guid buildingId, string? buildingName, string? location)
  {
    var buildingExists = _context.Buildings.Include(b => b.Company).ThenInclude(c => c.Buildings).SingleOrDefault(b => b.BuildingId == buildingId);
    if (buildingExists == null)
      throw new EntityNotFoundException($"There is no building with id '{buildingId}'");

    if (buildingExists.CompanyId != companyId)
      throw new InsufficientPermissionException($"Your company has no access to administrate building '{buildingExists.BuildingName}'");

    //change location
    if (location != null)
    {
      if (location == "")
        throw new ArgumentInvalidException($"Location must not be empty");
      buildingExists.Location = location;
    }

    //change building name
    if (buildingName != null)
    {
      if (buildingName == "")
        throw new ArgumentInvalidException($"Building name must not be empty");
      var buildingNameIsUnique = buildingExists.Company.Buildings.Select(b => b.BuildingName).All(name => name != buildingName);
      if (!buildingNameIsUnique)
        throw new ArgumentInvalidException($"There is already a building named '{buildingName}' in your company");

      buildingExists.BuildingName = buildingName;
    }

    _context.Buildings.Update(buildingExists);
    _context.SaveChanges();

    return buildingExists;
  }

  public Floor UpdateFloor(Guid companyId, Guid floorId, string? floorName, Guid? buildingId)
  {
    var floorExists = _context.Floors.Include(f => f.Building).SingleOrDefault(f => f.FloorId == floorId);
    if (floorExists == null)
      throw new EntityNotFoundException($"There is no floor with id '{floorId}'");

    if (floorExists.Building.CompanyId != companyId)
      throw new InsufficientPermissionException($"Your company has no access to administrate floor '{floorExists.FloorName}'");

    //change building
    if (buildingId != null)
    {
      var buildingExists = _context.Buildings.SingleOrDefault(b => b.BuildingId == (Guid)buildingId);
      if (buildingExists == null)
        throw new EntityNotFoundException($"Building does not exist with id '{(Guid)buildingId}'");
      if (buildingExists.CompanyId != companyId)
        throw new InsufficientPermissionException($"Your company has no access to move a floor to building '{buildingExists.BuildingName}'");
      var floorNameToBeChecked = floorName != null ? floorName : floorExists.FloorName;
      var floorNameExists = _context.Floors.SingleOrDefault(f => f.BuildingId == buildingId && f.FloorName == floorNameToBeChecked);
      if (floorNameExists != null)
        throw new ArgumentInvalidException($"You cant move floor '{floorExists.FloorName}' to building '{buildingExists.BuildingName}'. In building '{buildingExists.BuildingName}' already exists a floor called '{floorNameToBeChecked}'");

      floorExists.BuildingId = (Guid)buildingId;
    }

    //change floorName
    if (floorName != null)
    {
      if (floorName == "")
        throw new ArgumentInvalidException($"Floor name must not be empty");
      if (buildingId == null)
      {
        var floorNameIsUnique = floorExists.Building.Floors.Select(f => f.FloorName).All(name => name != floorName);
        if (!floorNameIsUnique)
          throw new ArgumentInvalidException($"There is already a floor named '{floorName}' in building '{floorExists.Building.BuildingName}'");
      }

      floorExists.FloorName = floorName;
    }

    _context.Floors.Update(floorExists);
    _context.SaveChanges();

    return floorExists;
  }

  public Room UpdateRoom(Guid companyId, Guid roomId, string? roomName, Guid? floorId)
  {
    var roomExists = _context.Rooms.Include(r => r.Floor).ThenInclude(f => f.Building).SingleOrDefault(r => r.RoomId == roomId);
    if (roomExists == null)
      throw new EntityNotFoundException($"There is no room with id '{roomId}'");

    if (roomExists.Floor.Building.CompanyId != companyId)
      throw new InsufficientPermissionException($"Your company has no access to administrate room '{roomExists.RoomName}'");

    //change floor
    if (floorId != null)
    {
      var floorExists = _context.Floors.Include(f => f.Building).SingleOrDefault(f => f.FloorId == (Guid)floorId);
      if (floorExists == null)
        throw new EntityNotFoundException($"Floor does not exist with id '{(Guid)floorId}'");
      if (floorExists.Building.CompanyId != companyId)
        throw new InsufficientPermissionException($"Your company has no access to move a room to floor '{floorExists.FloorName}'");
      var roomNameToBeChecked = roomName != null ? roomName : roomExists.RoomName;
      var roomNameExists = _context.Rooms.SingleOrDefault(r => r.FloorId == floorId && r.RoomName == roomNameToBeChecked);
      if (roomNameExists != null)
        throw new ArgumentInvalidException($"You cant move room '{roomExists.RoomName}' to floor '{floorExists.FloorName}'. In floor '{floorExists.FloorName}' already exists a room called '{roomNameToBeChecked}'");

      roomExists.FloorId = (Guid)floorId;
    }

    //change roomName
    if (roomName != null)
    {
      if (roomName == "")
        throw new ArgumentInvalidException($"Room name must not be empty");
      if (floorId == null)
      {
        var roomNameIsUnique = roomExists.Floor.Rooms.Select(r => r.RoomName).All(name => name != roomName);
        if (!roomNameIsUnique)
          throw new ArgumentInvalidException($"There is already a room named '{roomName}' in floor '{roomExists.Floor.FloorName}'");
      }

      roomExists.RoomName = roomName;
    }

    _context.Rooms.Update(roomExists);
    _context.SaveChanges();

    return roomExists;
  }

  public Desk UpdateDesk(Guid companyId, Guid deskId, string? deskName, Guid? roomId, Guid? deskTypeId)
  {
    var deskExists = _context.Desks.Include(d => d.DeskType).Include(d => d.Room).ThenInclude(r => r.Floor).ThenInclude(f => f.Building).SingleOrDefault(d => d.DeskId == deskId);
    if (deskExists == null)
      throw new EntityNotFoundException($"There is no desk with id '{deskId}'");

    if (deskExists.Room.Floor.Building.CompanyId != companyId)
      throw new InsufficientPermissionException($"Your company has no access to administrate desk '{deskExists.DeskName}'");

    //change room
    if (roomId != null)
    {
      var roomExists = _context.Rooms.Include(r => r.Floor).ThenInclude(f => f.Building).SingleOrDefault(r => r.RoomId == (Guid)roomId);
      if (roomExists == null)
        throw new EntityNotFoundException($"Room does not exist with id '{(Guid)roomId}'");
      if (roomExists.Floor.Building.CompanyId != companyId)
        throw new InsufficientPermissionException($"Your company has no access to add a desk to room '{roomExists.RoomName}'");
      var deskNameToBeChecked = deskName != null ? deskName : deskExists.DeskName;
      var deskNameExists = _context.Desks.SingleOrDefault(d => d.RoomId == roomId && d.DeskName == deskNameToBeChecked);
      if (deskNameExists != null)
        throw new ArgumentInvalidException($"You cant move desk '{deskExists.DeskName}' to room '{roomExists.RoomName}'. In room '{roomExists.RoomName}' already exists a desk called '{deskNameToBeChecked}'");

      deskExists.RoomId = (Guid)roomId;
    }

    //change deskName
    if (deskName != null)
    {
      if (deskName == "")
        throw new ArgumentInvalidException($"Desk name must not be empty");
      if (roomId == null)
      {
        var deskNameIsUnique = deskExists.Room.Desks.Select(d => d.DeskName).All(name => name != deskName);
        if (!deskNameIsUnique)
          throw new ArgumentInvalidException($"There is already a desk named '{deskName}' in room '{deskExists.Room.RoomName}'");
      }

      deskExists.DeskName = deskName;
    }

    //change desk type
    if (deskTypeId != null)
    {
      var deskTypeExists = _context.DeskTypes.SingleOrDefault(dt => dt.DeskTypeId == (Guid)deskTypeId);
      if (deskTypeExists == null)
        throw new EntityNotFoundException($"DeskType does not exist with id '{(Guid)deskTypeId}'");
      if (deskTypeExists.CompanyId != companyId)
        throw new InsufficientPermissionException($"Your company has no access to desk type '{deskTypeExists.DeskTypeName}'");

      deskExists.DeskTypeId = (Guid)deskTypeId;
    }

    _context.Desks.Update(deskExists);
    _context.SaveChanges();

    return deskExists;
  }
  public DeskType UpdateDeskType(Guid companyId, Guid deskTypeId, string? deskTypeName)
  {
    var deskTypeExists = _context.DeskTypes.SingleOrDefault(dt => dt.DeskTypeId == deskTypeId);
    if (deskTypeExists == null)
      throw new EntityNotFoundException($"There is no desk type with id '{deskTypeId}'");

    if (deskTypeExists.CompanyId != companyId)
      throw new InsufficientPermissionException($"Your company has no access to administrate desk type '{deskTypeExists.DeskTypeName}'");

    //change deskTypeName
    if (deskTypeName != null)
    {
      if (deskTypeName == "")
        throw new ArgumentInvalidException($"Desk type name must not be empty");
      var deskTypeNameExists = _context.DeskTypes.SingleOrDefault(dt => dt.CompanyId == companyId && dt.DeskTypeName == deskTypeName);
      if (deskTypeNameExists != null)
        throw new ArgumentInvalidException($"There is already a desktype named '{deskTypeName}'");

      deskTypeExists.DeskTypeName = deskTypeName;
    }

    _context.DeskTypes.Update(deskTypeExists);
    _context.SaveChanges();

    return deskTypeExists;
  }
  public List<CurrentBuilding> GetBuildings(Guid userId)
  {
    IQueryable<Building> databaseBuildings;
    try
    {
      var companyId = _context.Users.Where(user => user.UserId == userId).Select(user => user.CompanyId).First();
      databaseBuildings = _context.Buildings.Where(building => building.CompanyId == companyId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{userId}' is not a valid UserId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no User with id '{userId}'");
    }

    if (databaseBuildings.ToList().Count == 0) return new List<CurrentBuilding>();

    var mapBuildingsToCurrentBuildings = databaseBuildings.Select(b => new CurrentBuilding
    {
      Location = b.Location,
      BuildingId = b.BuildingId.ToString(),
      BuildingName = b.BuildingName,
      IsMarkedForDeletion = b.IsMarkedForDeletion
    });

    return mapBuildingsToCurrentBuildings.ToList();
  }

  public List<CurrentFloor> GetFloors(Guid callerId, string buildingId)
  {
    IQueryable<Floor> databaseFloors;

    Guid buildingGuidId;
    try
    {
      buildingGuidId = new Guid(buildingId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{buildingId}' is not a valid BuildingId");
    }

    try
    {
      databaseFloors = _context.Floors.Include(b=>b.Building).Where(floor => floor.BuildingId == buildingGuidId);
      if (databaseFloors.ToList().Count == 0)
      {
        var databaseBuilding = _context.Buildings.First(building => building.BuildingId == buildingGuidId);
        if (databaseBuilding == null) throw new ArgumentException($"There is no Building with id '{buildingGuidId}'");
      }
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{buildingGuidId}' is not a valid FloorId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no Floor with id '{buildingGuidId}'");
    }

    if (databaseFloors.ToList().Count == 0) return new List<CurrentFloor>();

    var mapFloorsToCurrentFloors = databaseFloors.Select(f => new CurrentFloor
    {
      BuildingName = f.Building.BuildingName,
      FloorName = f.FloorName,
      FloorId = f.FloorId.ToString(),
      Location = f.Building.Location,
      IsMarkedForDeletion = f.IsMarkedForDeletion
    });

    return mapFloorsToCurrentFloors.ToList();
  }

  public List<CurrentFloor> GetAllFloors(Guid userId)
  {
    IQueryable<Floor> databaseFloors;
    try
    {
      var companyId = _context.Users.Where(user => user.UserId == userId).Select(user => user.CompanyId).First();
      databaseFloors = _context.Floors
        .Include(b => b.Building).Where(floor => floor.Building.CompanyId == companyId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{userId}' is not a valid UserId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no User with id '{userId}'");
    }

    if (databaseFloors.ToList().Count == 0) return new List<CurrentFloor>();

    var mapFloorsToCurrentFloors = databaseFloors.Select(f => new CurrentFloor
    {
      BuildingId = f.BuildingId.ToString(),
      BuildingName = f.Building.BuildingName,
      FloorName = f.FloorName,
      FloorId = f.FloorId.ToString(),
      IsMarkedForDeletion = f.IsMarkedForDeletion
    });

    return mapFloorsToCurrentFloors.ToList();
  }
  public List<CurrentRoom> GetRooms(Guid callerId, string floorId)
  {
    IQueryable<Room> databaseRooms;
    Guid floorGuidId;
    try
    {
      floorGuidId = new Guid(floorId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{floorId}' is not a valid FloorId");
    }

    try
    {
      databaseRooms = _context.Rooms.Include(r=>r.Floor).ThenInclude(f=>f.Building).Where(room => room.FloorId == floorGuidId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{floorGuidId}' is not a valid FloorId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentInvalidException($"There is no Floor with id '{floorGuidId}'");
    }

    if (databaseRooms.ToList().Count == 0) return new List<CurrentRoom>();

    var mapRoomsToCurrentRooms = databaseRooms.Select(r => new CurrentRoom
    {
      RoomId = r.RoomId.ToString(),
      RoomName = r.RoomName,
      Floor = r.Floor.FloorName,
      Building = r.Floor.Building.BuildingName,
      Location = r.Floor.Building.Location,
      IsMarkedForDeletion = r.IsMarkedForDeletion
    });

    return mapRoomsToCurrentRooms.ToList();
  }

  public List<CurrentRoom> GetAllRooms(Guid userId)
  {
    IQueryable<Room> databaseRooms;
    try
    {
      var companyId = _context.Users.Where(user => user.UserId == userId).Select(user => user.CompanyId).First();
      databaseRooms = _context.Rooms.Include(r=>r.Floor).ThenInclude(f=>f.Building).Where(room => room.Floor.Building.CompanyId == companyId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{userId}' is not a valid UserId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no User with id '{userId}'");
    }

    if (databaseRooms.ToList().Count == 0) return new List<CurrentRoom>();

    var mapRoomsToCurrentRooms = databaseRooms.Select(r => new CurrentRoom
    {
      RoomId = r.RoomId.ToString(),
      RoomName = r.RoomName,
      FloorId = r.FloorId.ToString(),
      IsMarkedForDeletion = r.IsMarkedForDeletion
    });

    return mapRoomsToCurrentRooms.ToList();
  }
  public List<CurrentDesk> GetDesks(Guid callerId, string roomId, DateTime start, DateTime end)
  {
    IQueryable<CurrentDesk> mapDesksToCurrentDesks;
    Guid roomGuid;
    try
    {
      roomGuid = new Guid(roomId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{roomId}' is not a valid RoomId");
    }

    try
    {
      var databaseDesks = _context.Desks.Include(d => d.Room)
        .ThenInclude(r=>r.Floor).ThenInclude(f=>f.Building).Where(desk => desk.RoomId == roomGuid);
      if (databaseDesks.ToList().Count == 0)
      {
        var databaseRoom = _context.Rooms.First(room => room.RoomId == roomGuid);
        if (databaseRoom == null) throw new ArgumentException($"There is no Room with id '{roomGuid}'");
      }

      mapDesksToCurrentDesks = MapDesksToCurrentDesks(databaseDesks, end, start);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{roomId}' is not a valid RoomId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no Room with id '{roomId}'");
    }

    return mapDesksToCurrentDesks.ToList();
  }

  private static IQueryable<CurrentDesk> MapDesksToCurrentDesks(IQueryable<Desk> databaseDesks, DateTime end,
    DateTime start)
  {
    return databaseDesks.Select(desk => new CurrentDesk
    {
      DeskId = desk.DeskId.ToString(),
      DeskName = desk.DeskName,
      DeskTyp = desk.DeskType.DeskTypeName,
      Bookings = desk.Bookings.Where(booking => booking.StartTime < end && booking.EndTime > start)
        .Select(booking => new BookingDesks
        {
          BookingId = booking.BookingId.ToString(),
          StartTime = booking.StartTime,
          EndTime = booking.EndTime,
          UserId = booking.UserId.ToString(),
          UserName = booking.User.FirstName + " " + booking.User.LastName,
        }).ToList(),
      FloorName = desk.Room.Floor.FloorName,
      FloorId = desk.Room.Floor.FloorId.ToString(),
      RoomId = desk.Room.RoomId.ToString(),
      RoomName = desk.Room.RoomName,
      BuildingId = desk.Room.Floor.Building.BuildingId.ToString(),
      BuildingName = desk.Room.Floor.Building.BuildingName,
      Location = desk.Room.Floor.Building.Location,
      IsMarkedForDeletion = desk.IsMarkedForDeletion
    });
  }

  public List<CurrentDesk> GetAllDesks(Guid userId)
  {
    IQueryable<CurrentDesk> mapDesksToCurrentDesks;
    try
    {
      var companyId = _context.Users.Where(user => user.UserId == userId).Select(user => user.CompanyId).First();
      var databaseDesks = _context.Desks.Include(d => d.Room)
        .ThenInclude(r=>r.Floor).ThenInclude(f=>f.Building)
        .Where(desk => desk.Room.Floor.Building.CompanyId == companyId);

      mapDesksToCurrentDesks = databaseDesks.Select(desk => new CurrentDesk
      {
        DeskId = desk.DeskId.ToString(),
        DeskName = desk.DeskName,
        DeskTyp = desk.DeskType.DeskTypeName,
        FloorName = desk.Room.Floor.FloorName,
        FloorId = desk.Room.Floor.FloorId.ToString(),
        RoomId = desk.Room.RoomId.ToString(),
        RoomName = desk.Room.RoomName,
        BuildingId = desk.Room.Floor.Building.BuildingId.ToString(),
        BuildingName = desk.Room.Floor.Building.BuildingName,
        Location = desk.Room.Floor.Building.Location,
        IsMarkedForDeletion = desk.IsMarkedForDeletion
      });
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{userId}' is not a valid UserId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no User with id '{userId}'");
    }

    return mapDesksToCurrentDesks.ToList();
  }
  public CurrentDesk GetDesk(Guid deskId, DateTime startDateTime, DateTime endDateTime)
  {
    CurrentDesk mapDeskToCurrentDesk;
    try
    {
      var databaseDesks = _context.Desks.Include(d => d.Room)
        .ThenInclude(r=>r.Floor).ThenInclude(f=>f.Building).Where(desk => desk.DeskId == deskId);

      mapDeskToCurrentDesk = databaseDesks.Select(desk => new CurrentDesk
      {
        DeskId = desk.DeskId.ToString(),
        DeskName = desk.DeskName,
        DeskTyp = desk.DeskType.DeskTypeName,
        Bookings = desk.Bookings.Where(booking =>
            (booking.StartTime >= startDateTime && booking.EndTime <= endDateTime))
          .Select(booking => new BookingDesks
          {
            BookingId = booking.BookingId.ToString(),
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            UserId = booking.UserId.ToString(),
          }).ToList(),
        FloorName = desk.Room.Floor.FloorName,
        FloorId = desk.Room.Floor.FloorId.ToString(),
        RoomId = desk.Room.RoomId.ToString(),
        RoomName = desk.Room.RoomName,
        BuildingId = desk.Room.Floor.Building.BuildingId.ToString(),
        BuildingName = desk.Room.Floor.Building.BuildingName,
        Location = desk.Room.Floor.Building.Location,
        IsMarkedForDeletion = desk.IsMarkedForDeletion
      }).First();
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{deskId}' is not a valid DeskId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no Desk with id '{deskId}'");
    }

    return mapDeskToCurrentDesk;
  }

  public List<DeskType> GetDeskTypes(Guid companyId)
  {
    return _context.DeskTypes.Where(d => d.CompanyId == companyId).ToList();
  }

  public Desk CreateDesk(string deskName, Guid deskTypeId, Guid roomId)
  {
    var desktype = _context.DeskTypes.SingleOrDefault(dt => dt.DeskTypeId == deskTypeId);
    if (desktype == null)
      throw new EntityNotFoundException($"There is no desk type with id '{deskTypeId}'");

    var room = _context.Rooms.SingleOrDefault(r => r.RoomId == roomId);
    if (room == null)
      throw new EntityNotFoundException($"There is no room with id '{roomId}'");

    if (deskName == "")
      throw new ArgumentInvalidException($"'{deskName}' is not a valid name of a desk");
    var deskNameExists = _context.Desks.SingleOrDefault(d => d.RoomId == roomId && d.DeskName == deskName) != null;
    if (deskNameExists)
      throw new ArgumentInvalidException($"In this room there is already a desk named '{deskName}'");

    var deskId = Guid.NewGuid();
    var desk = new Desk
    {
      DeskId = deskId,
      DeskName = deskName,
      DeskTypeId = deskTypeId,
      RoomId = roomId
    };

    _context.Desks.Add(desk);
    _context.SaveChanges();

    return desk;
  }

  public DeskType CreateDeskType(string deskTypeName, Guid companyId)
  {
    var company = _context.Companies.SingleOrDefault(c => c.CompanyId == companyId);
    if (company == null)
      throw new EntityNotFoundException($"There is no company with id '{companyId}'");

    if (deskTypeName == "")
      throw new ArgumentInvalidException($"'{deskTypeName}' is not a valid name for a desk type'");
    var deskTypeExists =
      _context.DeskTypes.SingleOrDefault(dt => dt.CompanyId == companyId && dt.DeskTypeName == deskTypeName) != null;
    if (deskTypeExists)
      throw new ArgumentInvalidException($"There is already a deskType called '{deskTypeName}'");
    var deskTypeId = Guid.NewGuid();
    var deskType = new DeskType
    {
      DeskTypeId = deskTypeId,
      DeskTypeName = deskTypeName,
      CompanyId = companyId
    };

    _context.DeskTypes.Add(deskType);
    _context.SaveChanges();

    return deskType;
  }

  public Room CreateRoom(string roomName, Guid floorId)
  {
    var floor = _context.Floors.SingleOrDefault(f => f.FloorId == floorId);
    if (floor == null)
      throw new EntityNotFoundException($"There is no floor with id '{floorId}'");

    if (roomName == "")
      throw new ArgumentInvalidException($"'{roomName}' is not a valid name for a room'");
    var roomNameExists = _context.Rooms.SingleOrDefault(r => r.FloorId == floorId && r.RoomName == roomName) != null;
    if (roomNameExists)
      throw new ArgumentInvalidException($"There is already a room called '{roomName}'");

    var roomId = Guid.NewGuid();
    var room = new Room
    {
      RoomId = roomId,
      RoomName = roomName,
      FloorId = floorId
    };

    _context.Rooms.Add(room);
    _context.SaveChanges();

    return room;
  }

  public Floor CreateFloor(string floorName, Guid buildingId)
  {
    var building = _context.Buildings.SingleOrDefault(b => b.BuildingId == buildingId);
    if (building == null)
      throw new EntityNotFoundException($"There is no building with id 'buildingId'");

    if (floorName == "")
      throw new ArgumentInvalidException($"'{floorName}' is not a valid name for a floor");
    var floorNameExists =
      _context.Floors.SingleOrDefault(f => f.BuildingId == buildingId && f.FloorName == floorName) != null;
    if (floorNameExists)
      throw new ArgumentInvalidException($"There is already a floor called '{floorName}'");

    var floorId = Guid.NewGuid();
    var floor = new Floor
    {
      FloorId = floorId,
      FloorName = floorName,
      BuildingId = buildingId
    };

    _context.Floors.Add(floor);
    _context.SaveChanges();

    return floor;
  }

  public Building CreateBuilding(string buildingName, string location, Guid companyId)
  {
    var company = _context.Companies.SingleOrDefault(c => c.CompanyId == companyId);
    if (company == null)
      throw new EntityNotFoundException($"There is no company with id '{companyId}'");

    if (location == "")
      throw new ArgumentInvalidException($"'{location}' is not a valid name for a building'");

    if (buildingName == "")
      throw new ArgumentInvalidException($"'{buildingName}' is not a valid name for a building'");
    var buildingExists =
      _context.Buildings.SingleOrDefault(b => b.CompanyId == companyId && b.BuildingName == buildingName) != null;
    if (buildingExists)
      throw new ArgumentInvalidException($"There is already a building with the name '{buildingName}'");

    var buildingId = Guid.NewGuid();
    var building = new Building
    {
      BuildingId = buildingId,
      BuildingName = buildingName,
      Location = location,
      CompanyId = companyId
    };

    _context.Buildings.Add(building);
    _context.SaveChanges();

    return building;
  }

  public void DeleteDesk(Guid adminId, string deskId)
  {
    Guid deskGuid;
    try
    {
      deskGuid = new Guid(deskId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{deskId}' is not a valid DeskId");
    }

    var deskDbInstance = _context.Desks.Include(d => d.Room)
      .ThenInclude(r => r.Floor)
      .ThenInclude(b => b.Building).SingleOrDefault(d => d.DeskId == deskGuid);
    if (deskDbInstance == null)
      throw new EntityNotFoundException($"There is no desk with id '{deskId}'");

    var companyDbObject =
      _context.Companies.SingleOrDefault(c => c.CompanyId == deskDbInstance.Room.Floor.Building.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This desk with id '{deskId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new InsufficientPermissionException(
        $"The desk with id '{deskId}' is not from the same company as the admin with id '{adminId}'");
    deskDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();

    notifybookers(deskDbInstance);
  }

  private void notifybookers(Desk deletedDesk)
  {
    var bookings = _context.Bookings.Where(b => b.DeskId == deletedDesk.DeskId).Include(b => b.User).ToList();
    foreach (var booking in bookings)
    {
      var body = $"Hello {booking.User.FirstName},<br/> " +
                 $"some problems with your booking of desk {booking.Desk.DeskName} from " +
                 $"{booking.StartTime} until {booking.EndTime} occured.<br/>" +
                 "The desk was deleted by an admin. Please make sure to book another desk for your day in the office.<br/>";
      EmailHelper.SendEmail(_logger, booking.User.MailAddress, $"A problem with your booking of desk {booking.Desk.DeskName} occured!", body);
    }
  }

  public void DeleteDeskType(Guid adminId, string typeId)
  {
    Guid deskTypeGuid;
    try
    {
      deskTypeGuid = new Guid(typeId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{typeId}' is not a valid DeskTypeId");
    }

    var deskTypeDbInstance = _context.DeskTypes.SingleOrDefault(d => d.DeskTypeId == deskTypeGuid);
    if (deskTypeDbInstance == null)
      throw new EntityNotFoundException($"There is no desk type with id '{typeId}'");

    var companyDbObject = _context.Companies.SingleOrDefault(c => c.CompanyId == deskTypeDbInstance.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This desk type with id '{typeId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The desk type with id '{typeId}' is not from the same company as the admin with id '{adminId}'");
    if (_context.Desks.Where(d => d.DeskTypeId == deskTypeGuid).Where(d => !d.IsMarkedForDeletion).ToList().Any())
    {
      throw new ArgumentInvalidException($"There are still desks with given type with id '{typeId}'");
    }

    deskTypeDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();
  }

  public void DeleteRoom(Guid adminId, string roomId)
  {
    Guid roomGuid;
    try
    {
      roomGuid = new Guid(roomId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{roomId}' is not a valid RoomId");
    }

    var roomDbInstance = _context.Rooms.Include(r => r.Floor)
      .ThenInclude(b => b.Building).SingleOrDefault(r => r.RoomId == roomGuid);
    if (roomDbInstance == null)
      throw new EntityNotFoundException($"There is no room with id '{roomId}'");

    var companyDbObject =
      _context.Companies.SingleOrDefault(c => c.CompanyId == roomDbInstance.Floor.Building.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This room with id '{roomId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The room with id '{roomId}' is not from the same company as the admin with id '{adminId}'");

    roomDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();

    _context.Desks.Where(d => d.RoomId == roomGuid).ToList().ForEach(d => DeleteDesk(adminId, d.DeskId.ToString()));
  }

  public void DeleteFloor(Guid adminId, string floorId)
  {
    Guid floorGuid;
    try
    {
      floorGuid = new Guid(floorId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{floorId}' is not a valid FloorId");
    }

    var floorDbInstance = _context.Floors.Include(b => b.Building).SingleOrDefault(f => f.FloorId == floorGuid);
    if (floorDbInstance == null)
      throw new EntityNotFoundException($"There is no floor with id '{floorId}'");

    var companyDbObject = _context.Companies.Single(c => c.CompanyId == floorDbInstance.Building.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This floor with id '{floorId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The floor with id '{floorId}' is not from the same company as the admin with id '{adminId}'");

    floorDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();

    _context.Rooms.Where(r => r.FloorId == floorGuid).ToList().ForEach(r => DeleteRoom(adminId, r.RoomId.ToString()));
  }

  public void DeleteBuilding(Guid adminId, string buildingId)
  {
    Guid buildingGuid;
    try
    {
      buildingGuid = new Guid(buildingId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{buildingId}' is not a valid BuildingId");
    }

    var buildingDbInstance = _context.Buildings.SingleOrDefault(b => b.BuildingId == buildingGuid);
    if (buildingDbInstance == null)
      throw new EntityNotFoundException($"There is no building with id '{buildingId}'");

    var companyDbObject = _context.Companies.Single(c => c.CompanyId == buildingDbInstance.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This building with id '{buildingId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The building with id '{buildingId}' is not from the same company as the admin with id '{adminId}'");

    buildingDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();

    _context.Floors.Where(f => f.BuildingId == buildingGuid).ToList()
      .ForEach(f => DeleteFloor(adminId, f.FloorId.ToString()));
  }

  public void RestoreDesk(Guid adminId, string deskId)
  {
    Guid deskGuid;
    try
    {
      deskGuid = new Guid(deskId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{deskId}' is not a valid DeskId");
    }

    var deskDbInstance = _context.Desks
      .Include(d => d.Room)
      .ThenInclude(r => r.Floor)
      .ThenInclude(b => b.Building).SingleOrDefault(d => d.DeskId == deskGuid);
    if (deskDbInstance == null)
      throw new EntityNotFoundException($"There is no desk with id '{deskId}'");
    if (!deskDbInstance.IsMarkedForDeletion)
      throw new ArgumentInvalidException($"This room with id '{deskId}' is not deleted.");
    var companyDbObject =
      _context.Companies.SingleOrDefault(c => c.CompanyId == deskDbInstance.Room.Floor.Building.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This desk with id '{deskId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new InsufficientPermissionException(
        $"The desk with id '{deskId}' is not from the same company as the admin with id '{adminId}'");
    if (deskDbInstance.Room.IsMarkedForDeletion)
      throw new ArgumentInvalidException($"The desk with id '{deskId}' cannot be restored " +
                                         $"because the room with id '{deskDbInstance.RoomId}' needs to be restored first.");
    deskDbInstance.IsMarkedForDeletion = false;
    _context.SaveChanges();
  }

  public void RestoreDeskType(Guid adminId, string typeId)
  {
    Guid deskTypeGuid;
    try
    {
      deskTypeGuid = new Guid(typeId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{typeId}' is not a valid DeskTypeId");
    }

    var deskTypeDbInstance = _context.DeskTypes.SingleOrDefault(d => d.DeskTypeId == deskTypeGuid);
    if (deskTypeDbInstance == null)
      throw new EntityNotFoundException($"There is no desk type with id '{typeId}'");
    if (!deskTypeDbInstance.IsMarkedForDeletion)
      throw new ArgumentException($"This room with id '{typeId}' is not deleted.");

    var companyDbObject = _context.Companies.SingleOrDefault(c => c.CompanyId == deskTypeDbInstance.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This desk type with id '{typeId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The desk type with id '{typeId}' is not from the same company as the admin with id '{adminId}'");
    if (_context.Desks.Where(d => d.DeskTypeId == deskTypeGuid).Where(d => !d.IsMarkedForDeletion).ToList().Any())
    {
      throw new ArgumentInvalidException($"There are still desks with given type with id '{typeId}'");
    }

    deskTypeDbInstance.IsMarkedForDeletion = false;
    _context.SaveChanges();
  }

  public void RestoreRoom(Guid adminId, string roomId)
  {
    Guid roomGuid;
    try
    {
      roomGuid = new Guid(roomId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{roomId}' is not a valid RoomId");
    }

    var roomDbInstance = _context.Rooms.Include(r => r.Floor)
      .ThenInclude(b => b.Building).SingleOrDefault(r => r.RoomId == roomGuid);
    if (roomDbInstance == null)
      throw new EntityNotFoundException($"There is no room with id '{roomId}'");
    if (!roomDbInstance.IsMarkedForDeletion)
      throw new ArgumentException($"This room with id '{roomId}' is not deleted.");

    var companyDbObject =
      _context.Companies.SingleOrDefault(c => c.CompanyId == roomDbInstance.Floor.Building.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This room with id '{roomId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The room with id '{roomId}' is not from the same company as the admin with id '{adminId}'");

    if (roomDbInstance.Floor.IsMarkedForDeletion)
      throw new ArgumentInvalidException($"The room with id '{roomId}' cannot be restored " +
                                         $"because the floor with id '{roomDbInstance.FloorId}' needs to be restored first.");
    roomDbInstance.IsMarkedForDeletion = false;
    _context.SaveChanges();
  }

  public void RestoreFloor(Guid adminId, string floorId)
  {
    Guid floorGuid;
    try
    {
      floorGuid = new Guid(floorId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{floorId}' is not a valid FloorId");
    }

    var floorDbInstance = _context.Floors.Include(b => b.Building).SingleOrDefault(f => f.FloorId == floorGuid);
    if (floorDbInstance == null)
      throw new EntityNotFoundException($"There is no floor with id '{floorId}'");
    if (!floorDbInstance.IsMarkedForDeletion)
      throw new ArgumentException($"This floor with id '{floorId}' is not deleted.");
    var companyDbObject = _context.Companies.Single(c => c.CompanyId == floorDbInstance.Building.CompanyId);
    if (companyDbObject == null)
    {
      throw new EntityNotFoundException($"This floor with id '{floorId}' doesn't belong to any company. How?");
    }

    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The floor with id '{floorId}' is not from the same company as the admin with id '{adminId}'");

    if (floorDbInstance.Building.IsMarkedForDeletion)
      throw new ArgumentInvalidException($"The floor with id '{floorId}' cannot be restored " +
                                         $"because the building with id '{floorDbInstance.BuildingId}' needs to be restored first.");
    floorDbInstance.IsMarkedForDeletion = false;
    _context.SaveChanges();
  }

  public void RestoreBuilding(Guid adminId, string buildingId)
  {
    Guid buildingGuid;
    try
    {
      buildingGuid = new Guid(buildingId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{buildingId}' is not a valid BuildingId");
    }

    var buildingDbInstance = _context.Buildings.SingleOrDefault(b => b.BuildingId == buildingGuid);
    if (buildingDbInstance == null)
      throw new EntityNotFoundException($"There is no building with id '{buildingId}'");
    if (!buildingDbInstance.IsMarkedForDeletion)
      throw new ArgumentException($"This building with id '{buildingId}' is not deleted.");
    var companyDbObject = _context.Companies.Single(c => c.CompanyId == buildingDbInstance.CompanyId);
    if (companyDbObject == null)
      throw new EntityNotFoundException($"This building with id '{buildingId}' doesn't belong to any company. How?");
    var companyId = companyDbObject.CompanyId;
    var userDbObject = _context.Users.SingleOrDefault(u => u.UserId == adminId);
    if (userDbObject == null)
    {
      throw new ArgumentInvalidException($"There is no user with id '{adminId}'");
    }

    if (userDbObject.CompanyId != companyId)
      throw new ArgumentInvalidException(
        $"The building with id '{buildingId}' is not from the same company as the admin with id '{adminId}'");

    buildingDbInstance.IsMarkedForDeletion = false;
    _context.SaveChanges();
  }
}
