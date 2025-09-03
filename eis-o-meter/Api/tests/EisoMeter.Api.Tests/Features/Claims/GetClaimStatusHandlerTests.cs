namespace EisoMeter.Api.Tests.Features.Claims;

public class GetClaimStatusHandlerTests
{
        private readonly IUserRepository _userRepo = Substitute.For<IUserRepository>();
        private readonly IUserClaimRepository _userClaimRepo = Substitute.For<IUserClaimRepository>();
        private readonly ITemperatureService _temperatureService = Substitute.For<ITemperatureService>();
        private readonly IDateProvider _dateProvider = Substitute.For<IDateProvider>();

        private readonly GetClaimStatusHandler _handler;

        public GetClaimStatusHandlerTests()
        {
            _handler = new GetClaimStatusHandler(
                _userRepo,
                _userClaimRepo,
                _temperatureService,
                _dateProvider
            );
        }

        [Fact]
        public async Task Handle_EmptyEmail_ReturnsLockedClaim()
        {
            // Arrange
            string email = "";
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            _dateProvider.DateOnlyUtcNow.Returns(today);
            _temperatureService.GetHighestTemperature(today, Arg.Any<CancellationToken>())
                .Returns(Constants.RewardThreshold + 1);

            // Act
            var result = await _handler.Handle(email, CancellationToken.None);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal(GetClaimStatusType.Locked, result.Value.Status);
            Assert.Equal(today.ToString(Constants.DateFormat), result.Value.LastUpdate);
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsReadyToClaim()
        {
            // Arrange
            string email = "nonexistent@example.com";
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            _dateProvider.DateOnlyUtcNow.Returns(today);
            _temperatureService.GetHighestTemperature(today, Arg.Any<CancellationToken>())
                .Returns(Constants.RewardThreshold + 1);
            _userRepo.GetByEmail(email, Arg.Any<CancellationToken>()).Returns((User)null);

            // Act
            var result = await _handler.Handle(email, CancellationToken.None);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal(GetClaimStatusType.ReadyToClaim, result.Value.Status);
        }

        [Fact]
        public async Task Handle_FirstTimeClaim_ReturnsReadyToClaim()
        {
            // Arrange
            string email = "newuser@example.com";
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            _dateProvider.DateOnlyUtcNow.Returns(today);
            _temperatureService.GetHighestTemperature(today, Arg.Any<CancellationToken>())
                .Returns(Constants.RewardThreshold + 1);
            
            var user = new User { Id = 1, Email = email };
            _userRepo.GetByEmail(email, Arg.Any<CancellationToken>()).Returns(user);
            _userClaimRepo.GetClaimForToday(user.Id, today, Arg.Any<CancellationToken>()).Returns((UserClaim)null);

            // Act
            var result = await _handler.Handle(email, CancellationToken.None);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal(GetClaimStatusType.ReadyToClaim, result.Value.Status);
        }

        [Fact]
        public async Task Handle_AlreadyClaimedToday_ReturnsAlreadyClaimed()
        {
            // Arrange
            string email = "existing@example.com";
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            _dateProvider.DateOnlyUtcNow.Returns(today);
            _temperatureService.GetHighestTemperature(today, Arg.Any<CancellationToken>())
                .Returns(Constants.RewardThreshold + 1);

            var user = new User { Id = 5, Email = email };
            _userRepo.GetByEmail(email, Arg.Any<CancellationToken>()).Returns(user);

            var existingClaim = new UserClaim { UserId = 5, LastClaimDate = today };
            _userClaimRepo.GetClaimForToday(user.Id, today, Arg.Any<CancellationToken>()).Returns(existingClaim);

            // Act
            var result = await _handler.Handle(email, CancellationToken.None);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal(GetClaimStatusType.AlreadyClaimed, result.Value.Status);
        }

        [Fact]
        public async Task Handle_PreviouslyClaimedThresholdNotReached_ReturnsLocked()
        {
            // Arrange
            string email = "existing@example.com";
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var yesterday = today.AddDays(-1);
            _dateProvider.DateOnlyUtcNow.Returns(today);
            _temperatureService.GetHighestTemperature(today, Arg.Any<CancellationToken>())
                .Returns(Constants.RewardThreshold - 1); // below threshold

            var user = new User { Id = 5, Email = email };
            _userRepo.GetByEmail(email, Arg.Any<CancellationToken>()).Returns(user);

            var existingClaim = new UserClaim { UserId = 5, LastClaimDate = yesterday };
            _userClaimRepo.GetClaimForToday(user.Id, today, Arg.Any<CancellationToken>()).Returns(existingClaim);

            // Act
            var result = await _handler.Handle(email, CancellationToken.None);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal(GetClaimStatusType.Locked, result.Value.Status);
        }
    }