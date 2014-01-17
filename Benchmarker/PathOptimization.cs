using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5.Intervals;

namespace Benchmarker
{
    class PathOptimization
    {


        [TestFixture]
        class FindOverlaps_PathOptimizing
        {
            private DynamicIntervalTree<IInterval<int>, int> _collection;

            [SetUp]
            public void SetUp()
            {
                //var intervals = BenchmarkTestCases.DataSetA(31);
                //_collection = new DynamicIntervalTree<IInterval<int>, int>(intervals);
            }

            [Test]
            public void FindOverlaps_Middle()
            {
                _collection.FindOverlaps(new IntervalBase<int>(30, 32));
            }

            [Test]
            public void FindOverlaps_RightLeft()
            {
                _collection.FindOverlaps(new IntervalBase<int>(32, 33));
            }

            [Test]
            public void FindOverlaps_RightOff()
            {
                _collection.FindOverlaps(new IntervalBase<int>(61, 62));
            }

            [Test]
            public void FindOverlaps_Right()
            {
                _collection.FindOverlaps(new IntervalBase<int>(60, 61));
            }

            [Test]
            public void FindOverlaps_LeftMiddle()
            {
                _collection.FindOverlaps(new IntervalBase<int>(22, 24));
            }

            [Test]
            public void FindOverlapsStabbing_LeftMiddle()
            {
                _collection.FindOverlaps(new IntervalBase<int>(23));
            }

            [Test]
            public void FindOverlapsStabbing_Middle()
            {
                _collection.FindOverlaps(new IntervalBase<int>(31));
            }

            [Test]
            public void FindOverlaps_LeftRightLeft()
            {
                _collection.FindOverlaps(new IntervalBase<int>(16, 17));
            }

            [Test]
            public void FindOverlaps_Left()
            {
                _collection.FindOverlaps(new IntervalBase<int>(0, 1));
            }

            [Test]
            public void FindOverlaps_RightMiddle()
            {
                _collection.FindOverlaps(new IntervalBase<int>(38, 40));
            }
        }

    }
}
