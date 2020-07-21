using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive> where TWrapper : ITensorElement<TPrimitive>, new()
    {
        private TWrapper[] Data { get; set; }
        
        private readonly List<int> Blocks = new List<int>(); // 3 x 4 x 5
        public int Volume => Data.Length;
        
        private void BlockRecompute()
        {
            int len = 1;
            Blocks.Clear();
            for (int i = Shape.Count - 1; i >= 0; i--)
            {
                Blocks.Add(len);
                len *= Shape[i];
            }
            Blocks.Reverse();
        }

        protected Tensor(TensorShape dimensions, List<int> blocks, List<int> axesOrder, TWrapper[] data)
        {
            Shape = dimensions;
            AxesOrder = axesOrder;
            Blocks = blocks;
            Data = data;
        }

        private void Init(TensorShape dimensions)
        {
            Shape = dimensions;
            int len = 1;
            for (int i = 0; i < dimensions.Length; i++)
            {
                len *= dimensions[i];
                AxesOrder.Add(i);
            }
            var data = new TWrapper[len];
            for (int i = 0; i < len; i++)
                data[i] = new TWrapper();
            Data = data;
            LinOffset = 0;
            BlockRecompute();
        }

        public Tensor(TensorShape dimensions)
            => Init(dimensions);

        public Tensor(params int[] dimensions)
            => Init(new TensorShape(dimensions));

        public TPrimitive this[params int[] indecies]
        {
            get
            {
                if (indecies.Length == Shape.Count)
                {
                    var actualIndex = GetFlattenedIndexWithCheck(indecies);
                    if (actualIndex >= Data.Length)
                        throw new IndexOutOfRangeException();
                    return Data[actualIndex].GetValue();
                }
                if (indecies.Length > Shape.Count)
                    throw new IndexOutOfRangeException();
                throw new NotImplementedException();
            }
            set
            {
                if (indecies.Length == Shape.Count)
                {
                    var actualIndex = GetFlattenedIndexWithCheck(indecies);
                    if (actualIndex >= Data.Length)
                        throw new IndexOutOfRangeException();
                    Data[actualIndex].SetValue(value);
                    return;
                }
                if (indecies.Length > Shape.Count)
                    throw new IndexOutOfRangeException();
                throw new NotImplementedException();
            }
        }
    }
}
