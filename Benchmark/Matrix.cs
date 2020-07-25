using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;
using GenericTensor.Core;

namespace Benchmark
{
    using TS = GenTensor<int>;

    public class MatrixBenchmark
    {
        public MatrixBenchmark()
        {
            BuiltinTypeInitter.InitForInt();
        }


        static TS CreateMatrix(int size)
            => TS.CreateMatrix(size, size, (x, y) => x + y);

        private static TS createdMatrix3 = CreateMatrix(3);
        private static TS createdMatrix6 = CreateMatrix(6);
        private static TS createdMatrix9 = CreateMatrix(9);
        private static TS createdMatrix20 = CreateMatrix(20);
        private static TS createdMatrix50 = CreateMatrix(50);

        [Benchmark] public void MatrixAndLaplace3()
            => createdMatrix3.DeterminantLaplace();
        [Benchmark] public void MatrixAndLaplace6()
            => createdMatrix6.DeterminantLaplace();
        [Benchmark] public void MatrixAndLaplace9()
            => createdMatrix9.DeterminantLaplace();

        [Benchmark] public void MatrixAndGaussian3()
            => createdMatrix3.DeterminantGaussianSafeDivision();

        [Benchmark] public void MatrixAndGaussian6()
            => createdMatrix6.DeterminantGaussianSafeDivision();

        [Benchmark] public void MatrixAndGaussian9()
            => createdMatrix9.DeterminantGaussianSafeDivision();

        [Benchmark] public void CreatingMatrix20()
            => CreateMatrix(20);

        [Benchmark] public void CreatingMatrix50()
            => CreateMatrix(50);

        [Benchmark] public void Transpose20()
            => createdMatrix20.TransposeMatrix();

        [Benchmark] public void MatrixAndMultiply6()
            => TS.MatrixMultiply(createdMatrix6, createdMatrix6);

        [Benchmark] public void MatrixAndMultiply20()
            => TS.MatrixMultiply(createdMatrix20, createdMatrix20);

        [Benchmark] public void MatrixAndAdd6()
            => TS.PiecewiseAdd(createdMatrix6, createdMatrix6);

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

        [Benchmark] public void FastIndexing()
        {
            for (int i = 0; i < createdMatrix9.Shape[0]; i++)
            for (int j = 0; j < createdMatrix9.Shape[1]; j++)
            {
                var c = createdMatrix9.GetValueNoCheck(i, j);
            }
        }
    }
}
