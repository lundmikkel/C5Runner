using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using C5.Performance.Wpf.Benchmarks;
using Microsoft.Win32;

namespace C5.Performance.Wpf
{
    // Tool for running and plotting benchmarks that are of type Benchmarkable.
    public partial class Benchmarker
    {
        #region Fields
        private const int StandardRepeats = 10;
        private int _repeats = 1000;
        // Parameters for running the benchmarks
        private const double MaxExecutionTimeInSeconds = 0.25;
        private readonly Plotter _plotter;
        // Every time we benchmark we count this up in order to get a new color for every benchmark
        private int _lineSeriesIndex;
        private const int OriginalMax = int.MaxValue / 10;
        private static int _maxCount = OriginalMax;
        private bool _runSequential;
        private bool _runWarmups;

        private string basepath = "benchmark";
        private string testName;
        private string directory;

        #endregion

        private static Benchmarkable[] Benchmarks { get; set; }

        #region Constructor
        public Benchmarker()
        {
            directory = Path.Combine(basepath, testName = DateTime.Now.Ticks.ToString());

            _plotter = new Plotter(directory);
            DataContext = _plotter;
            Console.Out.WriteLine();
            Benchmarks = Wpf.Benchmarks.Benchmarks.List;
        }
        #endregion

        #region Benchmark Running

        private Boolean SerializeToDisk = false;
        private Boolean WriteToDisk = true;
        private Boolean RunFromDisk = false;
        // Method that gets called when the benchmark button is used.
        private void benchmarkStart(object sender, RoutedEventArgs e)
        {
            runSequentialCheckBox.IsEnabled = false;

            if (RunFromDisk)
                readBenchmarksFromDisk(Benchmarks); // Is only reliable if you have serialized a sequential run
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
        private void runBenchmarks(Benchmarkable[] benchmarks)
        {
            //runSequential;
            foreach (var b in benchmarks)
            {
                _plotter.AddAreaSeries(b.BenchMarkName());
                foreach (var collectionSize in b.CollectionSizes())
                {
                    updateStatusLabel("Running " + b.BenchMarkName() + " with collection size " + collectionSize);
                    var benchmark = b.Benchmark(_maxCount, _repeats, collectionSize, MaxExecutionTimeInSeconds, updateStatusLabel, runWarmup: _runWarmups);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => _plotter.AddDataPoint(_lineSeriesIndex, benchmark, SerializeToDisk, WriteToDisk)));
                    Thread.Sleep(100);
                    updateProgressBar(benchmarks.Length);
                }
                _lineSeriesIndex++;
            }
            UpdateRunningLabel();
            updateStatusLabel("Finished");
            Thread.Sleep(1000);
            updateStatusLabel("");
        }

        // Sequential run from disk
        private void readBenchmarksFromDisk(Benchmarkable[] benchmarks)
        {
            var benchmarkCounter = 0;
            //runSequential;
            foreach (var b in benchmarks)
            {
                _plotter.AddAreaSeries(b.BenchMarkName());
                foreach (var collectionSize in b.CollectionSizes())
                {
                    updateStatusLabel("Running " + b.BenchMarkName() + " with collection size " + collectionSize);
                    var benchmark = _plotter.ReadBenchmarkFromDisk(benchmarkCounter++);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => _plotter.AddDataPoint(_lineSeriesIndex, benchmark, false, WriteToDisk)));
                    Thread.Sleep(100);
                    updateProgressBar(benchmarks.Length);
                }
                _lineSeriesIndex++;
            }
            UpdateRunningLabel();
            updateStatusLabel("Finished");
        }

        // "Parallel" run of all the benchmarks. Each benchmarkable will get 1 run after another. Making it easier to compare benchmarks as they run.
        private void runBenchmarksParallel(Benchmarkable[] benchmarks)
        {
            foreach (var benchmarkable in benchmarks)
                _plotter.AddAreaSeries(benchmarkable.BenchMarkName());

            foreach (var collectionSize in benchmarks.First().CollectionSizes())
            {
                _lineSeriesIndex = 0;
                foreach (var b in benchmarks)
                {
                    updateStatusLabel("Running " + b.BenchMarkName() + " with collection size " + collectionSize);
                    var benchmark = b.Benchmark(_maxCount, _repeats, collectionSize, MaxExecutionTimeInSeconds, updateStatusLabel, _runWarmups);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => _plotter.AddDataPoint(_lineSeriesIndex, benchmark, false, WriteToDisk)));
                    Thread.Sleep(100);
                    _lineSeriesIndex++;
                    updateProgressBar(benchmarks.Length);
                }
            }
            UpdateRunningLabel();
            addPdfToBenchmarks();
            updateStatusLabel("Finished");
            Thread.Sleep(1000);
        }
        #endregion

        #region Util
        private void savePdf(object sender, RoutedEventArgs routedEventArgs)
        {
            var dlg = new SaveFileDialog
            {
                FileName = Benchmarks.First().BenchMarkName() + " " + DateTime.Now.Ticks,
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            // Show save file dialog box
            var result = dlg.ShowDialog();
            if (result == false) return;

            // Save document
            _plotter.ExportPdf(dlg.FileName, ActualWidth, ActualHeight);
        }

        private void addPdfToBenchmarks()
        {
            var filename = Path.Combine(directory, testName + ".pdf");
            _plotter.ExportPdf(filename, 1456, 916);
        }

        #endregion

        #region UI Utils

        private void updateProgressBar(int numberOfBenchmarks)
        {
            Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action(() => progress.Value += (100.0 / 100) / numberOfBenchmarks)
            );
        }

        private void updateStatusLabel(String s)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => StatusLabel.Content = s));
        }

        public void UpdateRunningLabel(String s = "")
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