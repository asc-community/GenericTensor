﻿#region copyright
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
    public partial class Tensor<TWrapper, TPrimitive>
    {
        private int LinOffset;
        public Tensor<TWrapper, TPrimitive> GetSubtensor(params int[] indecies)
        {
            if (indecies.Length == 0)
                return this;
            var currIndex = indecies[0];
            var newLinIndexDelta = GetFlattenedIndexSilent(new []{currIndex});
            var newBlocks = Blocks.Select(c => c).ToList();
            var rootAxis = AxesOrder[0];
            newBlocks.RemoveAt(rootAxis);
            var newAxesOrder = AxesOrder.Select(c => c).ToList();
            for (int i = 0; i < newAxesOrder.Count; i++)
                if (newAxesOrder[i] > rootAxis)
                    newAxesOrder[i] -= 1;
            newAxesOrder.RemoveAt(0);
            var newShape = Shape.CutOffset1();
            var aff = new Tensor<TWrapper, TPrimitive>(newShape, newBlocks, newAxesOrder, Data);
            aff.LinOffset = newLinIndexDelta;
            var sfdgd = aff.ToString();
            if (indecies.Length == 1)
                return aff;
            else
                return aff.GetSubtensor(new ArraySegment<int>(indecies, 1, indecies.Length - 1).ToArray());
        }

        public void SetSubtensor(Tensor<TWrapper, TPrimitive> sub, params int[] indecies)
        {
            if (indecies.Length >= Shape.Count)
                throw new InvalidShapeException($"{nameof(indecies.Length)} should be less than {nameof(Shape.Length)}");
            for (int i = 0; i < indecies.Length; i++)
                if (indecies[i] < 0 || indecies[i] >= Shape[i])
                    throw new IndexOutOfRangeException();
            if (Shape.Count - indecies.Length != sub.Shape.Count)
                throw new InvalidShapeException($"Number of {nameof(sub.Shape.Length)} + {nameof(indecies.Length)} should be equal to {Shape.Count}");
            var thisSub = GetSubtensor(indecies);
            if (thisSub.Shape != sub.Shape)
                throw new InvalidShapeException($"{nameof(sub.Shape)} must be equal to {nameof(Shape)}");
            thisSub.Assign(sub);
        }
    }

}
