using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deskstar.Usecases;

public interface IUserUsecases
{
    public List<User> ReadAllUsers(Guid adminId);
    public Guid UpdateUser(User user);
    public User ReadSpecificUser(Guid userId);
    public Guid ApproveUser(Guid adminId, string userId);
    public Guid DeclineUser(Guid adminId, string userId);
}

public class UserUsecases : IUserUsecases
{
    private readonly ILogger<UserUsecases> _logger;

    private readonly DataContext _context;
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

        return guid;

    }

    private void CheckSameCompany(Guid adminId, Guid guid)
    {
        var accessDenied = _context.Users.Where(u => u.UserId == adminId || u.UserId == guid).Select(u => u.CompanyId).ToHashSet().Count != 1;
        if (accessDenied)
            throw new InsufficientPermissionException($"'{adminId}' has no access to administrate '{guid}'");
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
            throw new ArgumentInvalidException($"You cannot decline an already approved user '{guid}'");

        _context.Users.Remove(user);
        _context.SaveChanges();

        return guid;
    }

    public List<User> ReadAllUsers(Guid adminId)
    {
        var admin = _context.Users.SingleOrDefault(user => user.UserId == adminId);
        if (admin == null)
            throw new EntityNotFoundException($"There is no admin with id '{adminId}'");
        return _context.Users.Where(user => user.CompanyId == admin.CompanyId).ToList();
    }

    public Guid UpdateUser(User user)
    {
        var userDbInstance = _context.Users.SingleOrDefault(u => u.UserId == user.UserId);
        if (userDbInstance == null)
            throw new EntityNotFoundException($"There is no user with id '{user.UserId}'");
        _context.Users.Update(user);
        _context.SaveChanges();
        return user.UserId;
    }
}