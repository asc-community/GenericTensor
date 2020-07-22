using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{

    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static TPrimitive VectorDotProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (!a.IsVector || !b.IsVector)
                throw new InvalidShapeException("Dot product for vectors requires vectors only");
            if (a.Shape[0] != b.Shape[0])
                throw new InvalidShapeException($"{nameof(a)}'s length should be the same as {nameof(b)}'s");
            var res = Constants<TWrapper, TPrimitive>.CreateZero();
            for (int i = 0; i < a.Shape[0]; i++)
            {
                var term = Constants<TWrapper, TPrimitive>.Create(a[i]);
                term.Multiply(b.GetCell(i));
                res.Add(term);
            }
            return res.GetValue();
        }
    }
}
