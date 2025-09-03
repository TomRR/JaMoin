namespace EisoMeter.Api.Features.Claims.GetClaimStatus;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder MapGetClaimStatusEndPoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/status", GetClaimStatus);
        return endpoint;
    }

    private static async Task<IResult> GetClaimStatus([AsParameters]GetClaimStatusParameter parameter, [FromServices]IGetClaimStatusHandler handler, CancellationToken cancellationToken)
    {

        var email = parameter.Email;
        
        var response = await handler.Handle(email, cancellationToken);
        return response.Match<IResult>( 
            onSuccess: value => Results.Ok(value),
            onFailure: error => error.ToProblemDetail());
    }
}