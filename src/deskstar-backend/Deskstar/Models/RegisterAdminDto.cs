using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class RegisterAdminDto
{
  [Required] public string MailAddress { get; set; } = null!;

  [Required] public string FirstName { get; set; } = null!;

  [Required] public string LastName { get; set; } = null!;

  [Required] public string Password { get; set; } = null!;

  [Required] public string CompanyName { get; set; } = null!;
}
