namespace EisoMeter.Api.Features.Claims.GetClaimStatus;


public sealed record GetClaimStatusParameter(
    [property: JsonPropertyName("email")] string Email = ""
);

public sealed record GetClaimStatusResponse(
    [property: JsonPropertyName("lastUpdate")] string LastUpdate,
    [property: JsonPropertyName("highestTemperature")] double HighestTemperature,
    [property: JsonPropertyName("status")] GetClaimStatusType Status
);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GetClaimStatusType
{
    Locked,
    AlreadyClaimed,
    ReadyToClaim
}