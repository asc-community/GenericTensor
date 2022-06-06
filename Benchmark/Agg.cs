using BenchmarkDotNet.Attributes;
using GenericTensor.Core;
using GenericTensor.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    public class Agg
    {
        // 3 x 2 x 2 x 2
        private static readonly GenTensor<int, IntWrapper> frog = GenTensor<int, IntWrapper>.CreateTensor(new[, , ,] {
                {
                    { { 1, 2 }, { 3, 4 } },
                    { { 10, 20 }, { 30, 40 } },
                },
                {
                    { { 100, 200 }, { 300, 400 } },
                    { { 1000, 2000 }, { 3000, 4000 } },
                },
                {
                    { { 10000, 20000 }, { 30000, 40000 } },
                    { { 100000, 200000 }, { 300000, 400000 } },
                }
            });

        private static readonly GenTensor<int, IntWrapper> frogCopy = frog.Copy(true);

        [Benchmark]
        public void SumAx0()
        {
            var acc = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(2, 2, 2), id => 0);
            GenTensor<int, IntWrapper>.Aggregate(frog, acc, new HonkPerf.NET.Core.PureValueDelegate<int, int, int>((a, b) => a + b), axis: 0);
        }
    }
}
