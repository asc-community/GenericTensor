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


using System;
using System.Data;

namespace GenericTensor.Core
{
    /// <summary>
    /// Occurs when an axis mismatch happens
    /// </summary>
    public class InvalidShapeException : ArgumentException
    {
        internal InvalidShapeException(string msg) : base(msg) {}
        internal InvalidShapeException() : base() {}

        internal static void NeedTensorSquareMatrix<T, TWrapper>(GenTensor<T, TWrapper> m) where TWrapper : struct, IOperations<T>
        {
            if (m.Shape.Length <= 2)
                throw new InvalidShapeException("Should be 3+ dimensional");
            if (m.Shape.shape[m.Shape.Length - 1] != m.Shape.shape[m.Shape.Length - 2])
                throw new InvalidShapeException("The last two dimensions should be equal");
        }
    }

    /// <summary>
    /// Thrown when a wrong determinant tensor was provided
    /// </summary>
    public class InvalidDeterminantException : DataException
    {
        internal InvalidDeterminantException(string msg) : base(msg) {}
        internal InvalidDeterminantException() : base() {}
    }
    
    /// <summary>
    /// Thrown when there is no decomposition for provided matrix
    /// </summary>
    public class ImpossibleDecomposition : DataException
    {
        internal ImpossibleDecomposition(string msg) : base(msg) {}
        internal ImpossibleDecomposition() : base() {}
    }
}
