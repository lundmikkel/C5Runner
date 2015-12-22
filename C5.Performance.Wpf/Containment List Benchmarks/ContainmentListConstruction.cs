using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;
using C5.Intervals.Tests;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

    class ContainmentListConstruction : Benchmarkable
    {
        private const int HighestHigh =  (int) 100.10 * 1000000;
        private readonly IntervalCollectionConstructor _constructor;
        private readonly string _collectionName;
        private IInterval<int>[] _intervals;

        public ContainmentListConstruction(IntervalCollectionConstructor constructor, string collectionName)
        {
            _constructor = constructor;
            _collectionName = collectionName;
        }

        public override string BenchMarkName()
        {
            return string.Format("Construction - {0}", _collectionName);
        }

        public override void CollectionSetup(int collectionSize)
        {
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
        }

        public override void Setup(int collectionSize)
        {
            _intervals = IntervalsFactory.ContainmentListIntervals(collectionSize, HighestHigh).ToArray().Shuffle();
        }

        public override double Call(int i, int collectionSize)
        {
            return _constructor(_intervals).Count;
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

}
