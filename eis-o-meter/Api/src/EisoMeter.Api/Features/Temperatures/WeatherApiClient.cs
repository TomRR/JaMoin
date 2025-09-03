using System.Text.Json;

namespace EisoMeter.Api.Features.Temperatures;

public interface IWeatherApiClient
{
    Task<double> GetCurrentTemperatureAsync();
}
public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double> GetCurrentTemperatureAsync()
    {
        try
        {
            var json = await _httpClient.GetStringAsync($"/current_weather?lat={Constants.LatitudeHamburg}&lon={Constants.LongitudeHamburg}");
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("weather", out var weatherElement) &&
                weatherElement.TryGetProperty("temperature", out var tempElement) &&
                tempElement.TryGetDouble(out var temperature))
            {
                return temperature;
            }

            return 0;
        }
        catch (HttpRequestException e)
        {
            // LOG
            return 0;
        }
        catch (JsonException e)
        {
            // LOG
            return 0;
        }
    }
}

public static class WeatherApiClientExtensions
{
    public static IServiceCollection AddWeatherApiClient(this IServiceCollection services)
    {
        services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.brightsky.dev");
        });

        return services;
    }
}