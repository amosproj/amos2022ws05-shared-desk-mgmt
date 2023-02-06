using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class CreateRoomResponseObject
{
  [Required] public Guid RoomId { get; set; }

  [Required] public string RoomName { get; set; } = null!;

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Room, CreateRoomResponseObject>();
  }
}
