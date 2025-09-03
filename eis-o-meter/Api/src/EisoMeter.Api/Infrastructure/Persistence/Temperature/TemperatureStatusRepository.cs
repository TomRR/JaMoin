namespace EisoMeter.Api.Infrastructure.Persistence.Temperature;

public interface ITemperatureStatusRepository
{
    Task<TemperatureStatus?> GetTemperatureStatus(DateOnly date, CancellationToken cancellationToken);
    Task<bool> Update(TemperatureStatus temperatureStatus, CancellationToken cancellationToken);
    Task Insert(TemperatureStatus status, CancellationToken cancellationToken);
}

public sealed class TemperatureStatusRepository : ITemperatureStatusRepository
{
    
    private readonly IDbConnectionFactory _connectionFactory;

    public TemperatureStatusRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<TemperatureStatus?> GetTemperatureStatus(DateOnly date, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        const string sql = "SELECT * FROM TemperatureStatus WHERE Date = @Date LIMIT 1;";
        
        var result = await connection.QueryFirstOrDefaultAsync<TemperatureStatus>(sql, new { Date = date.ToString(Constants.DateFormat) });
        return result;
    }
    
    public async Task<bool> Update(TemperatureStatus temperatureStatus, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"UPDATE TemperatureStatus SET HighestTemperature = @HighestTemperature WHERE Date = @Date;";
        
        var result = await connection.ExecuteAsync(sql, new {
            HighestTemperature = temperatureStatus.HighestTemperature,
            Date = temperatureStatus.Date
        });
        
        return result is 1;
    }
    
    public async Task Insert(TemperatureStatus status, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = "INSERT INTO TemperatureStatus (Date, HighestTemperature) VALUES (@Date, @HighestTemperature);";
        await connection.ExecuteAsync(
            sql, 
            new { 
                Date = status.Date, 
                status.HighestTemperature, 
            });
    }
}