using System;
using BenchmarkDotNet.Running;
using GenericTensor.Core;
using GenericTensor.Functions;

namespace Benchmark
{
    using TS = Tensor<int>;
    class Program
    {
        static TS CreateMatrix(int size)
            => TS.CreateMatrix(size, size, (x, y) => x + y);
        static void Main(string[] args)
        {
            BuiltinTypeInitter.InitForInt();
            //CreateMatrix(5).DeterminantGaussianSafeDivision()
            BenchmarkRunner.Run<MatrixBenchmark>();
            //TS.MatrixDotProduct(CreateMatrix(3), CreateMatrix(3));
            /*
            var mat = CreateMatrix(6);
            mat.DeterminantGaussianSafeDivision();
            mat.DeterminantGaussianSafeDivision();
            TS.MatrixDotProduct(mat, mat);
            TS.PiecewiseAdd(mat, mat);
            */
        }
    }
}
