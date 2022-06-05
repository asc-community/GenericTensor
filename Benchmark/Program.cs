using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<MatrixBenchmark>();
            BenchmarkRunner.Run<Iterating>();
            // new Iterating().ForEach();
            // new Iterating().Iterate();
        }
    }
}
