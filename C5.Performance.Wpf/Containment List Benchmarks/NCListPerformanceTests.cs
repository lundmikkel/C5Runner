using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using C5.Intervals;
using C5.Intervals.Performance;

namespace C5.Performance.Wpf.Containment_List_Benchmarks
{
    class NCListPerformanceTests
    {
        private const string Directory = "benchmarks";

        public static void Test4a()
        {
            var filename = "4a_" + DateTime.Now.Ticks + ".txt";
            var count = 1600 * 1000;
            var highestLow = 1000 * 1000 * 1000;
            var runCount = 1000;
            var iterations = 1000;
            var intervals = IntervalsFactory.ContainmentListIntervals(count, highestLow).ToArray();
            var collection = new NestedContainmentList<IInterval<int>, int>(intervals);
            var random = new Random();

            for (var run = 0; run < runCount; ++run)
            {
                var timer = new Timer();
                var length = random.Next(1, 1500 * 1000);

                var sum = 0;
                for (var iteration = 0; iteration < iterations; ++iteration)
                {
                    var low = random.Next(0, highestLow);
                    var query = new IntervalBase<int>(low, low + length);

                    timer.Play();
                    sum += collection.FindOverlaps(query).Count();
                    timer.Pause();
                }

                var resultSize = sum / iterations;
                var queryTime = timer.Check() / iterations;
                appendText(filename, resultSize, queryTime);
            }
        }

        public static void Test4b()
        {
            var filename = "4b_" + DateTime.Now.Ticks + ".txt";
            var highestLow = 1000 * 1000 * 1000;
            var iterations = 1000;
            var random = new Random();

            for (var collectionSize = 12500; collectionSize <= 3200 * 1000; collectionSize *= 2)
            {
                var intervals = IntervalsFactory.ContainmentListIntervals(collectionSize, highestLow).ToArray();
                var collection = new NestedContainmentList<IInterval<int>, int>(intervals);
                var length = highestLow / collectionSize * 1000;

                int resultSize;
                double queryTime;
                double runningTimeInMilliSeconds;
                var timer = new Timer();
                do
                {
                    var sum = 0;
                    for (var iteration = 0; iteration < iterations; ++iteration)
                    {
                        var low = random.Next(0, highestLow);
                        var query = new IntervalBase<int>(low, low + length);

                        timer.Play();
                        sum += collection.FindOverlaps(query).Count();
                        timer.Pause();
                    }

                    runningTimeInMilliSeconds = timer.Check();
                    resultSize = sum / iterations;
                    queryTime = runningTimeInMilliSeconds / iterations;
                } while (runningTimeInMilliSeconds < 5 * 1000);

                appendText(filename, (double) collectionSize / 1000, queryTime);
                Console.WriteLine("Count: {0} - Result size: {1}", (double) collectionSize / 1000, resultSize);
            }
        }

        private static void appendText(string filename, double x, double y)
        {
            if (!File.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

            using (var w = File.AppendText(Path.Combine(Directory, filename)))
            {
                w.WriteLine("{0}\t{1}", x, y);
            }
        }
    }
}
