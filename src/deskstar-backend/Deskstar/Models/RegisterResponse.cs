using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;
public class RegisterResponse
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Registration, RegisterResponse>();
  }
  [Required] public RegisterStatus Message { get; set; }
}
