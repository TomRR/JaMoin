namespace EisoMeter.Api.Tests.Infrastructure
{
    public class TemperatureStatusRepositoryTests : IAsyncLifetime
    {
        private IDbConnection _connection;
        private ITemperatureStatusRepository _repository;

        public async Task InitializeAsync()
        {
            // Use a temporary file-based SQLite DB
            var tempDbPath = Path.Combine(Path.GetTempPath(), $"TestDb_{Guid.NewGuid()}.db");
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = tempDbPath
            }.ToString();

            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            // Create TemperatureStatus table
            await _connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS TemperatureStatus (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    HighestTemperature REAL NOT NULL
                );
            ");

            // Repository with test connection factory
            _repository = new TemperatureStatusRepository(new TestDbConnectionFactory(_connection));
        }

        public Task DisposeAsync()
        {
            _connection.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Insert_ThenGetTemperatureStatus_ReturnsInserted()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var tempStatus = new TemperatureStatus
            {
                Date = date.ToString(Constants.DateFormat),
                HighestTemperature = 28.5
            };

            // Act
            await _repository.Insert(tempStatus, CancellationToken.None);
            var result = await _repository.GetTemperatureStatus(date, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tempStatus.Date, result.Date);
            Assert.Equal(tempStatus.HighestTemperature, result.HighestTemperature);
        }

        [Fact]
        public async Task GetTemperatureStatus_NoRecord_ReturnsNull()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);

            // Act
            var result = await _repository.GetTemperatureStatus(date, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_ExistingRecord_ReturnsTrueAndUpdatesValue()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var initial = new TemperatureStatus
            {
                Date = date.ToString(Constants.DateFormat),
                HighestTemperature = 25
            };

            await _repository.Insert(initial, CancellationToken.None);

            var updated = new TemperatureStatus
            {
                Date = date.ToString(Constants.DateFormat),
                HighestTemperature = 30,
            };

            // Act
            var success = await _repository.Update(updated, CancellationToken.None);
            var result = await _repository.GetTemperatureStatus(date, CancellationToken.None);

            // Assert
            Assert.True(success);
            Assert.Equal(updated.HighestTemperature, result.HighestTemperature);
        }

        [Fact]
        public async Task Update_NonExistingRecord_ReturnsFalse()
        {
            // Arrange
            var updated = new TemperatureStatus
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow).ToString(Constants.DateFormat),
                HighestTemperature = 30
            };

            // Act
            var success = await _repository.Update(updated, CancellationToken.None);

            // Assert
            Assert.False(success);
        }
    }
}
