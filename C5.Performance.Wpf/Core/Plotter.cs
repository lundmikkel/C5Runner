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
        private const string Path = "benchmarks/benchmark";

        #endregion

        #region Constructors

        public Plotter()
        {
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
        public void AddDataPoint(int indexOfAreaSeries, Benchmark benchmark, bool serialize)
        {
            if (serialize)
                writeBenchmarkToDisk(benchmark);
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

        private void writeBenchmarkToDisk(Benchmark benchmark)
        {
            _serializer.Serialize(File.CreateText(Path + (_benchmarkCounter++) + ".xml"), benchmark);
        }

        public Benchmark ReadBenchmarkFromDisk(int number)
        {
            return (Benchmark) _serializer.Deserialize(File.OpenText(Path + number + ".xml"));
        }

        #endregion
    }
}