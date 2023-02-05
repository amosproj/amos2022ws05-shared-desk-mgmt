using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class CompanyDto
{
  [Required] public string CompanyId { get; set; } = null!;

  [Required] public string CompanyName { get; set; } = null!;

  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Company, CompanyDto>();
  }
}
