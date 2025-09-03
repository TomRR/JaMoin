namespace Benchmarks;

// [SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class TimeboxedCounterBenchmarksSpan
{
    private TimeboxedCounterSpan _sut = null!;

    [Params(10_000, 100_000, 1_000_000, 10_000_000)]
    public int RequestCount;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var dateTime = new DateTimeProxy();
        _sut = new TimeboxedCounterSpan(dateTime);
    }

    [IterationSetup(Target = nameof(SumOfLastMinute_Span))]
    public void SetupForSumOfLastMinute()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }

    [IterationSetup(Target = nameof(SumOfLastHour_Span))]
    public void SetupForSumOfLastHour()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }
    [IterationSetup(Target = nameof(SumOfLastMinute_Span_Faster))]
    public void SetupForSumOfLastMinute_Faster()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }
    [IterationSetup(Target = nameof(SumOfLastHour_Span_Faster))]
    public void SetupForSumOfLastHour_Faster()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }

    [Benchmark]
    public void AddRequests_Fastest()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }

    [Benchmark]
    public int SumOfLastMinute_Span() => _sut.SumOfLastMinute;

    [Benchmark]
    public int SumOfLastHour_Span() => _sut.SumOfLastHour;
    [Benchmark]
    public int SumOfLastMinute_Span_Faster() => _sut.SumOfLastMinute_Fastest;
    [Benchmark]
    public int SumOfLastHour_Span_Faster() => _sut.SumOfLastHour_Fastest;
}