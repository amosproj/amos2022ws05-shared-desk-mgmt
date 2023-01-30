using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class UpdateDeskTypeResponseObject
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Entities.DeskType, UpdateDeskTypeResponseObject>();
  }
  [Required]
  public Guid DeskTypeId { get; set; }
  [Required]
  public string DeskTypeName { get; set; } = null!;
  
}