using System;
using System.Collections.Generic;
using System.Linq;

namespace C5.Intervals.PerformanceRunner
{
    using C5.Intervals;
    using C5.Intervals.Performance;

    using SimpleSpeedTester.Core;
    using SimpleSpeedTester.Core.OutcomeFilters;

    class Program
    {
        private static int numberOfIterations = 5;

        static void Main(string[] args)
        {
            var collectionSizes = new List<int> { 100, 1000, 10 * 1000, 100 * 1000, 1000 * 1000 };

            var results = new Dictionary<string, List<double>>();

            var functions = Functions();
            var intervalCollections = IntervalCollections();

            foreach (var intervalCollectionDefinition in intervalCollections)
            {
                Console.WriteLine(" ");
                Console.WriteLine("=================================================");
                Console.WriteLine("Interval Collection: {0}", intervalCollectionDefinition.Item1);
                Console.WriteLine("=================================================");

                foreach (var function in functions)
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Function: {0}", function.Item1);
                    Console.WriteLine("-------------------------------");

                    foreach (var collectionSize in collectionSizes)
                    {
                        var intervals = IntervalsFactory.ConstantLengthMeets(collectionSize);
                        var intervalCollection = IntervalCollectionFactory<IInterval<int>, int>.CreateCollection(intervals, intervalCollectionDefinition.Item2, intervalCollectionDefinition.Item3, intervalCollectionDefinition.Item4);

                        var result = function.Item2(intervalCollection);

                        Console.WriteLine("{0} : {1}", collectionSize, result);

                    }
                }
            }

            Console.ReadLine();
        }

        public static List<Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>> Functions()
        {
            var functions = new List<Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>>();

            functions.Add(new Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>("Find First Gap", FindFirstGap));
            //functions.Add(new Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>("Find Last Gap", FindLastGap));
            //functions.Add(new Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>("Find First Gap in Range", FindFirstGapInRange));
            //functions.Add(new Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>("Find Last Gap in Range", FindLastGapInRange));
            functions.Add(new Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>("Find Gap InRange With Small Duration", FindGapInRangeWithSmallDuration));
            functions.Add(new Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>("Find Gap InRange With Large Duration", FindGapInRangeWithLargeDuration));


            return functions;
        }

        public static List<Tuple<string, bool, bool, bool>> IntervalCollections()
        {
            var intervalCollections = new List<Tuple<string, bool, bool, bool>>();

            //intervalCollections.Add(new Tuple<string, bool, bool, bool>("Overlaps - Dynamic (IBS)", true, false, false));
            //intervalCollections.Add(new Tuple<string, bool, bool, bool>("Overlaps - Dynamic (DIT)", true, false, true));
            //intervalCollections.Add(new Tuple<string, bool, bool, bool>("Overlaps - Static (LCList)", true, true, false));
            intervalCollections.Add(new Tuple<string, bool, bool, bool>("NoOverlaps - Dynamic (DLFIT)", false, false, false));
            intervalCollections.Add(new Tuple<string, bool, bool, bool>("NoOverlaps - Static (SFIL)", false, true, false));

            return intervalCollections;
        }

        public new Dictionary<string, List<double>> InitializeResults()
        {
            throw new Exception();
        }

        public static double FindFirstGap(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            Action task = () => intervalCollection.Gaps.FirstOrDefault();

            var testGroup = new TestGroup("Find First Gap");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            return result.AverageExecutionTime;
        }

        public static double FindLastGap(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            Action task = () => intervalCollection.Gaps.LastOrDefault();

            var testGroup = new TestGroup("Find First Gap");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            return result.AverageExecutionTime;
        }

        public static double FindFirstGapInRange(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            var startPoint = (intervalCollection.Span.High - intervalCollection.Span.Low) / 10;
            var queryRange = new IntervalBase<int>(startPoint, intervalCollection.Span.High);
            Action task = () => intervalCollection.FindGaps(queryRange).First();

            var testGroup = new TestGroup("Find First Gap in range");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            return result.AverageExecutionTime;
        }

        public static double FindLastGapInRange(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            var startPoint = (intervalCollection.Span.High - intervalCollection.Span.Low) / 10;
            var queryRange = new IntervalBase<int>(startPoint, intervalCollection.Span.High);
            Action task = () => intervalCollection.FindGaps(queryRange).LastOrDefault();

            var testGroup = new TestGroup("Find Last Gap in range");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            return result.AverageExecutionTime;
        }

        public static double FindGapInRangeWithSmallDuration(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            var startPoint = (intervalCollection.Span.High - intervalCollection.Span.Low) / 10;
            var queryRange = new IntervalBase<int>(startPoint, intervalCollection.Span.High);
            Action task = () => intervalCollection.FindGaps(queryRange).FirstOrDefault(x => x.High - x.Low >= 1);

            var testGroup = new TestGroup("Find First Gap in range");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            return result.AverageExecutionTime;
        }

        public static double FindGapInRangeWithLargeDuration(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            var startPoint = (intervalCollection.Span.High - intervalCollection.Span.Low) / 10;
            var queryRange = new IntervalBase<int>(startPoint, intervalCollection.Span.High);
            Action task = () => intervalCollection.FindGaps(queryRange).FirstOrDefault(x => x.High - x.Low >= 2000);

            var testGroup = new TestGroup("Find First Gap in range");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            return result.AverageExecutionTime;
        }
    }
}
