using System;
using System.IO;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Xml.Serialization;

namespace C5.Performance.Wpf
{
    public class Plotter
    {
        #region Fields

        public PlotModel PlotModel { get; set; }
        private int _benchmarkCounter;
        readonly XmlSerializer _serializer = new XmlSerializer(typeof(Benchmark));
        private string directory;

        #endregion

        #region Constructors

        public Plotter(string directory)
        {
            this.directory = directory;
            PlotModel = new PlotModel();
            setUpModel();
        }

        #endregion

        /// <summary>
        /// Prepare the plotter
        /// </summary>
        private void setUpModel(bool logarithmicXAxis = false)
        {
            //PlotModel.Title = "Interval Plotter";
            //PlotModel.LegendTitle = "Legend";
            PlotModel.LegendPosition = LegendPosition.TopLeft;
            PlotModel.LegendBackground = OxyColors.White;
            PlotModel.LegendBorder = OxyColors.Black;
            PlotModel.LegendPlacement = LegendPlacement.Inside;

            var sizeAxis = logarithmicXAxis ? (Axis) new LogarithmicAxis(AxisPosition.Bottom) : new LinearAxis(AxisPosition.Bottom);
            sizeAxis.AxisTitleDistance = 10;
            sizeAxis.Title = "Collection Size";
            sizeAxis.MajorGridlineStyle = LineStyle.Solid;
            sizeAxis.MinorGridlineStyle = LineStyle.Dot;

            // Comment in the line with the axis you want
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                AxisTitleDistance = 10,
                Title = "Execution Time in milliseconds",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };

            sizeAxis.AbsoluteMinimum = 0;
            valueAxis.AbsoluteMinimum = 0;

            PlotModel.Axes.Add(sizeAxis);
            PlotModel.Axes.Add(valueAxis);
        }

        public void ToggleLogarithmicAxis(bool logarithmicXAxis)
        {
            PlotModel.Axes.Clear();
            setUpModel(logarithmicXAxis);
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Export the plot as a pdf file
        /// </summary>
        /// <param name="path">The file path where the pdf should be created</param>
        /// <param name="width">Width in pixels of the generated pfd</param>
        /// <param name="height">Height in pixels of the generated pfd</param>
        public void ExportPdf(String path, double width, double height)
        {
            PdfExporter.Export(PlotModel, new StreamWriter(path).BaseStream, width, height);
        }

        /// <summary>
        /// Add a benchmark data point to the graph being drawn
        /// </summary>
        /// <param name="indexOfAreaSeries">Index of the graph you wish to add data to</param>
        /// <param name="benchmark">Benchmark containing the data to be added</param>
        /// <param name="serialize"></param>
        /// <param name="writeToFile"></param>
        public void AddDataPoint(int indexOfAreaSeries, Benchmark benchmark, bool serialize, bool writeToFile)
        {
            if (serialize)
                serializeBenchmark(benchmark);
            if (writeToFile)
                writeBenchmarkToFile(benchmark);
            var areaSeries = PlotModel.Series[indexOfAreaSeries] as AreaSeries;
            if (areaSeries != null)
            {
                Console.WriteLine(@"Mean: {0} std.dev.: {1}", benchmark.MeanTime, benchmark.StandardDeviation);
                areaSeries.Points.Add(new DataPoint(benchmark.CollectionSize, (benchmark.MeanTime + benchmark.StandardDeviation)));
                areaSeries.Points2.Add(new DataPoint(benchmark.CollectionSize, (benchmark.MeanTime - benchmark.StandardDeviation)));
            }

            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Add a plot to the graph showing the benchmark you are running
        /// </summary>
        /// <param name="name">Name of the benchmark you wish to plot</param>
        public void AddAreaSeries(String name)
        {
            PlotModel.Series.Add(
                new AreaSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Black,
                    MarkerType = MarkerType.Circle,
                    Title = name
                }
            );
        }

        #region Serializer

        public void WriteDataToDisk(int indexOfAreaSeries, Benchmark benchmark)
        {

            var areaSeries = PlotModel.Series[indexOfAreaSeries] as AreaSeries;
            if (areaSeries != null)
            {
                areaSeries.Points.Add(new DataPoint(benchmark.CollectionSize,
                    (benchmark.MeanTime + benchmark.StandardDeviation)));
                areaSeries.Points2.Add(new DataPoint(benchmark.CollectionSize,
                    (benchmark.MeanTime - benchmark.StandardDeviation)));
            }
        }

        private void serializeBenchmark(Benchmark benchmark)
        {
            _serializer.Serialize(File.CreateText(directory + "/benchmark_" + (_benchmarkCounter++) + ".xml"), benchmark);
        }

        private void writeBenchmarkToFile(Benchmark benchmark)
        {
            if (!File.Exists(directory))
                Directory.CreateDirectory(directory);

            var filename = benchmark.BenchmarkName.Replace(" ", String.Empty) + ".dat";

            using (var w = File.AppendText(Path.Combine(directory, filename)))
            {
                w.WriteLine("{0}\t{1}\t{2}", benchmark.CollectionSize, benchmark.MeanTime, benchmark.StandardDeviation);
            }


            var meanFilename = benchmark.BenchmarkName.Replace(" ", String.Empty) + ".mean.txt";

            using (var w = File.AppendText(Path.Combine(directory, meanFilename)))
            {
                w.WriteLine(benchmark.MeanTime);
            }
        }

        public Benchmark ReadBenchmarkFromDisk(int number)
        {
            return (Benchmark) _serializer.Deserialize(File.OpenText(directory + number + ".xml"));
        }

        #endregion
    }
}