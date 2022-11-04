using System;
using System.Collections.Generic;

namespace Deskstar.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Sessions = new HashSet<Session>();
            Roles = new HashSet<Role>();
        }

        public string FirstName { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string LastName { get; set; } = null!;
        public string MailAddress { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
