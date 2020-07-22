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
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{

    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> TensorVectorDotProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (a.Shape.SubShape(0, 1) != b.Shape.SubShape(0, 1))
                throw new InvalidShapeException("Other dimensions of tensors should be equal");
            var resTensor = new Tensor<TWrapper, TPrimitive>(a.Shape.SubShape(0, 1));
            foreach (var index in resTensor.IterateOverElements())
            {
                var scal = VectorDotProduct(a.GetSubtensor(index), b.GetSubtensor(index));
                resTensor[index] = scal;
            }
            return resTensor;
        }

        public static TPrimitive VectorDotProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (!a.IsVector || !b.IsVector)
                throw new InvalidShapeException($"{nameof(a)} and {nameof(b)} should be vectors");
            if (a.Shape[0] != b.Shape[0])
                throw new InvalidShapeException($"{nameof(a)}'s length should be the same as {nameof(b)}'s");

            var res = ConstantsAndFunctions<TWrapper, TPrimitive>.CreateZero();
            for (int i = 0; i < a.Shape[0]; i++)
            {
                var term = ConstantsAndFunctions<TWrapper, TPrimitive>.Create(a[i]);
                term.Multiply(b.GetCell(i));
                res.Add(term);
            }
            return res.GetValue();
        }
    }
}
