using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class CreateFloorResponseObject
{
    public CreateFloorResponseObject() { }

    public static void createMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Entities.Floor, CreateFloorResponseObject>();
    }

    [Required]
    public Guid FloorId { get; set; }

    [Required]
    public string FloorName { get; set; } = null!;
}