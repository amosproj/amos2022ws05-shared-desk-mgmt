namespace Deskstar.Models
{
    public class RegisterUser
    {
        public string MailAddress { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid CompanyId { get; set; }
    }
}