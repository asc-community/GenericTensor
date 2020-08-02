using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using GenericTensor.Core;

namespace Benchmark
{
    using TS = GenTensor<float>;

    public class OneVsMulti
    {
        static TS CreateTensor(int width, int size)
            => TS.CreateTensor(new TensorShape(width, size, size), (ind) => ind[0] + ind[1] + ind[2]);

        static TS CreateTensorPar(int width, int size)
            => TS.CreateTensor(new TensorShape(width, size, size), (ind) => ind[0] + ind[1] + ind[2], Threading.Multi);

        private TS created;

        private Dictionary<(int width, int height), TS> storage = new Dictionary<(int width, int height), TS>();

        private TS GetT(int width, int height)
        {
            var key = (width, height);
            if (!storage.ContainsKey(key))
                storage[key] = CreateTensor(width, height);
            return storage[key];
        }


        [Params(5, 15)] public int Width { set; get; }
        [Params(5, 15)] public int Height { set; get; }

        /*
        [Benchmark]
        public void CreatingTensor()
            => CreateTensor(Width, Height);

        [Benchmark]
        public void CreatingTensorPar()
            => CreateTensorPar(Width, Height);
            */
        [Benchmark]
        public void Multiply()
            => TS.TensorMatrixMultiply(GetT(Width, Height), GetT(Width, Height));

        [Benchmark]
        public void MultiplyPar()
            => TS.TensorMatrixMultiply(GetT(Width, Height), GetT(Width, Height), Threading.Multi);

        [Benchmark]
        public void PiecewiseMultiply()
            => TS.PiecewiseMultiply(GetT(Width, Height), GetT(Width, Height));

        [Benchmark]
        public void PiecewiseMultiplyPar()
            => TS.PiecewiseMultiply(GetT(Width, Height), GetT(Width, Height), Threading.Multi);

    }
}
