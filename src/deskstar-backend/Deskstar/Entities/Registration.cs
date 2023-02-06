using System.ComponentModel.DataAnnotations;

namespace Deskstar.Entities;

public enum RegisterStatus
{
  Ok,
  MailAddressInUse,
  CompanyNotFound
}

public class Registration
{
  [Required] public RegisterStatus Message { get; set; }
}
