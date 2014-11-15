using System;
using System.Linq;
using C5.Performance.Wpf.Benchmarks;
using C5.Intervals.Tests;
using C5.Intervals;

namespace C5.Performance.Wpf.Report_Benchmarks
{
    class QueryRange : IntervalBenchmarkable
    {
        public QueryRange(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        public override void CollectionSetup(int collectionSize)
        {
            Intervals = IntervalConstruction(collectionSize);
            IntervalCollection = IntervalCollectionConstruction(Intervals);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
        }

        public override void Setup(int collectionSize)
        {
            ItemsArray.Shuffle();
        }

        public override double Call(int i, int collectionSize)
        {
            var success = IntervalCollection.FindOverlaps(Intervals[i]);
            return success.Count();
        }
    }
}
