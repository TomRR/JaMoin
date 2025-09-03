namespace EisoMeter.Api.Tests.Infrastructure;

public class TestDbConnectionFactory : IDbConnectionFactory
{
    private readonly IDbConnection _connection;

    public TestDbConnectionFactory(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task<IDbConnection> CreateConnectionAsync()
    {
        return Task.FromResult(_connection);
    }
}