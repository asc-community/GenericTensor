using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{

    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> TensorVectorDotProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (a.Shape.SubShape(0, 1) != b.Shape.SubShape(0, 1))
                throw new InvalidShapeException("Other dimensions of tensors should be equal");
            var resTensor = new Tensor<TWrapper, TPrimitive>(a.Shape.SubShape(0, 1));
            foreach (var index in resTensor.IterateOverElements())
            {
                var scal = VectorDotProduct(a.GetSubtensor(index), b.GetSubtensor(index));
                resTensor[index] = scal;
            }
            return resTensor;
        }

        public static TPrimitive VectorDotProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (!a.IsVector || !b.IsVector)
                throw new InvalidShapeException($"{nameof(a)} and {nameof(b)} should be vectors");
            if (a.Shape[0] != b.Shape[0])
                throw new InvalidShapeException($"{nameof(a)}'s length should be the same as {nameof(b)}'s");

            var res = ConstantsAndFunctions<TWrapper, TPrimitive>.CreateZero();
            for (int i = 0; i < a.Shape[0]; i++)
            {
                var term = ConstantsAndFunctions<TWrapper, TPrimitive>.Create(a[i]);
                term.Multiply(b.GetCell(i));
                res.Add(term);
            }
            return res.GetValue();
        }
    }
}
