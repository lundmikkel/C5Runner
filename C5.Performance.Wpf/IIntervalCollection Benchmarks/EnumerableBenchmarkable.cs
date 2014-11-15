using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5.Intervals;
using C5.Performance.Wpf.Benchmarks;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.IIntervalCollection_Benchmarks
{
    abstract class EnumerableBenchmarkable : IntervalBenchmarkable
    {
        protected EnumerableBenchmarkable(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override void CollectionSetup(int collectionSize)
        {
            Intervals = IntervalConstruction(collectionSize);
            IntervalCollection = IntervalCollectionConstruction(Intervals);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
        }
    }

    class EnumerableBenchmarkable_GetEnumerator : EnumerableBenchmarkable
    {
        public EnumerableBenchmarkable_GetEnumerator(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            return IntervalCollection.Count(x => x != null);
        }
    }

    class EnumerableBenchmarkable_Sorted : EnumerableBenchmarkable
    {
        public EnumerableBenchmarkable_Sorted(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            return IntervalCollection.Sorted.Count(x => x != null);
        }
    }
}
