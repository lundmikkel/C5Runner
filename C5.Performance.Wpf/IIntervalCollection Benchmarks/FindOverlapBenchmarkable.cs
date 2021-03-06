﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5.Intervals;
using C5.Performance.Wpf.Benchmarks;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.IIntervalCollection_Benchmarks
{
    abstract class FindOverlapBenchmarkable : IntervalBenchmarkable
    {
        internal IInterval<int> QueryInterval;
        protected FindOverlapBenchmarkable(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override void CollectionSetup(int collectionSize)
        {
            Intervals = IntervalConstruction(collectionSize);
            IntervalCollection = IntervalCollectionConstruction(Intervals);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
            QueryInterval = new IntervalBase<int>(collectionSize * 1 / 5, collectionSize * 2 / 8);
        }
    }

    class FindOverlapBenchmarkable_FindOverlapRange : FindOverlapBenchmarkable
    {
        public FindOverlapBenchmarkable_FindOverlapRange(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            IInterval<int> overlap;
            IntervalCollection.FindOverlap(QueryInterval, out overlap);
            return overlap.Low;
        }
    }

    class FindOverlapBenchmarkable_FindOverlapStabbing : FindOverlapBenchmarkable
    {
        public FindOverlapBenchmarkable_FindOverlapStabbing(Func<int, IInterval<int>[]> intervalConstruction, Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> intervalCollectionConstruction)
            : base(intervalConstruction, intervalCollectionConstruction)
        {
        }

        public override double Call(int i, int collectionSize)
        {
            IInterval<int> overlap;
            IntervalCollection.FindOverlap(QueryInterval.High, out overlap);
            IntervalCollection.FindOverlap(QueryInterval.Low, out overlap);
            return overlap.Low;
        }
    }
}
