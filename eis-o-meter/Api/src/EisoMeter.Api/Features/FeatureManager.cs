namespace EisoMeter.Api.Features;

internal static class FeatureManager
{
    private const string ClaimsEndpointTagName = "claims";
    private const string ClaimsEndpointPrefix = $"/api/v1/{ClaimsEndpointTagName}";
    
    internal static IEndpointRouteBuilder MapFeaturesEndpoints(this IEndpointRouteBuilder endpoint)
    {
        MapClaimsEndpoints(endpoint);

        return endpoint;
    }
    private static void MapClaimsEndpoints(IEndpointRouteBuilder endpoint)
    {
        var claimsGroupEndpoint = endpoint.MapGroup(ClaimsEndpointPrefix)
            .WithTags(ClaimsEndpointTagName)
            .WithDescription("Provides endpoints related to claims management.");

        claimsGroupEndpoint.MapClaimRewardHandlerEndPoint();
        claimsGroupEndpoint.MapGetClaimStatusEndPoint();
    }
}