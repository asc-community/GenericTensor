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

namespace GenericTensor.Functions
{
    internal static class Power<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        public static GenTensor<T, TWrapper> MatrixPower(GenTensor<T, TWrapper> m, int power, Threading threading)
        {
            #if ALLOW_EXCEPTIONS
            if (!m.IsSquareMatrix)
                throw new InvalidShapeException("Square matrix required");
            #endif
            if (power == 0)
                return Constructors<T, TWrapper>.CreateIdentityMatrix(m.Shape.shape[0]);
            if (power < 0)
            {
                m = m.Forward();
                m.InvertMatrix();
                power *= -1;
            }
            if (power == 1)
                return m;
            if (power == 2)
                return MatrixMultiplication<T, TWrapper>.Multiply(m, m, threading);
            var half = power / 2;
            var m1 = MatrixPower(m, half, threading);
            var dotted = MatrixMultiplication<T, TWrapper>.Multiply(m1, m1, threading);
            if (power % 2 == 0)
                return dotted;
            else
                return MatrixMultiplication<T, TWrapper>.Multiply(dotted, m, threading);
        }

        public static GenTensor<T, TWrapper> TensorMatrixPower(GenTensor<T, TWrapper> m, int power, Threading threading)
        {
            #if ALLOW_EXCEPTIONS
            InvalidShapeException.NeedTensorSquareMatrix(m);
            #endif

            var res = new GenTensor<T, TWrapper>(m.Shape);
            foreach (var ind in res.IterateOverMatrices())
                res.SetSubtensor(
                    MatrixPower(m.GetSubtensor(ind), power, threading),
                    ind
                );

            return res;
        }
    }
}
