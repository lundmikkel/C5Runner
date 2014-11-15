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

        public override void CollectionSetup(int collectionSize)
        {
            Intervals = IntervalConstruction(collectionSize);
            IntervalCollection = IntervalCollectionConstruction(EmptyIntervals);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
            Order(collectionSize);
        }

        internal abstract void Order(int collectionSize);

        public override void Setup(int collectionSize)
        {
            IntervalCollection.Clear();
        }

        public override double Call(int i, int collectionSize)
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

        internal override void Order(int collectionSize)
        {
            C5.Sorting.IntroSort(Intervals, 0, collectionSize, IntervalExtensions.CreateComparer<IInterval<int>, int>());
        }
    }

    internal class AddSortedReverse : Add
    {
        public AddSortedReverse(Func<int, IInterval<int>[]> intervalConstruction,
            Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        internal override void Order(int collectionSize)
        {
            C5.Sorting.IntroSort(Intervals, 0, collectionSize, IntervalExtensions.CreateReversedComparer<IInterval<int>, int>());
        }
    }

    internal class AddRandom : Add
    {
        public AddRandom(Func<int, IInterval<int>[]> intervalConstruction,
            Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override void Setup(int collectionSize)
        {
            base.Setup(collectionSize);
            Order(collectionSize);
        }

        internal override void Order(int collectionSize)
        {
            Intervals.Shuffle();
        }
    }
}