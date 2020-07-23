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
using System.Runtime.CompilerServices;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<T>
    {
        /// <summary>
        /// Shape represents axes' lengths of the tensor
        /// </summary>
        public TensorShape Shape { get; private set; }
        private int[] AxesOrder;

        /// <summary>
        /// Swaps axes in tensor.
        /// 0 - the first dimension
        /// </summary>
        public void Transpose(int axis1, int axis2)
        {
            (AxesOrder[axis1], AxesOrder[axis2]) = (AxesOrder[axis2], AxesOrder[axis1]);
            Shape.Swap(axis1, axis2);
        }

        /// <summary>
        /// Swaps two last axes or throws InvalidShapeException if a tensor is too low-dimensional
        /// </summary>
        public void TransposeMatrix()
        {
            #if ALLOW_EXCEPTIONS
            if (Shape.Count < 2)
                throw new InvalidShapeException("this should be at least matrix");
            #endif
            Transpose(Shape.Count - 2, Shape.Count - 1);
        }

        private int GetFlattenedIndexWithCheck(int[] indecies)
        {
            #if ALLOW_EXCEPTIONS
            if (indecies.Length != Shape.Count)
                throw new ArgumentException($"There should be {Shape.Count} indecies, not {indecies.Length}");
            #endif
            var res = 0;
            for (int i = 0; i < indecies.Length; i++)
            {
                #if ALLOW_EXCEPTIONS
                if (indecies[i] < 0 || indecies[i] >= Shape[i])
                    throw new IndexOutOfRangeException($"Bound vialoting: axis {i} is {Shape[i]} long, your input is {indecies[i]}");
                #endif
                res += Blocks[AxesOrder[i]] * indecies[i];
            }
            #if ALLOW_EXCEPTIONS
            if (res >= Data.Length)
                throw new IndexOutOfRangeException();
            #endif
            return res + LinOffset;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFlattenedIndexSilent(int x) 
            => Blocks[AxesOrder[0]] * x + 
               LinOffset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFlattenedIndexSilent(int x, int y) 
            => Blocks[AxesOrder[0]] * x +
               Blocks[AxesOrder[1]] * y +
               LinOffset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFlattenedIndexSilent(int x, int y, int z) 
            => Blocks[AxesOrder[0]] * x +
               Blocks[AxesOrder[1]] * y +
               Blocks[AxesOrder[2]] * z +
               LinOffset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFlattenedIndexSilent(int x, int y, int z, int[] other)
        {
            var res = GetFlattenedIndexSilent(x, y, z);
            for (int i = 0; i < other.Length; i++)
                res += other[i] * Blocks[AxesOrder[i + 3]];
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFlattenedIndexSilent(int[] other)
        {
            var res = 0;
            for (int i = 0; i < other.Length; i++)
                res += other[i] * Blocks[AxesOrder[i]];
            return res + LinOffset;
        }
    }
}
