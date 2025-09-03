using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ja.training.csharp._03_RangeSubstraction
{
    public static class IntRangeExtensions
    {

        public static List<IntRange> Substract(this List<IntRange> me, IntRange toSubstract)
        {
            var result = new List<IntRange>();
            foreach (var r in me)
            {
                result.AddRange(r.Substract(toSubstract));
            }
            return result;
        }

        public static List<IntRange> Substract(this List<IntRange> me, List<IntRange> toSubstract)
        {
            var result = new List<IntRange>(me);
            foreach (var sub in toSubstract)
            {
                result = result.Substract(sub);
            }
            return result;
        }

        /// <summary>
        /// gibt 0-2 Teil-Bundles zurück, die übrigbleiben, wenn "other" abgezogen wird
        ///
        /// ich:    |----------------| gibt nichts zurück
        /// other:  |----------------|
        /// 
        /// ich:    |----------------| gibt rechten Teil zurück
        /// other:  |-----|
        ///
        /// ich:    |----------------| gibt rechten und linken Teil zurück
        /// other:     |-----|
        ///
        /// ich:    |----------------| gibt linken Teil zurück
        /// other:             |-----|
        /// </summary>
        public static List<IntRange> Substract(this IntRange me, IntRange other)
        {
            var result = new List<IntRange>();

            // kein Überlapp => Original behalten
            if (other.End < me.Start || other.Start > me.End)
            {
                result.Add(me);
                return result;
            }

            // linker Teil
            if (other.Start > me.Start)
            {
                result.Add(new IntRange(me.Start.ToString(), (other.Start - 1).ToString()));
            }

            // rechter Teil
            if (other.End < me.End)
            {
                result.Add(new IntRange((other.End + 1).ToString(), me.End.ToString()));
            }

            return result;
        }
    }
}
