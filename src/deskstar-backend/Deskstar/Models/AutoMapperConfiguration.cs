using AutoMapper;

namespace Deskstar.Models
{
    public interface IAutoMapperConfiguration
    {
        public MapperConfiguration GetConfiguration();
    }

    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        public MapperConfiguration GetConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                UserProfileDto.createMappings(cfg);
                UserProfileCompanyDto.createMappings(cfg);
                ExtendedBooking.createMappings(cfg);
            });
            return config;
        }
    }
}