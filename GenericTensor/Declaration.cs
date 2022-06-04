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
using System.Runtime.CompilerServices;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    /// <summary>
    /// The main class of tensor.
    /// </summary>
    /// <typeparam name="T">The primitive to be an element in the tensor</typeparam>
    /// <typeparam name="TWrapper">The set of rules for working with primitives</typeparam>
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


        /// <summary>
        /// Works similary to Linq's Aggregate, but aggregates over the given axis
        /// and mutates the given accumulated value.
        /// </summary>
        /// <param name="tensor">
        /// The tensor to aggregate over.
        /// Shape: N1 x N2 x N3 x ... Nn
        /// </param>
        /// <param name="accumulated">
        /// The base value of the aggregation,
        /// simultaneously being the destination tensor
        /// for accumulation (so <see cref="Aggregate{TAggregatorFunc, U, UWrapper}(GenTensor{T, TWrapper}, GenTensor{U, UWrapper}, TAggregatorFunc, int)"/>
        /// does not return a new tensor, but instead
        /// modifiers the accumulated value.
        /// Shape: N1 x ... x N[<paramref name="axis"/>-1] x N[<paramref name="axis"/>+1] x ... x Nn
        /// </param>
        /// <param name="accumulator">
        /// Function which maps accumulated value and the current one
        /// to the new value.
        /// </param>
        /// <param name="axis">
        /// The number of axis to aggregate over.
        /// </param>
        public static void Aggregate<TAggregatorFunc, U, UWrapper>(GenTensor<T, TWrapper> tensor, GenTensor<U, UWrapper> accumulated, TAggregatorFunc accumulator, int axis)
            where TAggregatorFunc : struct, HonkPerf.NET.Core.IValueDelegate<U, T, U>
            where UWrapper : struct, IOperations<U>
            => Composition<T, TWrapper>.Aggregate(tensor, accumulated, accumulator, axis);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a tensor whose all matrices are identity matrices
        /// <para>1 is achieved with <see cref="IOperations{T}.CreateOne"/></para>
        /// <para>0 is achieved with <see cref="IOperations{T}.CreateZero"/></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateIdentityTensor(int[] dimensions, int finalMatrixDiag)
            => Constructors<T, TWrapper>.CreateIdentityTensor(dimensions, finalMatrixDiag);

        /// <summary>
        /// Creates an indentity matrix whose width and height are equal to diag
        /// <para>1 is achieved with <see cref="IOperations{T}.CreateOne"/></para>
        /// <para>0 is achieved with <see cref="IOperations{T}.CreateZero"/></para>
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
        public static GenTensor<T, TWrapper> CreateTensor(TensorShape shape, Func<int[], T> operation, Threading threading = Threading.Single) => Constructors<T, TWrapper>.CreateTensor(shape, operation, threading);

        /// <summary>
        /// Creates a tensor from an array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(T[] data) => Constructors<T, TWrapper>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from a two-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(T[,] data) => Constructors<T, TWrapper>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from a three-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(T[,,] data) => Constructors<T, TWrapper>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from an n-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> CreateTensor(Array data) => Constructors<T, TWrapper>.CreateTensor(data);

        #endregion

        #region Echelon forms

        /// <summary>
        /// Decomposes a matrix into a triangular one.
        /// Is of the Row Echelon Form (leading elements might be differ from ones).
        /// </summary>
        public GenTensor<T, TWrapper> RowEchelonFormSimple()
            => EchelonForm<T, TWrapper>.RowEchelonFormSimple(this);
        
        /// <summary>
        /// Decomposes a matrix into a triangular one.
        /// Is of the Row Echelon Form (leading elements might be differ from ones).
        /// 
        /// In addition, returns a permutation that algorithm performs with rows.
        /// Permutation array is size of rows there are in matrix.
        ///
        /// Initial state of that array is:
        /// 1 2 3 ... numberOfRows
        /// 
        /// For example, algorithm swaps first and third rows then:
        /// 3 2 1 ... numberOfRows
        ///
        /// It can be useful performing decompositions
        /// </summary>
        public (GenTensor<T, TWrapper>, int[]) RowEchelonFormPermuteSimple()
            => EchelonForm<T, TWrapper>.RowEchelonFormPermuteSimple(this);
        
        /// <summary>
        /// Decomposes a matrix into a triangular one.
        /// Is of the Row Echelon Form (leading elements might be differ from ones).
        /// 
        /// In addition, returns a permutation that algorithm performs with rows.
        /// Permutation array is size of rows there are in matrix.
        ///
        /// Initial state of that array is:
        /// 1 2 3 ... numberOfRows
        /// 
        /// For example, algorithm swaps first and third rows then:
        /// 3 2 1 ... numberOfRows
        ///
        /// It can be useful performing decompositions
        /// </summary>
        public (GenTensor<T, TWrapper>, int[]) RowEchelonFormPermuteSafeDivision()
            => EchelonForm<T, TWrapper>.RowEchelonFormPermuteSafeDivision(this);

        /// <summary>
        /// Decomposes a matrix into a triangular one.
        /// Is of the Row Echelon Form (leading elements might be differ from ones).
        /// Uses safe division, i. e. perform division only when computing the final result.
        /// </summary>
        public GenTensor<T, TWrapper> RowEchelonFormSafeDivision()
            => EchelonForm<T, TWrapper>.RowEchelonFormSafeDivision(this);


        /// <summary>
        /// Decomposes a matrix into a triangular one.
        /// Is of the Row Echelon Form (leading elements are ones).
        /// </summary>
        /// <returns></returns>
        public GenTensor<T, TWrapper> RowEchelonFormLeadingOnesSimple()
            => EchelonForm<T, TWrapper>.RowEchelonFormLeadingOnesSimple(this);

        /// <summary>
        /// Decomposes a matrix into a triangular one.
        /// Is of the Row Echelon Form (leading elements are ones).
        /// Uses safe division, i. e. perform division only when computing the final result.
        /// </summary>
        /// <returns></returns>
        public GenTensor<T, TWrapper> RowEchelonFormLeadingOnesSafeDivision()
            => EchelonForm<T, TWrapper>.RowEchelonFormLeadingOnesSafeDivision(this);


        /// <summary>
        /// Finds the reduced echelon form of a matrix.
        /// </summary>
        public GenTensor<T, TWrapper> ReducedRowEchelonFormSimple()
            => EchelonForm<T, TWrapper>.ReducedRowEchelonFormSimple(this);

        /// <summary>
        /// Finds the reduced echelon form of a matrix.
        /// Uses safe division, i. e. perform division only when computing the final result.
        /// </summary>
        public GenTensor<T, TWrapper> ReducedRowEchelonFormSafeDivision()
            => EchelonForm<T, TWrapper>.ReducedRowEchelonFormSafeDivision(this);
        
        /// <summary>
        /// Finds the reduced echelon form of a matrix.
        /// Uses safe division, i. e. perform division only when computing the final result.
        ///
        /// Additionally returns row permutation
        /// </summary>
        public (GenTensor<T, TWrapper>, int[]) ReducedRowEchelonFormPermuteSafeDivision()
            => EchelonForm<T, TWrapper>.ReducedRowEchelonFormPermuteSafeDivision(this);

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
        /// Computers Determinant via Guassian elimination and safe division
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

        #region Elementary matrix operations

        /// <summary>
        /// Multiples the given row by the given coefficient.
        /// Modifies the matrix.
        /// </summary>
        public void RowMultiply(int rowId, T coef)
            => ElementaryRowOperations<T, TWrapper>.RowMultiply(this, rowId, coef);

        /// <summary>
        /// To the first row adds the second row multiplied by the coef.
        /// Modifies the matrix.
        /// </summary>
        public void RowAdd(int dstRowId, int srcRowId, T coef)
            => ElementaryRowOperations<T, TWrapper>.RowAdd(this, dstRowId, srcRowId, coef);

        /// <summary>
        /// From the first row subtracts the second row multiplied by the coef.
        /// Modifies the matrix.
        /// </summary>
        public void RowSubtract(int dstRowId, int srcRowId, T coef)
            => ElementaryRowOperations<T, TWrapper>.RowSubtract(this, dstRowId, srcRowId, coef);

        /// <summary>
        /// Swaps the given two rows.
        /// Modifies the matrix.
        /// </summary>
        public void RowSwap(int row1Id, int row2Id)
            => ElementaryRowOperations<T, TWrapper>.RowSwap(this, row1Id, row2Id);

        /// <summary>
        /// Finds the leading element of the row (the first non-zero element).
        /// </summary>
        /// <returns>
        /// Null if all elements are zero,
        /// Tuple of index and value of the first non-zero element otherwise
        /// </returns>
        public (int index, T value)? RowGetLeadingElement(int rowId)
            => ElementaryRowOperations<T, TWrapper>.LeadingElement(this, rowId);

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
        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseAdd(a, b, threading);

        /// <summary>
        /// T1 - T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// T1 * T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseMultiply(a, b, threading);

        /// <summary>
        /// T1 / T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseDivide(a, b, threading);

        /// <summary>
        /// T1 + const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a, T b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseAdd(a, b, threading);

        /// <summary>
        /// T1 - const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a, T b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// const - T1
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseSubtract(T a, GenTensor<T, TWrapper> b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// T1 * const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a, T b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseMultiply(a, b, threading);

        /// <summary>
        /// T1 / const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a, T b,
            Threading threading = Threading.Single)
            => PiecewiseArithmetics<T, TWrapper>.PiecewiseDivide(a, b, threading);

        /// <summary>
        /// const / T1
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T, TWrapper> PiecewiseDivide(T a, GenTensor<T, TWrapper> b,
            Threading threading = Threading.Single)
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

        #region Decompositions

        /// <summary>
        /// https://www.geeksforgeeks.org/l-u-decomposition-system-linear-equations/
        /// </summary>
        /// <returns>
        /// LU decomposition
        /// </returns>
        public (GenTensor<T, TWrapper> l, GenTensor<T, TWrapper> u) LuDecomposition()
            => LuDecomposition<T, TWrapper>.Decompose(this);
        
        /// <summary>
        /// Find PLU decomposition: matrices P, L, U such that for original matrix A: PA = LU.
        /// 
        /// P stands for permutation matrix, permutations are made during the Gauss elimination process
        /// L stands for LIBERTY lower triangle matrix
        /// U stands for upper triangle matrix
        ///
        /// Algorithm, given matrix A:
        /// 1. Form an adjacent matrix (A|E)
        /// 2. Find row echelon form of that matrix (U|L_0) and permutation of the rows
        /// 3. Form permutation matrix P such that P_ij = \delta_{}
        /// 4. Compute L = P * L_0^{-1}
        ///
        /// Results are: P, L, U
        /// </summary>
        /// <returns>
        /// LUP decomposition of given matrix
        /// </returns>
        public (GenTensor<T, TWrapper> p, GenTensor<T, TWrapper> l, GenTensor<T, TWrapper> u) PluDecomposition()
            => PluDecomposition<T, TWrapper>.Decompose(this);

        #endregion

        #region ToString & GetHashCode

        /// <inheritdoc/>
        public override string ToString()
            => DefaultOverridings<T, TWrapper>.InToString(this);

        /// <inheritdoc/>
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
        public static GenTensor<T, TWrapper> TensorVectorCrossProduct(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b)
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

        #region Serialization

        /*
         * Serialization protocol:
         *
         * First int n: number of dimensions
         * Next n ints Shapes: dimensions' lengths
         * Next Shapes[0] * Shapes[1] * ...:
         *     First int n: size
         *     Next n bytes: data
         *
         */

        /// <summary>
        /// Serializes this to a byte array so it can be easily
        /// transmitted or stored
        /// </summary>
        /// <returns>
        /// Byte array with the serialized object in the serialization protocol
        /// </returns>
        public byte[] Serialize()
            => Serializer<T, TWrapper>.Serialize(this);

        /// <summary>
        /// Deserializes data into a tensor
        /// </summary>
        /// <param name="data">
        /// Byte array which must follow the serialization protocol
        /// </param>
        /// <returns>
        /// A tensor with the same data as stored before serialization
        /// (if serialization and deserialization in TWrapper work correctly)
        /// </returns>
        public static GenTensor<T, TWrapper> Deserialize(byte[] data)
            => Serializer<T, TWrapper>.Deserialize(data);

        #endregion
    }
}