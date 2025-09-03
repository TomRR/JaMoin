namespace EisoMeter.Api.Common;

public interface IDateProvider
{
    DateOnly DateOnlyUtcNow { get; }
    public DateTime DateTimeUtcNow { get; }
}

public class DateProvider : IDateProvider
{
    public DateOnly DateOnlyUtcNow { get; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateTime DateTimeUtcNow { get; } = DateTime.UtcNow;
}