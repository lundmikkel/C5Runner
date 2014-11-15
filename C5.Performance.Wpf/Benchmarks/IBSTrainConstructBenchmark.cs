using C5.Intervals;
using C5.UserGuideExamples.Intervals;

namespace C5.Performance.Wpf.Benchmarks
{
    public class IBSTrainConstructBenchmark : Benchmarkable
    {
        private Trains.TrainRide[] _trains;
        private IntervalBinarySearchTree<Trains.TrainRide, double> _intervalTrains;


        private int trainConstruct(int trainId)
        {
            _intervalTrains = new IntervalBinarySearchTree<Trains.TrainRide, double>(_trains);
            return 1;
        }

        public override void CollectionSetup(int collectionSize)
        {
            // Get the number of trains from the csv file matching the collectionsize
            _trains = TrainUtilities.GetTrains(collectionSize);

            C5.Sorting.IntroSort(_trains, 0, collectionSize, IntervalExtensions.CreateComparer<Trains.TrainRide, double>());

            /*
             * Setup an items array with things to look for. Not used in this benchmark.
             */
            ItemsArray = SearchAndSort.FillIntArray(collectionSize);
        }

        public override double Call(int i, int collectionSize)
        {
            return trainConstruct(i);
        }

        public override string BenchMarkName()
        {
            return "IBS Train Construct";
        }
    }
}