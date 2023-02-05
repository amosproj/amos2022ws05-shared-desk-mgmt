namespace Deskstar.Entities;

public class DeskType
{
  public DeskType()
  {
    Desks = new HashSet<Desk>();
  }

  public Guid DeskTypeId { get; set; }
  public string DeskTypeName { get; set; } = null!;
  public Guid CompanyId { get; set; }
  public bool IsMarkedForDeletion { get; set; } = false;

  public virtual Company Company { get; set; } = null!;
  public virtual ICollection<Desk> Desks { get; set; }
}
