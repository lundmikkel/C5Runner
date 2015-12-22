using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

    abstract class ContainmentListSpeedRatio : Benchmarkable
    {
        private const int HighestHigh =  (int)  100.10 * 1000000;
        private readonly IntervalCollectionConstructor _constructor;
        protected IIntervalCollection<IInterval<int>, int> Collection;
        protected const int QueryCount = 1000000;
        protected IInterval<int>[] Queries;
        private readonly Random _random = new Random();
        protected readonly string CollectionName;
        private readonly int _testNumber;

        protected ContainmentListSpeedRatio(IntervalCollectionConstructor constructor, string collectionName, int testNumber)
        {
            _constructor = constructor;
            CollectionName = collectionName;
            _testNumber = testNumber;
        }

        public override void CollectionSetup(int collectionSize)
        {
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);

            var intervals = IntervalsFactory.ContainmentListIntervals(collectionSize, HighestHigh).ToArray();
            Collection = _constructor(intervals);

            var length = QueryWidth();
            Queries = new IInterval<int>[QueryCount];

            for (var i = 0; i < QueryCount; ++i)
            {
                var low = _random.Next(HighestHigh - length);
                Queries[i] = new IntervalBase<int>(low, low + length, IntervalType.Closed);
            }
        }

        public int QueryWidth()
        {
            var queryWidths = new[]
            {
                   100,
                   500,
                  1000,
                  5000,
                 10000,
                 50000,
                100000,
            };

            return queryWidths[_testNumber];
        }

        public override IEnumerable<int> CollectionSizes()
        {
            return new[]
            {
                   50000,
                  100000,
                  200000,
                  500000,
                 1000000,
                 2000000,
                 5000000,
            };
        }
    }

    class ContainmentListSpeedRatioFindOverlaps : ContainmentListSpeedRatio
    {
        public ContainmentListSpeedRatioFindOverlaps(IntervalCollectionConstructor constructor, string collectionName, int testNumber)
            : base(constructor, collectionName, testNumber)
        {
        }
        
        public override string BenchMarkName()
        {
            return String.Format("Speed ratio - Find ({0}) - {1}", QueryWidth(), CollectionName);
        }

        public override double Call(int i, int collectionSize)
        {
            return Collection.FindOverlaps(Queries[i % QueryCount]).Count();
        }
    }

    class ContainmentListSpeedRatioCountOverlaps : ContainmentListSpeedRatio
    {
        public ContainmentListSpeedRatioCountOverlaps(IntervalCollectionConstructor constructor, string collectionName, int testNumber)
            : base(constructor, collectionName, testNumber)
        {
        }


        public override string BenchMarkName()
        {
            return String.Format("Speed ratio - Count ({0}) - {1}", QueryWidth(), CollectionName);
        }

        public override double Call(int i, int collectionSize)
        {
            return Collection.CountOverlaps(Queries[i % QueryCount]);
        }
    }
}
