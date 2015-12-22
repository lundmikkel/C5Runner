using System;
using System.Collections.Generic;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;

namespace C5.Performance.Wpf.Benchmarks
{
    using IntervalCollectionConstructor = Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>>;

    abstract class ContainmentListUcsc : Benchmarkable
    {
        protected IIntervalCollection<IInterval<int>, int> Collection;
        protected const int QueryCount = 1000000;
        protected IInterval<int>[] Queries;
        private readonly Random _random = new Random();
        protected readonly string CollectionName;

        protected ContainmentListUcsc(IIntervalCollection<IInterval<int>, int> collection, string collectionName)
        {
            Collection = collection;
            CollectionName = collectionName;
        }

        public override void CollectionSetup(int collectionSize)
        {
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);

            var highestHigh = Collection.Span.High;
            var length = collectionSize;
            Queries = new IInterval<int>[QueryCount];

            for (var i = 0; i < QueryCount; ++i)
            {
                var low = _random.Next(highestHigh - length);
                Queries[i] = new IntervalBase<int>(low, low + length);
            }
        }

        public override IEnumerable<int> CollectionSizes()
        {
            // Query widths
            return new[]
            {
                   100,
                   500,
                  1000,
                  5000,
                 10000,
                 50000,
                100000,
            };
        }
    }

    class ContainmentListUcscFindOverlaps : ContainmentListUcsc
    {
        public ContainmentListUcscFindOverlaps(IIntervalCollection<IInterval<int>, int> collection, string collectionName)
            : base(collection, collectionName)
        {
        }
        
        public override string BenchMarkName()
        {
            return String.Format("UCSC - Find - {0}", CollectionName);
        }

        public override double Call(int i, int collectionSize)
        {
            return Collection.FindOverlaps(Queries[i % QueryCount]).Count();
        }

    }

    class ContainmentListUcscCountOverlaps : ContainmentListUcsc
    {
        public ContainmentListUcscCountOverlaps(IIntervalCollection<IInterval<int>, int> collection, string collectionName)
            : base(collection, collectionName)
        {
        }

        public override string BenchMarkName()
        {
            return String.Format("UCSC - Count - {0}", CollectionName);
        }

        public override double Call(int i, int collectionSize)
        {
            return Collection.CountOverlaps(Queries[i % QueryCount]);
        }
    }
}
