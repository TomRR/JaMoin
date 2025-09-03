namespace EisoMeter.Api.Infrastructure;

public static class InfrastructureManager
{
    public static WebApplicationBuilder AddDatabaseSupport(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
            new SqliteConnectionFactory( 
                builder.Configuration.GetValue<string>("Database:ConnectionString")
            ));
        
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        
        return builder;
    }
    
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseInitializer>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserClaimRepository, UserClaimRepository>();
        services.AddScoped<ITemperatureStatusRepository, TemperatureStatusRepository>();

        return services;
    }
    
    public static async Task<WebApplication> InitDatabase(this WebApplication app)
    {

        var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.InitializeAsync();
        
        return app;
    }
}