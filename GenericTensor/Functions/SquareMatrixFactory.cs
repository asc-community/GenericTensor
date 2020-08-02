using System.Collections.Generic;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class SquareMatrixFactory<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        // [0] is 2x2 matrix, [1] is 3x3 matrix, etc.
        static readonly List<GenTensor<T, TWrapper>> tensorTempFactorySquareMatrices = new List<GenTensor<T, TWrapper>>();

        internal static GenTensor<T, TWrapper> GetMatrix(int diagLength)
        {
            if (diagLength >= tensorTempFactorySquareMatrices.Count + 2)
                lock (tensorTempFactorySquareMatrices)
                    if (diagLength >= tensorTempFactorySquareMatrices.Count + 2)
                        for (int i = tensorTempFactorySquareMatrices.Count + 1; i <= diagLength; i++)
                            tensorTempFactorySquareMatrices.Add(new GenTensor<T, TWrapper>(i, i));
            return tensorTempFactorySquareMatrices[diagLength - 2];
        }
    }
}
