namespace Deskstar.Models
{
    public class UserDto
    {
        public static readonly UserDto Null = new UserDto();
        
        public UserDto()
        {

        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MailAddress { get; set; } = null!;
        public Guid CompanyId { get; set; }
        public bool IsApproved { get; set; }
        
        public bool IsCompanyAdmin { get; set; }

        public string CompanyName { get; set; } = null!;
    }
}
