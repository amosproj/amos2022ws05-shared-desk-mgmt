using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class CreateFloorResponseObject
{
  [Required] public Guid FloorId { get; set; }

  [Required] public string FloorName { get; set; } = null!;

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Floor, CreateFloorResponseObject>();
  }
}
