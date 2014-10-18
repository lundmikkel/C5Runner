using System.Diagnostics.Contracts;

namespace C5.Intervals.Performance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IntervalsFactory
    {
        public static IEnumerable<IInterval<int>> CreateIntervalStream(Func<int, int> intervalLength, Func<int, int> gap, int offset = 0)
        {
            var count = 0;

            var low = offset;
            var high = low + intervalLength(count);

            while (true)
            {
                yield return new IntervalBase<int>(low, high);
                ++count;

                low = high + gap(count);
                high = low + intervalLength(count);

                Contract.Assert(0 < intervalLength(count));
            }
        }

        public static IInterval<int>[] NoOverlaps(int count)
        {
            return NoOverlapsConstantLenghtConstantGap(count).ToArray();
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-| |-| |-| |-| 
        /// </summary>
        public static IEnumerable<IInterval<int>> NoOverlapsConstantLenghtConstantGap(int count, int length = 1, int gap = 1, int offset = 0)
        {
            Contract.Requires(0 <= count);
            Contract.Requires(0 < length);
            Contract.Requires(0 < gap);

            return CreateIntervalStream(x => length, x => gap, offset).Take(count);
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-| |-|  |-|   |-|    |-|     |-|
        /// </summary>
        public static IEnumerable<IInterval<int>> NoOverlapsConstantLenghtGrowingGap(int count, int length = 1, int offset = 0)
        {
            Contract.Requires(0 <= count);
            Contract.Requires(0 < length);

            return CreateIntervalStream(x => length, x => x, offset).Take(count);
        }

        public static IInterval<int>[] Meets(int count)
        {
            return ConstantLengthMeets(count).ToArray();
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-|-|-|-|-|-|
        /// </summary>
        public static IEnumerable<IInterval<int>> ConstantLengthMeets(int count, int length = 1, int offset = 0)
        {
            Contract.Requires(0 <= count);
            Contract.Requires(0 < length);

            return CreateIntervalStream(x => length, x => 0, offset).Take(count);
        }

        public static IInterval<int>[] Overlaps(int count)
        {
            return OverlapsConstantLength(count).ToArray();
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |----------|
        ///              |----------|
        ///                |----------|
        /// </summary>
        public static IEnumerable<IInterval<int>> OverlapsConstantLength(int count, int length = 5, int gap = -4, int offset = 0)
        {
            Contract.Requires(0 <= count);
            Contract.Requires(0 < length);

            return CreateIntervalStream(x => length, x => gap, offset).Take(count);
        }

        /// <summary>
        ///    Offset
        /// |---------> 
        ///            |-|
        ///           |---|
        ///          |-----|
        /// </summary>
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
        public static IEnumerable<IInterval<int>> ContainsDescending(int count, int offset = 0)
        {
            Func<int, int> length = x => 2 * count - 2 * x - 1;
            Func<int, int> gap = x => -1 * length(x) - 1;

            return CreateIntervalStream(length, gap, offset).Take(count);
        }

        public static IInterval<int>[] PineTreeForest(int count)
        {
            return PineTreeForest(count, 5).ToArray();
        }

        public static IEnumerable<IInterval<int>> PineTreeForest(int count, int depth, int offset = 0)
        {
            return CreateIntervalStream(
                i => (depth - i % depth) * 2 - 1,
                i => i % depth == 0 ? depth : (i % depth - depth) * 2,
                offset).Take(count);
        }

        public static IEnumerable<IInterval<int>> ContainmentListIntervals(int count, int highestLow)
        {
            Contract.Requires(0 <= count);
            Contract.Requires(count % 5 == 0);
            Contract.Requires(highestLow > 0);
            Contract.Ensures((count == 0) != Contract.Result<IEnumerable<IInterval<int>>>().Any());
            Contract.Ensures(
                Contract.ForAll(
                    new[] { 1, 10, 100, 1000, 10000 },
                    length => Contract.Result<IEnumerable<IInterval<int>>>().Count(x => x.High - x.Low == length) == count / 5
                )
            );

            count /= 5;
            var random = new Random();
            int low;

            for (var length = 1; length <= 10000; length *= 10)
                for (var i = 0; i < count; ++i)
                    yield return new IntervalBase<int>(low = random.Next(0, highestLow), low + length);
        }
    }
}