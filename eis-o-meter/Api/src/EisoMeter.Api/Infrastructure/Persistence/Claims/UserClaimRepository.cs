namespace EisoMeter.Api.Infrastructure.Persistence.Claims;

public interface IUserClaimRepository
{
    Task<UserClaim?> GetClaimForToday(int userId, DateOnly today, CancellationToken cancellationToken);
    Task Insert(UserClaim claim, CancellationToken cancellationToken);
}
public sealed class UserClaimRepository(IDbConnectionFactory connectionFactory) : IUserClaimRepository
{
    private const string SqlGetClaimForToday =
        "SELECT * FROM UserClaim WHERE UserId = @UserId AND LastClaimDate = @LastClaimDate LIMIT 1;";
    public async Task<UserClaim?> GetClaimForToday(int userId, DateOnly today, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        
        return await connection.QueryFirstOrDefaultAsync<UserClaim>(SqlGetClaimForToday, new {
            UserId = userId,
            LastClaimDate = today.ToString(Constants.DateFormat)
        });
    }

    private const string SqlInsert =
        "INSERT INTO UserClaim (UserId, LastClaimDate) VALUES (@UserId, @LastClaimDate);";
    public async Task Insert(UserClaim claim, CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        
        await connection.ExecuteAsync(SqlInsert, new {
            claim.UserId, 
            LastClaimDate = claim.LastClaimDate.ToString(Constants.DateFormat)
        });
    }
}