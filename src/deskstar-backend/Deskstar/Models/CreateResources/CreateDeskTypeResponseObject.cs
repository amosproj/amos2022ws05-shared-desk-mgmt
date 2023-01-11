using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class CreateDeskTypeResponseObject
{
    public CreateDeskTypeResponseObject() { }

    public static void createMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Entities.DeskType, CreateDeskTypeResponseObject>();
    }

    [Required]
    public Guid DeskTypeId { get; set; }

    [Required]
    public string DeskTypeName { get; set; } = null!;

}