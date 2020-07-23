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
        #region Laplace
        
        internal TPrimitive DeterminantLaplace(int diagLength)
        {
            if (diagLength == 1)
                return this[0, 0];
            var det = ConstantsAndFunctions<TWrapper, TPrimitive>.CreateZero();
            var sign = ConstantsAndFunctions<TWrapper, TPrimitive>.CreateOne();
            var temp = new Tensor<TWrapper, TPrimitive>(diagLength, diagLength);
            for (int i = 0; i < diagLength; i++)
            {
                GetCofactor(this, temp, 0, i, diagLength);
                det.Add(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Create(
                        ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(
                            sign.GetValue(),
                            ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(
                                this[0, i],
                                temp.DeterminantLaplace(diagLength - 1)
                            )))
                );
                sign.Negate();
            }
            return det.GetValue();
        }

        /// <summary>
        /// Finds Determinant with the 100% precision for O(N!) where
        /// N is your matrix' width
        /// The matrix should be square
        /// Borrowed from here: https://github.com/ZacharyPatten/Towel/blob/master/Sources/Towel/Mathematics/Matrix.cs#L528
        /// </summary>
        public TPrimitive DeterminantLaplace()
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
        internal sealed class SafeDivisionWrapper<W, P> : ITensorElement<W> where W : ITensorElement<P>, new()
        {
            internal W num;
            internal W den;

            public SafeDivisionWrapper(){}

            public SafeDivisionWrapper(P v)
            {
                SetValue(ConstantsAndFunctions<W, P>.Create(v));
            }

            public W GetValue()
            {
                var res = new W();
                res.SetValue(num.GetValue());
                res.Divide(den);
                return res;
            }

            public void SetValue(W newValue)
            {
                num = newValue;
                den = ConstantsAndFunctions<W, P>.CreateOne();
            }

            public ITensorElement<W> Copy()
            {
                throw new NotImplementedException();
            }

            public ITensorElement<W> Forward()
            {
                var res = new SafeDivisionWrapper<W, P>();
                res.num = (W)num.Forward();
                res.den = (W)den.Forward();
                return res;
            }

            public void SetZero()
            {
                SetValue(ConstantsAndFunctions<W, P>.CreateZero());
            }

            public void SetOne()
            {
                SetValue(ConstantsAndFunctions<W, P>.CreateOne());
            }

            public void Add(ITensorElement<W> other)
            {
                var frac = other as SafeDivisionWrapper<W, P>;
                var newNum = ConstantsAndFunctions<W, P>.Add(
                    ConstantsAndFunctions<W, P>.Multiply(num, frac.den),
                    ConstantsAndFunctions<W, P>.Multiply(den, frac.num)
                );
                var newDen = ConstantsAndFunctions<W, P>.Multiply(
                    frac.den,
                    den
                );
                num = ConstantsAndFunctions<W, P>.Create(newNum);
                den = ConstantsAndFunctions<W, P>.Create(newDen);
            }

            public void Multiply(ITensorElement<W> other)
            {
                var frac = other as SafeDivisionWrapper<W, P>;
                num.Multiply(frac.num);
                den.Multiply(frac.den);
            }

            public void Subtract(ITensorElement<W> other)
            {
                var frac = other as SafeDivisionWrapper<W, P>;
                var newNum = ConstantsAndFunctions<W, P>.Subtract(
                    ConstantsAndFunctions<W, P>.Multiply(num, frac.den),
                    ConstantsAndFunctions<W, P>.Multiply(den, frac.num)
                );
                var newDen = ConstantsAndFunctions<W, P>.Multiply(
                    frac.den,
                    den
                );
                num = ConstantsAndFunctions<W, P>.Create(newNum);
                den = ConstantsAndFunctions<W, P>.Create(newDen);
            }

            public void Divide(ITensorElement<W> other)
            {
                var frac = other as SafeDivisionWrapper<W, P>;
                num.Multiply(frac.den);
                den.Multiply(frac.num);
            }

            public void Negate()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return num.ToString() + " / " + den.ToString();
            }

            public bool IsZero()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        /// <summary>
        /// Finds Determinant with possible overflow
        /// because it uses fractions for avoiding division
        /// Works for O(N^3)
        /// </summary>
        public TPrimitive DeterminantGaussianSafeDivision()
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

            var elemMatrix = Tensor<SafeDivisionWrapper<TWrapper, TPrimitive>, TWrapper>
                .CreateMatrix(n, n,
                k => new SafeDivisionWrapper<TWrapper, TPrimitive>(this[k.x, k.y])
                );
            for (int k = 1; k < n; k++)
            for (int j = k; j < n; j++)
            {
                var m = ConstantsAndFunctions<SafeDivisionWrapper<TWrapper, TPrimitive>, TWrapper>.DivideSaveWrapper(
                    elemMatrix.GetCell(j, k - 1),
                    elemMatrix.GetCell(k - 1, k - 1)
                    );
                for (int i = 0; i < n; i++)
                    elemMatrix.GetCell(j, i).Subtract(
                        ConstantsAndFunctions<SafeDivisionWrapper<TWrapper, TPrimitive>, TWrapper>.MultiplySaveWrapper(
                            m,
                            elemMatrix.GetCell(k - 1, i)
                        )
                    );
            }

            var det = 
                ConstantsAndFunctions<SafeDivisionWrapper<TWrapper, TPrimitive>, TWrapper>.CreateOne();
            for (int i = 0; i < n; i++)
            {
                det.Multiply(elemMatrix.GetCell(i, i));
            }

            if (det.den.IsZero())
                return ConstantsAndFunctions<TWrapper, TPrimitive>.CreateZero().GetValue();
            return det.GetValue().GetValue();
        }

        /// <summary>
        /// Performs simple Gaussian elimination method on a tensor
        /// if you do not change TPrimitives in your TWrapper but
        /// call its methods or change fields,
        /// Rather use DeterminantGaussian(copy: true) so that its
        /// temporarily tensor will not change tensor's values
        /// </summary>
        public TPrimitive DeterminantGaussianSimple()
            => DeterminantGaussianSimple(copy: false);

        // TODO: how to avoid code duplication?
        /// <summary>
        /// Copy = true will slower the performance, but is necessary if your
        /// TPrimitive's pointer does not change through operators
        /// </summary>
        public TPrimitive DeterminantGaussianSimple(bool copy)
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
                var m = ConstantsAndFunctions<TWrapper, TPrimitive>.DivideSaveWrapper(
                    elemMatrix.GetCell(j, k - 1),
                    elemMatrix.GetCell(k - 1, k - 1)
                );
                for (int i = 0; i < n; i++)
                    elemMatrix.GetCell(j, i).Subtract(
                        ConstantsAndFunctions<TWrapper, TPrimitive>.MultiplySaveWrapper(
                            m,
                            elemMatrix.GetCell(k - 1, i)
                        )
                    );
            }

            var det = ConstantsAndFunctions<TWrapper, TPrimitive>.CreateOne();
            for (int i = 0; i < n; i++)
            {
                det.Multiply(elemMatrix.GetCell(i, i));
            }

            return det.GetValue();
        }

        #endregion
    }
}
