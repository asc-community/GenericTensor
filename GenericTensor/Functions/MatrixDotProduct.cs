using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{

    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> MatrixDotProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (!a.IsMatrix || !b.IsMatrix)
                throw new InvalidShapeException($"Arguments should be matrices while their shapes are {a.Shape} and {b.Shape}");
            if (a.Shape[1] != b.Shape[0])
                throw new InvalidShapeException($"{nameof(a)}'s height must be equal to {nameof(b)}'s width");
            var width = a.Shape[0];
            var height = b.Shape[1];
            var res = Tensor<TWrapper, TPrimitive>.CreateMatrix(width, height,((int, int) _) => Constants<TWrapper, TPrimitive>.CreateZero());
            b.Transpose(0, 1);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var v1 = a.GetSubtensor(x);
                var v2 = b.GetSubtensor(y);
                var scalar = Tensor<TWrapper, TPrimitive>.VectorDotProduct(v1, v2);
                res[x, y] = scalar;
            }
            return res;
        }
    }
}
