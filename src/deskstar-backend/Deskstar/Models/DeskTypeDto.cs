using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class DeskTypeDto
{
    public static readonly UserProfileDto Null = new UserProfileDto();

    public DeskTypeDto()
    {

    }

    public static void createMappings(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<DeskTypeDto, DeskType>();
        cfg.CreateMap<DeskType, DeskTypeDto>();
    }

    public Guid DeskTypeId { get; set; }
    public string DeskTypeName { get; set; } = null!;
}