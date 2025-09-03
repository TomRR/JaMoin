namespace EisoMeter.Api;

internal static class ApiDependenciesInjection
{
    internal static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClaimRewardHandler, ClaimRewardHandler>();
        services.AddScoped<IGetClaimStatusHandler, GetClaimStatusHandler>();
        services.AddScoped<IDateProvider, DateProvider>();
        services.AddScoped<ITemperatureService, TemperatureService>();
        
        return services;
    }
}