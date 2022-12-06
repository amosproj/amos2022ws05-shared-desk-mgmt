using AutoMapper;

namespace Deskstar.Models
{
    public class UserProfileCompanyDto
    {
        public static readonly UserProfileCompanyDto Null = new UserProfileCompanyDto();

        public static void createMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Entities.Company, UserProfileCompanyDto>();
        }

        public UserProfileCompanyDto()
        {

        }

        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public bool? Logo { get; set; }
    }
}
