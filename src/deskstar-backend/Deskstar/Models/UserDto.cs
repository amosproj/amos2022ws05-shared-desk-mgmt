using AutoMapper;

namespace Deskstar.Models
{
    public class UserDto
    {
        public static readonly UserDto Null = new UserDto();

        public UserDto()
        {

        }

        public static IMappingExpression<Entities.User, UserDto> getMapping(IMapperConfigurationExpression cfg)
        {
            return cfg.CreateMap<Entities.User, UserDto>()
                .ForMember(dest => dest.CompanyName, act => act.MapFrom(src => src.Company.CompanyName));
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MailAddress { get; set; } = null!;
        public Guid CompanyId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCompanyAdmin { get; set; }

        public string CompanyName { get; set; } = null!;
    }
}
