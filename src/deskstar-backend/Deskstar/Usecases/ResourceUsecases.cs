using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;
using Microsoft.EntityFrameworkCore;

namespace Deskstar.Usecases;

public interface IResourceUsecases
{
    public List<CurrentBuilding> GetBuildings(Guid userId);
    public List<CurrentFloor> GetFloors(Guid buildingId);
    public List<CurrentRoom> GetRooms(Guid floorId);
    public List<CurrentDesk> GetDesks(Guid roomId);
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
        var companyId = _context.Users.Where(user => user.UserId==userId).Select(user => user.CompanyId).First();
        var databaseBuildings =_context.Buildings.Where(building => building.CompanyId == companyId);
        if (databaseBuildings.ToList().Count == 0)
        {
            return new List<CurrentBuilding>();
        }
        var mapBuildingsToCurrentBuildings = databaseBuildings.Select(b => new CurrentBuilding()
        {
            Location    = b.Location,
            BuildingId = b.BuildingId.ToString(),
            BuildingName = b.BuildingName
        });
        
        return mapBuildingsToCurrentBuildings.ToList();
    }

    public List<CurrentFloor> GetFloors(Guid buildingId)
    {
        var databaseFloors =_context.Floors.Where(floor => floor.BuildingId == buildingId);
        if (databaseFloors.ToList().Count == 0)
        {
            return new List<CurrentFloor>();
        }
        var mapFloorsToCurrentFloors = databaseFloors.Select(f => new CurrentFloor()
        {
            BuildingName = f.Building.BuildingName,
            FloorName = f.FloorName,
            FloorID = f.FloorId.ToString()
        });
        
        return mapFloorsToCurrentFloors.ToList();
    }

    public List<CurrentRoom> GetRooms(Guid floorId)
    {
        var databaseRooms =_context.Rooms.Where(room => room.FloorId == floorId);
        if (databaseRooms.ToList().Count == 0)
        {
            return new List<CurrentRoom>();
        }
        var mapRoomsToCurrentRooms = databaseRooms.Select(r => new CurrentRoom()
        {
            RoomId = r.RoomId.ToString(),
            RoomName = r.RoomName
        });
        
        return mapRoomsToCurrentRooms.ToList();
    }

    public List<CurrentDesk> GetDesks(Guid roomId)
    {
        var databaseDesks =_context.Desks.Where(desk => desk.RoomId == roomId);
        if (databaseDesks.ToList().Count == 0)
        {
            return new List<CurrentDesk>();
        }
        var mapDesksToCurrentDesks = databaseDesks.Select(d => new CurrentDesk()
        {
            DeskId = d.DeskId.ToString(),
            DeskName = d.DeskName,
            DeskTyp = d.DeskType.DeskTypeName
        });
        
        return mapDesksToCurrentDesks.ToList();
    }
}