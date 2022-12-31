using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class CreateRoomResponseObject
{
    public CreateRoomResponseObject() { }

    public static void createMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Entities.Room, CreateRoomResponseObject>();
    }

    [Required]
    public Guid RoomId { get; set; }

    [Required]
    public string RoomName { get; set; } = null!;
}