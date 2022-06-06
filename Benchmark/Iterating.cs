using BenchmarkDotNet.Attributes;
using GenericTensor.Core;
using GenericTensor.Functions;
using HonkPerf.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    /*

    |  Method |     Mean |     Error |    StdDev |   Median |
    |-------- |---------:|----------:|----------:|---------:|
    | Iterate | 7.129 us | 0.1396 us | 0.3234 us | 7.019 us |
    | ForEach | 2.342 us | 0.0495 us | 0.1459 us | 2.287 us |

     */
    public class Iterating
    {
        public GenTensor<int, IntWrapper> tensor = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(10, 4, 5), _ => 3);

        [Benchmark]
        public void Iterate()
        {
            foreach (var (index, value) in tensor.Iterate())
                tensor[index] = value * 2;
        }

        public struct Multiply : IValueAction<int[], int>
        {
            private GenTensor<int, IntWrapper> tensor;
            public Multiply(GenTensor<int, IntWrapper> t) => tensor = t;
            public void Invoke(int[] arg1, int arg2)
            {
                tensor[arg1] = arg2 * 2;
            }
        }


        [Benchmark]
        public void ForEach()
        {
            tensor.ForEach(new Multiply(tensor));
        }
    }
}
