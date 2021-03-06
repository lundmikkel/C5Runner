﻿using C5.Intervals;
using C5.Intervals.Tests;

namespace C5.Performance.Wpf.Benchmarks
{
    class LCListEnumeratorSorted : Benchmarkable
    {
        private LayeredContainmentList<IInterval<int>, int> _lclist;

        public override string BenchMarkName()
        {
            return "LCList Enumerable Sorted";
        }

        public override void CollectionSetup(int collectionSize)
        {
            _lclist = new LayeredContainmentList<IInterval<int>, int>(BenchmarkTestCases.DataSetA(collectionSize));
        }

        public override double Call(int i, int collectionSize)
        {
            var sum = 0.0;
            foreach (var interval in _lclist.Sorted)
                sum += interval.Low;
            return sum;
        }
    }
}
