using Deskstar.Core;
using Deskstar.Entities;
using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    /// Returns all users for a specific company
    /// </summary>
    /// <returns> List of user information in JSON Format </returns>
    /// <remarks>
    /// Sample request:
    ///     Get /users with JWT Token
    /// </remarks>
    /// 
    /// <response code="200">List of user information in JSON Format</response>
    /// <response code="500">Internal Server Error</response>
    /// <response code="400">Bad Request</response>
    [HttpGet]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult Get()
    {
        var userId = RequestInteractions.ExtractIdFromRequest(Request);
        try
        {
            var entities = _userUsecases.ReadAllUsers(userId);
            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var users = entities.Select<User, UserProfileDto>(user => mapper.Map<User, UserProfileDto>(user)).ToList();

            return Ok(users);
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
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult GetMe()
    {
        var userId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            var me = _userUsecases.ReadSpecificUser(userId);
            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var UserProfileDto = mapper.Map<Entities.User, UserProfileDto>(me);
            return Ok(UserProfileDto);
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
    /// Lets an admin approve a users registration for their company
    /// </summary>
    /// <returns> empty response </returns>
    /// <remarks>
    /// Sample request:
    ///     Post /users/{userId}/approve
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
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            _userUsecases.ApproveUser(adminId, userId);
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
    ///     Post /users/{userId}/decline
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
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            _userUsecases.DeclineUser(adminId, userId);
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
    /// Update user information
    /// </summary>
    /// <returns> empty response </returns>
    /// <remarks>
    /// Sample request:
    ///     Post /users/me
    /// </remarks>
    ///
    /// <response code="200">Empty Response</response>
    /// <response code="500">Internal Server Error</response>
    /// <response code="400">Bad Request</response>
    [HttpPost("me")]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateUser(UserProfileDto userDto)
    {
        var userId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var user = mapper.Map<UserProfileDto, User>(userDto);
            _userUsecases.UpdateUser(user);
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