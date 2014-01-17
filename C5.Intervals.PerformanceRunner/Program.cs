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
                Console.WriteLine("Interval Collection: {0}", intervalCollectionDefinition.Item1);
                Console.WriteLine("================================");

                foreach (var function in functions)
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Function: {0}", function.Item1);
                    Console.WriteLine("-------------------------------");

                    foreach (var collectionSize in collectionSizes)
                    {
                        var intervals = IntervalsFactory.NoOverlapsConstantLenghtGrowingGap(collectionSize);
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
            functions.Add(new Tuple<string, Func<IIntervalCollection<IInterval<int>, int>, double>>("Find Last Gap", FindLastGap));

            return functions;
        }

        public static List<Tuple<string, bool, bool, bool>> IntervalCollections()
        {
            var intervalCollections = new List<Tuple<string, bool, bool, bool>>();

            intervalCollections.Add(new Tuple<string, bool, bool, bool>("Overlaps - Dynamic (IBS)", true, false, false));
            intervalCollections.Add(new Tuple<string, bool, bool, bool>("Overlaps - Dynamic (DIT)", true, false, true));
            intervalCollections.Add(new Tuple<string, bool, bool, bool>("Overlaps - Static (SIT)", true, true, false));
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
            Action task = () => intervalCollection.Gaps.First();

            var testGroup = new TestGroup("Find First Gap");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            //Console.WriteLine(result);

            return result.AverageExecutionTime;
        }

        public static double FindLastGap(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            Action task = () => intervalCollection.Gaps.Last();
            var testGroup = new TestGroup("Find First Gap");
            var result = testGroup.PlanAndExecute(intervalCollection.GetType().Name, task, numberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());

            //Console.WriteLine(result);

            return result.AverageExecutionTime;
        }
    }
}
