namespace C5.Intervals.Performance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IntervalsFactory
    {
        public static IEnumerable<IInterval<int>> CreateIntervalStream(Func<int, int> intervalLength, Func<int, int> gap, int offset = 0)
        {
            int count = 0;

            int low = offset;
            int high = low + intervalLength(count);

            for (; ; )
            {
                yield return new IntervalBase<int>(low, high);
                count++;

                low = high + gap(count);
                high = low + intervalLength(count);
            }
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-| |-| |-| |-| 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IEnumerable<IInterval<int>> NoOverlapsConstantLenghtConstantGap(int count, int offset = 0)
        {
            return CreateIntervalStream(x => 1, x => 1, offset).Take(count);
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-| |-|  |-|   |-|    |-|     |-|
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IEnumerable<IInterval<int>> NoOverlapsConstantLenghtGrowingGap(int count, int offset = 0)
        {
            return CreateIntervalStream(x => 1, x => x, offset).Take(count);
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-|-|-|-|-|-|
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IEnumerable<IInterval<int>> ConstantLengthMeets(int count, int offset = 0)
        {
            return CreateIntervalStream(x => 1, x => 0, offset).Take(count);
        }


        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |----------|
        ///              |----------|
        ///                |----------|
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IEnumerable<IInterval<int>> OverlapsConstantLength(int count, int offset = 0)
        {
            return CreateIntervalStream(x => 10, x => -9, offset).Take(count);
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-|
        ///           |---|
        ///          |-----|
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IEnumerable<IInterval<int>> ContainsAscending(int count, int offset = 0)
        {
            Func<int, int> length = x => 2 * (x + 1) - 1;
            Func<int, int> gap = x => -1 * length(x) + 1;

            return CreateIntervalStream(length, gap, offset).Take(count);
        }

        /// <summary>
        ///   Offset
        /// |-------> 
        ///          |-----|
        ///           |---|
        ///            |-|
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IEnumerable<IInterval<int>> ContainsDescending(int count, int offset = 0)
        {
            Func<int, int> length = x => 2 * count - 2 * x - 1;
            Func<int, int> gap = x => -1 * length(x) - 1;

            return CreateIntervalStream(length, gap, offset).Take(count);
        }
    }
}