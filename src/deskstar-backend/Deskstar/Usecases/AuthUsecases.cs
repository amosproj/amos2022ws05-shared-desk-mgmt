using Deskstar.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Deskstar.Models;
using Deskstar.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;

namespace Deskstar.Usecases
{
    public enum RegisterReturn
    {
        Ok,
        MailAddressInUse,
        CompanyNotFound
    }

    public enum LoginReturn{
        NotYetApproved,
        CreditialsWrong,
        Ok
    }
    public interface IAuthUsecases
    {
        LoginReturn CheckCredentials(string mail, string password);
        string CreateToken(IConfiguration configuration, string mail);
        RegisterReturn RegisterUser(RegisterUser registerUser);

    }
    public class AuthUsecases : IAuthUsecases
    {
        private readonly ILogger<AuthUsecases> _logger;
        private readonly DataContext _context;
        private readonly PasswordHasher<User> _hasher;

        public AuthUsecases(ILogger<AuthUsecases> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        public LoginReturn CheckCredentials(string mail, string password)
        {
            try
            {
                var user = _context.Users.Single(u => u.MailAddress == mail);
                if (!user.IsApproved)
                    return LoginReturn.NotYetApproved;
                if (_hasher.VerifyHashedPassword(user, user.Password, password)
                    == PasswordVerificationResult.Success)
                    return LoginReturn.Ok;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            return LoginReturn.CreditialsWrong;
        }

        public string CreateToken(IConfiguration configuration, String mail)
        {
            var user = _getUser(mail);
            if (user == User.Null)
            {
                return "";
            }

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
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        public RegisterReturn RegisterUser(RegisterUser registerUser)
        {
            if (_getUser(registerUser.MailAddress) != User.Null)
            {
                return RegisterReturn.MailAddressInUse;
            }

            if (_getCompany(registerUser.CompanyId) == Company.Null)
            {
                return RegisterReturn.CompanyNotFound;
            }

            var newUser = new User
            {
                CompanyId = registerUser.CompanyId,
                MailAddress = registerUser.MailAddress,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                IsApproved = false
            };
            newUser.Password = _hasher.HashPassword(newUser, registerUser.Password);

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return RegisterReturn.Ok;
        }

        private User _getUser(string mail)
        {
            try
            {
                return _context.Users.Single(u => u.MailAddress == mail);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return User.Null;
            }
        }

        private Company _getCompany(Guid id)
        {
            try
            {
                return _context.Companies.Single(c => c.CompanyId == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Company.Null;
            }
        }
    }
}