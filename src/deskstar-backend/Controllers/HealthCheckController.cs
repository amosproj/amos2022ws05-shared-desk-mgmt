using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public String Get()
    {
        return "Hello World! We're live.";
    }

    [HttpGet("withToken")]
    [Authorize]
    public String Auth()
    {
        return "authenticated. We're live.";
    }
}
