using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deskstar.Controllers;

[ApiController]
[Route("/auth")]
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

    [HttpPost("createToken")]
    [AllowAnonymous]
    public IActionResult CreateToken(CreateTokenUser user)
    {
        if (_authUsecases.checkCredentials(user.MailAddress, user.Password))
        {
            return Ok(_authUsecases.createToken(_configuration, user.MailAddress));
        }
        return Unauthorized();
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterUser registerUser)
    {
        var result = _authUsecases.registerUser(registerUser);
        if (result != RegisterReturn.Ok)
        {
            return BadRequest(result);
        }

        return Ok();
    }
}
