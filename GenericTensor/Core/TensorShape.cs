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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public struct TensorShape
    {
        private int[] shape;
        public int Length => shape.Length;
        public int DimensionCount => shape.Length;
        public int Count => shape.Length;

        public TensorShape(params int[] shape)
        {
            this.shape = shape;
        }

        internal TensorShape CutOffset1()
        {
            var newShape = new int[Length - 1];
            for (int i = 0; i < newShape.Length; i++)
                newShape[i] = shape[i + 1];
            return new TensorShape(newShape);
        }

        internal TensorShape SubShape(int offsetFromLeft, int offsetFromRight)
        {
            var newShape = new int[Length - offsetFromLeft - offsetFromRight];
            for (int i = offsetFromLeft; i < Length - offsetFromRight; i++)
                newShape[i - offsetFromLeft] = shape[i];
            return new TensorShape(newShape);
        }

        internal void Swap(int id1, int id2)
            => (shape[id1], shape[id2]) = (shape[id2], shape[id1]);

        public int this[int axisId] => shape[axisId];

        internal int[] ToArray() => shape;

        public override string ToString()
            => string.Join(" x ", shape.Select(c => c.ToString()));

        public override bool Equals(object obj)
        {
            if (!(obj is TensorShape sh))
                return false;
            if (sh.Length != Length)
                return false;
            for (int i = 0; i < sh.Length; i++)
                if (sh[i] != this[i])
                    return false;
            return true;
        }

        public static bool operator ==(TensorShape a, TensorShape b)
            => a.Equals(b);

        public static bool operator !=(TensorShape a, TensorShape b)
            => !a.Equals(b);
    }
}
