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
        private int width;

        [Params(3, 10, 20)]
        public int Width
        {
            get => width;
            set
            {
                width = value;
                if (Height != 0)
                    created = CreateTensor(Width, Height);
            }
        }
        

        private int height;

        [Params(3, 10, 20)]
        public int Height
        {
            get => height;
            set
            {
                height = value;
                if (Width != 0)
                    created = CreateTensor(Width, Height);
            }
        }

        [Benchmark]
        public void CreatingTensor()
            => CreateTensor(Width, Height);

        [Benchmark]
        public void CreatingTensorPar()
            => CreateTensorPar(Width, Height);

        [Benchmark]
        public void Multiply()
            => TS.TensorMatrixMultiply(created, created);

        [Benchmark]
        public void MultiplyPar()
            => TS.TensorMatrixMultiply(created, created, Threading.Multi);

        [Benchmark]
        public void PiecewiseMultiply()
            => TS.PiecewiseMultiply(created, created);

        [Benchmark]
        public void PiecewiseMultiplyPar()
            => TS.PiecewiseMultiply(created, created, Threading.Multi);

    }
}
