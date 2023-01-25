using AutoMapper;

namespace Deskstar.Models;

public class DeskTypeDto
{
  public static readonly UserProfileDto Null = new UserProfileDto();

  public DeskTypeDto()
  {

  }

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<DeskTypeDto, Entities.DeskType>();
    cfg.CreateMap<Entities.DeskType, DeskTypeDto>();
  }

  public Guid DeskTypeId { get; set; }
  public string DeskTypeName { get; set; } = null!;
  public Guid CompanyId { get; set; }

  public bool IsMarkedForDeletion { get; set; } = false;
}
