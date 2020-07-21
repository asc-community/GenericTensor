using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GetSet
    {
        private TensorInt GetT() => new TensorInt(2, 3, 4);

        [TestMethod]
        public void SetGet1()
        {
            var tensor = GetT();
            for (int i = 0; i < tensor.Shape[0]; i++)
            for (int j = 0; j < tensor.Shape[1]; j++)
            for (int k = 0; k < tensor.Shape[2]; k++)
            {
                tensor[i, j, k] = i + j + k;
            }

            for (int i = 0; i < tensor.Shape[0]; i++)
            for (int j = 0; j < tensor.Shape[1]; j++)
            for (int k = 0; k < tensor.Shape[2]; k++)
            {
                Assert.AreEqual(tensor[i, j, k], i + j + k);
            }
        }

        [TestMethod]
        public void Violate1()
        {
            var tensor = GetT();
            Assert.ThrowsException<IndexOutOfRangeException>(() => tensor[tensor.Shape[0], 0, 0]);
        }

        [TestMethod]
        public void Violate2()
        {
            var tensor = GetT();
            Assert.ThrowsException<IndexOutOfRangeException>(() => tensor[0, 0, -1]);
        }

        [TestMethod]
        public void Violate3()
        {
            var tensor = GetT();
            Assert.ThrowsException<IndexOutOfRangeException>(() => tensor[tensor.Shape[0] + 100, 0, 0]);
        }

        [TestMethod]
        public void Violate4()
        {
            var tensor = GetT();
            Assert.ThrowsException<IndexOutOfRangeException>(() => tensor[tensor.Shape[0], -1, -1]);
        }

        [TestMethod]
        public void ViolateArg1()
        {
            var tensor = GetT();
            Assert.ThrowsException<ArgumentException>(() => tensor[1]);
        }

        [TestMethod]
        public void ViolateArg2()
        {
            var tensor = GetT();
            Assert.ThrowsException<ArgumentException>(() => tensor[1, 2]);
        }

        [TestMethod]
        public void ViolateArg3()
        {
            var tensor = GetT();
            Assert.ThrowsException<ArgumentException>(() => tensor[0, 0]);
        }

        [TestMethod]
        public void ViolateArg4()
        {
            var tensor = GetT();
            Assert.ThrowsException<ArgumentException>(() => tensor[1, 4, 5, 6, 7]);
        }
    }
}
