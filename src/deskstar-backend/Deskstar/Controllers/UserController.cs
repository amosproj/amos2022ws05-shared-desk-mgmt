using System.IdentityModel.Tokens.Jwt;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Controllers;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserUsecases _adminUsecases;
    public UserController(ILogger<UserController> logger, IUserUsecases adminUsecases)
    {
        _logger = logger;
        _adminUsecases = adminUsecases;
    }

    /// <summary>
    /// Lets an admin approve a users registration for their company
    /// </summary>
    /// <returns> empty response </returns>
    /// <remarks>
    /// Sample request:
    ///     Post /admin/approve/{userId}
    /// </remarks>
    ///
    /// <response code="200">Empty Response</response>
    /// <response code="500">Internal Server Error</response>
    /// <response code="400">Bad Request</response>
    [HttpPost("{userId}/approve")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ApproveUser(string userId)
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var adminId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);

        try
        {
            _adminUsecases.ApproveUser(adminId, userId);
            return Ok();
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return Problem(detail: e.Message, statusCode: 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
    }

    /// <summary>
    /// Lets an admin decline a users registration for their company
    /// </summary>
    /// <returns> empty response </returns>
    /// <remarks>
    /// Sample request:
    ///     Post /admin/decline/{userId}
    /// </remarks>
    ///
    /// <response code="200">Empty Response</response>
    /// <response code="500">Internal Server Error</response>
    /// <response code="400">Bad Request</response>
    [HttpPost("{userId}/decline")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeclineUser(string userId)
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var adminId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);

        try
        {
            _adminUsecases.DeclineUser(adminId, userId);
            return Ok();
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return Problem(detail: e.Message, statusCode: 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
    }
}