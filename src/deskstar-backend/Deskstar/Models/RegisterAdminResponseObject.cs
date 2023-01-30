using AutoMapper;

namespace Deskstar.Models;

public class RegisterAdminResponseObject
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Entities.User, RegisterAdminResponseObject>()
    .ForMember(dest => dest.CompanyName, act => act.MapFrom(src => src.Company.CompanyName));
  }
  public string MailAddress { get; set; } = null!;

  public string FirstName { get; set; } = null!;

  public string LastName { get; set; } = null!;

  public string Password { get; set; } = null!;

  public Guid CompanyId { get; set; }
  public string CompanyName { get; set; } = null!;
}