using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Helper;
using Deskstar.Models;

namespace Deskstar.Usecases;

public interface IResourceUsecases
{
  public List<DeskType> GetDeskTypes(Guid companyId);
  public List<CurrentBuilding> GetBuildings(Guid userId);
  public List<CurrentFloor> GetFloors(Guid buildingId);
  public List<CurrentRoom> GetRooms(Guid floorId);
  public List<CurrentDesk> GetDesks(Guid roomId, DateTime start, DateTime end);
  public CurrentDesk GetDesk(Guid deskId, DateTime startDateTime, DateTime endDateTime);

  public Desk CreateDesk(string deskName, Guid deskTypeId, Guid roomId);
  public DeskType CreateDeskType(string deskTypeName, Guid companyId);
  public Room CreateRoom(string roomName, Guid floorId);
  public Floor CreateFloor(string floorName, Guid buildingId);
  public Building CreateBuilding(string buildingName, string location, Guid companyId);

  public void DeleteDesk(Guid adminId, string deskId);
  public void DeleteDeskType(Guid adminId, string typeId);
  public void DeleteRoom(Guid adminId, string roomId);
  public void DeleteFloor(Guid adminId, string floorId);
  public void DeleteBuilding(Guid adminId, string buildingId);
}

public class ResourceUsecases : IResourceUsecases
{
  private readonly DataContext _context;
  private readonly ILogger<ResourceUsecases> _logger;

  private readonly IUserUsecases _userUsecases;

  public ResourceUsecases(ILogger<ResourceUsecases> logger, DataContext context, IUserUsecases userUsecases)
  {
    _logger = logger;
    _context = context;
    _userUsecases = userUsecases;
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
      BuildingName = b.BuildingName
    });

    return mapBuildingsToCurrentBuildings.ToList();
  }

  public List<CurrentFloor> GetFloors(Guid buildingId)
  {
    IQueryable<Floor> databaseFloors;
    try
    {
      databaseFloors = _context.Floors.Where(floor => floor.BuildingId == buildingId);
      if (databaseFloors.ToList().Count == 0)
      {
        var databaseBuilding = _context.Buildings.First(building => building.BuildingId == buildingId);
        if (databaseBuilding == null) throw new ArgumentException($"There is no Building with id '{buildingId}'");
      }
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{buildingId}' is not a valid FloorId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no Floor with id '{buildingId}'");
    }

    if (databaseFloors.ToList().Count == 0) return new List<CurrentFloor>();

    var mapFloorsToCurrentFloors = databaseFloors.Select(f => new CurrentFloor
    {
      BuildingName = f.Building.BuildingName,
      FloorName = f.FloorName,
      FloorId = f.FloorId.ToString()
    });

    return mapFloorsToCurrentFloors.ToList();
  }

  public List<CurrentRoom> GetRooms(Guid floorId)
  {
    IQueryable<Room> databaseRooms;
    try
    {
      databaseRooms = _context.Rooms.Where(room => room.FloorId == floorId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentException($"'{floorId}' is not a valid FloorId");
    }
    catch (InvalidOperationException)
    {
      throw new ArgumentException($"There is no Floor with id '{floorId}'");
    }

    if (databaseRooms.ToList().Count == 0) return new List<CurrentRoom>();

    var mapRoomsToCurrentRooms = databaseRooms.Select(r => new CurrentRoom
    {
      RoomId = r.RoomId.ToString(),
      RoomName = r.RoomName
    });

    return mapRoomsToCurrentRooms.ToList();
  }

  public List<CurrentDesk> GetDesks(Guid roomId, DateTime start, DateTime end)
  {
    IQueryable<CurrentDesk> mapDesksToCurrentDesks;
    try
    {
      var databaseDesks = _context.Desks.Where(desk => desk.RoomId == roomId);
      if (databaseDesks.ToList().Count == 0)
      {
        var databaseRoom = _context.Rooms.First(room => room.RoomId == roomId);
        if (databaseRoom == null) throw new ArgumentException($"There is no Room with id '{roomId}'");
      }

      mapDesksToCurrentDesks = databaseDesks.Select(desk => new CurrentDesk
      {
        DeskId = desk.DeskId.ToString(),
        DeskName = desk.DeskName,
        DeskTyp = desk.DeskType.DeskTypeName,
        Bookings = desk.Bookings.Where(booking => (booking.StartTime < end && booking.EndTime > start))
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
        Location = desk.Room.Floor.Building.Location
      });
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

  public CurrentDesk GetDesk(Guid deskId, DateTime startDateTime, DateTime endDateTime)
  {
    CurrentDesk mapDeskToCurrentDesk;
    try
    {
      var databaseDesks = _context.Desks.Where(desk => desk.DeskId == deskId);

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
        Location = desk.Room.Floor.Building.Location
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
    throw new NotImplementedException();
  }

  public void DeleteDeskType(Guid adminId, string typeId)
  {
    throw new NotImplementedException();
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

    var roomDbInstance= _context.Rooms.SingleOrDefault(r => r.RoomId == roomGuid);
    if (roomDbInstance == null)
      throw new EntityNotFoundException($"There is no room with id '{roomId}'");
    //Check if the floor is from the same company as the admin

    var companyId = _context.Companies.Single(c => c.CompanyId == roomDbInstance.Floor.Building.CompanyId).CompanyId;

    if (_context.Users.Single(u => u.UserId == adminId).CompanyId != companyId)
      throw new ArgumentInvalidException($"The room with id '{roomId}' is not from the same company as the admin with id '{adminId}'");

    roomDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();
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

    var floorDbInstance= _context.Floors.SingleOrDefault(f => f.FloorId == floorGuid);
    if (floorDbInstance == null)
      throw new EntityNotFoundException($"There is no floor with id '{floorId}'");
    //Check if the floor is from the same company as the admin

    var companyId = _context.Companies.Single(c => c.CompanyId == floorDbInstance.Building.CompanyId).CompanyId;

    if (_context.Users.Single(u => u.UserId == adminId).CompanyId != companyId)
      throw new ArgumentInvalidException($"The floor with id '{floorId}' is not from the same company as the admin with id '{adminId}'");

    floorDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();
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
    //Check if the building is from the same company as the admin

    var companyId = _context.Companies.Single(c => c.CompanyId == buildingDbInstance.CompanyId).CompanyId;

    if (_context.Users.Single(u => u.UserId == adminId).CompanyId != companyId)
      throw new ArgumentInvalidException($"The building with id '{buildingId}' is not from the same company as the admin with id '{adminId}'");

    buildingDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();
  }
}
