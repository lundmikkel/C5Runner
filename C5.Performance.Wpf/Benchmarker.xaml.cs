using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using C5.Intervals;
using C5.Intervals.Performance;
using C5.Intervals.Tests;
using C5.Performance.Wpf.Benchmarks;
using C5.Performance.Wpf.IIntervalCollection_Benchmarks;
using C5.Performance.Wpf.Report_Benchmarks;
using Microsoft.Win32;

namespace C5.Performance.Wpf
{
    // Tool for running and plotting benchmarks that are of type Benchmarkable.
    public partial class Benchmarker
    {
        #region Benchmark setup
        // Parameters for running the benchmarks
        private const int MinCollectionSize = 100;
        private const int MaxCollectionSize = 1000001; //TrainUtilities.TrainSetACount;
        private const int CollectionMultiplier = 10;
        private const int StandardRepeats = 10;
        private const double MaxExecutionTimeInSeconds = 0.25;
        private readonly Plotter _plotter;
        internal int MaxIterations;
        // Every time we benchmark we count this up in order to get a new color for every benchmark
        private int _lineSeriesIndex;
        private const int OriginalMax = Int32.MaxValue / 10;
        private static int _maxCount = OriginalMax;
        private int _repeats = 1;
        private bool _runSequential;
        private bool _runWarmups = false;

        private static readonly Func<int, IInterval<int>[]> A = BenchmarkTestCases.DataSetA;
        private static readonly Func<int, IInterval<int>[]> B = BenchmarkTestCases.DataSetB;
        private static readonly Func<int, IInterval<int>[]> C = BenchmarkTestCases.DataSetC;
        private static readonly Func<int, IInterval<int>[]> D = BenchmarkTestCases.DataSetD;

        private static readonly Func<int, IInterval<int>[]> NoOverlaps = IntervalsFactory.NoOverlaps;
        private static readonly Func<int, IInterval<int>[]> Meets = IntervalsFactory.Meets;
        private static readonly Func<int, IInterval<int>[]> Overlaps = IntervalsFactory.Overlaps;
        private static readonly Func<int, IInterval<int>[]> Containments = IntervalsFactory.PineTreeForest;

        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> FIXED = IntervalBenchmarkable.IBS;

        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> DIT = IntervalBenchmarkable.DIT;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> IBS = IntervalBenchmarkable.IBS;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> IBSOLD = IntervalBenchmarkable.IBSOLD;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> LCList = IntervalBenchmarkable.LCList;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> NCList = IntervalBenchmarkable.NCList;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> NCList2 = IntervalBenchmarkable.NCList2;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> NCListArticle = IntervalBenchmarkable.NCListArticle;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> SIT = IntervalBenchmarkable.SIT;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> BITS = IntervalBenchmarkable.BITS;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> DLFIT = IntervalBenchmarkable.DLFIT;
        private static readonly Func<IInterval<int>[], IIntervalCollection<IInterval<int>, int>> SFIL = IntervalBenchmarkable.SFIL;

        //private static readonly Action<DateTime[]> c5timsorter = C5.Sorting.Timsort;
        //private static readonly Action<DateTime[]> introsorter = C5.Sorting.IntroSort;
        //private static readonly Action<DateTime[]> arraysorter = Array.Sort;
        //private static readonly Action<DateTime[]> timsorter = TimSortExtender.TimSort;

