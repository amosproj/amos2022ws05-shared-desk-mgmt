using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class DeskTypeDto
{
  public static readonly UserProfileDto Null = new();

  public Guid DeskTypeId { get; set; }
  public string DeskTypeName { get; set; } = null!;
  public Guid CompanyId { get; set; }

  public bool IsMarkedForDeletion { get; set; }

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<DeskTypeDto, DeskType>();
    cfg.CreateMap<DeskType, DeskTypeDto>();
  }
}
