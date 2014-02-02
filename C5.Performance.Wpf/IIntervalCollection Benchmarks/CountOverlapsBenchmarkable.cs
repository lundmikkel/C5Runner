using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public override void CollectionSetup()
        {
            Intervals = IntervalConstruction(CollectionSize);
            IntervalCollection = IntervalCollectionConstruction(Intervals);
            ItemsArray = SearchAndSort.FillIntArray(CollectionSize);
            QueryInterval = new IntervalBase<int>(CollectionSize * 1 / 5, CollectionSize * 3 / 5);
        }

        public override void Setup() { }
    }

    class CountOverlapsBenchmarkable_CountOverlapsRange : CountOverlapsBenchmarkable
    {
        public CountOverlapsBenchmarkable_CountOverlapsRange(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i)
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

        public override double Call(int i)
        {
            return IntervalCollection.CountOverlaps(QueryInterval.Low) + IntervalCollection.CountOverlaps(QueryInterval.High);
        }
    }
}
