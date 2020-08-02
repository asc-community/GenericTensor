#region copyright
/*
 * MIT License
 * 
 * Copyright (c) 2020 WhiteBlackGoose
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion


using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Transposition
    {
        public Transposition()
        {
            
        }

        private GenTensor<int, IntegerWrapper> GetBig()
        {
            var res = new GenTensor<int, IntegerWrapper>(2, 3, 4);
            foreach (var (index, _) in res.Iterate())
            {
                res[index] = index[0] * 12 + index[1] * 4 + index[2];
            }
            return res;
        }

        private GenTensor<int, IntegerWrapper> GetSmall()
        {
            var res = new GenTensor<int, IntegerWrapper>(2, 3);
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
