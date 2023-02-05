/**
 * UserUsecases
 *
 * Version 1.0
 *
 * 2023-01-03
 *
 * MIT License
 */

using System.Text.RegularExpressions;
using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Helper;
using Microsoft.EntityFrameworkCore;

namespace Deskstar.Usecases;

public interface IUserUsecases
{
  public List<User> ReadAllUsers(Guid adminId);
  public User ReadSpecificUser(Guid userId);

  public Guid UpdateUser(string requestUserId, User user);
  public Guid ApproveUser(Guid adminId, string userId);
  public Guid DeclineUser(Guid adminId, string userId);
  public Guid UpdateUser(Guid adminId, User user);
  public Guid DeleteUser(Guid adminId, string userId);
}

public class UserUsecases : IUserUsecases
{
  private readonly DataContext _context;
  private readonly ILogger<UserUsecases> _logger;

  public UserUsecases(ILogger<UserUsecases> logger, DataContext context)
  {
    _logger = logger;
    _context = context;
  }

  public User ReadSpecificUser(Guid userId)
  {
    var user = _context.Users.Include(u => u.Company).Include(u => u.Bookings).SingleOrDefault(u => u.UserId == userId);
    if (user == null)
      throw new EntityNotFoundException($"There is no user with Id '{userId}'");
    return user;
  }

  public Guid ApproveUser(Guid adminId, string userId)
  {
    Guid guid;
    try
    {
      guid = new Guid(userId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{userId}' is not a valid UserId");
    }

    var user = _context.Users.SingleOrDefault(u => u.UserId == guid);
    if (user == null)
      throw new EntityNotFoundException($"There is no user with id '{userId}'");

    CheckSameCompany(adminId, guid);
    user.IsApproved = true;

    _context.Update(user);
    _context.SaveChanges();

    var body = $"Hello {user.FirstName},<br/> " +
               $"your account has been approved by {ReadSpecificUser(adminId).FirstName}.<br/> " +
               "You can now log into the system.<br/>" +
               "You can now book your first desk and get to work.<br/>";
    EmailHelper.SendEmail(_logger, user.MailAddress, "Your Deskstar account has been approved!", body);

    return guid;
  }

  public Guid DeclineUser(Guid adminId, string userId)
  {
    Guid guid;
    try
    {
      guid = new Guid(userId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{userId}' is not a valid UserId");
    }

    var user = _context.Users.SingleOrDefault(u => u.UserId == guid);
    if (user == null)
      throw new EntityNotFoundException($"There is no user with id '{userId}'");

    CheckSameCompany(adminId, guid);
    if (user.IsApproved)
      throw new ArgumentInvalidException($"You cannot rejected an already approved user '{guid}'");

    var body = $"Hello {user.FirstName},<br/> " +
               "your account has been rejected.<br/> " +
               "Please contact one of your company's admins if you think this was a mistake.<br/>";
    EmailHelper.SendEmail(_logger, user.MailAddress, "Your Deskstar account has been rejected!", body);

    _context.Users.Remove(user);
    _context.SaveChanges();

    return guid;
  }

  public List<User> ReadAllUsers(Guid adminId)
  {
    var admin = _context.Users.SingleOrDefault(user => user.UserId == adminId);
    if (admin == null)
      throw new EntityNotFoundException($"There is no admin with id '{adminId}'");
    return _context.Users.Where(user => user.CompanyId == admin.CompanyId)
      .ToList();
  }

  public Guid UpdateUser(string requestUserId, User user)
  {
    Guid guid;
    try
    {
      guid = new Guid(requestUserId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{requestUserId}' is not a valid UserId");
    }

    if (!guid.Equals(user.UserId))
      throw new ArgumentInvalidException($"'{requestUserId}' is not equals with given userObject");

    return SaveUpdateUser(user);
  }

  public Guid UpdateUser(Guid adminId, User user)
  {
    var userDbInstance = _context.Users.SingleOrDefault(u => u.UserId == user.UserId);
    if (userDbInstance == null)
      throw new EntityNotFoundException($"There is no user with id '{user.UserId}'");
    CheckSameCompany(adminId, user.UserId);
    if (adminId == user.UserId && !user.IsCompanyAdmin)
      throw new ArgumentInvalidException("You cannot delete your own admin rights");
    return SaveUpdateUser(user);
  }

  public Guid DeleteUser(Guid adminId, string userId)
  {
    Guid guid;
    try
    {
      guid = new Guid(userId);
    }
    catch (Exception e) when (e is FormatException or ArgumentNullException or OverflowException)
    {
      _logger.LogError(e, e.Message);
      throw new ArgumentInvalidException($"'{userId}' is not a valid UserId");
    }

    var userDbInstance = _context.Users.SingleOrDefault(u => u.UserId == guid);
    if (userDbInstance == null)
      throw new EntityNotFoundException($"There is no user with id '{userId}'");
    CheckSameCompany(adminId, guid);
    if (adminId == guid)
      throw new ArgumentInvalidException("You cannot delete yourself");
    var body = $"Hello {userDbInstance.FirstName},<br/> " +
               "your account has been deleted by your Company admin.<br/>" +
               "If you think this was an mistake, get in touch with your company admin.<br/>";
    EmailHelper.SendEmail(_logger, userDbInstance.MailAddress, "Your Deskstar account has been deleted!", body);
    userDbInstance.IsMarkedForDeletion = true;
    _context.SaveChanges();

    return guid;
  }

  private void CheckSameCompany(Guid adminId, Guid guid)
  {
    var accessDenied = _context.Users.Where(u => u.UserId == adminId || u.UserId == guid).Select(u => u.CompanyId)
      .ToHashSet().Count != 1;
    if (accessDenied)
      throw new InsufficientPermissionException($"'{adminId}' has no access to administrate '{guid}'");
  }

  private Guid SaveUpdateUser(User user)
  {
    if (user.UserId == Guid.Empty)
      throw new ArgumentInvalidException($"'{nameof(user.UserId)}' is empty");
    if (string.IsNullOrEmpty(user.FirstName))
      throw new ArgumentInvalidException($"'{nameof(user.FirstName)}' is not set");
    if (string.IsNullOrEmpty(user.LastName))
      throw new ArgumentInvalidException($"'{nameof(user.LastName)}' is not set");
    if (string.IsNullOrEmpty(user.MailAddress))
      throw new ArgumentInvalidException($"'{nameof(user.MailAddress)}' is not set");
    var rx = new Regex(
      "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\\])",
      RegexOptions.IgnoreCase);
    if (rx.Matches(user.MailAddress).Count != 1)
      throw new ArgumentInvalidException("Mailaddress is not valid");
    var userDbInstance = _context.Users.SingleOrDefault(u => u.UserId == user.UserId);
    if (userDbInstance == null)
      throw new EntityNotFoundException($"There is no user with id '{user.UserId}'");
    userDbInstance.FirstName = user.FirstName;
    userDbInstance.LastName = user.LastName;
    userDbInstance.MailAddress = user.MailAddress;
    userDbInstance.IsCompanyAdmin = user.IsCompanyAdmin;
    userDbInstance.IsMarkedForDeletion = user.IsMarkedForDeletion;
    _context.SaveChanges();
    var body = $"Hello {user.FirstName},<br/> " +
               "your account details have been updated.<br/> " +
               "Please check if this was ok.<br/>" +
               "If not get in touch with your company admin.<br/>";
    EmailHelper.SendEmail(_logger, user.MailAddress, "Your Deskstar account has been updated!", body);
    return user.UserId;
  }
}
