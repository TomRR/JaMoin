namespace EisoMeter.Api.Tests.Infrastructure;

public class UserClaimRepositoryTests : IAsyncLifetime
    {
        private IDbConnection _connection;
        private IUserClaimRepository _repository;

        public async Task InitializeAsync()
        {
            // Create in-memory SQLite database
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = ":memory:",
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            }.ToString();

            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            // Create UserClaim table
            await _connection.ExecuteAsync(@"
CREATE TABLE IF NOT EXISTS UserClaim (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    LastClaimDate TEXT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
");
            // Provide a connection factory for the repository
            var connectionFactory = new TestDbConnectionFactory(_connection);
            _repository = new UserClaimRepository(connectionFactory);
        }

        public Task DisposeAsync()
        {
            _connection.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetClaimForToday_NoClaim_ReturnsNull()
        {
            // Arrange
            var userId = 99;
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // Act
            var result = await _repository.GetClaimForToday(userId, today, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }