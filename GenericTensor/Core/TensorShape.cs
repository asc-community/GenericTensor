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
using System.Runtime.CompilerServices;

namespace GenericTensor.Core
{
    /// <summary>
    /// This structure represents a shape of a tensor
    /// </summary>
    public struct TensorShape : IEquatable<TensorShape>
    {
        public readonly int[] shape;
        /// <summary>
        /// Length of a shape is basically number of dimensions
        /// </summary>
        public int Length => shape.Length;

        /// <summary>
        /// Synonym for Length
        /// </summary>
        public int DimensionCount => shape.Length;

        /// <summary>
        /// Synonym for Length
        /// </summary>
        public int Count => shape.Length;

        /// <summary>
        /// Create a TensorShape for further operations
        /// just listing necessary dimensions
        /// </summary>
        /// <param name="shape"></param>
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

        public TensorShape SubShape(int offsetFromLeft, int offsetFromRight)
        {
            var newShape = new int[Length - offsetFromLeft - offsetFromRight];
            for (int i = offsetFromLeft; i < Length - offsetFromRight; i++)
                newShape[i - offsetFromLeft] = shape[i];
            return new TensorShape(newShape);
        }

        public TensorShape Copy()
        {
            var resI = shape.ToArray();
            return new TensorShape(resI);
        }

        internal void Swap(int id1, int id2)
            => (shape[id1], shape[id2]) = (shape[id2], shape[id1]);

        /// <summary>
        /// You can only read some dimensions,
        /// otherwise it will cause unintended behaviour
        /// </summary>
        /// <param name="axisId"></param>
        /// <returns></returns>
        public int this[int axisId] => shape[axisId];

        public int[] ToArray() => shape;

        public override string ToString()
            => string.Join(" x ", shape.Select(c => c.ToString()));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TensorShape sh)
        {
            if (sh.Length != Length)
                return false;
            for (int i = 0; i < sh.Length; i++)
                if (sh.shape[i] != this.shape[i])
                    return false;
            return true;
        }

        public override int GetHashCode()
            => shape.GetHashCode();

        public override bool Equals(object obj)
            => obj is TensorShape ts ? Equals(ts) : false;

        public static bool operator ==(TensorShape a, TensorShape b)
            => a.Equals(b);

        public static bool operator !=(TensorShape a, TensorShape b)
            => !a.Equals(b);
    }
}
