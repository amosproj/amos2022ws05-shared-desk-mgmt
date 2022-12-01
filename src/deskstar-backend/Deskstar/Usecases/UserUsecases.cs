using Deskstar.DataAccess;
using Deskstar.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deskstar.Usecases;

public interface IUserUsecases
{
    public User ReadSpecificUser(Guid userId);
}

public class UserUsecases : IUserUsecases
{
    private readonly ILogger<UserUsecases> _logger;

    private readonly DataContext _context;
    UserUsecases(ILogger<UserUsecases> logger, DataContext context)
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
        catch (Exception e)
        {
            throw new ArgumentException($"There is no user with Id '{userId}'");
        }
    }
}