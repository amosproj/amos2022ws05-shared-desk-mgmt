using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Deskstar.Models;

public class CompanyDto
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Entities.Company, CompanyDto>();
  }
  public CompanyDto()
  {

  }

  [Required]
  public string CompanyId { get; set; } = null!;
  [Required]
  public string CompanyName { get; set; } = null!;
}
