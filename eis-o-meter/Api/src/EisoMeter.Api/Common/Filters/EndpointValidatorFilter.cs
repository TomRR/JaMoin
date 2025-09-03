namespace EisoMeter.Api.Common.Filters;

public sealed class EndpointValidatorFilter<T>(IValidator<T> validator) : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var inputData = context.Arguments.FirstOrDefault(a => a is T) as T;   
        
        if (inputData is null)
        {
            return Results.BadRequest("Value(s) not provided");
        }
        
        var validationResult = await validator.ValidateAsync(inputData);
        
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(),
                statusCode: (int)HttpStatusCode.BadRequest);
        }

        return await next.Invoke(context);
    }
}
public static class EndpointFilterExtensions
{
    public static RouteHandlerBuilder Validator<T>(this RouteHandlerBuilder handlerBuilder)
        where T : class
    {
        handlerBuilder.AddEndpointFilter<EndpointValidatorFilter<T>>();
        return handlerBuilder;
    }
}