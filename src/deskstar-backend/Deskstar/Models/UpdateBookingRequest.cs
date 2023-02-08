using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class UpdateBookingRequest
{
  [Required] public DateTime StartTime { get; set; }

  [Required] public DateTime EndTime { get; set; }
}
