using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class GenTensor<T>
    {
        /// <summary>
        /// Returns adjugate matrix
        ///
        /// O(N^5)
        /// </summary>
        public GenTensor<T> Adjoint()
        {
            #if ALLOW_EXCEPTIONS
            if (!IsSquareMatrix)
                throw new InvalidShapeException("Matrix should be square");
            #endif
            var diagLength = Shape.shape[0];
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
                GetCofactor(this, temp, x, y, diagLength);
                toNegate = (x + y) % 2 == 1;
                var det = temp.DeterminantGaussianSafeDivision(diagLength - 1);
                if (toNegate)
                    res.SetValueNoCheck(ConstantsAndFunctions<T>.Negate(det), y, x);
                else
                    res.SetValueNoCheck(det, y, x);
            }

            return res;
        }

        /// <summary>
        /// Inverts a matrix A to B so that A * B = I
        /// Borrowed from here: https://www.geeksforgeeks.org/adjoint-inverse-matrix/
        ///
        /// O(N^5)
        /// </summary>
        public void InvertMatrix()
        {
            #if ALLOW_EXCEPTIONS
            if (!IsSquareMatrix)
                throw new InvalidShapeException("this should be a square matrix");
            #endif

            var diagLength = Shape.shape[0];

            var det = DeterminantGaussianSafeDivision();
            #if ALLOW_EXCEPTIONS
            if (ConstantsAndFunctions<T>.IsZero(det))
                throw new InvalidDeterminantException("Cannot invert a singular matrix");
            #endif

            var adj = Adjoint();
            for (int x = 0; x < diagLength; x++)
            for (int y = 0; y < diagLength; y++)
                this.SetValueNoCheck(
                    ConstantsAndFunctions<T>.Divide(
                        adj.GetValueNoCheck(x, y),
                        det
                    ),
                    x, y
                );
        }

        /// <summary>
        /// A / B
        /// Finds such C = A / B that A = C * B
        ///
        /// O(N^5)
        /// </summary>
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
            return MatrixDotProduct(a, fwd);
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
    }
}
