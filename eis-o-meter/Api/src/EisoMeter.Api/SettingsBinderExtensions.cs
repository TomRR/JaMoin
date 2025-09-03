using EisoMeter.Api.Common.CORS;

namespace EisoMeter.Api;

public static class SettingsBinderExtensions
{
    public static IConfigurationBuilder AddConfigurationSources(this IConfigurationBuilder builder)
    {
        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        return builder;
    }
    
    public static WebApplicationBuilder AddSettingsOptions(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<CorsSettings>()
            .Bind(builder.Configuration.GetSection(CorsSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return builder;
    }
}