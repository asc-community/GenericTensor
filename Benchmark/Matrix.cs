using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;
using GenericTensor.Core;

namespace Benchmark
{
    using TS = Tensor<int>;

    public class MatrixBenchmark
    {
        public MatrixBenchmark()
        {
            BuiltinTypeInitter.InitForInt();
        }

        static TS CreateMatrix(int size)
            => TS.CreateMatrix(size, size, (k) => k.x + k.y);

        private static TS createdMatrix = CreateMatrix(6);
        
        [Params(3, 20, 50)] public int MatrixSize;

        [Benchmark] public void CreatingMatrix()
            => CreateMatrix(MatrixSize);

        [Benchmark] public void CreatingMatrixAndTranspose()
            => CreateMatrix(MatrixSize).TransposeMatrix();

        [Benchmark] public void Transpose()
            => createdMatrix.TransposeMatrix();

        [Benchmark] public void MatrixAndLaplace()
            => createdMatrix.DeterminantLaplace();

        [Benchmark] public void MatrixAndGaussian()
            => createdMatrix.DeterminantGaussianSafeDivision();

        [Benchmark] public void MatrixAndMultiply()
            => TS.MatrixDotProduct(createdMatrix, createdMatrix);

        [Benchmark] public void MatrixAndAdd()
            => TS.PiecewiseAdd(createdMatrix, createdMatrix);
    }
}
