using System;
using System.Collections.Generic;

namespace Deskstar.Models
{
    public partial class Company
    {
        public Company()
        {
            Buildings = new HashSet<Building>();
            DeskTypes = new HashSet<DeskType>();
            Roles = new HashSet<Role>();
            Users = new HashSet<User>();
        }

        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public bool? Logo { get; set; }

        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<DeskType> DeskTypes { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
