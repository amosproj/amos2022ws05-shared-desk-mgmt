using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class UpdateFloorResponseObject
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Entities.Floor, UpdateFloorResponseObject>()
        .ForMember(dest => dest.BuildingName, act => act.MapFrom(src => src.Building.BuildingName))
        .ForMember(dest => dest.Location, act => act.MapFrom(src => src.Building.Location));
  }

  [Required]
  public Guid FloorId { get; set; }
  [Required]
  public string FloorName { get; set; } = null!;
  [Required]
  public Guid BuildingId { get; set; }
  [Required]
  public string BuildingName { get; set; } = null!;
  [Required]
  public string Location { get; set; } = null!;
}