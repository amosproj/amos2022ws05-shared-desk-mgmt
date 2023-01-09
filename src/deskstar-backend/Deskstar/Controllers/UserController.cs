using Deskstar.Core;
using Deskstar.Models;
using Deskstar.Entities;
using Deskstar.Usecases;
using Deskstar.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Deskstar.Controllers;

[ApiController]
[Route("/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserUsecases _userUsecases;
    private readonly ILogger<UserController> _logger;
    private readonly IAutoMapperConfiguration _autoMapperConfiguration;

    public UserController(ILogger<UserController> logger, IUserUsecases userUsecases,
        IAutoMapperConfiguration autoMapperConfiguration)
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
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult Get()
    {
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);
        try
        {
            var entities = _userUsecases.ReadAllUsers(adminId);

            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var users = entities.Select<User, UserProfileDto>(user => mapper.Map<User, UserProfileDto>(user)).ToList();

            return Ok(users);
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
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
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        catch (EntityNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
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
    /// <response code="400">Bad Request</response>
    /// <response code="403">Forbid</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("{userId}/approve")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ApproveUser(string userId)
    {
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            _userUsecases.ApproveUser(adminId, userId);
            return Ok();
        }
        catch (ArgumentInvalidException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (InsufficientPermissionException e)
        {
            _logger.LogError(e, e.Message);
            return Forbid(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
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
    /// <response code="400">Bad Request</response>
    /// <response code="403">Forbid</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("{userId}/decline")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeclineUser(string userId)
    {
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);

        try
        {
            _userUsecases.DeclineUser(adminId, userId);
            return Ok();
        }
        catch (ArgumentInvalidException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (InsufficientPermissionException e)
        {
            _logger.LogError(e, e.Message);
            return Forbid(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
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
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("me")]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateUser(UserProfileDto userDto)
    {
        var userId = RequestInteractions.ExtractIdFromRequest(Request);
        try
        {
            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var user = mapper.Map<UserProfileDto, User>(userDto);
            
            _userUsecases.UpdateUser(userId, user);

            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
    }
    
    /// <summary>
    /// Update given user information
    /// </summary>
    /// <returns> empty response </returns>
    /// <remarks>
    /// Sample request:
    ///     Post /edit
    /// </remarks>
    ///
    /// <response code="200">Empty Response</response>
    /// <response code="400">Bad Request</response>
    /// <response code="403">Forbid</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("edit")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateGivenUser(UserProfileDto userDto)
    {
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);
        try
        {
            var mapper = _autoMapperConfiguration.GetConfiguration().CreateMapper();
            var user = mapper.Map<UserProfileDto, User>(userDto);
            _userUsecases.UpdateUser(adminId,user);
            return Ok();
        }
        catch (ArgumentInvalidException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (InsufficientPermissionException e)
        {
            _logger.LogError(e, e.Message);
            return Forbid(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
    }
    
    /// <summary>
    /// Delete given user
    /// </summary>
    /// <returns> empty response </returns>
    /// <remarks>
    /// Sample request:
    ///     Post /delete/{userId}
    /// </remarks>
    ///
    /// <response code="200">Empty Response</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("/delete/{userId}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteGivenUser(string userId)
    {
        var adminId = RequestInteractions.ExtractIdFromRequest(Request);
        try
        {
           _userUsecases.DeleteUser(adminId, userId);
            return Ok();
        }
        catch (ArgumentInvalidException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
        catch (InsufficientPermissionException e)
        {
            _logger.LogError(e, e.Message);
            return Forbid(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(statusCode: 500);
        }
    }
}