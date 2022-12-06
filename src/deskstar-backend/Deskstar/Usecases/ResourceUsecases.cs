using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;

namespace Deskstar.Usecases;

public interface IResourceUsecases
{
    public (bool, List<CurrentBuilding>) GetBuildings(Guid userId);
    public (bool, List<CurrentFloor>) GetFloors(Guid buildingId);
    public (bool, List<CurrentRoom>) GetRooms(Guid floorId);
    public (bool, List<CurrentDesk>) GetDesks(Guid roomId);
    public CurrentDesk? GetDesk(Guid deskId, DateTime startDateTime, DateTime endDateTime);
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

    public (bool, List<CurrentBuilding>) GetBuildings(Guid userId)
    {
        var companyId = _context.Users.Where(user => user.UserId == userId).Select(user => user.CompanyId).First();
        IQueryable<Building> databaseBuildings;
        try
        {
            databaseBuildings = _context.Buildings.Where(building => building.CompanyId == companyId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return (false, new List<CurrentBuilding>());
        }

        if (databaseBuildings.ToList().Count == 0) return (true, new List<CurrentBuilding>());

        var mapBuildingsToCurrentBuildings = databaseBuildings.Select(b => new CurrentBuilding
        {
            Location = b.Location,
            BuildingId = b.BuildingId.ToString(),
            BuildingName = b.BuildingName
        });

        return (false, mapBuildingsToCurrentBuildings.ToList());
    }

    public (bool, List<CurrentFloor>) GetFloors(Guid buildingId)
    {
        IQueryable<Floor> databaseFloors;
        try
        {
            databaseFloors = _context.Floors.Where(floor => floor.BuildingId == buildingId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return (false, new List<CurrentFloor>());
        }

        if (databaseFloors.ToList().Count == 0) return (true, new List<CurrentFloor>());

        var mapFloorsToCurrentFloors = databaseFloors.Select(f => new CurrentFloor
        {
            BuildingName = f.Building.BuildingName,
            FloorName = f.FloorName,
            FloorID = f.FloorId.ToString()
        });

        return (false, mapFloorsToCurrentFloors.ToList());
    }

    public (bool, List<CurrentRoom>) GetRooms(Guid floorId)
    {
        IQueryable<Room> databaseRooms;
        try
        {
            databaseRooms = _context.Rooms.Where(room => room.FloorId == floorId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return (false, new List<CurrentRoom>());
        }

        if (databaseRooms.ToList().Count == 0) return (true, new List<CurrentRoom>());

        var mapRoomsToCurrentRooms = databaseRooms.Select(r => new CurrentRoom
        {
            RoomId = r.RoomId.ToString(),
            RoomName = r.RoomName
        });

        return (false, mapRoomsToCurrentRooms.ToList());
    }

    public (bool, List<CurrentDesk>) GetDesks(Guid roomId)
    {
        IQueryable<Desk> databaseDesks;
        try
        {
            databaseDesks = _context.Desks.Where(desk => desk.RoomId == roomId);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return (false, new List<CurrentDesk>());
        }

        if (databaseDesks.ToList().Count == 0) return (true, new List<CurrentDesk>());

        var mapDesksToCurrentDesks = databaseDesks.Select(d => new CurrentDesk
        {
            DeskId = d.DeskId.ToString(),
            DeskName = d.DeskName,
            DeskTyp = d.DeskType.DeskTypeName
        });

        return (false, mapDesksToCurrentDesks.ToList());
    }

    public CurrentDesk? GetDesk(Guid deskId, DateTime startDateTime, DateTime endDateTime)
    {
        var booking = _context.Bookings.Where(booking => booking.DeskId == deskId);
        try
        {
            var desk = _context.Desks.Where(room => room.DeskId == deskId).Select(d => new CurrentDesk
            {
                DeskId = d.DeskId.ToString(),
                DeskName = d.DeskName,
                DeskTyp = d.DeskType.DeskTypeName,
                BookedAt = booking.ToList()
            }).First();

            return desk;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return null;
        }
    }
}