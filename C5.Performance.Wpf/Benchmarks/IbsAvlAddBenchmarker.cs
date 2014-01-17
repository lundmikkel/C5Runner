﻿using C5.Intervals;

namespace C5.Performance.Wpf.Benchmarks
{
    public class IbsAvlAddBenchmarker : Benchmarkable
    {
        private IntervalBinarySearchTree<IInterval<int>, int> _collection;
        private IInterval<int>[] _intervals;

        public override void CollectionSetup()
        {
            _intervals = Intervals.Tests.BenchmarkTestCases.DataSetC(CollectionSize);
            _collection = new IntervalBinarySearchTree<IInterval<int>, int>();
            ItemsArray = SearchAndSort.FillIntArray(CollectionSize);
            SearchAndSort.Shuffle(ItemsArray);
        }

        public override void Setup() { }

        public override double Call(int i)
        {
            foreach (var interval in _intervals)
                _collection.Add(interval);

            return _collection.Count;
        }

        public override string BenchMarkName()
        {
            return "IBS Add (AVL)";
        }
    }
}
