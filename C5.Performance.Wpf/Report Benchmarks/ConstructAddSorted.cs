using System;
using C5.Intervals;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Benchmarks;

namespace C5.Performance.Wpf.Report_Benchmarks
{
    internal abstract class Add : IntervalBenchmarkable
    {
        public Add(Func<int, IInterval<int>[]> intervalConstruction,
            Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override void CollectionSetup()
        {
            Intervals = IntervalConstruction(CollectionSize);
            IntervalCollection = IntervalCollectionConstruction(EmptyIntervals);
            ItemsArray = SearchAndSort.FillIntArray(CollectionSize);
            Order();
        }

        internal abstract void Order();

        public override void Setup()
        {
            IntervalCollection.Clear();
        }

        public override double Call(int i)
        {
            foreach (var interval in Intervals)
                IntervalCollection.Add(interval);

            return IntervalCollection.Count;
        }
    }
    internal class AddSorted : Add
    {
        public AddSorted(
            Func<int, IInterval<int>[]> intervalConstruction,
            Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction
        )
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        internal override void Order()
        {
            C5.Sorting.IntroSort(Intervals, 0, CollectionSize, IntervalExtensions.CreateComparer<IInterval<int>, int>());
        }
    }

    internal class AddSortedReverse : Add
    {
        public AddSortedReverse(Func<int, IInterval<int>[]> intervalConstruction,
            Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        internal override void Order()
        {
            C5.Sorting.IntroSort(Intervals, 0, CollectionSize, IntervalExtensions.CreateReversedComparer<IInterval<int>, int>());
        }
    }

    internal class AddRandom : Add
    {
        public AddRandom(Func<int, IInterval<int>[]> intervalConstruction,
            Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override void Setup()
        {
            base.Setup();
            Order();
        }

        internal override void Order()
        {
            Intervals.Shuffle();
        }
    }
}