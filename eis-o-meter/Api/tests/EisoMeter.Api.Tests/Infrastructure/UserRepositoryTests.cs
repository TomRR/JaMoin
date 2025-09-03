namespace EisoMeter.Api.Tests.Infrastructure;

public class UserRepositoryTests : IAsyncLifetime
    {
        private IDbConnection _connection;
        private IUserRepository _repository;

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

            // Create Users table
            await _connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Email TEXT NOT NULL UNIQUE
                );
            ");

            _repository = new UserRepository(new TestDbConnectionFactory(_connection));
        }

        public Task DisposeAsync()
        {
            _connection.Dispose();
            return Task.CompletedTask;
        }
        

        [Fact]
        public async Task GetByEmail_NonExisting_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";

            // Act
            var result = await _repository.GetByEmail(email, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Insert_DuplicateEmail_ThrowsException()
        {
            // Arrange
            var email = "duplicate@example.com";
            var user1 = new User { Email = email };
            var user2 = new User { Email = email };

            // Act
            await _repository.Insert(user1, CancellationToken.None);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(() => _repository.Insert(user2, CancellationToken.None));
        }
    }