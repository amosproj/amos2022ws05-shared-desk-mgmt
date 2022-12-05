using System.ComponentModel.DataAnnotations;
using Deskstar.Entities;

namespace Deskstar.Models;
public class CurrentDesk
{
    [Required]
    public string DeskId { get; set; } = null!;

    [Required]
    public string DeskName { get; set; } = null!;

    [Required]
    public string DeskTyp { get; set; } = null!;
    
    public List<Booking> BookedAt { get; set; } = null!;
}