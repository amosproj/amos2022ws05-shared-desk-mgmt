/**
 * UserProfileDto
 *
 * Version 1.0
 *
 * 2023-01-03
 *
 * MIT License
 */
using AutoMapper;

namespace Deskstar.Models
{
  public class UserProfileDto
  {
    public static readonly UserProfileDto Null = new UserProfileDto();

    public UserProfileDto()
    {

    }

    public static void CreateMappings(IMapperConfigurationExpression cfg)
    {
      cfg.CreateMap<Entities.User, UserProfileDto>()
          .ForMember(dest => dest.Company, act => act.MapFrom(src => src.Company));
      cfg.CreateMap<UserProfileDto, Entities.User>()
          .ForMember(dest => dest.Company, act => act.MapFrom(src => src.Company));
    }

    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MailAddress { get; set; } = null!;
    public Guid CompanyId { get; set; }
    public bool IsApproved { get; set; }
    public bool IsCompanyAdmin { get; set; }

    public bool IsMarkedForDeletion { get; set; }

    public UserProfileCompanyDto? Company { get; set; }
  }
}
