using System.IdentityModel.Tokens.Jwt;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Controllers;

[ApiController]
[Route("/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{

    private readonly IUserUsecases _userUsecases;
    private readonly ILogger<UserController> _logger;
    private readonly IAutoMapperConfiguration _autoMapperConfiguration;

    public UserController(ILogger<UserController> logger, IUserUsecases userUsecases, IAutoMapperConfiguration autoMapperConfiguration)
    {
        _logger = logger;
        _userUsecases = userUsecases;
        _autoMapperConfiguration = autoMapperConfiguration;
    }

    /// <summary>
    /// Returns user specific information
    /// </summary>
    /// <returns> User information in JSON Format </returns>
    /// <remarks>
    /// Sample request:
    ///     Get /users/me with JWT Token
    /// </remarks>
    /// 
    /// <response code="200">Returns information about the logged in user</response>
    /// <response code="500">Internal Server Error</response>
    /// <response code="400">Bad Request</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetMe()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var userId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);

        try
        {
            var me = _userUsecases.ReadSpecificUser(userId);
            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var userDto = mapper.Map<Entities.User, UserDto>(me);
            return Ok(userDto);
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