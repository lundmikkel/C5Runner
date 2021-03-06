﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5.Intervals;
using C5.Performance.Wpf.Benchmarks;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.IIntervalCollection_Benchmarks
{
    abstract class GapsBenchmarkable : IntervalBenchmarkable
    {
        protected GapsBenchmarkable(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
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

    class GapsBenchmarkable_GapsFirst : GapsBenchmarkable
    {
        public GapsBenchmarkable_GapsFirst(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            return IntervalCollection.Gaps.First().Low;
        }
    }

    class GapsBenchmarkable_GapsLast : GapsBenchmarkable
    {
        public GapsBenchmarkable_GapsLast(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            return IntervalCollection.Gaps.Last().Low;
        }
    }
}
