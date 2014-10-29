namespace C5.Performance
{
    public struct Benchmark
    {
        public string BenchmarkName { get; set; }
        public int CollectionSize { get; set; }
        public int NumberOfRuns { get; set; }
        public double MeanTime { get; set; }
        public double StandardDeviation { get; set; }
        public double Dummy { get; set; }
    }
}