using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Deskstar.Entities;

namespace Deskstar.Models;

public class LoginResponse
{
  public static void createMappings(IMapperConfigurationExpression cfg)
  {
    cfg.CreateMap<Login, LoginResponse>();
  }
  [Required] public LoginStatus Message { get; set; }
}
