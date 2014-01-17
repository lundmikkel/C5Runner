using C5;
using C5.Intervals;
using NPerf.Framework;

namespace NPerf
{
    [PerfTester(
        typeof(IIntervalCollection<IInterval<int>, int>),
        20,
        Description = "New Gaps")]
    public class Gaps
    {
        DynamicIntervalTree<IInterval<int>, int> _collection;

        [PerfTest]
        public void Test(IIntervalCollection<IInterval<int>, int> collection)
        {
            ICollectionValue<IInterval<int>> coll = collection;

            var ri = collection.Count;
        }
    }
}
