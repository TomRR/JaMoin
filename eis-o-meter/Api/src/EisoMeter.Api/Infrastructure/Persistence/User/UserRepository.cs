namespace EisoMeter.Api.Infrastructure.Persistence.User;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<int> Insert(User user, CancellationToken cancellationToken);
}

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private const string SqlGetByEmail = "SELECT * FROM Users WHERE Email = @Email LIMIT 1;";
    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<User>(SqlGetByEmail, new { Email = email });
    }

    private const string SqlInsert = "INSERT INTO Users (Email) VALUES (@Email); SELECT last_insert_rowid();";
    public async Task<int> Insert(User user, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<int>(SqlInsert, new { user.Email });
    }
}