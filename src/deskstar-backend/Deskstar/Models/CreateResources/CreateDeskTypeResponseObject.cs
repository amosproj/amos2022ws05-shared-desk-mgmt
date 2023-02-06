using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class CreateDeskTypeResponseObject
{
  [Required] public Guid DeskTypeId { get; set; }

  [Required] public string DeskTypeName { get; set; } = null!;

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<DeskType, CreateDeskTypeResponseObject>();
  }
}
