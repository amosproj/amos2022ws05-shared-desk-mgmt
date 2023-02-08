/**
 * Program
 *
 * Version 1.0
 *
 * 2023-01-03
 *
 * MIT License
 */

using Deskstar.Models;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deskstar.Controllers;

[ApiController]
[Route("/companies")]
[Produces("application/json")]
public class CompanyController : ControllerBase
{
  private readonly IAutoMapperConfiguration _autoMapperConfiguration;
  private readonly ICompanyUsecases _companyUsecases;

  private readonly ILogger<CompanyController> _logger;

  public CompanyController(ILogger<CompanyController> logger, ICompanyUsecases companyUsecases,
    IAutoMapperConfiguration autoMapperConfiguration)
  {
    _logger = logger;
    _companyUsecases = companyUsecases;
    _autoMapperConfiguration = autoMapperConfiguration;
  }

  /// <summary>
  ///   Get all companies
  /// </summary>
  /// <returns> A list of companies</returns>
  /// <remarks>
  ///   Sample request: Get /companies
  /// </remarks>
  /// <response code="200">Returns a list of companies</response>
  /// <response code="500">Internal Server Error</response>
  [HttpGet]
  [AllowAnonymous]
  [ProducesResponseType(typeof(List<CompanyDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public IActionResult GetCompanies()
  {
    List<CompanyDto> companies;
    try
    {
      companies = _companyUsecases.GetCompanies();
      return Ok(companies);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error while getting companies");
      return Problem(statusCode: 500);
    }
  }
}
