using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Subtensor
    {
        private TensorInt GetSmall()
        {
            var res = new TensorInt(2, 3);
            foreach (var (index, _) in res.Iterate())
            {
                res[index] = 3 * index[0] + index[1];
            }
            return res;
        }

        private TensorInt GetBig()
        {
            var res = new TensorInt(2, 3, 4);
            foreach (var (index, _) in res.Iterate())
            {
                res[index] = index[0] * 12 + index[1] * 4 + index[2];
            }
            return res;
        }

        [TestMethod]
        public void SubMatrix1()
        {
            var t = GetSmall();
            Assert.AreEqual(t.GetSubtensor(0), Tensor<TensorIntWrapper, int>.CreateVector(0, 1, 2));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(0), Tensor<TensorIntWrapper, int>.CreateVector(0, 3));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(0), Tensor<TensorIntWrapper, int>.CreateVector(0, 1, 2));
        }

        [TestMethod]
        public void SubMatrix2()
        {
            var t = GetSmall();
            Assert.AreEqual(t.GetSubtensor(1), Tensor<TensorIntWrapper, int>.CreateVector(3, 4, 5));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(1), Tensor<TensorIntWrapper, int>.CreateVector(1, 4));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(1), Tensor<TensorIntWrapper, int>.CreateVector(3, 4, 5));
        }

        [TestMethod]
        public void SubTensor()
        {
            var t = GetBig();
            Assert.AreEqual(t.GetSubtensor(0, 0), Tensor<TensorIntWrapper, int>.CreateVector(0, 1, 2, 3));
            t.Transpose(1, 2);
            Assert.AreEqual(t.GetSubtensor(0, 0), Tensor<TensorIntWrapper, int>.CreateVector(0, 4, 8));
            t.Transpose(1, 2);
            Assert.AreEqual(t.GetSubtensor(0, 0), Tensor<TensorIntWrapper, int>.CreateVector(0, 1, 2, 3));

            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(0, 0), Tensor<TensorIntWrapper, int>.CreateVector(0, 1, 2, 3));
            Assert.AreEqual(t.GetSubtensor(0, 1), Tensor<TensorIntWrapper, int>.CreateVector(12 + 0, 12 + 1, 12 + 2, 12 + 3));
        }

        [TestMethod]
        public void SubMatrixSet()
        {
            var t = GetSmall();
            t.SetSubtensor(Tensor<TensorIntWrapper, int>.CreateVector(10, 10, 10), 1);
            var k = t.ToString();
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(0), Tensor<TensorIntWrapper, int>.CreateVector(0, 10));
            Assert.AreEqual(t.GetSubtensor(1), Tensor<TensorIntWrapper, int>.CreateVector(1, 10));
        }
    }
}
