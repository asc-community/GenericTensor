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
