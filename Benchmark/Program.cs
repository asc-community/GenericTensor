using BenchmarkDotNet.Running;
using GenericTensor.Functions;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BuiltinTypeInitter.InitForInt();
            BenchmarkRunner.Run<MatrixBenchmark>();
        }
    }
}
