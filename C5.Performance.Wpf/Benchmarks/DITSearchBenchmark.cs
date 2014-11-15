using System.Linq;
using C5.Intervals;
using C5.Intervals.Tests;

namespace C5.Performance.Wpf.Benchmarks
{
    public class DITSearchBenchmark : Benchmarkable
    {
        private IInterval<int>[] _intervals;
        private IInterval<int>[] _intervalsNot;
        private DynamicIntervalTree<IInterval<int>, int> _intervalCollection;

        public override void CollectionSetup(int collectionSize)
        {
            _intervals = BenchmarkTestCases.DataSetA(collectionSize);
            _intervalsNot = BenchmarkTestCases.DataSetNotA(collectionSize);
            _intervalCollection = new DynamicIntervalTree<IInterval<int>, int>(_intervals);

            /*
             * Setup an items array with things to look for.
             * Fill in random numbers from 0 to the number of trains plus the number of trains not in the collection.
             * This should make roughly half the searched succesful if we find enough space to generate as many trains not in the collection as there is trains already.
             */
            ItemsArray = SearchAndSort.FillIntArrayRandomly(collectionSize, 0, collectionSize * 2);
        }

        public override double Call(int intervalId, int collectionSize)
        {
            if (intervalId < collectionSize)
                return _intervalCollection.FindOverlaps(_intervals[intervalId]).Count() > 0 ? 1 : 0;
            return _intervalCollection.FindOverlaps(_intervalsNot[intervalId - collectionSize]).Count() > 0 ? 1 : 0;
        }

        public override string BenchMarkName()
        {
            return "DIT Search";
        }
    }
}