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

namespace GenericTensor.Functions
{
    internal static class Determinant<T>
    {
        #region Matrix Determinant
        #region Laplace
        internal static T DeterminantLaplace(GenTensor<T> t, int diagLength)
        {
            if (diagLength == 1)
                return ConstantsAndFunctions<T>.Forward(t.GetValueNoCheck(0, 0));
            var det = ConstantsAndFunctions<T>.CreateZero();
            var sign = ConstantsAndFunctions<T>.CreateOne();
            var temp = SquareMatrixFactory<T>.GetMatrix(diagLength);
            for (int i = 0; i < diagLength; i++)
            {
                Inversion<T>.GetCofactor(t, temp, 0, i, diagLength);
                det = ConstantsAndFunctions<T>.Add(det,
                    ConstantsAndFunctions<T>.Multiply(
                        sign,
                        ConstantsAndFunctions<T>.Multiply(
                            t.GetValueNoCheck(0, i),
                            DeterminantLaplace(temp, diagLength - 1)
                        ))
                );
                sign = ConstantsAndFunctions<T>.Negate(sign);
            }
            return det;
        }

        public static T DeterminantLaplace(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("Determinant function should be only called from a matrix");
            if (t.Shape[0] != t.Shape[1])
                throw new InvalidShapeException("Matrix should be square");
            #endif
            return DeterminantLaplace(t, t.Shape[0]);
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

        public static T DeterminantGaussianSafeDivision(GenTensor<T> t)
            => DeterminantGaussianSafeDivision(t, t.Shape[0]);

        internal static T DeterminantGaussianSafeDivision(GenTensor<T> t, int diagLength)
        {
            InitIfNotInitted();
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            if (t.Shape[0] != t.Shape[1])
                throw new InvalidShapeException("this should be square matrix");
            #endif

            if (t.Shape[0] == 1)
                return ConstantsAndFunctions<T>.Forward(t.GetValueNoCheck(0, 0));

            var n = diagLength;
            var elemMatrix = InnerGaussianEliminationSafeDivision(t, n);

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

        private static GenTensor<SafeDivisionWrapper<T>> InnerGaussianEliminationSafeDivision(GenTensor<T> t, int n)
        {
            InitIfNotInitted();

            var elemMatrix = GenTensor<SafeDivisionWrapper<T>>
                .CreateMatrix(n, n,
                    (x, y) => new SafeDivisionWrapper<T>(ConstantsAndFunctions<T>.Forward(t.GetValueNoCheck(x, y)))
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

            return elemMatrix;
        }

        public static GenTensor<T> GaussianEliminationSafeDivision(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            if (t.Shape[0] != t.Shape[1])
                throw new InvalidShapeException("this should be square matrix");
            #endif
            var wrp = InnerGaussianEliminationSafeDivision(t, t.Shape[0]);
            return GenTensor<T>.CreateMatrix(t.Shape[0], t.Shape[1], (x, y) => wrp.GetValueNoCheck(x, y).Count());
        }

        public static T DeterminantGaussianSimple(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            if (t.Shape[0] != t.Shape[1])
                throw new InvalidShapeException("this should be square matrix");
            #endif
            if (t.Shape[0] == 1)
                return ConstantsAndFunctions<T>.Forward(t.GetValueNoCheck(0, 0));

            var n = t.Shape[0];

            var elemMatrix = t.Forward();
            for (int k = 1; k < n; k++)
            for (int j = k; j < n; j++)
            {
                var m = ConstantsAndFunctions<T>.Divide(
                    ConstantsAndFunctions<T>.Forward(elemMatrix.GetValueNoCheck(j, k - 1)),
                    ConstantsAndFunctions<T>.Forward(elemMatrix.GetValueNoCheck(k - 1, k - 1))
                );
                for (int i = 0; i < n; i++)
                {
                    var curr = ConstantsAndFunctions<T>.Forward(elemMatrix.GetValueNoCheck(j, i));
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
        #endregion

        #region Tensor Determinant

        public static GenTensor<T> TensorDeterminantLaplace(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            InvalidShapeException.NeedTensorSquareMatrix(t);
            #endif

            var res = GenTensor<T>.CreateTensor(t.Shape.SubShape(0, 2),
                ind => t.GetSubtensor(ind).DeterminantLaplace());
            return res;
        }

        public static GenTensor<T> TensorDeterminantGaussianSafeDivision(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            InvalidShapeException.NeedTensorSquareMatrix(t);
            #endif

            var res = GenTensor<T>.CreateTensor(t.Shape.SubShape(0, 2),
                ind => t.GetSubtensor(ind).DeterminantGaussianSafeDivision());
            return res;
        }

        public static GenTensor<T> TensorDeterminantGaussianSimple(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            InvalidShapeException.NeedTensorSquareMatrix(t);
            #endif

            var res = GenTensor<T>.CreateTensor(t.Shape.SubShape(0, 2),
                ind => t.GetSubtensor(ind).DeterminantGaussianSimple());
            return res;
        }

        #endregion
    }
}
