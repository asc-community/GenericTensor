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
    public partial class Tensor<T>
    {
        #region Laplace
        
        internal T DeterminantLaplace(int diagLength)
        {
            if (diagLength == 1)
                return this[0, 0];
            var det = ConstantsAndFunctions<T>.CreateZero();
            var sign = ConstantsAndFunctions<T>.CreateOne();
            var temp = new Tensor<T>(diagLength, diagLength);
            for (int i = 0; i < diagLength; i++)
            {
                GetCofactor(this, temp, 0, i, diagLength);
                det = ConstantsAndFunctions<T>.Add(det,
                    ConstantsAndFunctions<T>.Multiply(
                        sign,
                        ConstantsAndFunctions<T>.Multiply(
                            this[0, i],
                            temp.DeterminantLaplace(diagLength - 1)
                        ))
                );
                sign = ConstantsAndFunctions<T>.Negate(sign);
            }
            return det;
        }

        /// <summary>
        /// Finds Determinant with the 100% precision for O(N!) where
        /// N is your matrix' width
        /// The matrix should be square
        /// Borrowed from here: https://github.com/ZacharyPatten/Towel/blob/master/Sources/Towel/Mathematics/Matrix.cs#L528
        /// </summary>
        public T DeterminantLaplace()
        {
            #if ALLOW_EXCEPTIONS
            if (!this.IsMatrix)
                throw new InvalidShapeException("Determinant function should be only called from a matrix");
            if (Shape[0] != Shape[1])
                throw new InvalidShapeException("Matrix should be square");
            #endif
            return DeterminantLaplace(Shape[0]);
        }
        #endregion

        #region Gaussian

        #region Safe division wrapper
        internal struct SafeDivisionWrapper<W>
        {
            internal W num;
            internal W den;

            public SafeDivisionWrapper(W val)
            {
                num = val;
                den = ConstantsAndFunctions<W>.CreateOne();
            }

            public SafeDivisionWrapper(W num, W den)
            {
                this.num = num;
                this.den = den;
            }

            public W Count() => ConstantsAndFunctions<W>.Divide(num, den);
        }

        private static bool isFracInitted = false;
        private static void InitIfNotInitted()
        {
            if (isFracInitted)
                return;
            isFracInitted = true;

            ConstantsAndFunctions<SafeDivisionWrapper<T>>.Add =
                (a, b) =>
                    new SafeDivisionWrapper<T>(
                        ConstantsAndFunctions<T>.Add(
                            ConstantsAndFunctions<T>.Multiply(a.num, b.den),
                            ConstantsAndFunctions<T>.Multiply(a.den, b.num)
                            ),
                        ConstantsAndFunctions<T>.Multiply(a.den, b.den)
                        );

            ConstantsAndFunctions<SafeDivisionWrapper<T>>.Subtract =
                (a, b) =>
                    new SafeDivisionWrapper<T>(
                        ConstantsAndFunctions<T>.Subtract(
                            ConstantsAndFunctions<T>.Multiply(a.num, b.den),
                            ConstantsAndFunctions<T>.Multiply(a.den, b.num)
                        ),
                        ConstantsAndFunctions<T>.Multiply(a.den, b.den)
                    );

            ConstantsAndFunctions<SafeDivisionWrapper<T>>.Multiply =
                (a, b) =>
                    new SafeDivisionWrapper<T>(
                        ConstantsAndFunctions<T>.Multiply(a.num, b.num),
                        ConstantsAndFunctions<T>.Multiply(a.den, b.den)
                    );

            ConstantsAndFunctions<SafeDivisionWrapper<T>>.Divide =
                (a, b) =>
                    new SafeDivisionWrapper<T>(
                        ConstantsAndFunctions<T>.Multiply(a.num, b.den),
                        ConstantsAndFunctions<T>.Multiply(a.den, b.num)
                    );

            ConstantsAndFunctions<SafeDivisionWrapper<T>>.CreateOne = () =>
                new SafeDivisionWrapper<T>(ConstantsAndFunctions<T>.CreateOne());
        }

        #endregion

        /// <summary>
        /// Finds Determinant with possible overflow
        /// because it uses fractions for avoiding division
        /// Works for O(N^3)
        /// </summary>
        public T DeterminantGaussianSafeDivision()
        {
            InitIfNotInitted();
            #if ALLOW_EXCEPTIONS
            if (!IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            if (Shape[0] != Shape[1])
                throw new InvalidShapeException("this should be square matrix");
            #endif

            if (Shape[0] == 1)
                return this[0, 0];

            var n = Shape[0];

            var elemMatrix = Tensor<SafeDivisionWrapper<T>>
                .CreateMatrix(n, n,
                k => new SafeDivisionWrapper<T>(this.GetValueNoCheck(k.x, k.y))
                );
            for (int k = 1; k < n; k++)
            for (int j = k; j < n; j++)
            {
                var m = ConstantsAndFunctions<SafeDivisionWrapper<T>>.Divide(
                    elemMatrix.GetValueNoCheck(j, k - 1),
                    elemMatrix.GetValueNoCheck(k - 1, k - 1)
                    );
                for (int i = 0; i < n; i++)
                {
                    var curr = elemMatrix.GetValueNoCheck(j, i);
                    elemMatrix.SetValueNoCheck(ConstantsAndFunctions<SafeDivisionWrapper<T>>.Subtract(
                        curr,
                        ConstantsAndFunctions<SafeDivisionWrapper<T>>.Multiply(
                            m,
                            elemMatrix.GetValueNoCheck(k - 1, i)
                        )
                    ), j, i);
                }
            }

            var det = 
                ConstantsAndFunctions<SafeDivisionWrapper<T>>.CreateOne();
            for (int i = 0; i < n; i++)
            {
                det = ConstantsAndFunctions<SafeDivisionWrapper<T>>.Multiply(det, elemMatrix.GetValueNoCheck(i, i));
            }

            if (ConstantsAndFunctions<T>.IsZero(det.den))
                return ConstantsAndFunctions<T>.CreateZero();
            return det.Count();
        }

        /// <summary>
        /// Performs simple Gaussian elimination method on a tensor
        /// if you do not change TPrimitives in your TWrapper but
        /// call its methods or change fields,
        /// Rather use DeterminantGaussian(copy: true) so that its
        /// temporarily tensor will not change tensor's values
        /// </summary>
        public T DeterminantGaussianSimple()
            => DeterminantGaussianSimple(copy: false);

        // TODO: how to avoid code duplication?
        /// <summary>
        /// Copy = true will slower the performance, but is necessary if your
        /// TPrimitive's pointer does not change through operators
        /// </summary>
        public T DeterminantGaussianSimple(bool copy)
        {
            #if ALLOW_EXCEPTIONS
            if (!IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            if (Shape[0] != Shape[1])
                throw new InvalidShapeException("this should be square matrix");
            #endif
            if (Shape[0] == 1)
                return this[0, 0];

            var n = Shape[0];

            var elemMatrix = this.Forward();
            for (int k = 1; k < n; k++)
            for (int j = k; j < n; j++)
            {
                var m = ConstantsAndFunctions<T>.Divide(
                    elemMatrix.GetCell(j, k - 1),
                    elemMatrix.GetCell(k - 1, k - 1)
                );
                for (int i = 0; i < n; i++)
                {
                    var curr = elemMatrix.GetValueNoCheck(j, i);
                    elemMatrix.SetValueNoCheck(ConstantsAndFunctions<T>.Subtract(
                        curr,
                        ConstantsAndFunctions<T>.Multiply(
                            m,
                            elemMatrix.GetValueNoCheck(k - 1, i)
                        )
                    ), j, i);
                }
            }

            var det = ConstantsAndFunctions<T>.CreateOne();
            for (int i = 0; i < n; i++)
            {
                det = ConstantsAndFunctions<T>.Multiply(det, elemMatrix.GetValueNoCheck(i, i));
            }

            return det;
        }

        #endregion
    }
}
