using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class UpdateDeskResponseObject
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Entities.Desk, UpdateDeskResponseObject>()
        .ForMember(dest => dest.BuildingId, act => act.MapFrom(src => src.Room.Floor.Building.BuildingId))
        .ForMember(dest => dest.BuildingName, act => act.MapFrom(src => src.Room.Floor.Building.BuildingName))
        .ForMember(dest => dest.Location, act => act.MapFrom(src => src.Room.Floor.Building.Location))
        .ForMember(dest => dest.FloorId, act => act.MapFrom(src => src.Room.Floor.FloorId))
        .ForMember(dest => dest.FloorName, act => act.MapFrom(src => src.Room.Floor.FloorName))
        .ForMember(dest => dest.Location, act => act.MapFrom(src => src.Room.Floor.Building.Location))
        .ForMember(dest => dest.RoomId, act => act.MapFrom(src => src.Room.RoomId))
        .ForMember(dest => dest.RoomName, act => act.MapFrom(src => src.Room.RoomName))
        .ForMember(dest => dest.DeskTypeName, act => act.MapFrom(src => src.DeskType.DeskTypeName));
  }
  [Required]
  public Guid DeskId { get; set; }
  [Required]
  public string DeskName { get; set; } = null!;
  [Required]
  public Guid DeskTypeId { get; set; }
  [Required]
  public string DeskTypeName { get; set; } = null!;
  [Required]
  public Guid RoomId { get; set; }
  [Required]
  public string RoomName { get; set; } = null!;
  [Required]
  public Guid FloorId { get; set; }
  [Required]
  public string FloorName { get; set; } = null!;
  [Required]
  public Guid BuildingId { get; set; }
  [Required]
  public string BuildingName { get; set; } = null!;
  [Required]
  public string Location { get; set; } = null!;
}