using C5.Intervals;

namespace C5.Performance.Wpf.Benchmarks
{
    public class IbsAvlIntervalSetsBenchmarker : Benchmarkable
    {
        private IntervalBinarySearchTree<IInterval<int>, int> _collection;
        private IInterval<int>[] _intervals;

        public override void CollectionSetup(int collectionSize)
        {
            _intervals = Intervals.Tests.BenchmarkTestCases.DataSetA(collectionSize);
            _collection = new IntervalBinarySearchTree<IInterval<int>, int>();
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
            SearchAndSort.Shuffle(ItemsArray);
        }

        public override void Setup(int collectionSize)
        {
            _collection.Clear();
        }

        public override double Call(int i, int collectionSize)
        {
            foreach (var interval in _intervals)
                _collection.Add(interval);
            return _collection.Count;
        }

        public override string BenchMarkName()
        {
            return "IBS Interval Set New";
        }
    }

    public class IbsAvlIntervalSetsPrebuildBenchmarker : Benchmarkable
    {
        private IntervalBinarySearchTree<IInterval<int>, int> _collection;
        private IInterval<int>[] _intervals;

        public override void CollectionSetup(int collectionSize)
        {
            _intervals = Intervals.Tests.BenchmarkTestCases.DataSetA(collectionSize);
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
            SearchAndSort.Shuffle(ItemsArray);
        }

        public override double Call(int i, int collectionSize)
        {
            _collection = new IntervalBinarySearchTree<IInterval<int>, int>(_intervals);
            return _collection.Count;
        }

        public override string BenchMarkName()
        {
            return "IBS Interval Set New - PreBuild";
        }
    }
}
