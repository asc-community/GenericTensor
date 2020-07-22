using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        /// <summary>
        /// Borrowed from here: https://github.com/ZacharyPatten/Towel/blob/master/Sources/Towel/Mathematics/Matrix.cs#L528
        /// </summary>
        public TPrimitive DeterminantLaplace(int diagLength)
        {
            if (diagLength == 1)
                return this[0, 0];
            var det = ConstantsAndFunctions<TWrapper, TPrimitive>.CreateZero();
            var sign = ConstantsAndFunctions<TWrapper, TPrimitive>.CreateOne();
            var temp = new Tensor<TWrapper, TPrimitive>(diagLength, diagLength);
            for (int i = 0; i < diagLength; i++)
            {
                GetCofactor(this, temp, 0, i, diagLength);
                det.Add(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Create(
                        ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(
                            sign.GetValue(),
                            ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(
                                this[0, i],
                                temp.DeterminantLaplace(diagLength - 1)
                            )))
                );
                sign.Negate();
            }
            return det.GetValue();
        }

        public TPrimitive DeterminantLaplace()
        {
            if (!this.IsMatrix)
                throw new InvalidShapeException("Determinant function should be only called from a matrix");
            if (Shape[0] != Shape[1])
                throw new InvalidShapeException("Matrix should be square");
            return DeterminantLaplace(Shape[0]);
        }
    }
}
