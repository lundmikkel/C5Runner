using System;
using C5.Intervals;
using C5.Intervals.Tests;

namespace C5.Performance.Wpf.Benchmarks
{
    public class IbsAvlRandomRemoveBenchmarker : Benchmarkable
    {
        private IntervalBinarySearchTree<IInterval<int>, int> _collection;
        private IList<IInterval<int>> _intervals;
        private readonly Random _random = new Random(0);

        public override void CollectionSetup(int collectionSize)
        {
            _collection = new IntervalBinarySearchTree<IInterval<int>, int>();
            _intervals = new ArrayList<IInterval<int>>(collectionSize);
            _intervals.AddAll(BenchmarkTestCases.DataSetB(collectionSize));
            _intervals.Shuffle(_random);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
            SearchAndSort.Shuffle(ItemsArray);
        }

        public override double Call(int i, int collectionSize)
        {
            foreach (var interval in _intervals)
                _collection.Add(interval);

            foreach (var interval in _intervals)
                _collection.Remove(interval);

            return _collection.Count;
        }

        public override string BenchMarkName()
        {
            return "IBS RandomRemove (AVL)";
        }
    }
}
