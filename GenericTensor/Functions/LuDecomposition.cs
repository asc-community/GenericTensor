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
    internal static class LuDecomposition<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        public static (GenTensor<T, TWrapper>, GenTensor<T, TWrapper>) Decompose(GenTensor<T, TWrapper> t)
        {
#if ALLOW_EXCEPTIONS
            if (!t.IsSquareMatrix)
                throw new InvalidShapeException("this should be a square matrix");
#endif

            var n = t.Shape[0];
            var lower = GenTensor<T, TWrapper>.CreateMatrix(n, n);
            var upper = GenTensor<T, TWrapper>.CreateMatrix(n, n);

            var tw = default(TWrapper);
            var zero = tw.CreateZero();
            var one = tw.CreateOne();

            for (var i = 0; i < n; i++)
            {
                // Upper triangular
                for (var k = i; k < n; k++)
                {
                    var sum = zero;
                    for (var j = 0; j < i; j++)
                        sum = tw.Add(sum, tw.Multiply(lower[i, j], upper[j, k]));

                    upper[i, k] = tw.Subtract(t[i, k], sum);
                }

                // Lower triangular
                for (var k = i; k < n; k++)
                {
                    if (i == k)
                        lower[i, i] = one; // 1 at diagonals
                    else
                    {
                        var sum = zero;
                        for (var j = 0; j < i; j++)
                            sum = tw.Add(sum, tw.Multiply(lower[k, j], upper[j, i]));

                        #if ALLOW_EXCEPTIONS
                        if (Equals(upper[i, i], zero))
                        {
                            throw new ImpossibleDecomposition("There is no LU decomposition for given matrix");
                        }
                        #endif
                        
                        lower[k, i]
                            = tw.Divide(tw.Subtract(t[k, i], sum), upper[i, i]);
                    }
                }
            }

            return (lower, upper);
        }
    }
}