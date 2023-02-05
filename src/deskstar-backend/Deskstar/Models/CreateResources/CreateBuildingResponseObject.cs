using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class CreateBuildingResponseObject
{
  [Required] public Guid BuildingId { get; set; }

  [Required] public string BuildingName { get; set; } = null!;

  [Required] public string Location { get; set; } = null!;

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Building, CreateBuildingResponseObject>();
  }
}
