namespace EisoMeter.Api.Common;

public static class ErrorExtensions
{
    public static IResult ToProblemDetail(this Error error)
    {
        var problem = new ProblemDetails
        {
            Title = GetTitle(error.ErrorType),
            Detail = error.Description,
            Status = GetStatusCode(error.ErrorType),
            Type = $"https://httpstatuses.com/{GetStatusCode(error.ErrorType)}"
        };

        return Results.Problem(
            detail: problem.Detail,
            statusCode: problem.Status,
            title: problem.Title,
            type: problem.Type
        );
    }

    private static int GetStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status422UnprocessableEntity,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
        ErrorType.Failure => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    private static string GetTitle(ErrorType type) => type switch
    {
        ErrorType.Validation => "Validation Error",
        ErrorType.Conflict => "Conflict Error",
        ErrorType.NotFound => "Not Found",
        ErrorType.Unexpected => "Unexpected Error",
        ErrorType.Failure => "Failure",
        _ => "Error"
    };
}