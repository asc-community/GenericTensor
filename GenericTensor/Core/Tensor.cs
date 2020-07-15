using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public class Tensor<TWrapper, TPrimitive> where TWrapper : ITensorElement<TPrimitive>, new()
    {
        private readonly DataArray<TWrapper> Data;
        public List<int> Shape { get; }
        private readonly List<int> Blocks = new List<int>();
        public Tensor(params int[] dimensions)
        {
            Shape = dimensions.ToList();
            int len = 1;
            for (int i = 0; i < dimensions.Length; i++)
            {
                Blocks.Add(len);
                len *= dimensions[i];
            }
            var data = new TWrapper[len];
            for (int i = 0; i < len; i++)
                data[i] = new TWrapper();
            Data = new DataArray<TWrapper>(data);
            Blocks.Reverse();
        }

        public TPrimitive this[params int[] indecies]
        {
            get => Data[0].GetValue();
        }
    }
}
