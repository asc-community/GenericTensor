using System.Runtime.CompilerServices;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class Inversion<T>
    {
        /// <summary>
        /// Borrowed from here: https://www.geeksforgeeks.org/adjoint-inverse-matrix/
        ///
        /// O(N^2)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetCofactor(GenTensor<T> a, GenTensor<T> temp, int rowId,
            int colId, int diagLength)
        {
            int i = 0, j = 0;
            for (int row = 0; row < diagLength; row++)
            {
                for (int col = 0; col < diagLength; col++)
                {
                    if (row != rowId && col != colId)
                    {
                        temp.SetValueNoCheck(a.GetValueNoCheck(row, col), i, j++);
                        if (j == diagLength - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
        }

        
        public static GenTensor<T> Adjoint(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsSquareMatrix)
                throw new InvalidShapeException("Matrix should be square");
            #endif
            var diagLength = t.Shape.shape[0];
            var res = GenTensor<T>.CreateSquareMatrix(diagLength);
            var temp = SquareMatrixFactory<T>.GetMatrix(diagLength);

            if (diagLength == 1)
            {
                res.SetValueNoCheck(ConstantsAndFunctions<T>.CreateOne(), 0, 0);
                return res;
            }

            var toNegate = false;

            for (int x = 0; x < diagLength; x++)
            for (int y = 0; y < diagLength; y++)
            {
                GetCofactor(t, temp, x, y, diagLength);

                // TODO: is this statement correct?
                toNegate = (x + y) % 2 == 1;
                var det = Determinant<T>.DeterminantGaussianSafeDivision(temp, diagLength - 1);
                if (toNegate)
                    res.SetValueNoCheck(ConstantsAndFunctions<T>.Negate(det), y, x);
                else
                    res.SetValueNoCheck(det, y, x);
            }

            return res;
        }

        public static void InvertMatrix(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsSquareMatrix)
                throw new InvalidShapeException("this should be a square matrix");
            #endif

            var diagLength = t.Shape.shape[0];

            var det = Determinant<T>.DeterminantGaussianSafeDivision(t);
            #if ALLOW_EXCEPTIONS
            if (ConstantsAndFunctions<T>.IsZero(det))
                throw new InvalidDeterminantException("Cannot invert a singular matrix");
            #endif

            var adj = Adjoint(t);
            for (int x = 0; x < diagLength; x++)
            for (int y = 0; y < diagLength; y++)
                t.SetValueNoCheck(
                    ConstantsAndFunctions<T>.Divide(
                        adj.GetValueNoCheck(x, y),
                        det
                    ),
                    x, y
                );
        }

        public static GenTensor<T> MatrixDivide(GenTensor<T> a, GenTensor<T> b)
        {
            #if ALLOW_EXCEPTIONS
            if (!a.IsSquareMatrix || !b.IsSquareMatrix)
                throw new InvalidShapeException("Both should be square matrices");
            if (a.Shape != b.Shape)
                throw new InvalidShapeException("Given matrices should be of the same shape");
            #endif
            var fwd = b.Forward();
            fwd.InvertMatrix();
            return MatrixMultiplication<T>.Multiply(a, fwd);
        }

        public static GenTensor<T> TensorMatrixDivide(GenTensor<T> a, GenTensor<T> b)
        {
            #if ALLOW_EXCEPTIONS
            InvalidShapeException.NeedTensorSquareMatrix(a);
            InvalidShapeException.NeedTensorSquareMatrix(b);
            if (a.Shape != b.Shape)
                throw new InvalidShapeException("Should be of the same shape");
            #endif

            var res = new GenTensor<T>(a.Shape);
            foreach (var ind in res.IterateOverMatrices())
                res.SetSubtensor(
                    MatrixDivide(
                        a.GetSubtensor(ind),
                        b.GetSubtensor(ind)
                        ), ind);

            return res;
        }
        
        public static void TensorMatrixInvert(GenTensor<T> t)
        {
            #if ALLOW_EXCEPTIONS
            InvalidShapeException.NeedTensorSquareMatrix(t);
            #endif

            foreach (var ind in t.IterateOverMatrices())
                t.GetSubtensor(ind).InvertMatrix();
        }
    }
}
