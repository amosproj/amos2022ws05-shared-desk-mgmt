using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class UpdateBuildingResponseObject
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Entities.Building, UpdateBuildingResponseObject>();
  }
  [Required]
  public Guid BuildingId { get; set; }
  [Required]
  public string BuildingName { get; set; } = null!;
  [Required]
  public string Location { get; set; } = null!;
}