using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using TS = Tensor<TensorIntWrapper, int>;
    [TestClass]
    public class Identity
    {
        [TestMethod]
        public void IdenMatrix1()
        {
            Assert.AreEqual(TS.CreateIdentityMatrix(1), TS.CreateMatrix(new int[,]{{1}}));
        }

        [TestMethod]
        public void IdenMatrix2()
        {
            Assert.AreEqual(TS.CreateIdentityMatrix(3), TS.CreateMatrix(new int[,]
            {
                {1, 0, 0},
                {0, 1, 0},
                {0, 0, 1}
            }));
        }

        [TestMethod]
        public void IdenTensor()
        {
            var t = TS.CreateIdentityTensor(new []{4, 5, 6}, 8);
        }
    }
}
