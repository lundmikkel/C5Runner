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

        public override void CollectionSetup(int collectionSize)
        {
            Intervals = IntervalConstruction(collectionSize);
            Order(collectionSize);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
        }

        internal abstract void Order(int collectionSize);

        public override double Call(int i, int collectionSize)
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

        internal override void Order(int collectionSize)
        {
            C5.Sorting.IntroSort(Intervals, 0, collectionSize, IntervalExtensions.CreateComparer<IInterval<int>, int>());
        }
    }

    class ConstructSortedReverse : Construct
    {
        public ConstructSortedReverse(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        internal override void Order(int collectionSize)
        {
            C5.Sorting.IntroSort(Intervals, 0, collectionSize, IntervalExtensions.CreateReversedComparer<IInterval<int>, int>());
        }
    }

    class ConstructRandom : Construct
    {
        public ConstructRandom(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        { }

        internal override void Order(int collectionSize)
        {
            Intervals.Shuffle();
        }

        public override void Setup(int collectionSize)
        {
            base.Setup(collectionSize);
            Order(collectionSize);
        }
    }
}
