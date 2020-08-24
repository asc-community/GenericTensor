using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using GenericTensor.Functions;
using GenericTensor.Core;

namespace Benchmark
{
    using TS = GenTensor<Vector<double>, VectorDoubleWrapper>;
    public class MatrixBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            
        }

        static TS CreateMatrix(int size)
            => TS.CreateMatrix(size, size, (x, y) => new Vector<double>(x + y));

        static TS CreateTensor(int W, int size)
            => TS.Stack(Enumerable.Range(0, W).Select(c => CreateMatrix(size)).ToArray());

        private static readonly TS createdMatrix3 = CreateMatrix(3);
        private static readonly TS createdMatrix4x4 = CreateMatrix(4);
        private static readonly TS createdMatrix4x4_dupl = CreateMatrix(4);
        private static readonly TS createdMatrix6 = CreateMatrix(6);
        private static readonly TS createdMatrix9 = CreateMatrix(9);
        private static readonly TS createdMatrix20 = CreateMatrix(20);
        private static readonly TS createdMatrix20_dupl = CreateMatrix(20);
        private static readonly TS createdTensorMatrix15 = CreateTensor(40, 15);
        private static readonly TS createdMatrix100 = CreateMatrix(100);
        private static readonly TS createdMatrix100_dupl = CreateMatrix(100);
        
        private static readonly TS createdMatrix250 = CreateMatrix(250);
        private static readonly TS createdMatrix250_dupl = CreateMatrix(250);
        
        /*
        [Benchmark] public void MatrixAndAdd250()
            => TS.PiecewiseAdd(createdMatrix250, createdMatrix250_dupl);

        [Benchmark] public void MatrixAndAdd250Par()
            => TS.PiecewiseAdd(createdMatrix250, createdMatrix250_dupl, Threading.Multi);
            */

        
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
            => TS.MatrixMultiply(createdMatrix20, createdMatrix20_dupl);
        
        [Benchmark] public void TensorAndMultiply15()
            => TS.TensorMatrixMultiply(createdTensorMatrix15, createdTensorMatrix15);

        [Benchmark] public void MatrixAndMultiply6Parallel()
            => TS.MatrixMultiply(createdMatrix20, createdMatrix20_dupl, Threading.Multi);

        [Benchmark] public void MatrixAndMultiply20Parallel()
            => TS.MatrixMultiply(createdMatrix20, createdMatrix20_dupl, Threading.Multi);

        [Benchmark] public void TensorAndMultiply15Parallel()
            => TS.TensorMatrixMultiply(createdTensorMatrix15, createdTensorMatrix15, Threading.Multi);
            
        [Benchmark] public void MatrixAndAdd20()
            => TS.PiecewiseAdd(createdMatrix20, createdMatrix20_dupl);
        
        [Benchmark] public void MatrixAndAdd100()
            => TS.PiecewiseAdd(createdMatrix100, createdMatrix100_dupl);
        
        [Benchmark] public void MatrixAndAdd20Parallel()
            => TS.PiecewiseAdd(createdMatrix20, createdMatrix20_dupl, Threading.Multi);

        [Benchmark] public void MatrixAndAdd100Parallel()
            => TS.PiecewiseAdd(createdMatrix100, createdMatrix100_dupl, Threading.Multi);
            
        
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
