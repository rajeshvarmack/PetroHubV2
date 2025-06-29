using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PetroHub.Application.Resources;

namespace PetroHub.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class HealthController : BaseApiController
{
    public HealthController(IStringLocalizer<SharedResources> localizer) : base(localizer)
    {
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            Version = "1.0.0"
        });
    }

    [HttpGet("culture")]
    public IActionResult GetCulture()
    {
        return Ok(new
        {
            CurrentCulture = System.Globalization.CultureInfo.CurrentCulture.Name,
            CurrentUICulture = System.Globalization.CultureInfo.CurrentUICulture.Name,
            LocalizedMessage = _localizer["Success"].Value
        });
    }
}
