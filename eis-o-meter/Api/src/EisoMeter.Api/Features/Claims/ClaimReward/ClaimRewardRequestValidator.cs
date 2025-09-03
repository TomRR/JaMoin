namespace EisoMeter.Api.Features.Claims.ClaimReward;

public class ClaimRewardRequestValidator : AbstractValidator<ClaimRewardRequest>
{
    public ClaimRewardRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()         
            .NotEmpty()         
            .EmailAddress();
    }
}