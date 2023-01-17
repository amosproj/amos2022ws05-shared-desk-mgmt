namespace Deskstar.Entities
{
    public partial class User
    {
        public static readonly User Null = new User();
        
        public User()
        {
            Bookings = new HashSet<Booking>();
            Roles = new HashSet<Role>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid CompanyId { get; set; }
        public bool IsApproved { get; set; }
        
        public bool IsCompanyAdmin { get; set; }
        
        public bool IsMarkedForDeletion { get; set; }

        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
