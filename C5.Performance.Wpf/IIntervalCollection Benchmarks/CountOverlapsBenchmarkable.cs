using System;
using C5.Intervals;
using C5.Performance.Wpf.Benchmarks;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.IIntervalCollection_Benchmarks
{
    abstract class CountOverlapsBenchmarkable : IntervalBenchmarkable
    {
        internal IInterval<int> QueryInterval;
        protected CountOverlapsBenchmarkable(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override void CollectionSetup(int collectionSize)
        {
            Intervals = IntervalConstruction(collectionSize);
            IntervalCollection = IntervalCollectionConstruction(Intervals);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
            QueryInterval = new IntervalBase<int>(collectionSize * 1 / 5, collectionSize * 3 / 5);
        }
    }

    class CountOverlapsBenchmarkable_CountOverlapsRange : CountOverlapsBenchmarkable
    {
        public CountOverlapsBenchmarkable_CountOverlapsRange(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            return IntervalCollection.CountOverlaps(QueryInterval);
        }
    }

    class CountOverlapsBenchmarkable_CountOverlapsStabbing : CountOverlapsBenchmarkable
    {
        public CountOverlapsBenchmarkable_CountOverlapsStabbing(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            return IntervalCollection.CountOverlaps(QueryInterval.Low) + IntervalCollection.CountOverlaps(QueryInterval.High);
        }
    }
}
