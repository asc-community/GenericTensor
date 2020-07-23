#region copyright
/*
 * MIT License
 * 
 * Copyright (c) 2020 WhiteBlackGoose
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

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
        private int _volume = -1;
        /// <summary>
        /// Number of elements in tensor overall
        /// </summary>
        public int Volume
        {
            get
            {
                if (_volume == -1)
                {
                    _volume = 1;
                    for (var i = 0; i < Shape.Length; i++)
                        _volume *= Shape[i];
                }
                return _volume;
            }
        }

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

            /*
             We do not need to init it
            for (int i = 0; i < len; i++)
                data[i] = new TWrapper();
                */
            Data = data;
            LinOffset = 0;
            BlockRecompute();
        }

        /// <summary>
        /// Creates an empty tensor where each element is just created wrapper
        /// </summary>
        public Tensor(TensorShape dimensions)
            => Init(dimensions);

        /// <summary>
        /// Creates an empty tensor where each element is just created wrapper
        /// </summary>
        public Tensor(params int[] dimensions)
            => Init(new TensorShape(dimensions));
    }
}
