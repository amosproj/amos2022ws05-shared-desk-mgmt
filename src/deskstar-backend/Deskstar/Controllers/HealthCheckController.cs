using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deskstar.Controllers;

[ApiController]
[Route("/")]
[Produces("text/plain")]
public class HealthCheckController : ControllerBase
{
  private readonly ILogger<HealthCheckController> _logger;

  public HealthCheckController(ILogger<HealthCheckController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
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
  [Authorize(Policy = "Admin")]
  public string Admin()
  {
    return "you are an admin";
  }
}
