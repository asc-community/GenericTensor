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
using System.Runtime.CompilerServices;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public sealed partial class GenTensor<T, TWrapper>
        : ICloneable where TWrapper : struct, IOperations<T>
    {
        #region Composition
        /// <summary>
        /// Creates a new axis that is put backward
        /// and then sets all elements as children
        /// e. g.
        /// say you have a bunch of tensors {t1, t2, t3} with shape of [2 x 4]
        /// Stack(t1, t2, t3) => T
        /// where T is a tensor of shape of [3 x 2 x 4]
        ///
        /// O(V)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> Stack(params GenTensor<T, TWrapper>[] elements)
            => Composition<T, TWrapper>.Stack(elements);

        /// <summary>
        /// Concatenates two tensors over the first axis,
        /// for example, if you had a tensor of
        /// [4 x 3 x 5] and a tensor of [9 x 3 x 5], their concat's
        /// result will be of shape [13 x 3 x 5]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> Concat(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => Composition<T, TWrapper>.Concat(a, b);

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a tensor whose all matrices are identity matrices
        /// <para>1 is achieved with <see cref="ConstantsAndFunctionsForwarder{T}.CreateOne"/></para>
        /// <para>0 is achieved with <see cref="ConstantsAndFunctionsForwarder{T}.CreateZero"/></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateIdentityTensor(int[] dimensions, int finalMatrixDiag)
            => Constructors<T, TWrapper>.CreateIdentityTensor(dimensions, finalMatrixDiag);

        /// <summary>
        /// Creates an indentity matrix whose width and height are equal to diag
        /// <para>1 is achieved with <see cref="ConstantsAndFunctionsForwarder{T}.CreateOne"/></para>
        /// <para>0 is achieved with <see cref="ConstantsAndFunctionsForwarder{T}.CreateZero"/></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateIdentityMatrix(int diag)
            => Constructors<T, TWrapper>.CreateIdentityMatrix(diag);

        /// <summary>
        /// Creates a vector from an array of primitives
        /// Its length will be equal to elements.Length
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateVector(params T[] elements)
            => Constructors<T, TWrapper>.CreateVector(elements);

        /// <summary>
        /// Creates a vector from an array of primitives
        /// Its length will be equal to elements.Length
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateVector(int length)
            => Constructors<T, TWrapper>.CreateVector(length);

        /// <summary>
        /// Creates a matrix from a two-dimensional array of primitives
        /// for example
        /// <code>
        /// var M = Tensor.CreateMatrix(new[,]
        /// {
        ///     {1, 2},
        ///     {3, 4}
        /// });
        /// </code>
        /// where yourData.GetLength(0) is Shape[0] and
        /// yourData.GetLength(1) is Shape[1]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateMatrix(T[,] data)
            => Constructors<T, TWrapper>.CreateMatrix(data);

        /// <summary>
        /// Creates an uninitialized square matrix
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateSquareMatrix(int diagLength)
            => Constructors<T, TWrapper>.CreateSquareMatrix(diagLength);

        /// <summary>
        /// Creates a matrix of width and height size
        /// and iterator for each pair of coordinate
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateMatrix(int width, int height, Func<int, int, T> stepper)
            => Constructors<T, TWrapper>.CreateMatrix(width, height, stepper);

        /// <summary>
        /// Creates a matrix of width and height size
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateMatrix(int width, int height)
            => Constructors<T, TWrapper>.CreateMatrix(width, height);

        /// <summary>
        /// Creates a tensor of given size with iterator over its indices
        /// (its only argument is an array of integers which are indices of the tensor)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(TensorShape shape, Func<int[], T> operation, Threading threading = Threading.Single)
            => Constructors<T, TWrapper>.CreateTensor(shape, operation, threading);

        /// <summary>
        /// Creates a tensor from an array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(T[] data)
            => Constructors<T, TWrapper>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from a two-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(T[,] data)
            => Constructors<T, TWrapper>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from a three-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(T[,,] data)
            => Constructors<T, TWrapper>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from an n-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(Array data)
            => Constructors<T, TWrapper>.CreateTensor(data);

        #endregion

        #region Determinant

        /// <summary>
        /// Finds Determinant with the 100% precision for O(N!) where
        /// N is your matrix' width
        /// The matrix should be square
        /// Borrowed from here: https://www.geeksforgeeks.org/adjoint-inverse-matrix/
        ///
        /// O(N!)
        /// </summary>
        public T DeterminantLaplace()
            => Determinant<T, TWrapper>.DeterminantLaplace(this);

        /// <summary>
        /// Finds Determinant with possible overflow
        /// because it uses fractions for avoiding division
        ///
        /// O(N^3)
        /// </summary>
        public T DeterminantGaussianSafeDivision()
            => Determinant<T, TWrapper>.DeterminantGaussianSafeDivision(this);

        /// <summary>
        /// Decomposes a matrix into a triangular one
        /// </summary>
        public GenTensor<T, TWrapper> GaussianEliminationSafeDivision()
            => Determinant<T, TWrapper>.GaussianEliminationSafeDivision(this);

        // TODO: how to avoid code duplication?
        /// <summary>
        /// Performs simple Gaussian elimination method on a tensor
        ///
        /// O(N^3)
        /// </summary>
        public T DeterminantGaussianSimple()
            => Determinant<T, TWrapper>.DeterminantGaussianSimple(this);

        /// <summary>
        /// Computers Laplace's Determinant for all
        /// matrices in the tensor
        /// </summary>
        public GenTensor<T, TWrapper> TensorDeterminantLaplace()
            => Determinant<T, TWrapper>.TensorDeterminantLaplace(this);

        /// <summary>
        /// Computers Determinant via Guassian elimination & safe division
        /// for all matrices in the tensor
        /// </summary>
        public GenTensor<T, TWrapper> TensorDeterminantGaussianSafeDivision()
            => Determinant<T, TWrapper>.TensorDeterminantGaussianSafeDivision(this);

        /// <summary>
        /// Computers Determinant via Guassian elimination
        /// for all matrices in the tensor
        /// </summary>
        public GenTensor<T, TWrapper> TensorDeterminantGaussianSimple()
            => Determinant<T, TWrapper>.TensorDeterminantGaussianSimple(this);

        #endregion

        #region Inversion

        /// <summary>
        /// Returns adjugate matrix
        ///
        /// O(N^5)
        /// </summary>
        public GenTensor<T, TWrapper> Adjoint()
            => Inversion<T, TWrapper>.Adjoint(this);

        /// <summary>
        /// Inverts a matrix A to B so that A * B = I
        /// Borrowed from here: https://www.geeksforgeeks.org/adjoint-inverse-matrix/
        ///
        /// O(N^5)
        /// </summary>
        public void InvertMatrix()
            => Inversion<T, TWrapper>.InvertMatrix(this);

        /// <summary>
        /// Inverts all matrices in a tensor
        /// </summary>
        public void TensorMatrixInvert()
            => Inversion<T, TWrapper>.TensorMatrixInvert(this);
        #endregion

        #region Matrix multiplication & division

        /// <summary>
        /// A / B
        /// Finds such C = A / B that A = C * B
        ///
        /// O(N^5)
        /// </summary>
        public static GenTensor<T, TWrapper> MatrixDivide(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => Inversion<T, TWrapper>.MatrixDivide(a, b);

        /// <summary>
        /// Divides all matrices from tensor a over tensor b and returns a new
        /// tensor with them divided
        /// </summary>
        public static GenTensor<T, TWrapper> TensorMatrixDivide(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => Inversion<T, TWrapper>.TensorMatrixDivide(a, b);


        /// <summary>
        /// Finds matrix multiplication result
        /// a and b are matrices
        /// a.Shape[1] should be equal to b.Shape[0]
        /// the resulting matrix is [a.Shape[0] x b.Shape[1]] shape
        ///
        /// O(N^3)
        /// </summary>
        public static GenTensor<T, TWrapper> MatrixMultiply(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => MatrixMultiplication<T, TWrapper>.Multiply(a, b, threading);

        /// <summary>
        /// Applies matrix dot product operation for
        /// all matrices in tensors
        ///
        /// O(N^3)
        /// </summary>
        public static GenTensor<T, TWrapper> TensorMatrixMultiply(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => MatrixMultiplication<T, TWrapper>.TensorMultiply(a, b, threading);

        #endregion

        #region Piecewise arithmetics

        /// <summary>
        /// T1 + T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseAdd(a, b, threading);

        /// <summary>
        /// T1 - T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// T1 * T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseMultiply(a, b, threading);

        /// <summary>
        /// T1 / T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseDivide(a, b, threading);

        /// <summary>
        /// T1 + const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseAdd(a, b, threading);

        /// <summary>
        /// T1 - const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// const - T1
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseSubtract(T a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// T1 * const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseMultiply(a, b, threading);

        /// <summary>
        /// T1 / const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseDivide(a, b, threading);

        /// <summary>
        /// const / T1
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseDivide(T a, GenTensor<T, TWrapper> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseDivide(a, b, threading);

        #endregion

        #region Power

        /// <summary>
        /// Binary power
        /// 
        /// n positive:
        /// A ^ n = A * A * ... * A
        ///
        /// n negative:
        /// A ^ n = (A^(-1) * A^(-1) * ... * A^(-1))
        ///
        /// n == 0:
        /// A ^ n = I
        ///
        /// O(log(power) * N^3)
        /// </summary>
        public GenTensor<T, TWrapper> MatrixPower(int power, Threading threading = Threading.Single)
            => Power<T, TWrapper>.MatrixPower(this, power, threading);

        /// <summary>
        /// Performs MatrixPower operation on all matrices from this tensor
        /// </summary>
        public GenTensor<T, TWrapper> TensorMatrixPower(int power, Threading threading = Threading.Single)
            => Power<T, TWrapper>.TensorMatrixPower(this, power, threading);

        #endregion

        #region ToString & GetHashCode

        public override string ToString()
            => DefaultOverridings<T, TWrapper>.InToString(this);

        public override int GetHashCode()
            => DefaultOverridings<T, TWrapper>.GetHashCode(this);

        #endregion

        #region Vector operations

        /// <summary>
        /// Finds a perpendicular vector to two given
        /// TODO: So far only implemented for 3D vectors
        /// </summary>
        public static GenTensor<T, TWrapper> VectorCrossProduct(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => VectorProduct<T, TWrapper>.VectorCrossProduct(a, b);

        /// <summary>
        /// Calls VectorCrossProduct for every vector in the tensor
        /// </summary>
        public static GenTensor<T, TWrapper> TensorVectorCrossProduct(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => VectorProduct<T, TWrapper>.TensorVectorCrossProduct(a, b);

        /// <summary>
        /// Finds the scalar product of two vectors
        ///
        /// O(N)
        /// </summary>
        public static T VectorDotProduct(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => VectorProduct<T, TWrapper>.VectorDotProduct(a, b);

        /// <summary>
        /// Applies scalar product to every vector in a tensor so that
        /// you will get a one-reduced dimensional tensor
        /// (e. g. TensorVectorDotProduct([4 x 3 x 2], [4 x 3 x 2]) -> [4 x 3]
        ///
        /// O(V)
        /// </summary>
        public static GenTensor<T, TWrapper> TensorVectorDotProduct(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => VectorProduct<T, TWrapper>.TensorVectorDotProduct(a, b);

        #endregion

        #region Copy and forward

        /// <summary>
        /// Copies a tensor calling each cell with a .Copy()
        ///
        /// O(V)
        /// </summary>
        public GenTensor<T, TWrapper> Copy(bool copyElements)
            => CopyAndForward<T, TWrapper>.Copy(this, copyElements);

        /// <summary>
        /// You might need it to make sure you don't copy
        /// your data but recreate a wrapper (if have one)
        ///
        /// O(V)
        /// </summary>
        public GenTensor<T, TWrapper> Forward()
            => CopyAndForward<T, TWrapper>.Forward(this);

        #endregion
    }
}
