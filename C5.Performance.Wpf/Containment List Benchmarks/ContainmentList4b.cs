﻿using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

    class ContainmentList4b : Benchmarkable
    {
        private const int Count = 1600 * 1000;
        private const int HighestLow = 1000 * 1000 * 1000;
        private readonly IntervalCollectionConstructor _constructor;
        private IIntervalCollection<IInterval<int>, int> _collection;
        private const int QueryCount = 100;
        private IInterval<int>[] queryIntervals = new IInterval<int>[QueryCount];
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

            var intervals = IntervalsFactory.ContainmentListIntervals(collectionSize, HighestLow).ToArray();
            _collection = _constructor(intervals);

            const int length = (int) (((double) HighestLow) / Count * 100);
            for (var i = 0; i < QueryCount; ++i)
            {
                var low = _random.Next(HighestLow);
                queryIntervals[i] = new IntervalBase<int>(low, low + length, IntervalType.Closed);
            }
        }

        public override IEnumerable<int> CollectionSizes()
        {
            var size = 1000;
            yield return size;

            while (size < 1000 * 1000)
                yield return size *= 2;
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