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
                throw new InvalidShapeException($"Both {nameof(a)} and {nameof(b)} should be matrices");

            if (a.Shape[1] != b.Shape[0])
                throw new InvalidShapeException($"{nameof(a)}'s height must be equal to {nameof(b)}'s width");
            var width = a.Shape[0];
            var height = b.Shape[1];
            var res = Tensor<TWrapper, TPrimitive>.CreateMatrix(width, height,((int, int) _) => ConstantsAndFunctions<TWrapper, TPrimitive>.CreateZero());
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

        public static Tensor<TWrapper, TPrimitive> TensorMatrixDotProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (a.Shape.Count < 2 || b.Shape.Count < 2)
                throw new InvalidShapeException($"Arguments should be at least matrices while their shapes are {a.Shape} and {b.Shape}");
            if (a.Shape.SubShape(0, 2) != b.Shape.SubShape(0, 2))
                throw new InvalidShapeException("Other dimensions of tensors should be equal");
            var oldShape = a.Shape.SubShape(0, 2).ToArray();
            var newShape = new int[oldShape.Length + 2];
            for (int i = 0; i < oldShape.Length; i++)
                newShape[i] = oldShape[i];
            newShape[newShape.Length - 2] = a.Shape[a.Shape.Length - 2];
            newShape[newShape.Length - 1] = b.Shape[b.Shape.Length - 1];
            var resTensor = new Tensor<TWrapper, TPrimitive>(newShape);
            foreach (var subDimensions in a.IterateOverMatrices())
            {
                var product = MatrixDotProduct(a.GetSubtensor(subDimensions), b.GetSubtensor(subDimensions));
                resTensor.SetSubtensor(product, subDimensions);
            }
            return resTensor;
        }
    }
}
