namespace EisoMeter.Api.Features.Claims.GetClaimStatus;

public interface IGetClaimStatusHandler
{
    Task<Result<GetClaimStatusResponse, Error>> Handle(string email, CancellationToken cancellationToken);
}

public sealed class GetClaimStatusHandler : IGetClaimStatusHandler
{
    private readonly IUserRepository _userRepo;
    private readonly IUserClaimRepository _userClaimRepo;
    private readonly ITemperatureService _temperatureService;
    private readonly IDateProvider _dateProvider;

    public GetClaimStatusHandler(
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

    public async Task<Result<GetClaimStatusResponse, Error>> Handle(string email, CancellationToken cancellationToken)
    {
        var today = _dateProvider.DateOnlyUtcNow;

        var highestTemperature = await _temperatureService.GetHighestTemperature(today, cancellationToken);
        
        var thresholdReached = highestTemperature >= Constants.RewardThreshold;
        

        var status = await GetCurrentUserClaimStatus(email, today, thresholdReached, cancellationToken);

        return new GetClaimStatusResponse(
            LastUpdate: today.ToString(Constants.DateFormat),
            HighestTemperature: highestTemperature,
            Status: status
        );
    }

    private async Task<GetClaimStatusType> GetCurrentUserClaimStatus(string email, DateOnly today, bool thresholdReached, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return GetClaimStatusType.Locked;
        }
        
        var user = await _userRepo.GetByEmail(email, cancellationToken);
        if (user is null)
        {
            return GetClaimStatusType.ReadyToClaim;
        }

        var claims = await _userClaimRepo.GetClaimForToday(user.Id, today, cancellationToken);
        if (claims is null)
        {
            return GetClaimStatusType.ReadyToClaim;
        }

        if (claims.LastClaimDate == today)
        {
            return GetClaimStatusType.AlreadyClaimed;
        }

        return thresholdReached ? GetClaimStatusType.ReadyToClaim : GetClaimStatusType.Locked;
    }
}