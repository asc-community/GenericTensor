using System;
using System.Linq;
using BenchmarkDotNet.Running;
using GenericTensor.Core;
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
