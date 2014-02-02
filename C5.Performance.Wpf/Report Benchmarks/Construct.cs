using System;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Benchmarks;
using C5.Intervals;

namespace C5.Performance.Wpf.Report_Benchmarks
{
    abstract class Construct : IntervalBenchmarkable
    {
        public Construct(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        public override void CollectionSetup()
        {
            Intervals = IntervalConstruction(CollectionSize);
            Order();
            ItemsArray = SearchAndSort.FillIntArray(CollectionSize);
        }

        internal abstract void Order();

        public override void Setup()
        { }

        public override double Call(int i)
        {
            IntervalCollection = IntervalCollectionConstruction(Intervals);
            return 1;
        }
    }

    class ConstructSorted : Construct
    {
        public ConstructSorted(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        internal override void Order()
        {
            C5.Sorting.IntroSort(Intervals, 0, CollectionSize, IntervalExtensions.CreateComparer<IInterval<int>, int>());
        }
    }

    class ConstructSortedReverse : Construct
    {
        public ConstructSortedReverse(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        internal override void Order()
        {
            C5.Sorting.IntroSort(Intervals, 0, CollectionSize, IntervalExtensions.CreateReversedComparer<IInterval<int>, int>());
        }
    }

    class ConstructRandom : Construct
    {
        public ConstructRandom(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        internal override void Order()
        {
            Intervals.Shuffle();
        }

        public override void Setup()
        {
            base.Setup();
            Order();
        }
    }
}
