using System;
using BenchmarkDotNet.Running;
using GenericTensor.Core;
using GenericTensor.Functions;

namespace Benchmark
{
    using TS = Tensor<TensorIntWrapper, int>;
    class Program
    {
        static TS CreateMatrix(int size)
            => TS.CreateMatrix(size, size, (k) =>
                {
                    var res = new TensorIntWrapper();
                    res.SetValue(k.x + k.y);
                    return res;
                }
            );

        static void Main(string[] args)
        {
            //CreateMatrix(5).DeterminantGaussianSafeDivision();
            BenchmarkRunner.Run<MatrixBenchmark>();
        }
    }
}
