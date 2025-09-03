using EisoMeter.Api.Common.CORS;
using EisoMeter.Api.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfigurationSources();
// Automatically registers all [Settings] classes
builder.AddSettingsOptions();

builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

builder.AddDatabaseSupport();
builder.Services.AddInfrastructureDependencies();

builder.Services.AddApiDependencies();

builder.Services.AddWeatherApiClient();

// ✅ Add CORS
builder.Services.AddCors();

var app = builder.Build();

app.UseCorsFromSettings();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
await app.InitDatabase();
app.MapFeaturesEndpoints();

app.Run();
