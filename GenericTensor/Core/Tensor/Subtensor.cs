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
    public partial class GenTensor<T>
    {
        private int LinOffset;

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

        public GenTensor<T> GetSubtensor(int[] indecies)
            => GetSubtensor(indecies, 0);

        internal GenTensor<T> GetSubtensor(int[] indecies, int id)
            => id == indecies.Length ? this : this.GetSubtensor(indecies[id]).GetSubtensor(indecies, id + 1);

        /// <summary>
        /// Get a subtensor of a tensor
        /// If you have a t = Tensor[2 x 3 x 4],
        /// t.GetSubtensor(0) will return the proper matrix [3 x 4]
        /// </summary>
        public GenTensor<T> GetSubtensor(int index)
        {
            #if ALLOW_EXCEPTIONS
            ReactIfBadBound(index, 0);
            #endif
            var newLinIndexDelta = GetFlattenedIndexSilent(index);
            var newBlocks = Blocks.ToList();
            var rootAxis = AxesOrder[0];
            newBlocks.RemoveAt(rootAxis);
            var newAxesOrder = AxesOrder.ToList();
            for (int i = 0; i < newAxesOrder.Count; i++)
                if (newAxesOrder[i] > rootAxis)
                    newAxesOrder[i] -= 1;
            newAxesOrder.RemoveAt(0);
            var newShape = Shape.CutOffset1();
            var result = new GenTensor<T>(newShape, newBlocks.ToArray(), newAxesOrder.ToArray(), Data);
            result.LinOffset = newLinIndexDelta;
            return result;
        }

        /// <summary>
        /// Suppose you have t = tensor [2 x 3 x 4]
        /// and m = matrix[3 x 4]
        /// You need to set this matrix to t's second matrix
        /// t.SetSubtensor(m, 1);
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="indecies"></param>
        public void SetSubtensor(GenTensor<T> sub, params int[] indecies)
        {
            #if ALLOW_EXCEPTIONS
            if (indecies.Length >= Shape.Count)
                throw new InvalidShapeException($"{nameof(indecies.Length)} should be less than {nameof(Shape.Length)}");
            for (int i = 0; i < indecies.Length; i++)
                if (indecies[i] < 0 || indecies[i] >= Shape[i])
                    throw new IndexOutOfRangeException();
            if (Shape.Count - indecies.Length != sub.Shape.Count)
                throw new InvalidShapeException($"Number of {nameof(sub.Shape.Length)} + {nameof(indecies.Length)} should be equal to {Shape.Count}");
            #endif
            var thisSub = GetSubtensor(indecies);
            #if ALLOW_EXCEPTIONS
            if (thisSub.Shape != sub.Shape)
                throw new InvalidShapeException($"{nameof(sub.Shape)} must be equal to {nameof(Shape)}");
            #endif
            thisSub.Assign(sub);
        }
    }

}
