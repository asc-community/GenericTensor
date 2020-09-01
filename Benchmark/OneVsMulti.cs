using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using GenericTensor.Core;
using GenericTensor.Functions;

namespace Benchmark
{
    using TS = GenTensor<float, FloatWrapper>;

    public class OneVsMulti
    {
        static TS CreateTensor(int width, int size)
            => TS.CreateTensor(new TensorShape(width, size, size), (ind) => ind[0] + ind[1] + ind[2]);

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0051 // Remove unused private members

        static TS CreateTensorPar(int width, int size)
            => TS.CreateTensor(new TensorShape(width, size, size), (ind) => ind[0] + ind[1] + ind[2], Threading.Multi);

        private TS created;
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE0044 // Add readonly modifier

        private readonly Dictionary<(int width, int height), TS> storage = new Dictionary<(int width, int height), TS>();

        private TS GetT(int width, int height)
        {
            var key = (width, height);
            if (!storage.ContainsKey(key))
                storage[key] = CreateTensor(width, height);
            return storage[key];
        }

        /*
        [Benchmark]
        public void CreatingTensor()
            => CreateTensor(Width, Height);

        [Benchmark]
        public void CreatingTensorPar()
            => CreateTensorPar(Width, Height);
            */

        #region matrix multiplication single
        [Benchmark]
        public void Multiply5x5x5()
            => TS.TensorMatrixMultiply(GetT(5, 5), GetT(5, 5));

        [Benchmark]
        public void Multiply15x5x5()
            => TS.TensorMatrixMultiply(GetT(15, 5), GetT(15, 5));

        [Benchmark]
        public void Multiply5x15x15()
            => TS.TensorMatrixMultiply(GetT(5, 15), GetT(5, 15));

        [Benchmark]
        public void Multiply15x15x15()
            => TS.TensorMatrixMultiply(GetT(15, 15), GetT(15, 15));

        #endregion

        #region matrix multiplication multi

        [Benchmark]
        public void Multiply5x5x5Par()
            => TS.TensorMatrixMultiply(GetT(5, 5), GetT(5, 5), Threading.Multi);

        [Benchmark]
        public void Multiply15x5x5Par()
            => TS.TensorMatrixMultiply(GetT(15, 5), GetT(15, 5), Threading.Multi);

        [Benchmark]
        public void Multiply5x15x15Par()
            => TS.TensorMatrixMultiply(GetT(5, 15), GetT(5, 15), Threading.Multi);

        [Benchmark]
        public void Multiply15x15x15Par()
            => TS.TensorMatrixMultiply(GetT(15, 15), GetT(15, 15), Threading.Multi);

        #endregion


        #region piecewise single

        
        [Benchmark]
        public void Piecewise10x10x10()
            => TS.PiecewiseAdd(GetT(10, 10), GetT(10, 10));

        [Benchmark]
        public void Piecewise30x10x10()
            => TS.PiecewiseAdd(GetT(30, 10), GetT(30, 10));

        [Benchmark]
        public void Piecewise10x30x30()
            => TS.PiecewiseAdd(GetT(10, 30), GetT(10, 30));
            
        [Benchmark]
        public void Piecewise30x30x30()
            => TS.PiecewiseAdd(GetT(30, 30), GetT(30, 30));
            

        #endregion

        #region piecewise multi

        
        [Benchmark]
        public void Piecewise10x10x10Par()
            => TS.PiecewiseAdd(GetT(10, 10), GetT(10, 10), Threading.Multi);

        [Benchmark]
        public void Piecewise30x10x10Par()
            => TS.PiecewiseAdd(GetT(30, 10), GetT(30, 10), Threading.Multi);

        [Benchmark]
        public void Piecewise10x30x30Par()
            => TS.PiecewiseAdd(GetT(10, 30), GetT(10, 30), Threading.Multi);
            
        [Benchmark]
        public void Piecewise30x30x30Par()
            => TS.PiecewiseAdd(GetT(30, 30), GetT(30, 30), Threading.Multi);

        #endregion
    }
}
