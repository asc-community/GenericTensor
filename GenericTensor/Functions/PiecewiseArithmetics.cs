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
    public partial class Tensor<T>
    {
        /// <summary>
        /// [i, j, k...]th element of the resulting tensor is
        /// operation(a[i, j, k...], b[i, j, k...])
        /// </summary>
        public static Tensor<T> Zip(Tensor<T> a,
            Tensor<T> b, Func<T, T, T> operation)
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape != b.Shape)
                throw new InvalidShapeException("Arguments should be of the same shape");
            #endif
            var res = new Tensor<T>(a.Shape);
            if (res.Shape.Length == 1)
                res.IterateOver1(x =>
                    res.SetValueNoCheck(
                        operation(a.GetValueNoCheck(x), b.GetValueNoCheck(x)),
                        x
                    )
                );
            else if (res.Shape.Length == 2)
                res.IterateOver2((x, y) =>
                    res.SetValueNoCheck(
                        operation(a.GetValueNoCheck(x, y), b.GetValueNoCheck(x, y)),
                        x, y
                    )
                );
            else if (res.Shape.Length == 3)
                res.IterateOver3((x, y, z) =>
                    res.SetValueNoCheck(
                        operation(a.GetValueNoCheck(x, y, z), b.GetValueNoCheck(x, y, z)),
                        x, y, z
                    )
                );
            else
                foreach (var index in res.IterateOverElements())
                    res.SetValueNoCheck(operation(a.GetValueNoCheck(index), b.GetValueNoCheck(index)), index);
            return res;
        }

        public static Tensor<T> PiecewiseAdd(Tensor<T> a,
            Tensor<T> b)
            => Zip(a, b, ConstantsAndFunctions<T>.Add);

        public static Tensor<T> PiecewiseSubtract(Tensor<T> a,
            Tensor<T> b)
            => Zip(a, b, ConstantsAndFunctions<T>.Subtract);

        public static Tensor<T> PiecewiseMultiply(Tensor<T> a,
            Tensor<T> b)
            => Zip(a, b, ConstantsAndFunctions<T>.Multiply);

        public static Tensor<T> PiecewiseDivide(Tensor<T> a,
            Tensor<T> b)
            => Zip(a, b, ConstantsAndFunctions<T>.Divide);

        public static Tensor<T> PiecewiseAdd(Tensor<T> a,
            T b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<T>.Add(a[ind], b));

        public static Tensor<T> PiecewiseSubtract(Tensor<T> a,
            T b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<T>.Subtract(a[ind], b));

        public static Tensor<T> PiecewiseSubtract(
            T a, Tensor<T> b)
            => CreateTensor(b.Shape, ind => 
                ConstantsAndFunctions<T>.Subtract(a, b[ind]));

        public static Tensor<T> PiecewiseMultiply(Tensor<T> a,
            T b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<T>.Multiply(a[ind], b));

        public static Tensor<T> PiecewiseDivide(Tensor<T> a,
            T b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<T>.Divide(a[ind], b));

        public static Tensor<T> PiecewiseDivide(
            T a, Tensor<T> b)
            => CreateTensor(b.Shape, ind => 
                ConstantsAndFunctions<T>.Divide(a, b[ind]));
    }
}
