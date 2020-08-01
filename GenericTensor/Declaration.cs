using System;
using System.Runtime.CompilerServices;
using System.Threading;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class GenTensor<T>
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
        public static GenTensor<T> Stack(params GenTensor<T>[] elements)
            => Composition<T>.Stack(elements);

        /// <summary>
        /// Concatenates two tensors over the first axis,
        /// for example, if you had a tensor of
        /// [4 x 3 x 5] and a tensor of [9 x 3 x 5], their concat's
        /// result will be of shape [13 x 3 x 5]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> Concat(GenTensor<T> a, GenTensor<T> b)
            => Composition<T>.Concat(a, b);

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a tensor whose all matrices are identity matrices
        /// <para>1 is achieved with <see cref="ConstantsAndFunctions{T}.CreateOne"/></para>
        /// <para>0 is achieved with <see cref="ConstantsAndFunctions{T}.CreateZero"/></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateIdentityTensor(int[] dimensions, int finalMatrixDiag)
            => Constructors<T>.CreateIdentityTensor(dimensions, finalMatrixDiag);

        /// <summary>
        /// Creates an indentity matrix whose width and height are equal to diag
        /// <para>1 is achieved with <see cref="ConstantsAndFunctions{T}.CreateOne"/></para>
        /// <para>0 is achieved with <see cref="ConstantsAndFunctions{T}.CreateZero"/></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateIdentityMatrix(int diag)
            => Constructors<T>.CreateIdentityMatrix(diag);

        /// <summary>
        /// Creates a vector from an array of primitives
        /// Its length will be equal to elements.Length
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateVector(params T[] elements)
            => Constructors<T>.CreateVector(elements);

        /// <summary>
        /// Creates a vector from an array of primitives
        /// Its length will be equal to elements.Length
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateVector(int length)
            => Constructors<T>.CreateVector(length);

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
        public static GenTensor<T> CreateMatrix(T[,] data)
            => Constructors<T>.CreateMatrix(data);

        /// <summary>
        /// Creates an uninitialized square matrix
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateSquareMatrix(int diagLength)
            => Constructors<T>.CreateSquareMatrix(diagLength);

        /// <summary>
        /// Creates a matrix of width and height size
        /// and iterator for each pair of coordinate
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateMatrix(int width, int height, Func<int, int, T> stepper)
            => Constructors<T>.CreateMatrix(width, height, stepper);

        /// <summary>
        /// Creates a matrix of width and height size
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateMatrix(int width, int height)
            => Constructors<T>.CreateMatrix(width, height);

        /// <summary>
        /// Creates a tensor of given size with iterator over its indices
        /// (its only argument is an array of integers which are indices of the tensor)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateTensor(TensorShape shape, Func<int[], T> operation, Threading threading = Threading.Single)
            => Constructors<T>.CreateTensor(shape, operation, threading);

        /// <summary>
        /// Creates a tensor from an array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateTensor(T[] data)
            => Constructors<T>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from a two-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateTensor(T[,] data)
            => Constructors<T>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from a three-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateTensor(T[,,] data)
            => Constructors<T>.CreateTensor(data);

        /// <summary>
        /// Creates a tensor from an n-dimensional array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateTensor(Array data)
            => Constructors<T>.CreateTensor(data);

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
            => Determinant<T>.DeterminantLaplace(this);

        /// <summary>
        /// Finds Determinant with possible overflow
        /// because it uses fractions for avoiding division
        ///
        /// O(N^3)
        /// </summary>
        public T DeterminantGaussianSafeDivision()
            => Determinant<T>.DeterminantGaussianSafeDivision(this);

        /// <summary>
        /// Decomposes a matrix into a triangular one
        /// </summary>
        public GenTensor<T> GaussianEliminationSafeDivision()
            => Determinant<T>.GaussianEliminationSafeDivision(this);

        // TODO: how to avoid code duplication?
        /// <summary>
        /// Performs simple Gaussian elimination method on a tensor
        ///
        /// O(N^3)
        /// </summary>
        public T DeterminantGaussianSimple()
            => Determinant<T>.DeterminantGaussianSimple(this);

        /// <summary>
        /// Computers Laplace's Determinant for all
        /// matrices in the tensor
        /// </summary>
        public GenTensor<T> TensorDeterminantLaplace()
            => Determinant<T>.TensorDeterminantLaplace(this);

        /// <summary>
        /// Computers Determinant via Guassian elimination & safe division
        /// for all matrices in the tensor
        /// </summary>
        public GenTensor<T> TensorDeterminantGaussianSafeDivision()
            => Determinant<T>.TensorDeterminantGaussianSafeDivision(this);

        /// <summary>
        /// Computers Determinant via Guassian elimination
        /// for all matrices in the tensor
        /// </summary>
        public GenTensor<T> TensorDeterminantGaussianSimple()
            => Determinant<T>.TensorDeterminantGaussianSimple(this);

        #endregion

        #region Inversion

        /// <summary>
        /// Returns adjugate matrix
        ///
        /// O(N^5)
        /// </summary>
        public GenTensor<T> Adjoint()
            => Inversion<T>.Adjoint(this);

        /// <summary>
        /// Inverts a matrix A to B so that A * B = I
        /// Borrowed from here: https://www.geeksforgeeks.org/adjoint-inverse-matrix/
        ///
        /// O(N^5)
        /// </summary>
        public void InvertMatrix()
            => Inversion<T>.InvertMatrix(this);

        /// <summary>
        /// Inverts all matrices in a tensor
        /// </summary>
        public void TensorMatrixInvert()
            => Inversion<T>.TensorMatrixInvert(this);
        #endregion

        #region Matrix multiplication & division

        /// <summary>
        /// A / B
        /// Finds such C = A / B that A = C * B
        ///
        /// O(N^5)
        /// </summary>
        public static GenTensor<T> MatrixDivide(GenTensor<T> a, GenTensor<T> b)
            => Inversion<T>.MatrixDivide(a, b);

        /// <summary>
        /// Divides all matrices from tensor a over tensor b and returns a new
        /// tensor with them divided
        /// </summary>
        public static GenTensor<T> TensorMatrixDivide(GenTensor<T> a, GenTensor<T> b)
            => Inversion<T>.TensorMatrixDivide(a, b);


        /// <summary>
        /// Finds matrix multiplication result
        /// a and b are matrices
        /// a.Shape[1] should be equal to b.Shape[0]
        /// the resulting matrix is [a.Shape[0] x b.Shape[1]] shape
        ///
        /// O(N^3)
        /// </summary>
        public static GenTensor<T> MatrixMultiply(GenTensor<T> a, GenTensor<T> b, Threading threading = Threading.Single)
            => MatrixMultiplication<T>.Multiply(a, b, threading);

        /// <summary>
        /// Applies matrix dot product operation for
        /// all matrices in tensors
        ///
        /// O(N^3)
        /// </summary>
        public static GenTensor<T> TensorMatrixMultiply(GenTensor<T> a, GenTensor<T> b, Threading threading = Threading.Single)
            => MatrixMultiplication<T>.TensorMultiply(a, b, threading);

        #endregion

        #region Piecewise arithmetics

        /// <summary>
        /// [i, j, k...]th element of the resulting tensor is
        /// operation(a[i, j, k...], b[i, j, k...])
        /// </summary>
        public static GenTensor<T> Zip(GenTensor<T> a, GenTensor<T> b, Func<T, T, T> operation)
            => PiecewiseArithmetics<T>.Zip(a, b, operation);

        /// <summary>
        /// T1 + T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseAdd(GenTensor<T> a, GenTensor<T> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseAdd(a, b, threading);

        /// <summary>
        /// T1 - T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseSubtract(GenTensor<T> a, GenTensor<T> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// T1 * T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseMultiply(GenTensor<T> a, GenTensor<T> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseMultiply(a, b, threading);

        /// <summary>
        /// T1 / T2
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseDivide(GenTensor<T> a, GenTensor<T> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseDivide(a, b, threading);

        /// <summary>
        /// T1 + const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseAdd(GenTensor<T> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseAdd(a, b, threading);

        /// <summary>
        /// T1 - const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseSubtract(GenTensor<T> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// const - T1
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseSubtract(T a, GenTensor<T> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseSubtract(a, b, threading);

        /// <summary>
        /// T1 * const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseMultiply(GenTensor<T> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseMultiply(a, b, threading);

        /// <summary>
        /// T1 / const
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseDivide(GenTensor<T> a, T b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseDivide(a, b, threading);

        /// <summary>
        /// const / T1
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> PiecewiseDivide(T a, GenTensor<T> b, Threading threading = Threading.Single)
            => PiecewiseArithmetics<T>.PiecewiseDivide(a, b, threading);

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
        public GenTensor<T> MatrixPower(int power)
            => Power<T>.MatrixPower(this, power);

        /// <summary>
        /// Performs MatrixPower operation on all matrices from this tensor
        /// </summary>
        public GenTensor<T> TensorMatrixPower(int power)
            => Power<T>.TensorMatrixPower(this, power);

        #endregion

        #region ToString & GetHashCode

        public override string ToString()
            => DefaultOverridings<T>.InToString(this);

        public override int GetHashCode()
            => DefaultOverridings<T>.GetHashCode(this);

        #endregion

        #region Vector operations

        /// <summary>
        /// Finds a perpendicular vector to two given
        /// TODO: So far only implemented for 3D vectors
        /// </summary>
        public static GenTensor<T> VectorCrossProduct(GenTensor<T> a, GenTensor<T> b)
            => VectorProduct<T>.VectorCrossProduct(a, b);

        /// <summary>
        /// Calls VectorCrossProduct for every vector in the tensor
        /// </summary>
        public static GenTensor<T> TensorVectorCrossProduct(GenTensor<T> a, GenTensor<T> b)
            => VectorProduct<T>.TensorVectorCrossProduct(a, b);

        /// <summary>
        /// Finds the scalar product of two vectors
        ///
        /// O(N)
        /// </summary>
        public static T VectorDotProduct(GenTensor<T> a, GenTensor<T> b)
            => VectorProduct<T>.VectorDotProduct(a, b);

        /// <summary>
        /// Applies scalar product to every vector in a tensor so that
        /// you will get a one-reduced dimensional tensor
        /// (e. g. TensorVectorDotProduct([4 x 3 x 2], [4 x 3 x 2]) -> [4 x 3]
        ///
        /// O(V)
        /// </summary>
        public static GenTensor<T> TensorVectorDotProduct(GenTensor<T> a, GenTensor<T> b)
            => VectorProduct<T>.TensorVectorDotProduct(a, b);

        #endregion

        #region Copy and forward

        /// <summary>
        /// Copies a tensor calling each cell with a .Copy()
        ///
        /// O(V)
        /// </summary>
        public GenTensor<T> Copy(bool copyElements)
            => CopyAndForward<T>.Copy(this, copyElements);

        /// <summary>
        /// You might need it to make sure you don't copy
        /// your data but recreate a wrapper (if have one)
        ///
        /// O(V)
        /// </summary>
        public GenTensor<T> Forward()
            => CopyAndForward<T>.Forward(this);

        #endregion
    }
}
