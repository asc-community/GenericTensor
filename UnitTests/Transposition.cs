using System;
using System.Linq;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Transposition
    {
        private TensorInt GetBig()
        {
            var res = new TensorInt(2, 3, 4);
            foreach (var (index, _) in res.Iterate())
            {
                res[index] = index[0] * 12 + index[1] * 4 + index[2];
            }
            return res;
        }

        private TensorInt GetSmall()
        {
            var res = new TensorInt(2, 3);
            foreach (var (index, _) in res.Iterate())
            {
                res[index] = 3 * index[0] + index[1];
            }
            return res;
        }

        [TestMethod]
        public void SimpleShape1()
        {
            var sm = GetSmall();
            Assert.IsTrue(sm.Shape == new TensorShape(2, 3));
        }

        [TestMethod]
        public void SimpleShape2()
        {
            var sm = GetSmall();
            sm.Transpose(0, 1);
            Assert.IsTrue(sm.Shape == new TensorShape(3, 2));
        }

        [TestMethod]
        public void BigShape1()
        {
            var sm = GetBig();
            Assert.IsTrue(sm.Shape == new TensorShape(2, 3, 4));
        }

        [TestMethod]
        public void BigShape2()
        {
            var sm = GetBig();
            sm.Transpose(0, 1);
            Assert.IsTrue(sm.Shape == new TensorShape(3, 2, 4));
        }

        [TestMethod]
        public void BigShape3()
        {
            var sm = GetBig();
            sm.Transpose(0, 2);
            Assert.IsTrue(sm.Shape == new TensorShape(4, 3, 2));
        }

        [TestMethod]
        public void BigShape4()
        {
            var sm = GetBig();
            sm.Transpose(0, 1);
            sm.Transpose(0, 1);
            Assert.IsTrue(sm.Shape == new TensorShape(2, 3, 4));
        }

        [TestMethod]
        public void SmallFull1()
        {
            var sm = GetSmall();
            sm.Transpose(0, 1);
            Assert.AreEqual(sm[0, 0], 0);
            Assert.AreEqual(sm[1, 0], 1);
            Assert.AreEqual(sm[2, 0], 2);
            Assert.AreEqual(sm[0, 1], 3);
            Assert.AreEqual(sm[1, 1], 4);
            Assert.AreEqual(sm[2, 1], 5);
        }
    }
}
