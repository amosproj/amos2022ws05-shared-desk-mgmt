using Deskstar.Core.Exceptions;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deskstar.Controllers;

[ApiController]
[Route("/auth")]
[Produces("text/plain")]
public class AuthController : ControllerBase
{
  private readonly IAuthUsecases _authUsecases;
  private readonly IConfiguration _configuration;
  private readonly ILogger<AuthController> _logger;

  public AuthController(ILogger<AuthController> logger, IAuthUsecases authUsecases, IConfiguration configuration)
  {
    _logger = logger;
    _authUsecases = authUsecases;
    _configuration = configuration;
  }

  /// <summary>
  ///   Login functionality
  /// </summary>
  /// <returns> JWT, if users is approved and psw is correct </returns>
  /// <remarks>
  ///   Sample request:
  ///   Post /auth/createToken
  /// </remarks>
  /// <response code="200">Login succesful </response>
  /// <response code="401">Credentials wrong or user not approved</response>
  [HttpPost("createToken")]
  [AllowAnonymous]
  [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status401Unauthorized)]
  public IActionResult CreateToken(CreateTokenUser user)
  {
    var returnValue = _authUsecases.CheckCredentials(user.MailAddress, user.Password);
    if (returnValue.Message == LoginReturn.Ok) return Ok(_authUsecases.CreateToken(_configuration, user.MailAddress));

    return Unauthorized(returnValue.Message.ToString());
  }

  /// <summary>
  ///   Register functionality
  /// </summary>
  /// <remarks>
  ///   Sample request:
  ///   Post /auth/register
  /// </remarks>
  /// <response code="200">User added to db</response>
  /// <response code="400">Mail already in use</response>
  /// <response code="404">Company not found</response>
  [HttpPost("register")]
  [AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status404NotFound)]
  public IActionResult Register(RegisterUser registerUser)
  {
    var result = _authUsecases.RegisterUser(registerUser);
    return result.Message switch
    {
      RegisterReturn.Ok => Ok(),
      RegisterReturn.CompanyNotFound => NotFound(result.Message.ToString()),
      _ => BadRequest(result.Message.ToString())
    };
  }

  /// <summary>
  ///   Register functionality
  /// </summary>
  /// <remarks>
  ///   Sample request:
  ///   Post /auth/registerAdmin
  /// </remarks>
  /// <response code="200">Admin added to db</response>
  /// <response code="400">Mail or Company name already in use</response>
  [HttpPost("registerAdmin")]
  [AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public IActionResult RegisterAdmin(RegisterAdminDto registerAdmin)
  {
    try
    {
      var admin = _authUsecases.RegisterAdmin(registerAdmin.FirstName, registerAdmin.LastName,
        registerAdmin.MailAddress, registerAdmin.Password, registerAdmin.CompanyName);
      return Ok();
    }
    catch (ArgumentInvalidException e)
    {
      return BadRequest(e.Message);
    }
    catch (Exception e)
    {
      return Problem(statusCode: 500, detail: e.Message);
    }
  }
}
