using C5.Intervals;
using C5.Performance.Wpf.Benchmarks;
using C5.Intervals.Tests;

namespace C5.Performance.Wpf.Report_Benchmarks
{
    public class IBSIntervalSetEager : Benchmarkable
    {
        private IInterval<int>[] _intervals;
        private IntervalBinarySearchTreeOld<IInterval<int>, int> _intervalCollection;

        private int intervalSearch(int intervalId)
        {
            // TODO Do Something that shows the eager interval set creation here
            return 1;
        }

        public override void CollectionSetup(int collectionSize)
        {
            _intervals = BenchmarkTestCases.DataSetB(collectionSize);
            _intervalCollection.AddAll(_intervals);
            _intervalCollection = new IntervalBinarySearchTreeOld<IInterval<int>, int>(_intervals);

            /*
             * Setup an items array with things to look for.
             * Fill in random numbers from 0 to the number of trains plus the number of trains not in the collection.
             * This should make roughly half the searched succesful if we find enough space to generate as many trains not in the collection as there is trains already.
             */
            ItemsArray = SearchAndSort.FillIntArrayRandomly(collectionSize, 0, collectionSize * 2);
        }

        public override double Call(int i, int collectionSize)
        {
            return intervalSearch(i);
        }

        public override string BenchMarkName()
        {
            return "IBS Intervalset Eager";
        }
    }
}