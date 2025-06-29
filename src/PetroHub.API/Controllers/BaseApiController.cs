using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PetroHub.Application.Resources;
using PetroHub.Domain.Common;

namespace PetroHub.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly IStringLocalizer<SharedResources> _localizer;

    protected BaseApiController(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(new ApiResponse<T>
            {
                Success = true,
                Data = result.Data,
                Message = _localizer["Success"]
            });
        }

        return BadRequest(new ApiResponse<T>
        {
            Success = false,
            Message = result.ErrorMessage ?? _localizer["Error"],
            Errors = result.Errors
        });
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Message = _localizer["Success"]
            });
        }

        return BadRequest(new ApiResponse
        {
            Success = false,
            Message = result.ErrorMessage ?? _localizer["Error"],
            Errors = result.Errors
        });
    }

    protected IActionResult NotFoundResponse(string? message = null)
    {
        return NotFound(new ApiResponse
        {
            Success = false,
            Message = message ?? _localizer["NotFound"]
        });
    }

    protected IActionResult UnauthorizedResponse(string? message = null)
    {
        return Unauthorized(new ApiResponse
        {
            Success = false,
            Message = message ?? _localizer["Unauthorized"]
        });
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}
