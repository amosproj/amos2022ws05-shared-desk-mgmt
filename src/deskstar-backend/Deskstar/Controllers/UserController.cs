using System.IdentityModel.Tokens.Jwt;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Controllers;

[ApiController]
[Route("/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;
    private readonly IUserUsecases _userUsecases;

    UserController(ILogger<UserController> logger, IUserUsecases userUsecases)
    {
        _logger = logger;
        _userUsecases = userUsecases;
    }

    [HttpGet("me")]
    public IActionResult GetMe()
    {

        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var userId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);

        try
        {
            var me = _userUsecases.ReadSpecificUser(userId);
            var userDto = new UserDto
            {
                CompanyId = me.CompanyId,
                CompanyName = me.Company.CompanyName,
                FirstName = me.FirstName,
                IsApproved = me.IsApproved,
                IsCompanyAdmin = me.IsCompanyAdmin,
                LastName = me.LastName,
                MailAddress = me.MailAddress,
                UserId = me.UserId
            };
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