using System;
using System.Collections.Generic;

namespace Deskstar.Models
{
    public partial class Room
    {
        public Room()
        {
            Desks = new HashSet<Desk>();
        }

        public Guid RoomId { get; set; }
        public Guid FloorId { get; set; }
        public string RoomName { get; set; } = null!;

        public virtual Floor Floor { get; set; } = null!;
        public virtual ICollection<Desk> Desks { get; set; }
    }
}
