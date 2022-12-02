namespace Deskstar.Models;
using System.ComponentModel.DataAnnotations;
public class CurrentDesk
{
    [Required]
    public string DeskId { get; set; } = null!;

    [Required]
    public string DeskName { get; set; } = null!;

    [Required]
    public string DeskTyp { get; set; } = null!;
}