using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;
using GenericTensor.Core;

namespace Benchmark
{
    using TS = Tensor<TensorIntWrapper, int>;

    public class MatrixBenchmark
    {
        static TS CreateMatrix(int size)
            => TS.CreateMatrix(size, size, (k) =>
                {
                    var res = new TensorIntWrapper();
                    res.SetValue(k.x + k.y);
                    return res;
                }
        );

        [Params(3, 20, 50)] public int MatrixSize;

        [Benchmark] public void CreatingMatrix()
            => CreateMatrix(MatrixSize);

        [Benchmark] public void CreatingMatrixAndTranspose()
            => CreateMatrix(MatrixSize).TransposeMatrix();

        [Params(2, 4, 6)] public int LowMatrixSize;

        [Benchmark] public void CreatingMatrixAndLaplace()
            => CreateMatrix(LowMatrixSize).DeterminantLaplace();

        [Benchmark] public void CreatingMatrixAndGaussian()
            => CreateMatrix(LowMatrixSize).DeterminantGaussianSafeDivision();

        [Benchmark] public void CreatingMatrixAndMultiply()
            => TS.MatrixDotProduct(CreateMatrix(MatrixSize), CreateMatrix(MatrixSize));

        [Benchmark] public void CreatingMatrixAndAdd()
            => TS.PiecewiseAdd(CreateMatrix(MatrixSize), CreateMatrix(MatrixSize));
    }
}
