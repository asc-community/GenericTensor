using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> CreateIdentityTensor(int[] dimensions, int finalMatrixDiag)
        {
            var newDims = new int[dimensions.Length + 2];
            for (int i = 0; i < dimensions.Length; i++)
                newDims[i] = dimensions[i];
            newDims[newDims.Length - 2] = newDims[newDims.Length - 1] = finalMatrixDiag;
            var res = new Tensor<TWrapper, TPrimitive>(newDims);
            foreach (var index in res.IterateOverMatrices())
            {
                var iden = CreateIdentityMatrix(finalMatrixDiag);
                res.SetSubtensor(iden, index);
            }
            return res;
        }


        public static Tensor<TWrapper, TPrimitive> CreateIdentityMatrix(int diag)
        {
            var res = new Tensor<TWrapper, TPrimitive>(diag, diag);
            for (int i = 0; i < res.Data.Length; i++)
                res.Data[i].SetZero();
            for (int i = 0; i < diag; i++)
            {
                res.GetFlattenedWrapper(i, i).SetOne();
            }
            return res;
        }


        public static Tensor<TWrapper, TPrimitive> CreateVector(params TPrimitive[] elements)
        {
            var res = new Tensor<TWrapper, TPrimitive>(elements.Length);
            for (int i = 0; i < elements.Length; i++)
                res[i] = elements[i];
            return res;
        }

        private static (int height, int width) ExtractAndCheck<T>(T[,] data)
        {
            var width = data.GetLength(0);
            if (width <= 0)
                throw new InvalidShapeException();
            var height = data.GetLength(1);
            if (height <= 0)
                throw new InvalidShapeException();
            return (width, height);
        }

        public static Tensor<TWrapper, TPrimitive> CreateMatrix(TPrimitive[,] data)
        {
            var (width, height) = Tensor<TWrapper, TPrimitive>.ExtractAndCheck(data);
            var res = new Tensor<TWrapper, TPrimitive>(width, height);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                res[x, y] = data[x, y];
            return res;
        }

        public static Tensor<TWrapper, TPrimitive> CreateMatrix(TWrapper[,] data)
        {
            var (width, height) = Tensor<TWrapper, TPrimitive>.ExtractAndCheck(data);
            var res = new Tensor<TWrapper, TPrimitive>(width, height);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                res.SetCell(data[x, y], x, y);
            return res;
        }

        public static Tensor<TWrapper, TPrimitive> CreateMatrix(int width, int height, Func<(int x, int y), TWrapper> stepper)
        {
            var data = new TWrapper[width, height];
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                data[x, y] = stepper((x, y));
            return CreateMatrix(data);
        }
    }
}
