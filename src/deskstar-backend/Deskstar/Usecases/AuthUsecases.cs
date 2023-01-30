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
using System.Text.RegularExpressions;
using Deskstar.Core.Exceptions;

namespace Deskstar.Usecases
{
  public interface IAuthUsecases
  {
    LoginResponse CheckCredentials(string mail, string password);
    string CreateToken(IConfiguration configuration, string mail);
    User RegisterAdmin(RegisterAdminDto registerAdmin);
    RegisterResponse RegisterUser(RegisterUser registerUser);
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

    public LoginResponse CheckCredentials(string mail, string password)
    {
      try
      {
        var registerUser
          = _context.Users.Single(u => u.MailAddress == mail);
        if (registerUser.IsMarkedForDeletion)
          return new LoginResponse
          { Message = LoginReturn.Deleted };
        if (!registerUser
              .IsApproved)
          return new LoginResponse
          { Message = LoginReturn.NotYetApproved };
        if (_hasher.VerifyHashedPassword(registerUser
              , registerUser
                .Password, password)
            == PasswordVerificationResult.Success)
          return new LoginResponse
          { Message = LoginReturn.Ok };
      }
      catch (Exception e)
      {
        _logger.LogError(e, e.Message);
      }

      return new LoginResponse
      {
        Message = LoginReturn.CredentialsWrong
      };
    }

    public string CreateToken(IConfiguration configuration, String mail)
    {
      var registerUser = _getUser(mail);
      if (registerUser == User.Null)
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
          new Claim(JwtRegisteredClaimNames.NameId, registerUser
            .UserId.ToString()),
          registerUser
            .IsCompanyAdmin
            ? new Claim("IsCompanyAdmin", "True")
            : new Claim("IsNormalUser", "True"),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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

    public RegisterResponse RegisterUser(RegisterUser registerUser)
    {
      if (string.IsNullOrEmpty(registerUser.FirstName))
        throw new ArgumentInvalidException($"'{nameof(registerUser.FirstName)}' is not set");
      if (string.IsNullOrEmpty(registerUser.LastName))
        throw new ArgumentInvalidException($"'{nameof(registerUser.LastName)}' is not set");
      if (string.IsNullOrEmpty(registerUser.MailAddress))
        throw new ArgumentInvalidException($"'{nameof(registerUser.MailAddress)}' is not set");
      if (string.IsNullOrEmpty(registerUser.Password))
        throw new ArgumentInvalidException($"'{nameof(registerUser.Password)}' is not set");
      var rx = new Regex(
        "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\\])",
        RegexOptions.IgnoreCase);
      if (rx.Matches(registerUser.MailAddress).Count != 1)
        throw new ArgumentInvalidException("Mailaddress is not valid");
      if (_getUser(registerUser.MailAddress) != User.Null)
      {
        return new RegisterResponse
        {
          Message = RegisterReturn.MailAddressInUse
        };
      }

      if (_getCompany(registerUser.CompanyId) == Company.Null)
      {
        return new RegisterResponse
        {
          Message = RegisterReturn.CompanyNotFound
        };
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
      return new RegisterResponse
      {
        Message = RegisterReturn.Ok
      };
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

    public User RegisterAdmin(RegisterAdminDto registerAdmin)
    {
      if (string.IsNullOrEmpty(registerAdmin.FirstName))
        throw new ArgumentInvalidException($"'{nameof(registerAdmin.FirstName)}' is not set");
      if (string.IsNullOrEmpty(registerAdmin.LastName))
        throw new ArgumentInvalidException($"'{nameof(registerAdmin.LastName)}' is not set");
      if (string.IsNullOrEmpty(registerAdmin.MailAddress))
        throw new ArgumentInvalidException($"'{nameof(registerAdmin.MailAddress)}' is not set");
      if (string.IsNullOrEmpty(registerAdmin.Password))
        throw new ArgumentInvalidException($"'{nameof(registerAdmin.Password)}' is not set");
      if (string.IsNullOrEmpty(registerAdmin.CompanyName))
        throw new ArgumentInvalidException($"'{nameof(registerAdmin.Password)}' is not set");
      var rx = new Regex(
        "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\\])",
        RegexOptions.IgnoreCase);
      if (rx.Matches(registerAdmin.MailAddress).Count != 1)
        throw new ArgumentInvalidException("Mailaddress is not valid");
      if (_getUser(registerAdmin.MailAddress) != User.Null)
        throw new ArgumentInvalidException($"E-Mail '{registerAdmin.MailAddress}' already in use");


      var companyNameExists = _context.Companies.SingleOrDefault(c => c.CompanyName == registerAdmin.CompanyName);
      if (companyNameExists != null)
        throw new ArgumentInvalidException($"Company name '{registerAdmin.CompanyName}' already in use");

      var companyId = Guid.NewGuid();
      var company = new Company { CompanyName = registerAdmin.CompanyName, CompanyId = companyId };
      var admin = new User
      {
        CompanyId = companyId,
        MailAddress = registerAdmin.MailAddress,
        FirstName = registerAdmin.FirstName,
        LastName = registerAdmin.LastName,
        Company = company,
        IsApproved = true,
        IsCompanyAdmin = true,
      };
      admin.Password = _hasher.HashPassword(admin, registerAdmin.Password);
      _context.Companies.Add(company);
      _context.Users.Add(admin);
      _context.SaveChanges();

      return admin;
    }
  }
}
