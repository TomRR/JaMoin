namespace EisoMeter.Api.Tests.Features.Temperatures
{
    public class TemperatureServiceTests
    {
        private readonly ITemperatureStatusRepository _repo = Substitute.For<ITemperatureStatusRepository>();
        private readonly IWeatherApiClient _weatherApiClient = Substitute.For<IWeatherApiClient>();
        private readonly IDateProvider _dateProvider = Substitute.For<IDateProvider>();

        private readonly TemperatureService _service;

        public TemperatureServiceTests()
        {
            _service = new TemperatureService(_repo, _weatherApiClient, _dateProvider);
        }

        [Fact]
        public async Task GetHighestTemperature_NoStoredTemperature_InsertsAndReturnsCurrent()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            double currentTemperature = 25.5;

            _weatherApiClient.GetCurrentTemperatureAsync().Returns(currentTemperature);
            _repo.GetTemperatureStatus(date, Arg.Any<CancellationToken>()).Returns((TemperatureStatus)null);

            // Act
            var result = await _service.GetHighestTemperature(date, CancellationToken.None);

            // Assert
            Assert.Equal(currentTemperature, result);
            await _repo.Received(1).Insert(Arg.Is<TemperatureStatus>(t =>
                t.Date == date.ToString(Constants.DateFormat) && t.HighestTemperature == currentTemperature
            ), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetHighestTemperature_StoredTemperatureHigherOrEqual_ReturnsStored()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            double currentTemperature = 20;
            double storedTemperature = 25;

            _weatherApiClient.GetCurrentTemperatureAsync().Returns(currentTemperature);
            _repo.GetTemperatureStatus(date, Arg.Any<CancellationToken>()).Returns(new TemperatureStatus
            {
                Date = date.ToString(Constants.DateFormat),
                HighestTemperature = storedTemperature
            });

            // Act
            var result = await _service.GetHighestTemperature(date, CancellationToken.None);

            // Assert
            Assert.Equal(storedTemperature, result);
            await _repo.DidNotReceive().Update(Arg.Any<TemperatureStatus>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetHighestTemperature_CurrentHigherThanStored_UpdatesAndReturnsCurrent()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            double currentTemperature = 30;
            double storedTemperature = 25;

            _weatherApiClient.GetCurrentTemperatureAsync().Returns(currentTemperature);
            _repo.GetTemperatureStatus(date, Arg.Any<CancellationToken>()).Returns(new TemperatureStatus
            {
                Date = date.ToString(Constants.DateFormat),
                HighestTemperature = storedTemperature
            });

            // Act
            var result = await _service.GetHighestTemperature(date, CancellationToken.None);

            // Assert
            Assert.Equal(currentTemperature, result);
            await _repo.Received(1).Update(Arg.Is<TemperatureStatus>(t =>
                t.Date == date.ToString(Constants.DateFormat) && t.HighestTemperature == currentTemperature
            ), Arg.Any<CancellationToken>());
        }
    }
}