using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class CreateDeskResponseObject
{
  [Required] public string DeskId { get; set; } = null!;

  [Required] public string DeskName { get; set; } = null!;

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Desk, CreateDeskResponseObject>();
  }
}
