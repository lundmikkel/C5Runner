using System;
using System.Linq;
using NPerf.Framework;

namespace C5.Intervals.NPerf
{
    [PerfTester(typeof(
        IIntervalCollection<IInterval<int>, int>),
        20,
        Description = "Gaps - old",
        FeatureDescription = "Collection size")]
    public class IIntervalGapsPerfs
    {
        private readonly Random _random = new Random();
        private IInterval<int>[] _queries;
        private int _count;

        public double Dummy { get; set; }

        [PerfRunDescriptor]
        public double RunDescription(int testIndex)
        {
            return collectionCount(testIndex);
        }

        private static int collectionCount(int testIndex)
        {
            return testIndex * 10000;
        }

        [PerfSetUp]
        public void SetUp(int index, IIntervalCollection<IInterval<int>, int> collection)
        {
            // Save collection _count
            _count = collectionCount(index);

            IInterval<int> span;

            // TODO: Create from constructor?
            // Add intervals to collection
            collection.AddAll(intervals(out span));

            // Generate query intervals
            _queries = queries(span);
        }

        private IInterval<int>[] intervals(out IInterval<int> span, int length = 10)
        {
            var count = 10;

            var intervals = new IInterval<int>[count];

            intervals[0] = new IntervalBase<int>(0, _random.Next(1, 1 + length));
            for (var i = 1; i < count; i++)
            {
                var lastHigh = intervals[i - 1].High + 3;
                intervals[i] = new IntervalBase<int>(lastHigh, _random.Next(lastHigh + 1, lastHigh + 1 + length));
            }

            span = new IntervalBase<int>(intervals[0], intervals[count - 1]);

            return intervals;
        }

        private IInterval<int>[] queries(IInterval<int> span)
        {
            var queries = new IInterval<int>[_count];
            var duration = (span.High - span.Low) / _count;

            for (var i = 0; i < _count; i++)
                queries[i] = new IntervalBase<int>(i * duration, (i + 1) * duration);

            return queries;
        }

        [PerfTest]
        public void Test(IIntervalCollection<IInterval<int>, int> collection)
        {
            foreach (var query in _queries)
                Dummy += collection.Gaps(query).Count();
        }
    }
}
