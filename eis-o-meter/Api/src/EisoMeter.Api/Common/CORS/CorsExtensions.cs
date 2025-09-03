using Microsoft.Extensions.Options;

namespace EisoMeter.Api.Common.CORS;

public static class CorsExtensions
{
    public static WebApplication UseCorsFromSettings(this WebApplication app)
    {
        var corsSettings = app.Services.GetRequiredService<IOptions<CorsSettings>>().Value;

        app.UseCors(policyBuilder =>
            policyBuilder
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(corsSettings.OriginUri)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

        return app;
    }
}