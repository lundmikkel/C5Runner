using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Report_Benchmarks;
using C5.Ucsc;

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
        private static readonly IntervalCollectionConstructor LCList = IntervalBenchmarkable.LCList;
        private static readonly IntervalCollectionConstructor SLCList = IntervalBenchmarkable.SLCList;
        private static readonly IntervalCollectionConstructor NCList = IntervalBenchmarkable.NCList;
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
                new QueryRange(A, NCList),
            };
        }

        private static Benchmarkable[] containmentList4a()
        {
            return new Benchmarkable[]
            {
                new ContainmentList4a(NCList, "Nested Containment List"), 
                new ContainmentList4a(LCList, "Layered Containment List"),
                //new ContainmentList4a(BITS, "Binary Interval Search"), 
            };
        }

        private static Benchmarkable[] containmentList4b()
        {
            return new Benchmarkable[]
            {
                new ContainmentList4b(NCList, "Nested Containment List"),
                new ContainmentList4b(LCList, "Layered Containment List"),
                //new ContainmentList4b(BITS, "Binary Interval Search"), 
            };
        }

        private static Benchmarkable[] containmentListUcsc()
        {
            Console.WriteLine("Parsing compressed maf file");

            var intervals = UcscHumanGenomeParser.ParseCompressedMaf("chr1.compressed.txt").ToArray();

            var lclist = LCList(intervals);
            var slclist = SLCList(intervals);
            var nclist = NCList(intervals);
            var bits = BITS(intervals);

            return new Benchmarkable[] {
                //new ContainmentListUcscFindOverlaps(nclist,   "Nested Containment List"                   ),
                //new ContainmentListUcscFindOverlaps(lclist,   "Layered Containment List"                  ),
                //new ContainmentListUcscFindOverlaps(slclist,  "Layered Containment List Without Pointers" ),
                //new ContainmentListUcscFindOverlaps(bits,     "Binary Interval Search"                    ),
                                                              
                new ContainmentListUcscCountOverlaps(lclist,  "Layered Containment List"                  ),
                //new ContainmentListUcscCountOverlaps(slclist, "Layered Containment List Without Pointers" ),
                new ContainmentListUcscCountOverlaps(bits,    "Binary Interval Search"                    ),
            };
        }

        private static Benchmarkable[] containmentListConstruction()
        {
            return new Benchmarkable[] {
                new ContainmentListConstruction(NCList,   "Nested Containment List"                   ),
                new ContainmentListConstruction(LCList,   "Layered Containment List"                  ),
                new ContainmentListConstruction(SLCList,  "Layered Containment List Without Pointers" ),
                new ContainmentListConstruction(BITS,     "Binary Interval Search"                    ),
            };
        }

        private static Benchmarkable[] containmentListSpeedRatio()
        {
            //*
            var collections = new[]
            {
                // new {Type = NCList,   Name = "Nested Containment List",                     },
                new {Type = LCList,   Name = "Layered Containment List",                    },
                // new {Type = SLCList,  Name = "Layered Containment List Without Pointers",   },
                new {Type = BITS,     Name = "Binary Interval Search",                      },
            };

            var testNumbers = new[]
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
            };

            var list = new List<Benchmarkable>();

            // foreach (var testNumber in testNumbers)
            // {
            //     foreach (var collection in collections)
            //     {
            //         list.Add(new ContainmentListSpeedRatioFindOverlaps(collection.Type, collection.Name, testNumber));
            //     }
            // }

            foreach (var testNumber in testNumbers)
            {
                foreach (var collection in collections)
                {
                    if (collection.Type != NCList)
                        list.Add(new ContainmentListSpeedRatioCountOverlaps(collection.Type, collection.Name, testNumber));
                }
            }

            return list.ToArray();

            /*/

            var testNumber = 0;
            return new Benchmarkable[]
            {
                new ContainmentList5(NCList, "Nested Containment List", testNumber),
                new ContainmentList5(LCList, "Layered Containment List", testNumber),
                
                new ContainmentList5(BITS, "Binary Interval Search", testNumber), 
            };
            //*/
        }

        public static Benchmarkable[] List
        {
            get
            {
                //return containmentList4a();
                //return containmentList4b();
                //return containmentListSpeedRatio();
                //return containmentListUcsc();
                return containmentListConstruction();
            }
        }
    }
}
