using Deskstar.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Deskstar.Models;
using Deskstar.Entities;

namespace Deskstar.Usecases
{
    public interface IAuthUsecases
    {
        bool checkCredentials(String mail, String password);
        string createToken(IConfiguration configuration, CreateTokenUser incompleteUser);

    }
    public class AuthUsecases : IAuthUsecases
    {
        private readonly ILogger<AuthUsecases> _logger;
        private readonly DataContext _context;

        public AuthUsecases(ILogger<AuthUsecases> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public bool checkCredentials(string mail, string password)
        {
            try
            {
                var users = _context.Users.First();
                var user = _context.Users.Single(u => u.MailAddress == mail);
                return user.Password == password;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            return false;
        }

        public string createToken(IConfiguration configuration, CreateTokenUser incompleteUser)
        {
            var user = _getUser(incompleteUser);
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
            (configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Email, user.MailAddress),
                //new Claim(JwtRegisteredClaimNames.Name, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, 
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        private User _getUser(CreateTokenUser user)
        {
            try
            {
                return _context.Users.Single(u => u.MailAddress == user.MailAddress);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            return new User();
        }
    }
}