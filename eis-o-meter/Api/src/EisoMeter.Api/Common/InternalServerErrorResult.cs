namespace EisoMeter.Api.Common;

internal sealed class InternalServerErrorResult : StatusCodeResult
{
    internal InternalServerErrorResult() : base(StatusCodes.Status500InternalServerError)
    {
    }
}