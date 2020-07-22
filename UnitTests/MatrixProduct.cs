using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MatrixProduct
    {
        [TestMethod]
        public void MatrixDotProduct1()
        {
            var A = Tensor<TensorIntWrapper, int>.CreateMatrix(
                new [,]
                {
                    { 12, -1, 1 },
                    { 0,   1, 4 },
                });
            var B = Tensor<TensorIntWrapper, int>.CreateMatrix(
                new [,]
                {
                    { 1, -1 },
                    { 0,  1 },
                    { 3,  0 },
                    { 0,  3 },
                });

            A.TransposeMatrix();
            B.TransposeMatrix();
            Assert.AreEqual(Tensor<TensorIntWrapper, int>.MatrixDotProduct(A, B), Tensor<TensorIntWrapper, int>.CreateMatrix(
                new [,]
                {
                    { 12, 0, 36, 0 },
                    { -2, 1, -3, 3 },
                    {-3,  4, 3,  12}
                }
            ));
        }
    }
}
