namespace EisoMeter.Api.Infrastructure.Persistence.Temperature;

public sealed class TemperatureStatus
{
    public int Id { get; set; }
    public string Date { get; set; }
    public double HighestTemperature { get; set; }
}   