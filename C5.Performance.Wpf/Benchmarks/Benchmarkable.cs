using System;
using System.Linq;

namespace C5.Performance.Wpf.Benchmarks
{
    public abstract class Benchmarkable
    {
        public int CollectionSize;
        internal int[] ItemsArray;

        public abstract String BenchMarkName();

        // Prepare the collections used for the benchmark
        public abstract void CollectionSetup();

        // Do some setup before each benchmark run
        public abstract void Setup();

        public abstract double Call(int i);

        public Benchmark Benchmark(int maxCount, int repeats, double maxExecutionTimeInSeconds, Benchmarker caller, bool runWarmup = true)
        {
            CollectionSetup();

            var count = 1;
            double dummy = 0.0,
                runningTimeInMilliSeconds = 0.0,
                elapsedTime,
                elapsedSquaredTime;

            var times = new ArrayList<double>();

            // Warm up JIT
            if (runWarmup)
                dummy += Call(CollectionSize);

            dummy = 0.0;
            do
            {
                // Step up the count by a factor
                count *= 10;
                elapsedTime = elapsedSquaredTime = 0.0;
                for (var j = 0; j < repeats; j++)
                {
                    caller.UpdateRunningLabel(String.Format("Benchmarking {0} calls {1} of {2} times", count, (j + 1), repeats));

                    var t = new Timer();
                    for (var i = 0; i < count; i++)
                    {
                        Setup();
                        GC.Collect();

                        t.Play();
                        dummy += Call(ItemsArray[i % CollectionSize]);
                        t.Pause();
                    }
                    runningTimeInMilliSeconds = t.Check();
                    var time = runningTimeInMilliSeconds / count;
                    times.Add(time);
                    elapsedTime += time;
                    elapsedSquaredTime += time * time;
                }
            } while (runningTimeInMilliSeconds < maxExecutionTimeInSeconds * 1000 && count < maxCount);

            var meanTime = elapsedTime / repeats;

            var standardDeviation = Math.Sqrt(elapsedSquaredTime / repeats - meanTime * meanTime) / meanTime * 100;
            caller.UpdateRunningLabel("");
            return new Benchmark(BenchMarkName(), CollectionSize, meanTime, standardDeviation, count);
        }
    }
}