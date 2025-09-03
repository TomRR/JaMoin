using System.Collections.Generic;
using System.Numerics;

namespace ja.training.csharp._03_RangeSubstraction
{
    // TODO so anpassen, dass large-numbers auch funktionieren
    public struct IntRange : IEqualityComparer<IntRange>
    {
        public IntRange(string start, string end)
        {
            Start = BigInteger.Parse( start);
            End = BigInteger.Parse(end);
        }

        public BigInteger Start { get; }
        public BigInteger End { get; }

        public bool Equals(IntRange a, IntRange b)
        {
            return a.Start == b.Start && a.End == b.End;
        }

        public int GetHashCode(IntRange a)
        {
            return $"{a.Start};{a.End}".GetHashCode();
        }
        
        public override string ToString()
        {
            return $"[{Start}-{End}]";
        }
    }
}
