using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class CompanyUsecasesTests
{
  private void setupMockData(DataContext db, Guid companyID, String companyName)
  {
    var company = new Company
    {
      CompanyId = companyID,
      CompanyName = companyName
    };
    db.Companies.Add(company);
    db.SaveChanges();
  }

  private void GetCompanies_ShouldReturnAllCompanies()
  {
    // setup
    using var db = new DataContext();
    var companyID = Guid.NewGuid();
    var companyName = "Test Company";
    setupMockData(db, companyID, companyName);

    // arrange
    var logger = new Mock<ILogger<CompanyUsecases>>();
    var companyUsecases = new CompanyUsecases(db, logger.Object);


    // act
    var companies = companyUsecases.GetCompanies();

    // assert
    Assert.That(1 == companies.Count());
    Assert.That(companyID.ToString() == companies.First().CompanyId);
    Assert.That(companyName == companies.First().CompanyName);

    // cleanup
    db.Database.EnsureDeleted();
  }
}
