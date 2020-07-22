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
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> Zip(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b, Func<TWrapper, TWrapper, TWrapper> operation)
        {
            if (a.Shape != b.Shape)
                throw new InvalidShapeException("Arguments should be of the same shape");
            var res = new Tensor<TWrapper, TPrimitive>(a.Shape);
            foreach (var index in res.IterateOverElements())
                res.SetCell(operation(a.GetCell(index), b.GetCell(index)), index);
            return res;
        }

        public static Tensor<TWrapper, TPrimitive> PiecewiseAdd(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.AddSaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseSubtract(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.SubtractSaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseMultiply(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.MultiplySaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseDivide(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.DivideSaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseAdd(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Add(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseSubtract(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseSubtract(
            TPrimitive a, Tensor<TWrapper, TPrimitive> b)
            => CreateTensor(b.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(a, b[ind]));

        public static Tensor<TWrapper, TPrimitive> PiecewiseMultiply(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseDivide(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Divide(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseDivide(
            TPrimitive a, Tensor<TWrapper, TPrimitive> b)
            => CreateTensor(b.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Divide(a, b[ind]));
    }
}
