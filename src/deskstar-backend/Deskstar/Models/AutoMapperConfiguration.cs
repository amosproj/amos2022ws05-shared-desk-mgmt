using AutoMapper;
using Deskstar.Entities;

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
                UserDto.getMapping(cfg);
            });
            return config;
        }
    }
}