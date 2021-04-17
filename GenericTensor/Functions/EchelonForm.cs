#region copyright
/*
 * MIT License
 * 
 * Copyright (c) 2020-2021 WhiteBlackGoose
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
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Functions
{
    internal static class EchelonForm<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        #region Gaussian elimination safe division
        internal struct SafeDivisionWrapper<W, TW> where TW : struct, IOperations<W>
        {
            internal W num;
            internal W den;

            public SafeDivisionWrapper(W val)
            {
                num = val;
                den = default(TW).CreateOne();
            }

            public SafeDivisionWrapper(W num, W den)
            {
                this.num = num;
                this.den = den;
            }
            
            public W Count() => default(TW).Divide(num, den);
        }

        internal struct WrapperSafeDivisionWrapper<W, TW> : IOperations<SafeDivisionWrapper<W, TW>> where TW : struct, IOperations<W>
        {

            public SafeDivisionWrapper<W, TW> Add(SafeDivisionWrapper<W, TW> a, SafeDivisionWrapper<W, TW> b)
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).Add(default(TW).Multiply(a.num, b.den), default(TW).Multiply(b.num, a.den)),
                    default(TW).Multiply(a.den, b.den));
            }

            public SafeDivisionWrapper<W, TW> Subtract(SafeDivisionWrapper<W, TW> a, SafeDivisionWrapper<W, TW> b)
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).Subtract(default(TW).Multiply(a.num, b.den), default(TW).Multiply(b.num, a.den)),
                    default(TW).Multiply(a.den, b.den));
            }

            public SafeDivisionWrapper<W, TW> Multiply(SafeDivisionWrapper<W, TW> a, SafeDivisionWrapper<W, TW> b)
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).Multiply(a.num, b.num), default(TW).Multiply(a.den, b.den));
            }

            public SafeDivisionWrapper<W, TW> Negate(SafeDivisionWrapper<W, TW> a)
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).Negate(a.num), a.den);
            }

            public SafeDivisionWrapper<W, TW> Divide(SafeDivisionWrapper<W, TW> a, SafeDivisionWrapper<W, TW> b)
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).Multiply(a.num, b.den), default(TW).Multiply(a.den, b.num));
            }

            public SafeDivisionWrapper<W, TW> CreateOne()
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).CreateOne());
            }

            public SafeDivisionWrapper<W, TW> CreateZero()
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).CreateZero());
            }

            public SafeDivisionWrapper<W, TW> Copy(SafeDivisionWrapper<W, TW> a)
            {
                return new SafeDivisionWrapper<W, TW>(default(TW).Copy(a.num), default(TW).Copy(a.den));
            }

            public SafeDivisionWrapper<W, TW> Forward(SafeDivisionWrapper<W, TW> a)
            {
                return a;
            }

            public bool AreEqual(SafeDivisionWrapper<W, TW> a, SafeDivisionWrapper<W, TW> b)
            {
                return default(TW).AreEqual(a.num, b.num) && default(TW).AreEqual(a.den, b.den);
            }

            public bool IsZero(SafeDivisionWrapper<W, TW> a)
            {
                return default(TW).IsZero(a.num);
            }

            public string ToString(SafeDivisionWrapper<W, TW> a)
            {
                return default(TW).ToString(a.num) + " / " + default(TW).ToString(a.den);
            }

            public byte[] Serialize(SafeDivisionWrapper<W, TW> a)
            {
                throw new System.NotImplementedException();
            }

            public SafeDivisionWrapper<W, TW> Deserialize(byte[] data)
            {
                throw new System.NotImplementedException();
            }
        }

        internal static GenTensor<SafeDivisionWrapper<T, TWrapper>, WrapperSafeDivisionWrapper<T, TWrapper>> InnerGaussianEliminationSafeDivision(GenTensor<T, TWrapper> t, int m, int n)
        {
            var elemMatrix = GenTensor<SafeDivisionWrapper<T, TWrapper>, WrapperSafeDivisionWrapper<T, TWrapper>>
                .CreateMatrix(m, n,
                    (x, y) => new SafeDivisionWrapper<T, TWrapper>(t.GetValueNoCheck(x, y))
                );
            return EchelonForm<SafeDivisionWrapper<T, TWrapper>, WrapperSafeDivisionWrapper<T, TWrapper>>.InnerGaussianEliminationSimple(elemMatrix, m, n);
        }

        public static GenTensor<T, TWrapper> GaussianEliminationSafeDivision(GenTensor<T, TWrapper> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            #endif
            var wrp = InnerGaussianEliminationSafeDivision(t, t.Shape[0], t.Shape[1]);
            return GenTensor<T, TWrapper>.CreateMatrix(t.Shape[0], t.Shape[1], (x, y) => wrp.GetValueNoCheck(x, y).Count());
        }

        internal static GenTensor<T, TWrapper> InnerGaussianEliminationSimple(GenTensor<T, TWrapper> t, int m, int n)
        {
            var elemMatrix = t.Copy(copyElements: false);

            for (int k = 1; k < n; k++)
            for (int j = k; j < m; j++)
            {
                var c = default(TWrapper).Divide(
                    elemMatrix.GetValueNoCheck(j, k - 1),
                    elemMatrix.GetValueNoCheck(k - 1, k - 1)
                );
                elemMatrix.RowAdd(j, k - 1, default(TWrapper).Negate(c));
            }

            return elemMatrix;
        }

        public static GenTensor<T, TWrapper> GaussianEliminationSimple(GenTensor<T, TWrapper> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            #endif
            return InnerGaussianEliminationSimple(t, t.Shape[0], t.Shape[1]);
        }

        #endregion

        #region Reduced row echelon form

        private static GenTensor<T, TWrapper> InnerReducedRowEchelonFormSimple(GenTensor<T, TWrapper> t)
        {
            var upper = InnerGaussianEliminationSimple(t, t.Shape[0], t.Shape[1]);
            for (int r = t.Shape[0]; r >= 0; r--)
            {
                if (LeadingElement(t, r) is not { } leading)
                    continue;
                for (int i = 0; i < r; i++)
                    upper.RowAdd(i, r, default(TWrapper).Negate(
                                            default(TWrapper).Divide(
                                                upper.GetValueNoCheck(i, leading.id),
                                                leading.value
                                            )
                                       )
                    );
            }

            return upper;

            static (int id, T value)? LeadingElement(GenTensor<T, TWrapper> t, int row)
            {
                for (int i = 0; i < t.Shape[1]; i++)
                {
                    var value = t.GetValueNoCheck(row, i);
                    if (!default(TWrapper).IsZero(value))
                        return (i, value);
                }
                return null;
            }
        }

        public static GenTensor<T, TWrapper> ReducedRowEchelonFormSimple(GenTensor<T, TWrapper> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            #endif
            return InnerReducedRowEchelonFormSimple(t);
        }

        #endregion
    }
}
