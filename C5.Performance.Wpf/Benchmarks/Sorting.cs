using System;
using C5.Intervals;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Report_Benchmarks;

namespace C5.Performance.Wpf.Benchmarks
{
    abstract class Sorting : Benchmarkable
    {
        internal int[] numbers;
        internal Action<int[]> _sortingMethod;
        internal string _methodName;

        public Sorting(Action<int[]> sortingMethod, string methodName)
        {
            _sortingMethod = sortingMethod;
            _methodName = methodName;
        }

        public override void CollectionSetup()
        {
            numbers = SearchAndSort.FillIntArray(CollectionSize);
            ItemsArray = SearchAndSort.FillIntArray(CollectionSize);

            GC.Collect();
        }

        public override double Call(int i)
        {
            _sortingMethod(numbers);
            return 1;
        }
    }

    class SortingSorted : Sorting
    {
        public SortingSorted(Action<int[]> sortingMethod, string methodName)
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

    class SortingReverseSorted : Sorting
    {
        public SortingReverseSorted(Action<int[]> sortingMethod, string methodName)
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

    class SortingRandom : Sorting
    {
        public SortingRandom(Action<int[]> sortingMethod, string methodName)
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
}
