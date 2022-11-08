using System;
using System.Collections.Generic;

namespace Deskstar.Entities
{
    public partial class Floor
    {
        public Floor()
        {
            Rooms = new HashSet<Room>();
        }

        public Guid FloorId { get; set; }
        public Guid BuildingId { get; set; }
        public string FloorName { get; set; } = null!;

        public virtual Building Building { get; set; } = null!;
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
