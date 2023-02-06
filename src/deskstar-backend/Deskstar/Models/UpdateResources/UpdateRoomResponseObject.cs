using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class UpdateRoomResponseObject
{
  public Guid RoomId { get; set; }

  [Required] public string RoomName { get; set; } = null!;

  [Required] public Guid FloorId { get; set; }

  [Required] public string FloorName { get; set; } = null!;

  [Required] public Guid BuildingId { get; set; }

  [Required] public string BuildingName { get; set; } = null!;

  [Required] public string Location { get; set; } = null!;

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Room, UpdateRoomResponseObject>()
      .ForMember(dest => dest.BuildingName, act => act.MapFrom(src => src.Floor.Building.BuildingName))
      .ForMember(dest => dest.BuildingId, act => act.MapFrom(src => src.Floor.Building.BuildingId))
      .ForMember(dest => dest.FloorName, act => act.MapFrom(src => src.Floor.FloorName))
      .ForMember(dest => dest.Location, act => act.MapFrom(src => src.Floor.Building.Location));
  }
}
