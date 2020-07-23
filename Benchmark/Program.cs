using System;
using BenchmarkDotNet.Running;
using GenericTensor.Core;
using GenericTensor.Functions;

namespace Benchmark
{
    using TS = Tensor<int>;
    class Program
    {

        static void Main(string[] args)
        {
            //CreateMatrix(5).DeterminantGaussianSafeDivision();
            BenchmarkRunner.Run<MatrixBenchmark>();
        }
    }
}