        // These are the benchmarks that will be run by the benchmarker.
        private static Benchmarkable[] Benchmarks
        {

            get
            {
                return new Benchmarkable[]
                {
                    // Query Range
                    new QueryRange(A, LCList), 
                    new QueryRange(B, LCList), 
                    new QueryRange(C, LCList), 
                    new QueryRange(D, LCList),

                    // Query Range
                    new QueryRange(A, NCList), 
                    new QueryRange(B, NCList), 
                    new QueryRange(C, NCList), 
                    new QueryRange(D, NCList),

                    //new GapsBenchmarkable_GapsLast(NoOverlaps, DIT), 
                    //new GapsBenchmarkable_GapsLast(NoOverlaps, IBS), 
                    //new GapsBenchmarkable_GapsLast(NoOverlaps, LCList), 
                    //new GapsBenchmarkable_GapsLast(NoOverlaps, NCList), 
                    //new GapsBenchmarkable_GapsLast(NoOverlaps, SIT), 
                    //new GapsBenchmarkable_GapsLast(NoOverlaps, BITS), 
                    //new GapsBenchmarkable_GapsLast(NoOverlaps, DLFIT), 
                    //new GapsBenchmarkable_GapsLast(NoOverlaps, SFIL),


                    //new EnumerableBenchmarkable_GetEnumerator(Overlaps, DIT), 
                    //new EnumerableBenchmarkable_GetEnumerator(Overlaps, IBS), 
                    //new EnumerableBenchmarkable_GetEnumerator(Overlaps, LCList),
                    //new EnumerableBenchmarkable_GetEnumerator(Overlaps, NCList2),
                    //new EnumerableBenchmarkable_GetEnumerator(Overlaps, NCListArticle),
                    //new EnumerableBenchmarkable_GetEnumerator(Overlaps, SIT), 
                    //new EnumerableBenchmarkable_GetEnumerator(Overlaps, BITS),
                    

                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, DIT), 
                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, IBS), 
                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, LCList), 
                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, NCList), 
                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, SIT), 
                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, BITS), 
                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, DLFIT), 
                    //new CountOverlapsBenchmarkable_CountOverlapsRange(NoOverlaps, SFIL),
                    

                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, DIT), 
                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, IBS), 
                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, LCList), 
                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, NCList), 
                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, SIT), 
                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, BITS), 
                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, DLFIT), 
                    //new CountOverlapsBenchmarkable_CountOverlapsStabbing(Overlaps, SFIL),
                    

                    //new FindOverlapBenchmarkable_FindOverlapStabbing(NoOverlaps, DIT),
                    //new FindOverlapBenchmarkable_FindOverlapStabbing(NoOverlaps, SFIL),
                    //new FindOverlapBenchmarkable_FindOverlapStabbing(NoOverlaps, IBS),
                    //new FindOverlapBenchmarkable_FindOverlapStabbing(NoOverlaps, LCList),
                    //new FindOverlapBenchmarkable_FindOverlapStabbing(NoOverlaps, NCList),
                    //new FindOverlapBenchmarkable_FindOverlapStabbing(NoOverlaps, SIT),
                    //new FindOverlapBenchmarkable_FindOverlapStabbing(NoOverlaps, DLFIT),


                    /*
                    //new DateTimeSortingSorted(c5timsorter, "tim"), 
                    //new DateTimeSortingSorted(introsorter, "intro"),
                    //new DateTimeSortingSorted(arraysorter, "array"),

                    //new DateTimeSortingReverseSorted(c5timsorter, "tim"), 
                    //new DateTimeSortingReverseSorted(introsorter, "intro"),
                    //new DateTimeSortingReverseSorted(arraysorter, "array"),
                    /*/
                    //new DateTimeSortingRandom(c5timsorter, "c5 tim"),
                    //new DateTimeSortingRandom(introsorter, "intro"),
                    //new DateTimeSortingRandom(arraysorter, "array"),
                    //new DateTimeSortingRandom(timsorter, "tim"),
                    //*/ 

                    //new AddSorted(Containments, FIXED),
                    //new ConstructSorted(Containments, FIXED),

                    //new AddRandom(Overlaps, FIXED),
                    //new ConstructRandom(Overlaps, FIXED),

                    //new ConstructSortedReverse(NoOverlaps, FIXED), 
                    //new AddSortedReverse(NoOverlaps, FIXED),

                    //new ConstructRandom(NoOverlaps, FIXED), 
                    //new AddRandom(NoOverlaps, FIXED),

                    //new ConstructSorted(NoOverlaps, IBS), 
                    //new ConstructSorted(NoOverlaps, DIT), 

                    //new Enumerate(NoOverlaps, DIT), 
                    //new Enumerate(NoOverlaps, IBS),
                    //new Enumerate(NoOverlaps, IBSOLD),
                    //new Enumerate(NoOverlaps, DLFIT),
                    //new Enumerate(NoOverlaps, SFIL),

                    //new AddSorted(NoOverlaps, DIT), 
                    //new AddSorted(NoOverlaps, IBS),
                    //new AddSorted(NoOverlaps, IBSOLD),
                    //new AddSorted(NoOverlaps, DLFIT),

                    //new AddSortedReverse(NoOverlaps, DIT), 
                    //new AddSortedReverse(NoOverlaps, IBS),
                    //new AddSortedReverse(NoOverlaps, IBSOLD),
                    //new AddSortedReverse(NoOverlaps, DLFIT),

                    //new AddRandom(NoOverlaps, DIT), 
                    //new AddRandom(NoOverlaps, IBS),
                    //new AddRandom(NoOverlaps, IBSOLD),
                    //new AddRandom(NoOverlaps, DLFIT),

                    /*
                    // Enumerate
                    new Enumerate(A,DIT),
                    new Enumerate(B,DIT),
                    new Enumerate(C,DIT),
                    new Enumerate(D,DIT),

                    new Enumerate(A,IBS),
                    new Enumerate(B,IBS),
                    new Enumerate(C,IBS),
                    new Enumerate(D,IBS),

                    // DateTimeSorting Add All In DateTimeSorting
                    new DateTimeSorting(A,DIT), 
                    new DateTimeSorting(B,DIT), 
                    new DateTimeSorting(C,DIT), 
                    new DateTimeSorting(D,DIT), 

                    new DateTimeSorting(A,IBS), 
                    new DateTimeSorting(B,IBS), 
                    new DateTimeSorting(C,IBS), 
                    new DateTimeSorting(D,IBS), 

                    // DateTimeSorting Add Sorted
                    new AddSorted(A, DIT), 
                    new AddSorted(B, DIT), 
                    new AddSorted(C, DIT),
                    new AddSorted(D, DIT), 

                    new AddSorted(A, IBS), 
                    new AddSorted(B, IBS), 
                    new AddSorted(C, IBS), 
                    new AddSorted(D, IBS), 

                    // DateTimeSorting Add Unsorted
                    new ConstructAddUnsorted(A, DIT), 
                    new ConstructAddUnsorted(B, DIT), 
                    new ConstructAddUnsorted(C, DIT), 
                    new ConstructAddUnsorted(D, DIT), 
                    
                    new ConstructAddUnsorted(A, IBS), 
                    new ConstructAddUnsorted(B, IBS), 
                    new ConstructAddUnsorted(C, IBS), 
                    new ConstructAddUnsorted(D, IBS), 

                    //   Query Stabbing
                    new QueryStabbing(A, DIT), 
                    new QueryStabbing(B, DIT), 
                    new QueryStabbing(C, DIT), 
                    new QueryStabbing(D, DIT), 

                    new QueryStabbing(A, IBS), 
                    new QueryStabbing(B, IBS), 
                    new QueryStabbing(C, IBS), 
                    new QueryStabbing(D, IBS), 

                    // Query Range
                    new QueryRange(A, DIT), 
                    new QueryRange(B, DIT), 
                    new QueryRange(C, DIT), 
                    new QueryRange(D, DIT), 

                    new QueryRange(A, IBS), 
                    new QueryRange(B, IBS), 
                    new QueryRange(C, IBS), 
                    new QueryRange(D, IBS), 

                    // Query Range Span
                    new QueryRangeSpan(A, DIT), 
                    new QueryRangeSpan(B, DIT), 
                    new QueryRangeSpan(C, DIT), 
                    new QueryRangeSpan(D, DIT), 

                    new QueryRangeSpan(A, IBS), 
                    new QueryRangeSpan(B, IBS), 
                    new QueryRangeSpan(C, IBS), 
                    new QueryRangeSpan(D, IBS), 
                    */
                };
            }
        }
        #endregion

        #region DateTimeSorting
        public Benchmarker()
        {
            MaxIterations = Convert.ToInt32(Math.Round(Math.Log(MaxCollectionSize)));
            _plotter = new Plotter();
            DataContext = _plotter;
            Console.Out.WriteLine();
        }
        #endregion

        #region Benchmark Running

        private Boolean SerializeToDisk = false;
        private Boolean RunFromDisk = false;
        // Method that gets called when the benchmark button is used.
        private void benchmarkStart(object sender, RoutedEventArgs e)
        {
            runSequentialCheckBox.IsEnabled = false;

            if (RunFromDisk)
                redBenchmarksFromDisk(Benchmarks); // Is only reliable if you have serialized a sequential run
            else
            {
                // This benchmark is the one we use to compare with Sestoft's cmd line version of the tool
                var thread = _runSequential || SerializeToDisk
                    ? new Thread(() => runBenchmarks(Benchmarks))
                    : new Thread(() => runBenchmarksParallel(Benchmarks));
                //CheckBox checkbox = (CheckBox)this.Controls.Find("checkBox" + input.toString())[0];
                thread.Start();
            }
        }

        // Sequential run of all the benchmarks.
        private void runBenchmarks(params Benchmarkable[] benchmarks)
        {
            //runSequential;
            foreach (var b in benchmarks)
            {
                _plotter.AddAreaSeries(b.BenchMarkName());
                for (b.CollectionSize = MinCollectionSize;
                    b.CollectionSize < MaxCollectionSize;
                    b.CollectionSize *= CollectionMultiplier)
                {
                    updateStatusLabel("Running " + b.BenchMarkName() + " with collection size " + b.CollectionSize);
                    var benchmark = b.Benchmark(_maxCount, _repeats, MaxExecutionTimeInSeconds, this, _runWarmups);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        _plotter.AddDataPoint(_lineSeriesIndex, benchmark, SerializeToDisk)));
                    Thread.Sleep(100);
                    updateProgressBar(benchmarks.Length);
                }
                _lineSeriesIndex++;
            }
            UpdateRunningLabel("");
            updateStatusLabel("Finished");
            Thread.Sleep(1000);
            updateStatusLabel("");
        }

        // Sequential run from disk
        private void redBenchmarksFromDisk(params Benchmarkable[] benchmarks)
        {
            var benchmarkCounter = 0;
            //runSequential;
            foreach (var b in benchmarks)
            {
                _plotter.AddAreaSeries(b.BenchMarkName());
                for (b.CollectionSize = MinCollectionSize;
                    b.CollectionSize < MaxCollectionSize;
                    b.CollectionSize *= CollectionMultiplier)
                {
                    updateStatusLabel("Running " + b.BenchMarkName() + " with collection size " + b.CollectionSize);
                    var benchmark = _plotter.ReadBenchmarkFromDisk(benchmarkCounter++);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        _plotter.AddDataPoint(_lineSeriesIndex, benchmark, false)));
                    Thread.Sleep(100);
                    updateProgressBar(benchmarks.Length);
                }
                _lineSeriesIndex++;
            }
            UpdateRunningLabel("");
            updateStatusLabel("Finished");
        }

        // "Parallel" run of all the benchmarks. Each benchmarkable will get 1 run after another. Making it easier to compare benchmarks as they run.
        private void runBenchmarksParallel(params Benchmarkable[] benchmarks)
        {
            foreach (var benchmarkable in benchmarks)
                _plotter.AddAreaSeries(benchmarkable.BenchMarkName());
            var collectionSize = MinCollectionSize;
            while (collectionSize < MaxCollectionSize)
            {
                _lineSeriesIndex = 0;
                foreach (var b in benchmarks)
                {
                    b.CollectionSize = collectionSize;
                    updateStatusLabel("Running " + b.BenchMarkName() + " with collection size " + collectionSize);
                    var benchmark = b.Benchmark(_maxCount, _repeats, MaxExecutionTimeInSeconds, this, _runWarmups);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        _plotter.AddDataPoint(_lineSeriesIndex, benchmark, false)));
                    Thread.Sleep(100);
                    _lineSeriesIndex++;
                    updateProgressBar(benchmarks.Length);
                }
                collectionSize *= CollectionMultiplier;
            }
            UpdateRunningLabel("");
            updateStatusLabel("Finished");
            Thread.Sleep(1000);
        }
        #endregion

        #region Util
        private void savePdf(object sender, RoutedEventArgs routedEventArgs)
        {
            var dlg = new SaveFileDialog
            {
                FileName = Benchmarks[0].BenchMarkName(),
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            // Show save file dialog box
            var result = dlg.ShowDialog();
            if (result != true) return;

            // Save document
            var path = dlg.FileName;
            _plotter.ExportPdf(path, ActualWidth, ActualHeight);
        }
        #endregion

        #region UI Utils
        private void updateProgressBar(int numberOfBenchmarks)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => progress.Value += (100.0 / MaxIterations) / numberOfBenchmarks));
        }

        private void updateStatusLabel(String s)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => StatusLabel.Content = s));
        }

        public void UpdateRunningLabel(String s)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => RunningLabel.Content = s));
        }

        private void CheckBox_Checked_RunWarmups(object sender, RoutedEventArgs e)
        {
            _runWarmups = true;
        }

        private void CheckBox_Unchecked_RunWarmups(object sender, RoutedEventArgs e)
        {
            _runWarmups = false;
        }

        private void CheckBox_Checked_RunQuick(object sender, RoutedEventArgs e)
        {
            _repeats = 1;
            _maxCount = OriginalMax / 100;
        }

        private void CheckBox_Unchecked_RunQuick(object sender, RoutedEventArgs e)
        {
            _repeats = StandardRepeats;
            _maxCount = OriginalMax;
        }

        private void CheckBox_Checked_LogarithmicXAxis(object sender, RoutedEventArgs e)
        {
            _plotter.ToggleLogarithmicAxis(true);
        }

        private void CheckBox_Unchecked_LogarithmicXAxis(object sender, RoutedEventArgs e)
        {
            _plotter.ToggleLogarithmicAxis(false);
        }

        private void CheckBox_Checked_RunSequential(object sender, RoutedEventArgs e)
        {
            _runSequential = true;
        }

        private void CheckBox_Unchecked_RunSequential(object sender, RoutedEventArgs e)
        {
            _runSequential = false;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
        #endregion
    }
}