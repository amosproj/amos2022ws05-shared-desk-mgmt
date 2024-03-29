using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class ExtendedBooking
{
  public Guid BookingId { get; set; }

  [Required] public DateTime Timestamp { get; set; }

  [Required] public DateTime StartTime { get; set; }

  [Required] public DateTime EndTime { get; set; }

  [Required] public string DeskName { get; set; } = null!;

  [Required] public string BuildingName { get; set; } = null!;

  [Required] public string FloorName { get; set; } = null!;

  [Required] public string RoomName { get; set; } = null!;

  [Required] public bool usesDeletedDesk { get; set; }

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Booking, ExtendedBooking>()
      .ForMember(dest => dest.BuildingName, act => act.MapFrom(src => src.Desk.Room.Floor.Building.BuildingName))
      .ForMember(dest => dest.FloorName, act => act.MapFrom(src => src.Desk.Room.Floor.FloorName))
      .ForMember(dest => dest.RoomName, act => act.MapFrom(src => src.Desk.Room.RoomName))
      .ForMember(dest => dest.DeskName, act => act.MapFrom(src => src.Desk.DeskName))
      .ForMember(dest => dest.usesDeletedDesk, act => act.MapFrom(src => src.Desk.IsMarkedForDeletion));
  }
}
