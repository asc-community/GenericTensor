using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> VectorCrossProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (!a.IsVector || !b.IsVector)
                throw new InvalidShapeException($"Both {nameof(a)} and {nameof(b)} should be vectors");
            if (a.Shape[0] != b.Shape[0])
                throw new InvalidShapeException($"Length of {nameof(a)} and {nameof(b)} should be equal");
            if (a.Shape[0] != 3)
                throw new NotImplementedException("Other than vectors of the length of 3 aren't supported for VectorCrossProduct yet");
            return Tensor<TWrapper, TPrimitive>.CreateVector(
                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[1], b[2]),
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[2], b[1])),

                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[2], b[0]),
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[0], b[2])),

                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[0], b[1]),
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[1], b[0]))
            );
        }

        public static Tensor<TWrapper, TPrimitive> TensorVectorCrossProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (a.Shape != b.Shape)
                throw new InvalidShapeException($"Pre-shapes of {nameof(a)} and {nameof(b)} should be equal");
            var res = new Tensor<TWrapper, TPrimitive>(a.Shape);
            foreach (var index in a.IterateOverVectors())
                res.SetSubtensor(
                    VectorCrossProduct(a.GetSubtensor(index), b.GetSubtensor(index)),
                    index
                    );
            return res;
        }
    }
}
