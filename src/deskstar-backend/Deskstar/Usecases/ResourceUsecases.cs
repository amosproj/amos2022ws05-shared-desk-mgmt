using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;

namespace Deskstar.Usecases;

public interface IResourceUsecases
{
    public List<CurrentBuilding> GetBuildings(Guid userId);
    public List<CurrentFloor> GetFloors(Guid buildingId);
    public List<CurrentRoom> GetRooms(Guid floorId);
    public List<CurrentDesk> GetDesks(Guid roomId, DateTime start, DateTime end);
    public CurrentDesk GetDesk(Guid deskId, DateTime startDateTime, DateTime endDateTime);
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
                if( databaseBuilding== null) throw new ArgumentException($"There is no Building with id '{buildingId}'");
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
            FloorID = f.FloorId.ToString()
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
                if( databaseRoom== null) throw new ArgumentException($"There is no Room with id '{roomId}'");
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
}