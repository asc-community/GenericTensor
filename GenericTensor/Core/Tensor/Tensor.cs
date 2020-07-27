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
using System.Linq;

namespace GenericTensor.Core
{
    public partial class GenTensor<T> : ICloneable
    {
        private readonly T[] data;
        private readonly int[] blocks; // 3 x 4 x 5
        private int volume = -1;
        /// <summary>
        /// Number of elements in tensor overall
        /// </summary>
        public int Volume
        {
            get
            {
                if (volume == -1)
                {
                    volume = 1;
                    for (var i = 0; i < Shape.Length; i++)
                        volume *= Shape[i];
                }
                return volume;
            }
        }

        private void BlockRecompute()
        {
            int len = 1;
            for (int i = Shape.Count - 1; i >= 0; i--)
            {
                blocks[i] = len;
                len *= Shape[i];
            }
            blocks.Reverse();
        }

        protected GenTensor(TensorShape dimensions, int[] blocks, int[] axesOrder, T[] data)
        {
            Shape = dimensions;
            AxesOrder = axesOrder;
            this.blocks = blocks;
            this.data = data;
        }

        public object Clone()
            => Copy(copyElements: true);

        /// <summary>
        /// Creates an uninitialized tensor where each element is just created wrapper
        /// </summary>
        public GenTensor(TensorShape dimensions)
        {
            Shape = dimensions;
            int len = 1;
            AxesOrder = new int[dimensions.Count];
            for (int i = 0; i < dimensions.Length; i++)
            {
                len *= dimensions[i];
                AxesOrder[i] = i;
            }
            var data = new T[len];
            this.data = data;
            LinOffset = 0;
            blocks = new int[dimensions.Count];
            BlockRecompute();
        }

        /// <summary>
        /// Creates an uninitialized tensor where each element is just created wrapper
        /// </summary>
        public GenTensor(params int[] dimensions) : this(new TensorShape(dimensions)) { }
    }
}
