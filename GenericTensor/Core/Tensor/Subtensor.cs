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
    public partial class GenTensor<T>
    {
        private int LinOffset;

        /// <summary>
        /// Slice with data sharing like in python
        /// A[3:5] in python
        /// same as
        /// A.Slice(3, 5) in GT
        ///
        /// O(N)
        /// </summary>
        // TODO: Make it O(1)
        public GenTensor<T> Slice(int leftIncluding, int rightExcluding)
        {
            #if ALLOW_EXCEPTIONS
            ReactIfBadBound(leftIncluding, 0);
            ReactIfBadBound(rightExcluding - 1, 0);
            if (leftIncluding >= rightExcluding)
                throw new InvalidShapeException("Slicing cannot be performed");
            #endif
            var newLength = rightExcluding - leftIncluding;
            var toStack = new GenTensor<T>[newLength];
            for (int i = 0; i < newLength; i++)
                toStack[i] = GetSubtensor(i + leftIncluding);
            return Stack(toStack);
        }

        /// <summary>
        /// This Subtensor is sequential Subtensor(int)
        ///
        /// O(1)
        /// </summary>
        public GenTensor<T> GetSubtensor(int[] indices)
            => GetSubtensor(indices, 0);

        internal GenTensor<T> GetSubtensor(int[] indices, int id)
            => id == indices.Length ? this : this.GetSubtensor(indices[id]).GetSubtensor(indices, id + 1);

        /// <summary>
        /// Get a subtensor of a tensor
        /// If you have a t = Tensor[2 x 3 x 4],
        /// t.GetSubtensor(0) will return the proper matrix [3 x 4]
        ///
        /// O(1)
        /// </summary>
        public GenTensor<T> GetSubtensor(int index)
        {
            #if ALLOW_EXCEPTIONS
            ReactIfBadBound(index, 0);
            #endif
            var newLinIndexDelta = GetFlattenedIndexSilent(index);
            var newBlocks = _blocks.ToList();
            var rootAxis = AxesOrder[0];
            newBlocks.RemoveAt(rootAxis);
            var newAxesOrder = AxesOrder.ToList();
            for (int i = 0; i < newAxesOrder.Count; i++)
                if (newAxesOrder[i] > rootAxis)
                    newAxesOrder[i] -= 1;
            newAxesOrder.RemoveAt(0);
            var newShape = Shape.CutOffset1();
            var result = new GenTensor<T>(newShape, newBlocks.ToArray(), newAxesOrder.ToArray(), _data);
            result.LinOffset = newLinIndexDelta;
            return result;
        }

        /// <summary>
        /// Suppose you have t = tensor [2 x 3 x 4]
        /// and m = matrix[3 x 4]
        /// You need to set this matrix to t's second matrix
        /// t.SetSubtensor(m, 1);
        ///
        /// O(V)
        /// </summary>
        public void SetSubtensor(GenTensor<T> sub, params int[] indices)
        {
            #if ALLOW_EXCEPTIONS
            if (indices.Length >= Shape.Count)
                throw new InvalidShapeException($"Number of {nameof(indices)} should be less than number of {nameof(Shape)}");
            for (int i = 0; i < indices.Length; i++)
                if (indices[i] < 0 || indices[i] >= Shape[i])
                    throw new IndexOutOfRangeException();
            if (Shape.Count - indices.Length != sub.Shape.Count)
                throw new InvalidShapeException($"Number of {nameof(sub.Shape.Length)} + {nameof(indices.Length)} should be equal to {Shape.Count}");
            #endif
            var thisSub = GetSubtensor(indices);
            #if ALLOW_EXCEPTIONS
            if (thisSub.Shape != sub.Shape)
                throw new InvalidShapeException($"{nameof(sub.Shape)} must be equal to {nameof(Shape)}");
            #endif
            thisSub.Assign(sub);
        }
    }

}
