namespace EisoMeter.Api.Tests.Features.Temperatures;

public class WeatherApiClientTests
{
    [Fact]
    public async Task GetCurrentTemperatureAsync_ReturnsTemperature()
    {
        var handler = new FakeHttpMessageHandler("{\"weather\": {\"temperature\": 25.5}}");        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.brightsky.dev")
        };

        var apiClient = new WeatherApiClient(client);
        var temp = await apiClient.GetCurrentTemperatureAsync();

        Assert.Equal(25.5, temp);
    }
}