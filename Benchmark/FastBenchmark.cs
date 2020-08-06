using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using BenchmarkDotNet.Attributes;
using GenericTensor.Core;
using GenericTensor.Functions;

namespace Benchmark
{
    using TS = GenTensor<float, FloatWrapper>;
    
    public class FastBenchmark
    {
        static TS CreateMatrix(int size)
            => TS.CreateMatrix(size, size, (x, y) => (x + y));

        private static readonly TS createdMatrix6 = CreateMatrix(6);
        private static readonly TS createdMatrix9 = CreateMatrix(9);
        private static readonly TS createdMatrix20 = CreateMatrix(20);

        [Benchmark] public void MatrixAndGaussian6()
            => createdMatrix6.DeterminantGaussianSafeDivision();

        [Benchmark] public void CreatingMatrix20()
            => CreateMatrix(20);

        [Benchmark] public void MatrixAndMultiply20()
            => TS.MatrixMultiply(createdMatrix20, createdMatrix20);

        [Benchmark] public void MatrixAndAdd20()
            => TS.PiecewiseAdd(createdMatrix20, createdMatrix20);

        [Benchmark] public void SafeIndexing()
        {
            for (int i = 0; i < createdMatrix9.Shape[0]; i++)
            for (int j = 0; j < createdMatrix9.Shape[1]; j++)
            {
                var c = createdMatrix9[i, j];
            }
        }
    }
}
