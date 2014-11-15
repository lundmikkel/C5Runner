using System;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Benchmarks;
using C5.Intervals;

namespace C5.Performance.Wpf.Report_Benchmarks
{
    class ConstructAddUnsorted : IntervalBenchmarkable
    {
        public ConstructAddUnsorted(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        public override void CollectionSetup(int collectionSize)
        {
            Intervals = IntervalConstruction(collectionSize);
            Intervals.Shuffle();
            IntervalCollection = IntervalCollectionConstruction(EmptyIntervals);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
        }

        public override void Setup(int collectionSize)
        {
            Intervals.Shuffle();
            IntervalCollection.Clear();
        }

        public override double Call(int i, int collectionSize)
        {
            foreach (var interval in Intervals)
                IntervalCollection.Add(interval);
            return 1;
        }
    }
}
