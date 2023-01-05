using AutoMapper;

namespace Deskstar.Models;

public class CreateBuildingResponseObject
{
    public CreateBuildingResponseObject() { }

    public static void createMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Entities.Building, CreateBuildingResponseObject>();
    }

    public Guid BuildingId { get; set; }
    public string BuildingName { get; set; } = null!;
    public string Location { get; set; } = null!;
}
