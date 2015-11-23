using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

    class ContainmentList4b : Benchmarkable
    {
        private const int HighestHigh = 1000 * 1000 * 1000;
        private readonly IntervalCollectionConstructor _constructor;
        private IIntervalCollection<IInterval<int>, int> _collection;
        private const int QueryCount = 100;
        private IInterval<int>[] queries;
        private readonly Random _random = new Random();
        private readonly string _collectionName;

        public ContainmentList4b(IntervalCollectionConstructor constructor, string collectionName)
        {
            _constructor = constructor;
            _collectionName = collectionName;
        }


        public override string BenchMarkName()
        {
            return "4b - " + _collectionName;
        }

        public override void CollectionSetup(int collectionSize)
        {
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);

            var intervals = IntervalsFactory.ContainmentListIntervals(collectionSize, HighestHigh).ToArray();
            _collection = _constructor(intervals);

            var length = HighestHigh / collectionSize * 1000;
            queries = new IInterval<int>[QueryCount];

            for (var i = 0; i < QueryCount; ++i)
            {
                var low = _random.Next(HighestHigh - length);
                queries[i] = new IntervalBase<int>(low, low + length, IntervalType.Closed);
            }
        }

        public override IEnumerable<int> CollectionSizes()
        {
            var size = 125000;
            yield return size;

            while (size < 32 * 1000 * 1000)
                yield return size *= 2;
        }

        public override double Call(int i, int collectionSize)
        {
            return _collection.FindOverlaps(queries[i]).Count();
        }
    }
}
