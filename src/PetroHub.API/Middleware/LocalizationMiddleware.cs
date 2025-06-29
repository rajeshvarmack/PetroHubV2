using System.Globalization;

namespace PetroHub.API.Middleware;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LocalizationMiddleware> _logger;

    public LocalizationMiddleware(RequestDelegate next, ILogger<LocalizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = GetCultureFromRequest(context);
        
        if (!string.IsNullOrEmpty(culture))
        {
            try
            {
                var cultureInfo = new CultureInfo(culture);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
                
                _logger.LogDebug("Culture set to: {Culture}", culture);
            }
            catch (CultureNotFoundException ex)
            {
                _logger.LogWarning(ex, "Invalid culture specified: {Culture}", culture);
                // Fall back to default culture
                var defaultCulture = new CultureInfo("en-US");
                CultureInfo.CurrentCulture = defaultCulture;
                CultureInfo.CurrentUICulture = defaultCulture;
            }
        }

        await _next(context);
    }

    private static string? GetCultureFromRequest(HttpContext context)
    {
        // Priority order:
        // 1. Query parameter 'culture'
        // 2. Header 'Accept-Language'
        // 3. Cookie 'culture'

        // Check query parameter
        if (context.Request.Query.TryGetValue("culture", out var cultureQuery))
        {
            return cultureQuery.FirstOrDefault();
        }

        // Check Accept-Language header
        var acceptLanguage = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            // Parse the first culture from Accept-Language header
            var cultures = acceptLanguage.Split(',');
            if (cultures.Length > 0)
            {
                var primaryCulture = cultures[0].Split(';')[0].Trim();
                return primaryCulture;
            }
        }

        // Check cookie
        if (context.Request.Cookies.TryGetValue("culture", out var cultureCookie))
        {
            return cultureCookie;
        }

        return null;
    }
}
