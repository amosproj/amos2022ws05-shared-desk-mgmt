using Deskstar.DataAccess;

namespace Deskstar.Usecases;

public interface IUserUsecases
{
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
}