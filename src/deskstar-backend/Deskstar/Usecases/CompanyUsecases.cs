using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Models;

namespace Deskstar.Usecases;

public interface ICompanyUsecases
{
  public List<CompanyDto> GetCompanies();
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

  public List<CompanyDto> GetCompanies()
  {
    var dbCompanies = _context.Companies.ToList();

    if (dbCompanies.ToList().Count == 0) return new List<CompanyDto>();

    var mapCompaniesToCompaniesDto = dbCompanies.Select((c) => new CompanyDto
    {
      CompanyId = c.CompanyId.ToString(),
      CompanyName = c.CompanyName,
    }).ToList();

    return mapCompaniesToCompaniesDto;
  }
}
