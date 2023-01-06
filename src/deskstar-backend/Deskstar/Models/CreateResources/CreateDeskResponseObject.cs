using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class CreateDeskResponseObject
{
    public CreateDeskResponseObject() { }

    public static void createMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Entities.Desk, CreateDeskResponseObject>();
    }

    [Required]
    public string DeskId { get; set; } = null!;

    [Required]
    public string DeskName { get; set; } = null!;
}