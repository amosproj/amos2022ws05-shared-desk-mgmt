using System;
using System.Collections.Generic;

namespace Deskstar.Models
{
    public partial class Session
    {
        public Guid Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
