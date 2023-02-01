using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class CreateBuildingResponseObject
{
  public CreateBuildingResponseObject() { }

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Entities.Building, CreateBuildingResponseObject>();
  }

  [Required]
  public Guid BuildingId { get; set; }
  [Required]
  public string BuildingName { get; set; } = null!;
  [Required]
  public string Location { get; set; } = null!;
}
