using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class RegisterUser
{
  [Required] public string MailAddress { get; set; } = null!;

  [Required] public string FirstName { get; set; } = null!;

  [Required] public string LastName { get; set; } = null!;

  [Required] public string Password { get; set; } = null!;

  [Required] public Guid CompanyId { get; set; }
}
