namespace Benchmarks;

// [SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class TimeboxedCounterBenchmarksLinq
{
    private ITimeboxedCounter _sut = null!;

    [Params(10_000, 100_000, 1_000_000, 10_000_000)]
    public int RequestCount;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var dateTime = new DateTimeProxy();
        _sut = new TimeboxedCounterLinq(dateTime);
    }

    [IterationSetup(Target = nameof(SumOfLastMinute_Linq))]
    public void SetupForSumOfLastMinute()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }

    [IterationSetup(Target = nameof(SumOfLastHour_Linq))]
    public void SetupForSumOfLastHour()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }

    [Benchmark]
    public void AddRequests_Faster()
    {
        for (var i = 0; i < RequestCount; i++)
        {
            _sut.Add(1);
        }
    }

    [Benchmark]
    public int SumOfLastMinute_Linq() => _sut.SumOfLastMinute;

    [Benchmark]
    public int SumOfLastHour_Linq() => _sut.SumOfLastHour;
}