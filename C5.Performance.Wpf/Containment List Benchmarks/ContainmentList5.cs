using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

    class ContainmentList5 : Benchmarkable
    {
        private const int HighestHigh = 100100000;
        private readonly IntervalCollectionConstructor _constructor;
        private IIntervalCollection<IInterval<int>, int> _collection;
        private const int QueryCount = 100;
        private IInterval<int>[] queries;
        private readonly Random _random = new Random();
        private readonly string _collectionName;
        private readonly int _testNumber;

        public ContainmentList5(IntervalCollectionConstructor constructor, string collectionName, int testNumber)
        {
            _constructor = constructor;
            _collectionName = collectionName;
            _testNumber = testNumber;
        }


        public override string BenchMarkName()
        {
            return String.Format("5 ({0}) - {1}", QueryWidth(), _collectionName);
        }

        public override void CollectionSetup(int collectionSize)
        {
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);

            var intervals = IntervalsFactory.ContainmentListIntervals(collectionSize, HighestHigh).ToArray();
            _collection = _constructor(intervals);

            var length = QueryWidth();
            queries = new IInterval<int>[QueryCount];

            for (var i = 0; i < QueryCount; ++i)
            {
                var low = _random.Next(HighestHigh - length);
                queries[i] = new IntervalBase<int>(low, low + length, IntervalType.Closed);
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

        public override double Call(int i, int collectionSize)
        {
            return _collection.FindOverlaps(queries[i % QueryCount]).Count();
        }
    }
}
