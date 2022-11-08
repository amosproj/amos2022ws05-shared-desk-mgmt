using System;
using System.Collections.Generic;

namespace Deskstar.Entities
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public Guid CompanyId { get; set; }

        public virtual Company Company { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
