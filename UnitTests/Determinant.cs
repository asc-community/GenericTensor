using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using TS = Tensor<TensorIntWrapper, int>;

    [TestClass]
    public class Determinant
    {
        [TestMethod]
        public void SimpleLaplace1()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantLaplace());
        }

        [TestMethod]
        public void SimpleLaplace2()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantLaplace());
        }

        [TestMethod]
        public void SimpleGaussian1()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantGaussian());
        }

        [TestMethod]
        public void SimpleGaussian2()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new int[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussian());
        }
    }
}
