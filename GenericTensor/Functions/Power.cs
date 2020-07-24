namespace GenericTensor.Core
{
    public partial class GenTensor<T>
    {
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
        public static GenTensor<T> MatrixPower(GenTensor<T> m, int power)
        {
            #if ALLOW_EXCEPTIONS
            if (!m.IsSquareMatrix)
                throw new InvalidShapeException("Square matrix required");
            #endif
            if (power == 0)
                return CreateIdentityMatrix(m.Shape.shape[0]);
            if (power < 0)
            {
                m = m.Forward();
                m.InvertMatrix();
                power *= -1;
            }
            if (power == 1)
                return m;
            if (power == 2)
                return MatrixDotProduct(m, m);
            var half = power / 2;
            var m1 = MatrixPower(m, half);
            var dotted = MatrixDotProduct(m1, m1);
            if (power % 2 == 0)
                return dotted;
            else
                return MatrixDotProduct(dotted, m);
        }
    }
}
