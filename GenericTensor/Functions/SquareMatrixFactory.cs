using System.Collections.Generic;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class SquareMatrixFactory<T>
    {
        // [0] is 2x2 matrix, [1] is 3x3 matrix, etc.
        static readonly List<GenTensor<T>> tensorTempFactorySquareMatrices = new List<GenTensor<T>>();

        internal static GenTensor<T> GetMatrix(int diagLength)
        {
            if (diagLength >= tensorTempFactorySquareMatrices.Count + 2)
                lock (tensorTempFactorySquareMatrices)
                    if (diagLength >= tensorTempFactorySquareMatrices.Count + 2)
                        for (int i = tensorTempFactorySquareMatrices.Count + 1; i <= diagLength; i++)
                            tensorTempFactorySquareMatrices.Add(new GenTensor<T>(i, i));
            return tensorTempFactorySquareMatrices[diagLength - 2];
        }
    }
}
