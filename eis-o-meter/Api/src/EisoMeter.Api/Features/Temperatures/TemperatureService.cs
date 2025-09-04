namespace EisoMeter.Api.Features.Temperatures;

public interface ITemperatureService
{
    Task<double> GetHighestTemperature(DateOnly date, CancellationToken cancellationToken);
}

public class TemperatureService : ITemperatureService
{
    private readonly ITemperatureStatusRepository _repo;
    private readonly IWeatherApiClient _weatherApiClient;

    public TemperatureService(
        ITemperatureStatusRepository repo,
        IWeatherApiClient weatherApiClient)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _weatherApiClient = weatherApiClient ?? throw new ArgumentNullException(nameof(weatherApiClient));
    }

    public async Task<double> GetHighestTemperature(DateOnly date, CancellationToken cancellationToken)
    {

        var currentTemperature = await _weatherApiClient.GetCurrentTemperatureAsync();
        var lastStoredTemperature = await _repo.GetTemperatureStatus(date, cancellationToken);

        if (lastStoredTemperature is null)
        {
            await InsertTemperatureStatus(date, currentTemperature, cancellationToken);
            return currentTemperature;
        }

        if (currentTemperature <= lastStoredTemperature.HighestTemperature)
        {
            return lastStoredTemperature.HighestTemperature;
        }
        
        await UpdateTemperatureStatus(date, cancellationToken, currentTemperature);

        return currentTemperature;

    }

    private async Task InsertTemperatureStatus(DateOnly date, double currentTemperature, CancellationToken cancellationToken)
    {
        var newTemperature = new TemperatureStatus
        {
            Date = date.ToString(Constants.DateFormat),
            HighestTemperature = currentTemperature,

        };
        await _repo.Insert(newTemperature, cancellationToken);
    }

    private async Task UpdateTemperatureStatus(DateOnly date, CancellationToken cancellationToken, double currentTemperature)
    {
        var updatedTemperature = new TemperatureStatus
        {
            Date = date.ToString(Constants.DateFormat),
            HighestTemperature = currentTemperature,
        };
        
        await _repo.Update(updatedTemperature, cancellationToken);
    }
}