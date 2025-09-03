namespace EisoMeter.Api.Infrastructure.Persistence;

public sealed class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await CreateUserTableIfNotExists(connection);
        await CreateUserClaimTableIfNotExists(connection);
        await CreateUniqueIndexToAvaidMultipleClaims(connection);
        await CreateTemperatureStatusTableIfNotExists(connection);

        
    }

    private static async Task CreateUserTableIfNotExists(IDbConnection connection)
    {
        await connection.ExecuteAsync(
            @"
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Email TEXT NOT NULL UNIQUE
);"
        );
    }
    
    private static async Task CreateUserClaimTableIfNotExists(IDbConnection connection)
    {
        await connection.ExecuteAsync(
            @"
CREATE TABLE IF NOT EXISTS UserClaim (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    LastClaimDate TEXT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
"
        );
    }
    
    private static async Task CreateUniqueIndexToAvaidMultipleClaims(IDbConnection connection)
    {
        await connection.ExecuteAsync(
            @"
CREATE UNIQUE INDEX IF NOT EXISTS IX_UserClaim_UserId_ClaimDate
ON UserClaim (UserId, LastClaimDate);
"
        );
    }
    
    private static async Task CreateTemperatureStatusTableIfNotExists(IDbConnection connection)
    {
        await connection.ExecuteAsync(
            @"
CREATE TABLE IF NOT EXISTS TemperatureStatus (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    HighestTemperature REAL NOT NULL
);"
        );
    }
}