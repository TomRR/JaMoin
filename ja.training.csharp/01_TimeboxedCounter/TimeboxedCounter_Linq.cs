using System;
using System.Linq;
using ja.training.csharp._01_TimeboxedCounter;
using ja.training.csharp.Services;

public class TimeboxedCounterLinq : ITimeboxedCounter
{
    private readonly IDateTimeProxy _dateTime;
    private readonly (DateTime Timestamp, int Count)[] _seconds;

    public TimeboxedCounterLinq(IDateTimeProxy dateTime)
    {
        _dateTime = dateTime;
        _seconds = new (DateTime, int)[3600];
    }


    private readonly long _ticksPerSecond = TimeSpan.TicksPerSecond;

    public void Add(int count)
    {
        long ticks = _dateTime.UtcNow.Ticks;
        int index = (int)((ticks / _ticksPerSecond) % 3600);

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

            return _seconds
                .Where(entry => entry.Timestamp >= cutoff)
                .Sum(entry => entry.Count);
        }
    }

    public int SumOfLastHour
    {
        get
        {
            var now = _dateTime.UtcNow;
            var cutoff = now - TimeSpan.FromHours(1);

            return _seconds
                .Where(entry => entry.Timestamp >= cutoff)
                .Sum(entry => entry.Count);
        }
    }
}