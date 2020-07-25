using System;
using System.Collections.Generic;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class SquareMatrixFactory<T>
    {
        [ThreadStatic] internal static List<GenTensor<T>> TensorTempFactorySquareMatrices;

        internal static GenTensor<T> GetMatrix(int diagLength)
        {
            if (TensorTempFactorySquareMatrices is null)
                TensorTempFactorySquareMatrices = new List<GenTensor<T>> {null};
            for (int i = TensorTempFactorySquareMatrices.Count; i <= diagLength; i++)
                TensorTempFactorySquareMatrices.Add(new GenTensor<T>(i, i));
            return TensorTempFactorySquareMatrices[diagLength - 1];
        }
    }
}
