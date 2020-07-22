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
        public void Simple()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantLaplace());
        }

        [TestMethod]
        public void Simple2()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantLaplace());
        }
    }
}
