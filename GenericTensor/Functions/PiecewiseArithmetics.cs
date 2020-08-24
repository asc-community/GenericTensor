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


using GenericTensor.Core;
using GenericTensor.Core.Expressions;

namespace GenericTensor.Functions
{
    public interface IZipOperator<T>
    {
        T Operation(T a, T b);
    }

    internal static class WrapperStorage<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        internal struct AddWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Add(a, b);
        }

        internal struct SubtractWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Subtract(a, b);
        }

        internal struct MultiplyWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Multiply(a, b);
        }

        internal struct DivideWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Divide(a, b);
        }
    }

    internal static class PiecewiseArithmetics<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        
        private static bool DetermineThreading(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
        {
            var parallel = threading == Threading.Multi || (threading == Threading.Auto && a.Volume > 3000);
            return parallel && !a.IsVector;
        }

        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => ExpressionCompiler<T, TWrapper>.PiecewiseAdd(a, b, DetermineThreading(a, b, threading));

        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => ExpressionCompiler<T, TWrapper>.PiecewiseSubtract(a, b, DetermineThreading(a, b, threading));

        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => ExpressionCompiler<T, TWrapper>.PiecewiseMultiply(a, b, DetermineThreading(a, b, threading));

        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => ExpressionCompiler<T, TWrapper>.PiecewiseDivision(a, b, DetermineThreading(a, b, threading));

        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Add(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Subtract(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseSubtract(
            T a, GenTensor<T, TWrapper> b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(b.Shape, ind => 
                default(TWrapper).Subtract(a, b[ind]), threading);

        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Multiply(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Divide(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseDivide(
            T a, GenTensor<T, TWrapper> b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(b.Shape, ind => 
                default(TWrapper).Divide(a, b[ind]), threading);
    }
}
