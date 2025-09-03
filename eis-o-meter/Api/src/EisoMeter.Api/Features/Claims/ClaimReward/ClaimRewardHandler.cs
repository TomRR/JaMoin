namespace EisoMeter.Api.Features.Claims.ClaimReward;
public interface IClaimRewardHandler
{
    Task<Result<ClaimRewardResponse, Error>> Handle(string email, CancellationToken cancellationToken);
}

public sealed class ClaimRewardHandler : IClaimRewardHandler
{
    private readonly IUserRepository _userRepo;
    private readonly IUserClaimRepository _userClaimRepo;
    private readonly ITemperatureService _temperatureService;
    private readonly IDateProvider _dateProvider;
    public ClaimRewardHandler(
        IUserRepository userRepo,
        IUserClaimRepository userClaimRepo,
        ITemperatureService temperatureService,
        IDateProvider dateProvider)
    {
        _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        _userClaimRepo = userClaimRepo ?? throw new ArgumentNullException(nameof(userClaimRepo));
        _temperatureService = temperatureService ?? throw new ArgumentNullException(nameof(temperatureService));
        _dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
    }

    public async Task<Result<ClaimRewardResponse, Error>> Handle(string email, CancellationToken cancellationToken)
    {
        var today = _dateProvider.DateOnlyUtcNow;

        var temp = await _temperatureService.GetHighestTemperature(today, cancellationToken);

        if (temp < Constants.RewardThreshold)
        {
            return Error.Create("Temperature threshold not reached today.", ErrorType.Failure);
        }
        
        var user = await CreateUserIfNotExists(email, cancellationToken);
        
        var existing = await _userClaimRepo.GetClaimForToday(user.Id, today, cancellationToken);
        if (existing is null)
        {
            return await UpdateUserClaimForToday(user, cancellationToken);
        }

        if (existing.LastClaimDate == _dateProvider.DateOnlyUtcNow)
        {
            return new ClaimRewardResponse(ClaimRewardType.Expired);
        }

        return await UpdateUserClaimForToday(user, cancellationToken);
    }

    private async Task<Result<ClaimRewardResponse, Error>> UpdateUserClaimForToday(User user, CancellationToken cancellationToken)
    {
        var newUserClaim = new UserClaim
        {
            UserId = user.Id,
            LastClaimDate = _dateProvider.DateOnlyUtcNow,
        };
        await _userClaimRepo.Insert(newUserClaim, cancellationToken);
            
        return new ClaimRewardResponse(ClaimRewardType.SuccessfulClaimed);
    }

    private async Task<User> CreateUserIfNotExists(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByEmail(email, cancellationToken);
        if (user == null)
        {
            var newUserId = await _userRepo.Insert(new User { Email = email }, cancellationToken);
            user = new User { Id = newUserId, Email = email };
        }

        return user;
    }
}
