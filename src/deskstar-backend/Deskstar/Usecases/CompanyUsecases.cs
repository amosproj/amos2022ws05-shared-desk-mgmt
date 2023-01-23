using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;

namespace Deskstar.Usecases;

public interface ICompanyUsecases
{
  public List<Company> GetCompanies();
}

public class CompanyUsecases : ICompanyUsecases
{
  private readonly DataContext _context;

  private readonly ILogger<CompanyUsecases> _logger;

  public CompanyUsecases(DataContext context, ILogger<CompanyUsecases> logger)
  {
    _context = context;
    _logger = logger;
  }

  public List<Company> GetCompanies()
  {
    return _context.Companies.ToList();
  }
}