using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;
using Microsoft.EntityFrameworkCore;

namespace Deskstar.Usecases;

public interface IResourceUsecases
{
    public List<CurrentBuilding> GetBuildings(Guid userId);
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
}