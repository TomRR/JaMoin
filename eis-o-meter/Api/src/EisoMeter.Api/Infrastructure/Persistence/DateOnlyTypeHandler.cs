namespace EisoMeter.Api.Infrastructure.Persistence;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        if (value is string s)
            return DateOnly.ParseExact(s, Constants.DateFormat);
        throw new DataException($"Cannot convert {value.GetType()} to DateOnly");
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToString(Constants.DateFormat);
    }
}