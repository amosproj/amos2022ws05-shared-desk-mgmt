using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public enum LoginReturn
{
  NotYetApproved,
  CredentialsWrong,
  Deleted,
  Ok
}

public class LoginResponse
{
  [Required] public LoginReturn Message { get; set; }
}
