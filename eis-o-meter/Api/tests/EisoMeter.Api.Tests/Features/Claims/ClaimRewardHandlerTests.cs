// using EisoMeter.Api.Features.Claims.ClaimReward;
//
// namespace EisoMeter.Api.Tests;
//
//
// public class ClaimRewardHandlerTests
// {
//     private readonly IUserRepository _users = Substitute.For<IUserRepository>();
//     private readonly IUserClaimRepository _claims = Substitute.For<IUserClaimRepository>();
//     private readonly ITemperatureStatusRepository _temps = Substitute.For<ITemperatureStatusRepository>();
//     private readonly IDateProvider _dateProvider = Substitute.For<IDateProvider>();
//
//     [Fact]
//     public async Task Fails_WhenThresholdNotReached()
//     {
//         var today = new DateOnly(2025, 8, 31); // fixed date for test
//         _dateProvider.DateOnlyUtcNow.Returns(today);
//
//         _temps.GetTemperatureStatus(today, Arg.Any<CancellationToken>())
//               .Returns(new TemperatureStatus { Date = today, HighestTemperature = 27.0, LastUpdatedUtc = DateTime.UtcNow });
//
//         var handler = new ClaimRewardHandler(_users, _claims, _temps, _dateProvider);
//
//         var result = await handler.Handle("x@y.com", CancellationToken.None);
//
//         Assert.True(result.HasError);
//     }
//
//     [Fact]
//     public async Task Returns_AlreadyClaimed_WhenClaimExists()
//     {
//         var today = new DateOnly(2025, 8, 31);
//         _dateProvider.DateOnlyUtcNow.Returns(today);
//
//         _temps.GetTemperatureStatus(today, Arg.Any<CancellationToken>())
//               .Returns(new TemperatureStatus { Date = today, HighestTemperature = 33.0, LastUpdatedUtc = DateTime.UtcNow });
//
//         _users.GetByEmail("x@y.com", Arg.Any<CancellationToken>())
//               .Returns(new User { Id = 5, Email = "x@y.com" });
//
//         _claims.GetClaimForToday(5, today, Arg.Any<CancellationToken>())
//                .Returns(new UserClaim { UserId = 5, ClaimDate = today, ClaimCode = "EXISTING" });
//
//         var handler = new ClaimRewardHandler(_users, _claims, _temps, _dateProvider);
//
//         var result = await handler.Handle("x@y.com", CancellationToken.None);
//
//         Assert.True(result.IsSuccessful);
//         Assert.Equal(ClaimStatus.Claimed, result.Value!.Status);
//         Assert.Equal("EXISTING", result.Value!.Code);
//         await _claims.DidNotReceive().Insert(Arg.Any<UserClaim>(), Arg.Any<CancellationToken>());
//     }
//
//     [Fact]
//     public async Task CreatesUserAndClaim_WhenEligibleAndNoClaim()
//     {
//         var today = new DateOnly(2025, 8, 31);
//         _dateProvider.DateOnlyUtcNow.Returns(today);
//
//         _temps.GetTemperatureStatus(today, Arg.Any<CancellationToken>())
//               .Returns(new TemperatureStatus { Date = today, HighestTemperature = 31.0, LastUpdatedUtc = DateTime.UtcNow });
//
//         _users.GetByEmail("z@z.com", Arg.Any<CancellationToken>())
//               .Returns((User?)null);
//
//         _users.Insert(Arg.Any<User>(), Arg.Any<CancellationToken>())
//               .Returns(99);
//
//         _claims.GetClaimForToday(99, today, Arg.Any<CancellationToken>())
//                .Returns((UserClaim?)null);
//
//         var handler = new ClaimRewardHandler(_users, _claims, _temps, _dateProvider);
//
//         var result = await handler.Handle("z@z.com", CancellationToken.None);
//
//         Assert.True(result.IsSuccessful);
//         Assert.Equal(ClaimStatus.Claimable, result.Value!.Status);
//         Assert.NotNull(result.Value!.Code);
//         Assert.NotEmpty(result.Value!.Code);
//
//         await _claims.Received(1).Insert(
//             Arg.Is<UserClaim>(c => c.UserId == 99 && c.ClaimDate == today && !string.IsNullOrWhiteSpace(c.ClaimCode)),
//             Arg.Any<CancellationToken>());
//     }
// }