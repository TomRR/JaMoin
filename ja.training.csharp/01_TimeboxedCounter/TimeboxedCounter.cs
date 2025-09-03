using System;
using ja.training.csharp._01_TimeboxedCounter;
using ja.training.csharp.Services;

public class TimeboxedCounter : ITimeboxedCounter
{
    private readonly IDateTimeProxy _dateTime;
    private readonly (DateTime Timestamp, int Count)[] _seconds;

    public TimeboxedCounter(IDateTimeProxy dateTime)
    {
        _dateTime = dateTime;
        _seconds = new (DateTime, int)[3600];
    }

    public void Add(int count)
    {
        var now = _dateTime.UtcNow;
        var index = (int)(now.Ticks / TimeSpan.TicksPerSecond % 3600);

        if (_seconds[index].Timestamp != now)
        {
            // alter Wert überschreiben
            _seconds[index] = (now, count);
        }
        else
        {
            // gleiche Sekunde => addieren
            _seconds[index] = (now, _seconds[index].Count + count);
        }
    }

    public int SumOfLastMinute
    {
        get
        {
            var now = _dateTime.UtcNow;
            var cutoff = now - TimeSpan.FromMinutes(1);

            var sum = 0;
            foreach (var entry in _seconds)
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

            var sum = 0;
            foreach (var entry in _seconds)
            {
                if (entry.Timestamp >= cutoff)
                    sum += entry.Count;
            }
            return sum;
        }
    }
}