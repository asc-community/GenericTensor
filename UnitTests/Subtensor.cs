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
    public class Subtensor
    {
        public Subtensor()
        {
            BuiltinTypeInitter.InitForInt();
        }

        private GenTensor<int> GetSmall()
        {
            var res = new GenTensor<int>(2, 3);
            foreach (var (index, _) in res)
            {
                res[index] = 3 * index[0] + index[1];
            }
            return res;
        }

        private GenTensor<int> GetBig()
        {
            var res = new GenTensor<int>(2, 3, 4);
            foreach (var (index, _) in res)
            {
                res[index] = index[0] * 12 + index[1] * 4 + index[2];
            }
            return res;
        }

        [TestMethod]
        public void SubMatrix1()
        {
            var t = GetSmall();
            Assert.AreEqual(t.GetSubtensor(0), GenTensor<int>.CreateVector(0, 1, 2));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(0), GenTensor<int>.CreateVector(0, 3));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(0), GenTensor<int>.CreateVector(0, 1, 2));
        }

        [TestMethod]
        public void SubMatrix2()
        {
            var t = GetSmall();
            Assert.AreEqual(t.GetSubtensor(1), GenTensor<int>.CreateVector(3, 4, 5));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(1), GenTensor<int>.CreateVector(1, 4));
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(1), GenTensor<int>.CreateVector(3, 4, 5));
        }

        [TestMethod]
        public void SubTensor()
        {
            var t = GetBig();
            Assert.AreEqual(t.GetSubtensor(new []{0, 0}), GenTensor<int>.CreateVector(0, 1, 2, 3));
            t.Transpose(1, 2);
            Assert.AreEqual(t.GetSubtensor(new []{0, 0}), GenTensor<int>.CreateVector(0, 4, 8));
            t.Transpose(1, 2);
            Assert.AreEqual(t.GetSubtensor(new []{0, 0}), GenTensor<int>.CreateVector(0, 1, 2, 3));

            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(new []{0, 0}), GenTensor<int>.CreateVector(0, 1, 2, 3));
            Assert.AreEqual(t.GetSubtensor(new []{0, 1}), GenTensor<int>.CreateVector(12 + 0, 12 + 1, 12 + 2, 12 + 3));
        }

        [TestMethod]
        public void SubMatrixSet()
        {
            var t = GetSmall();
            t.SetSubtensor(GenTensor<int>.CreateVector(10, 10, 10), 1);
            var k = t.ToString();
            t.Transpose(0, 1);
            Assert.AreEqual(t.GetSubtensor(0), GenTensor<int>.CreateVector(0, 10));
            Assert.AreEqual(t.GetSubtensor(1), GenTensor<int>.CreateVector(1, 10));
        }
    }
}
