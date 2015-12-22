using System;
using System.Linq;
using C5.Intervals;
using C5.Intervals.Performance;

namespace C5.IntervalHotSpots
{
    class Program
    {
        static void Main(string[] args)
        {
            //var random = new Random();
            //var intervals = IntervalsFactory.CreateIntervalStream(i => random.Next(1, 5), i => random.Next(-5, 5)).Take(100000).ToArray();
            //
            //const int repeats = 250;
            //var collection = new IntervalBinarySearchTree<IInterval<int>, int>(intervals);
            //var dummy = 0;
            //
            //for (var i = 0; i < repeats; i++)
            //    dummy += collection.Count();

            //Console.Out.WriteLine(dummy);


            var intervals = IntervalsFactory.ContainmentListIntervals(500000, 1000000000).ToArray();

            const int repeats = 25;
            var dummy = 0;

            for (var i = 0; i < repeats; i++)
                dummy += new LayeredContainmentList<IInterval<int>, int>(intervals).Count;

            Console.Out.WriteLine(dummy);
        }
    }
}
