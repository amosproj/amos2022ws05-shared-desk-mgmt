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
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthUsecases _authUsecases;
    private readonly IConfiguration _configuration;


    public AuthController(ILogger<AuthController> logger, IAuthUsecases authUsecases, IConfiguration configuration)
    {
        _logger = logger;
        _authUsecases = authUsecases;
        _configuration = configuration;
    }

    /// <summary>
    /// Login functionality
    /// </summary>
    /// <returns> JWT, if users is approved and psw is correct </returns>
    /// <remarks>
    /// Sample request:
    ///     Post /auth/createToken
    /// </remarks>
    /// 
    /// <response code="200">Login succesful </response>
    /// <response code="401">Credentials wrong or user not approved</response>
    [HttpPost("createToken")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public IActionResult CreateToken(CreateTokenUser user)
    {
        var returnValue = _authUsecases.CheckCredentials(user.MailAddress, user.Password);
        if (returnValue == LoginReturn.Ok)
        {
            return Ok(_authUsecases.CreateToken(_configuration, user.MailAddress));
        }

        return Unauthorized(returnValue.ToString());
    }

    /// <summary>
    /// Register functionality
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     Post /auth/register
    /// </remarks>
    /// 
    /// <response code="200">User added to db</response>
    /// <response code="400">Mail already in use</response>
    /// <response code="404">Company not found</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult Register(RegisterUser registerUser)
    {
        var result = _authUsecases.RegisterUser(registerUser);
        if (result != RegisterReturn.Ok)
        {
            if (result == RegisterReturn.CompanyNotFound)
            {
                return NotFound(result.ToString());
            }
            return BadRequest(result.ToString());
        }

        return Ok();
    }
}