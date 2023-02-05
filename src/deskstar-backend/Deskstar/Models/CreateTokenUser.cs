using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateTokenUser
{
  [Required] public string MailAddress { get; set; } = null!;

  [Required] public string Password { get; set; } = null!;
}
