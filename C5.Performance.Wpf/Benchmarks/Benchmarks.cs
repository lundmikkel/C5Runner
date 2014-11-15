using System;
using C5.Intervals;
using C5.Intervals.Performance;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

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

        private static readonly IntervalCollectionConstructor DIT = IntervalBenchmarkable.DIT;
        private static readonly IntervalCollectionConstructor IBS = IntervalBenchmarkable.IBS;
        private static readonly IntervalCollectionConstructor IBSOLD = IntervalBenchmarkable.IBSOLD;
        private static readonly IntervalCollectionConstructor LCList = IntervalBenchmarkable.LCList;
        private static readonly IntervalCollectionConstructor LCListNew = IntervalBenchmarkable.LCListNew;
        private static readonly IntervalCollectionConstructor LCListNewSplit = IntervalBenchmarkable.LCListNewSplit;
        private static readonly IntervalCollectionConstructor LCListNewSplitGallop = IntervalBenchmarkable.LCListNewSplitGallop;
        private static readonly IntervalCollectionConstructor NCList = IntervalBenchmarkable.NCList;
        private static readonly IntervalCollectionConstructor NCList2 = IntervalBenchmarkable.NCList2;
        private static readonly IntervalCollectionConstructor NCListArticle = IntervalBenchmarkable.NCListArticle;
        private static readonly IntervalCollectionConstructor SIT = IntervalBenchmarkable.SIT;
        private static readonly IntervalCollectionConstructor BITS = IntervalBenchmarkable.BITS;
        private static readonly IntervalCollectionConstructor DLFIT = IntervalBenchmarkable.DLFIT;
        private static readonly IntervalCollectionConstructor SFIL = IntervalBenchmarkable.SFIL;

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

        private static Benchmarkable[] containmentList4a()
        {
            return new Benchmarkable[]
            {
                new ContainmentList4a(NCList, "Nested Containment List"), 
                new ContainmentList4a(NCListArticle, "Nested Containment List Article"), 
                new ContainmentList4a(LCList, "Layered Containment List"), 
                new ContainmentList4a(LCListNew, "Layered Containment List New"), 
                new ContainmentList4a(LCListNewSplit, "Layered Containment List New Split"), 
                new ContainmentList4a(LCListNewSplitGallop, "Layered Containment List New Split Gallop"),
            };
        }

        private static Benchmarkable[] containmentList4b()
        {
            return new Benchmarkable[]
            {
                //new ContainmentList4b(NCList, "Nested Containment List"),
                //new ContainmentList4b(NCListArticle, "Nested Containment List Article"),
                new ContainmentList4b(LCList, "Layered Containment List"),
                new ContainmentList4b(LCListNew, "Layered Containment List New"),
                new ContainmentList4b(LCListNewSplit, "Layered Containment List New Split"),
                new ContainmentList4b(LCListNewSplitGallop, "Layered Containment List New Split Gallop"),
            };
        }

        public static Benchmarkable[] List
        {
            get
            {
                //return queryRange();
                return containmentList4a();
                return containmentList4b();
            }
        }
    }
}
