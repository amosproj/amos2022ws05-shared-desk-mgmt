using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class RecentBooking
{
    public static void createMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Entities.Booking, RecentBooking>()
        .ForMember(dest => dest.BuildingName, act => act.MapFrom(src => src.Desk.Room.Floor.Building.BuildingName))
        .ForMember(dest => dest.FloorName, act => act.MapFrom(src => src.Desk.Room.Floor.FloorName))
        .ForMember(dest => dest.RoomName, act => act.MapFrom(src => src.Desk.Room.RoomName))
        .ForMember(dest => dest.DeskName, act => act.MapFrom(src => src.Desk.DeskName));
    }
    public RecentBooking()
    {

    }
    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public string DeskName { get; set; } = null!;

    [Required]
    public string BuildingName { get; set; } = null!;

    [Required]
    public string FloorName { get; set; } = null!;

    [Required]
    public string RoomName { get; set; } = null!;


}