using System;
using C5.Intervals;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.Benchmarks
{
    abstract class DateTimeSorting : Benchmarkable
    {
        internal DateTime[] numbers;
        internal Action<DateTime[]> _sortingMethod;
        internal string _methodName;

        public DateTimeSorting(Action<DateTime[]> sortingMethod, string methodName)
        {
            _sortingMethod = sortingMethod;
            _methodName = methodName;
        }

        public override void CollectionSetup(int collectionSize)
        {
            numbers = new DateTime[collectionSize];
            for (var i = 0; i < collectionSize; i++)
                numbers[i] = DateTime.Now + new TimeSpan(i, 0, 0);

            ItemsArray = SearchAndSort.FillIntArray(collectionSize);

            GC.Collect();
        }

        public override double Call(int i, int collectionSize)
        {
            _sortingMethod(numbers);
            return 1;
        }
    }

    class DateTimeSortingRandom : DateTimeSorting
    {
        public DateTimeSortingRandom(Action<DateTime[]> sortingMethod, string methodName)
            : base(sortingMethod, methodName)
        {
        }

        public override string BenchMarkName()
        {
            return "DateTimeSorting Random - " + _methodName;
        }

        public override void Setup(int collectionSize)
        {
            numbers.Shuffle();
        }
    }

    class DateTimeSortingSorted : DateTimeSorting
    {
        public DateTimeSortingSorted(Action<DateTime[]> sortingMethod, string methodName)
            : base(sortingMethod, methodName)
        {
        }

        public override string BenchMarkName()
        {
            return "DateTimeSorting Sorted - " + _methodName;
        }
    }

    class DateTimeSortingReverseSorted : DateTimeSorting
    {
        public DateTimeSortingReverseSorted(Action<DateTime[]> sortingMethod, string methodName)
            : base(sortingMethod, methodName)
        {
        }

        public override string BenchMarkName()
        {
            return "DateTimeSorting Reverse Sorted - " + _methodName;
        }

        public override void Setup(int collectionSize)
        {
            Array.Reverse(numbers);
        }
    }
}
