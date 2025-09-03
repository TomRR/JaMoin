namespace EisoMeter.Api.Infrastructure.Persistence.Claims;

public sealed class UserClaim
{   
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateOnly LastClaimDate { get; set; }
}