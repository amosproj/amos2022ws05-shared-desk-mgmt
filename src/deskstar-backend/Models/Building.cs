using System;
using System.Collections.Generic;

namespace Deskstar.Models
{
    public partial class Building
    {
        public Building()
        {
            Floors = new HashSet<Floor>();
        }

        public Guid BuildingId { get; set; }
        public string BuildingName { get; set; } = null!;
        public Guid CompanyId { get; set; }
        public string Location { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<Floor> Floors { get; set; }
    }
}
