using System;
using System.Threading;
using ja.training.csharp._01_TimeboxedCounter;
using ja.training.csharp.Services;

public class TimeboxedCounterSpan : ITimeboxedCounter
{
    private readonly IDateTimeProxy _dateTime;
    private readonly (DateTime Timestamp, int Count)[] _seconds;
    private const long TicksPerSecond = TimeSpan.TicksPerSecond;

    public TimeboxedCounterSpan(IDateTimeProxy dateTime)
    {
        _dateTime = dateTime;
        _seconds = new (DateTime, int)[3600];
    }
    

    // No Concurrent
    public void Add(int count)
    {
        long ticks = _dateTime.UtcNow.Ticks;
        int index = (int)((ticks / TicksPerSecond) % _seconds.Length);

        ref var slot = ref _seconds[index];
        if (slot.Timestamp.Ticks != ticks)
        {
            slot = (new DateTime(ticks), count);
        }
        else
        {
            slot = (slot.Timestamp, slot.Count + count);
        }
    }

    public int SumOfLastMinute
    {
        get
        {
            var now = _dateTime.UtcNow;
            var cutoff = now - TimeSpan.FromMinutes(1);
            int sum = 0;

            Span<(DateTime Timestamp, int Count)> span = _seconds;
            foreach (ref readonly var entry in span)
            {
                if (entry.Timestamp >= cutoff)
                    sum += entry.Count;
            }

            return sum;
        }
    }

    public int SumOfLastHour
    {
        get
        {
            var now = _dateTime.UtcNow;
            var cutoff = now - TimeSpan.FromHours(1);
            int sum = 0;

            Span<(DateTime Timestamp, int Count)> span = _seconds;
            foreach (ref readonly var entry in span)
            {
                if (entry.Timestamp >= cutoff)
                    sum += entry.Count;
            }

            return sum;
        }
    }

    public int SumOfLastHour_Fastest 
    { get 
        { 
            var now = _dateTime.UtcNow; 
            var cutoff = now - TimeSpan.FromHours(1); 
            
            int sum = 0; 
            var span = _seconds.AsSpan();
            
            foreach (ref readonly var entry in span)
            {
                if (entry.Timestamp >= cutoff)
                {
                    sum += entry.Count;
                }
            } 
            return sum; 
        } 
    }
    
    public int SumOfLastMinute_Fastest
    {
        get
        {
            var now = _dateTime.UtcNow;
            var cutoff = now - TimeSpan.FromMinutes(1);

            int sum = 0;
            var span = _seconds.AsSpan();

            foreach (ref readonly var entry in span)
            {
                if (entry.Timestamp >= cutoff)
                    sum += entry.Count;
            }

            return sum;
        }
    }
}