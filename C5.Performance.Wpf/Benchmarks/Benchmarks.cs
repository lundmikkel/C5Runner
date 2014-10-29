using System;
using C5.Intervals;
using C5.Intervals.Performance;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.Benchmarks
{
    internal class Benchmarks
    {
        private static readonly Func<int, IInterval<int>[]> A = BenchmarkTestCases.DataSetA;
        private static readonly Func<int, IInterval<int>[]> B = BenchmarkTestCases.DataSetB;
        private static readonly Func<int, IInterval<int>[]> C = BenchmarkTestCases.DataSetC;
        private static readonly Func<int, IInterval<int>[]> D = BenchmarkTestCases.DataSetD;

        private static readonly Func<int, IInterval<int>[]> NoOverlaps = IntervalsFactory.NoOverlaps;
        private static readonly Func<int, IInterval<int>[]> Meets = IntervalsFactory.Meets;
        private static readonly Func<int, IInterval<int>[]> Overlaps = IntervalsFactory.Overlaps;
        private static readonly Func<int, IInterval<int>[]> Containments = IntervalsFactory.PineTreeForest;

        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> FIXED = IntervalBenchmarkable.IBS;

        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> DIT = IntervalBenchmarkable.DIT;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> IBS = IntervalBenchmarkable.IBS;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> IBSOLD = IntervalBenchmarkable.IBSOLD;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> LCList = IntervalBenchmarkable.LCList;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> LCListNew = IntervalBenchmarkable.LCListNew;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> NCList = IntervalBenchmarkable.NCList;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> NCList2 = IntervalBenchmarkable.NCList2;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> NCListArticle = IntervalBenchmarkable.NCListArticle;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> SIT = IntervalBenchmarkable.SIT;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> BITS = IntervalBenchmarkable.BITS;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> DLFIT = IntervalBenchmarkable.DLFIT;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> SFIL = IntervalBenchmarkable.SFIL;

        //private static readonly Action<DateTime[]> c5timsorter = C5.Sorting.Timsort;
        //private static readonly Action<DateTime[]> introsorter = C5.Sorting.IntroSort;
        //private static readonly Action<DateTime[]> arraysorter = Array.Sort;
        //private static readonly Action<DateTime[]> timsorter = TimSortExtender.TimSort;

        // These are the benchmarks that will be run by the benchmarker.

        private static Benchmarkable[] queryRange()
        {
            return new Benchmarkable[]
            {
                new QueryRange(A, LCList),
                new QueryRange(A, LCListNew),
                new QueryRange(A, NCList),
                new QueryRange(A, NCListArticle),
            };
        } 

        public static Benchmarkable[] List
        {
            get
            {
                return queryRange();
            }
        }
    }
}
