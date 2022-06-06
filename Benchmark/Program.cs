using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<MatrixBenchmark>();
            // BenchmarkRunner.Run<Iterating>();
            BenchmarkRunner.Run<Agg>();
            // new Iterating().ForEach();
            // new Iterating().Iterate();
        }
    }
}
