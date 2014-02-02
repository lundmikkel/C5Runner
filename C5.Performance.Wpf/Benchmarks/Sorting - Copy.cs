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

        public override void CollectionSetup()
        {
            numbers = new DateTime[CollectionSize];
            for (var i = 0; i < CollectionSize; i++)
                numbers[i] = DateTime.Now + new TimeSpan(i, 0, 0);

            ItemsArray = SearchAndSort.FillIntArray(CollectionSize);

            GC.Collect();
        }

        public override double Call(int i)
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

        public override void Setup()
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

        public override void Setup()
        {
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

        public override void Setup()
        {
            Array.Reverse(numbers);
        }
    }
}
