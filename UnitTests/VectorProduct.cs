using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class VectorProduct
    {
        [TestMethod]
        public void TestVectorDotProduct1()
        {
            var v1 = Tensor<TensorIntWrapper, int>.CreateVector(1, 2, 3);
            var v2 = Tensor<TensorIntWrapper, int>.CreateVector(-3, 5, 3);
            Assert.AreEqual(16, Tensor<TensorIntWrapper, int>.VectorDotProduct(v1, v2));
        }

        [TestMethod]
        public void TestVectorDotProduct2()
        {
            var v1 = Tensor<TensorIntWrapper, int>.CreateVector(1, 2, 3);
            var v2 = Tensor<TensorIntWrapper, int>.CreateVector(-3, 5);
            Assert.ThrowsException<InvalidShapeException>(() => Tensor<TensorIntWrapper, int>.VectorDotProduct(v1, v2));
        }
    }
}
