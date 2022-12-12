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
        try
        {
            return _context.Users.Include(u => u.Company).Include(u => u.Bookings).Single(u => u.UserId == userId);
        }
        catch (Exception)
        {
            throw new ArgumentException($"There is no user with Id '{userId}'");
        }
    }
    public Guid ApproveUser(Guid adminId, string userId)
    {
        try
        {
            var guid = new Guid(userId);
            var user = _context.Users.Single(u => u.UserId == guid);

            CheckSameCompany(adminId, guid);
            user.IsApproved = true;

            _context.Update(user);
            _context.SaveChanges();

            return guid;
        }
        catch (Exception e) when (e is FormatException || e is ArgumentNullException || e is OverflowException)
        {
            _logger.LogError(e, e.Message);
            throw new ArgumentException($"'{userId}' is not a valid UserId");
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException($"There is no user with id '{userId}'");
        }

    }

    private void CheckSameCompany(Guid adminId, Guid guid)
    {
        var accessDenied = _context.Users.Where(u => u.UserId == adminId || u.UserId == guid).Select(u => u.CompanyId).ToHashSet().Count != 1;
        if (accessDenied)
            throw new ArgumentException($"'{adminId}' has no access to administrate '{guid}'");
    }

    public Guid DeclineUser(Guid adminId, string userId)
    {
        try
        {
            var guid = new Guid(userId);
            var user = _context.Users.Single(u => u.UserId == guid);

            CheckSameCompany(adminId, guid);
            if (user.IsApproved)
                throw new ArgumentException($"You cannot decline an already approved user '{guid}'");

            _context.Users.Remove(user);
            _context.SaveChanges();
            return guid;
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException($"There is no user with id '{userId}'");
        }
    }

    public List<User> ReadAllUsers(Guid adminId)
    {
        try
        {
            var admin = _context.Users.Single(user => user.UserId == adminId);
            return _context.Users.Where(user => user.CompanyId == admin.CompanyId).ToList();
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException($"There is no admin with id '{adminId}'");
        }
    }

    public Guid UpdateUser(User user)
    {
        try
        {
            _context.Users.Single(u => u.UserId == user.UserId);
            _context.Users.Update(user);
            _context.SaveChanges();
            return user.UserId;
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException($"There is no user with id '{user.UserId}'");
        }
    }
}