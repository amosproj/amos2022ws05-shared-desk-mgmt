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
        UserProfileDto.CreateMappings(cfg);
        UserProfileCompanyDto.createMappings(cfg);
        DeskTypeDto.createMappings(cfg);
        ExtendedBooking.createMappings(cfg);
        CreateBuildingResponseObject.createMappings(cfg);
        CreateDeskResponseObject.createMappings(cfg);
        CreateDeskTypeResponseObject.createMappings(cfg);
        CreateFloorResponseObject.createMappings(cfg);
        CreateRoomResponseObject.createMappings(cfg);
        UpdateBuildingResponseObject.createMappings(cfg);
        UpdateDeskResponseObject.createMappings(cfg);
        UpdateDeskTypeResponseObject.createMappings(cfg);
        UpdateFloorResponseObject.createMappings(cfg);
        UpdateRoomResponseObject.createMappings(cfg);
      });
      return config;
    }
  }
}