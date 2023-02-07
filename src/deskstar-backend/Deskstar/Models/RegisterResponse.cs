using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public enum RegisterReturn
{
  Ok,
  MailAddressInUse,
  CompanyNotFound
}

public class RegisterResponse
{
  [Required] public RegisterReturn Message { get; set; }
}
