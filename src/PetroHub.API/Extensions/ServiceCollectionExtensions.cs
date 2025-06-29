using System.Globalization;
using Asp.Versioning;
using FluentValidation;
using Microsoft.Extensions.Localization;
using PetroHub.API.Middleware;
using PetroHub.Application.Resources;
using PetroHub.Infrastructure.Extensions;

namespace PetroHub.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Infrastructure
        services.AddInfrastructure(configuration);

        // Controllers
        services.AddControllers();

        // API Versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader("version"),
                new HeaderApiVersionReader("X-Version")
            );
        }).AddApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        // API Explorer for Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() 
            { 
                Title = "PetroHub API", 
                Version = "v1",
                Description = "A comprehensive API for petrol station management"
            });
            
            // Configure Swagger to include API versioning
            c.DocInclusionPredicate((version, desc) => true);
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        // Localization
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar"),
                new CultureInfo("hi")
            };

            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<SharedResources>();

        // Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<PetroHub.Infrastructure.Data.ApplicationDbContext>();

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }

    public static WebApplication ConfigureApiPipeline(this WebApplication app)
    {
        // Development specific middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetroHub API v1");
                c.RoutePrefix = string.Empty; // Makes Swagger UI available at root
            });
        }

        // Global middleware
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<LocalizationMiddleware>();

        app.UseHttpsRedirection();
        app.UseCors("DefaultPolicy");

        // Localization
        app.UseRequestLocalization();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Health checks
        app.MapHealthChecks("/health");

        return app;
    }
}
