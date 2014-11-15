using C5.Intervals;

namespace C5.Performance.Wpf.Benchmarks
{
    public class IbsAvlAddBenchmarker : Benchmarkable
    {
        private IntervalBinarySearchTree<IInterval<int>, int> _collection;
        private IInterval<int>[] _intervals;

        public override void CollectionSetup(int collectionSize)
        {
            _intervals = Intervals.Tests.BenchmarkTestCases.DataSetC(collectionSize);
            _collection = new IntervalBinarySearchTree<IInterval<int>, int>();
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
            SearchAndSort.Shuffle(ItemsArray);
        }

        public override double Call(int i, int collectionSize)
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
