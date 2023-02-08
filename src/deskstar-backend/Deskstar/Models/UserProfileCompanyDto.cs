using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class UserProfileCompanyDto
{
  public static readonly UserProfileCompanyDto Null = new();

  public Guid CompanyId { get; set; }
  public string CompanyName { get; set; } = null!;
  public bool? Logo { get; set; }

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Company, UserProfileCompanyDto>();
  }
}
