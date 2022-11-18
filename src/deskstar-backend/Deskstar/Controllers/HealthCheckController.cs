using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Controllers;

[ApiController]
[Route("/")]
public class HealthCheckController : ControllerBase
{
    

    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ILogger<HealthCheckController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    [AllowAnonymous]
    public string Get()
    {
        return "Hello World! We're live.";
    }

    [HttpGet("withToken")]
    [Authorize]
    public string Auth()
    {
        return "authenticated. We're live.";
    }

    [HttpGet("admin")]
    [Authorize]
    public string Admin()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        
        return jwtSecurityToken.Claims.First(claim => claim.Type == "IsCompanyAdmin").Value;
    }
}
