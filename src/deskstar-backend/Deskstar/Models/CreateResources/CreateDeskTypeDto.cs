using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateDeskTypeDto
{
    public CreateDeskTypeDto() { }

    [Required]
    public string DeskTypeName { get; set; } = null!;

}