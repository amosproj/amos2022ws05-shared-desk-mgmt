using System.ComponentModel.DataAnnotations;

namespace Deskstar.Entities;

public enum LoginStatus
{
  NotYetApproved,
  CredentialsWrong,
  Deleted,
  Ok
}

public class Login
{
  [Required] public LoginStatus Message { get; set; }
}