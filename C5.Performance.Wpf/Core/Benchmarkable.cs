using System;
using System.Collections.Generic;

namespace C5.Performance.Wpf.Benchmarks
{
    public abstract class Benchmarkable
    {
        internal int[] ItemsArray;

        public abstract String BenchMarkName();

        /// <summary>
        /// Prepare the collections used for the benchmark
        /// </summary>
        public virtual void CollectionSetup(int collectionSize) { }

        /// <summary>
        /// Do some setup before each benchmark run
        /// </summary>
        public virtual void Setup(int collectionSize) { }

        public abstract double Call(int i, int collectionSize);

        public virtual IEnumerable<int> CollectionSizes()
        {
            var size = 100;
            yield return size;

            while (size < Int32.MaxValue / 2)
                yield return size *= 2;
        }

        public Benchmark Benchmark(int maxCount, int runCount, int collectionSize, double maxExecutionTimeInSeconds, Action<string> updateLabel, bool runWarmup = true)
        {
            Console.Write("Setting up collection... ");
            CollectionSetup(collectionSize);
            Console.WriteLine("done.");

            var count = 1;
            double dummy = 0.0,
                runningTimeInMilliSeconds = 0.0,
                elapsedTime,
                elapsedSquaredTime;


            // Warm up JIT
            if (runWarmup)
            {
                updateLabel("Runs warmup");
                dummy += Call(collectionSize, collectionSize);
                updateLabel(string.Empty);

                // Ensure the warmup isn't optimized away
                if ((long) dummy == DateTime.Now.Ticks)
                    Console.Write(@" ");
            }


            do
            {
                dummy = 0.0;

                // Step up the count by a factor
                count *= 2;
                elapsedTime = elapsedSquaredTime = 0.0;
                for (var run = 1; run <= runCount; ++run)
                {
                    
                    updateLabel(String.Format("Benchmarking {0} calls {1} of {2} times", count, run, runCount));

                    var timer = new Timer();
                    for (var iteration = 0; iteration < count; ++iteration)
                    {
                        Setup(collectionSize);
                        //GC.Collect();

                        timer.Play();
                        dummy += Call(ItemsArray[iteration % collectionSize], collectionSize);
                        timer.Pause();
                    }
                    runningTimeInMilliSeconds = timer.Check();
                    var time = runningTimeInMilliSeconds / count;
                    elapsedTime += time;
                    elapsedSquaredTime += time * time;
                }
            } while (runningTimeInMilliSeconds < maxExecutionTimeInSeconds * 1000 && count < maxCount);

            var meanTime = elapsedTime / runCount;
            var standardDeviation = Math.Sqrt(elapsedSquaredTime / runCount - meanTime * meanTime);
            
            updateLabel(string.Empty);

            return new Benchmark
                {
                    BenchmarkName = BenchMarkName(),
                    CollectionSize = collectionSize,
                    MeanTime = meanTime,
                    StandardDeviation = standardDeviation,
                    NumberOfRuns = count,
                    Dummy = dummy
                };
        }
    }
}