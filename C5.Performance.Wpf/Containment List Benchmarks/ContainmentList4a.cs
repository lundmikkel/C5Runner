using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;
using C5.Intervals.Tests;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

    class ContainmentList4a : Benchmarkable
    {
        private const int Count = 1600 * 1000;
        private const int HighestLow = 1000 * 1000 * 1000;
        private static readonly IInterval<int>[] Intervals = IntervalsFactory.ContainmentListIntervals(Count, HighestLow).ToArray().Shuffle();
        private readonly IntervalCollectionConstructor _constructor;
        private IIntervalCollection<IInterval<int>, int> _collection;
        private const int QueryCount = 100;
        private IInterval<int>[] queryIntervals = new IInterval<int>[QueryCount];
        private readonly Random _random = new Random();
        private readonly string _collectionName;

        public ContainmentList4a(IntervalCollectionConstructor constructor, string collectionName)
        {
            _constructor = constructor;
            _collectionName = collectionName;
        }


        public override string BenchMarkName()
        {
            return "4a - " + _collectionName;
        }

        public override void CollectionSetup(int collectionSize)
        {
            _collection = _constructor(Intervals);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
        }

        public override void Setup(int collectionSize)
        {
            var length = (int) (((double) HighestLow) / Count * collectionSize);
            for (var i = 0; i < QueryCount; ++i)
            {
                var low = _random.Next(HighestLow);
                queryIntervals[i] = new IntervalBase<int>(low, low + length, IntervalType.Closed);
            }
        }

        public override IEnumerable<int> CollectionSizes()
        {
            var size = 500;
            yield return size;

            while (size < 4000)
                yield return size += 500;
        }

        public override double Call(int something, int collectionSize)
        {
            var sum = 0;

            for (var i = 0; i < QueryCount; ++i)
                sum += _collection.FindOverlaps(queryIntervals[i]).Count();

            sum /= QueryCount;
            //Console.WriteLine(@"{0}: {1} ({2})", _collectionName, sum, collectionSize);
            return sum;
        }
    }
}
