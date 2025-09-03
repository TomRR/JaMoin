namespace EisoMeter.Api.Features.Claims.ClaimReward;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder MapClaimRewardHandlerEndPoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/", ClaimReward).Validator<ClaimRewardRequest>();
        return endpoint;
    }

    private static async Task<IResult> ClaimReward([FromBody]ClaimRewardRequest request, [FromServices]IClaimRewardHandler handler, CancellationToken cancellationToken)
    {

        var email = request.Email;
        
        var response = await handler.Handle(email, cancellationToken);
        return response.Match<IResult>( 
            onSuccess: value => Results.Ok(value),
            onFailure: error => error.ToProblemDetail());
    }
}