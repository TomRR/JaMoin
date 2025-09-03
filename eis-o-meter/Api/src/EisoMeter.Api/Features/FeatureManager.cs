namespace EisoMeter.Api.Features;

internal static class FeatureManager
{
    private const string ClaimsEndpointTagName = "claims";
    private const string ClaimsEndpointPrefix = $"/api/v1/{ClaimsEndpointTagName}";
    
    private const string TemperatureEndpointTagName = "temperatures";
    private const string TemperatureEndpointPrefix = $"/api/v1/{TemperatureEndpointTagName}";
    

    
    internal static IEndpointRouteBuilder MapFeaturesEndpoints(this IEndpointRouteBuilder endpoint)
    {
        MapClaimsEndpoints(endpoint);
        // MapStatusEndpoints(endpoint);

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
    // private static void MapStatusEndpoints(IEndpointRouteBuilder endpoint)
    // {
    //     var statusGroupEndpoint = endpoint.MapGroup(StatusEndpointPrefix)
    //         .WithTags(StatusEndpointTagName)
    //         .WithDescription("Provides endpoints related to status management.");
    //
    //     statusGroupEndpoint.MapGetStatusEndPoint();
    // }
}