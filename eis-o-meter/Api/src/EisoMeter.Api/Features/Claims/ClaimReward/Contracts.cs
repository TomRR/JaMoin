namespace EisoMeter.Api.Features.Claims.ClaimReward;

public sealed record ClaimRewardRequest(
    [property: JsonPropertyName("email")]string Email
);

public sealed record ClaimRewardResponse(
    [property: JsonPropertyName("status")]ClaimRewardType Status
);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClaimRewardType
{
    SuccessfulClaimed,
    Expired,
}