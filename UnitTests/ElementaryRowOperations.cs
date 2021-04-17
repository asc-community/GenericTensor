#region copyright
/*
 * MIT License
 * 
 * Copyright (c) 2020-2021 WhiteBlackGoose
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
    public class ElementaryRowOperations
    {
        [TestMethod]
        public void RowMultiply1()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            m.RowMultiply(1, 10);
            var exp = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4 * 10, 5 * 10 },
                    { 4, 8 },
                }
            );
            Assert.AreEqual(exp, m);
        }

        [TestMethod]
        public void RowMultiply2()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            m.RowMultiply(1, 10);
            m.RowMultiply(2, 0);
            var exp = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4 * 10, 5 * 10 },
                    { 0, 0 },
                }
            );
            Assert.AreEqual(exp, m);
        }

        [TestMethod]
        public void RowAdd1()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            m.RowAdd(1, 2, 1);
            var exp = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1 + 4, 2 + 5 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            Assert.AreEqual(exp, m);
        }

        [TestMethod]
        public void RowAdd2()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            m.RowAdd(0, 1, 10);
            var exp = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1 + 4 * 10, 2 + 5 * 10 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            Assert.AreEqual(exp, m);
        }

        [TestMethod]
        public void RowAdd3()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            m.RowAdd(0, 1, 10);
            m.RowAdd(2, 1, 3);
            var exp = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1 + 4 * 10, 2 + 5 * 10 },
                    { 4, 5 },
                    { 4 + 4 * 3, 8 + 5 * 3 },
                }
            );
            Assert.AreEqual(exp, m);
        }

        [TestMethod]
        public void RowSwap1()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            m.RowSwap(0, 1);
            var exp = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 4, 5 },
                    { 1, 2 },
                    { 4, 8 },
                }
            );
            Assert.AreEqual(exp, m);
        }

        [TestMethod]
        public void RowSwap2()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            m.RowSwap(0, 1);
            m.RowSwap(0, 1);
            m.RowSwap(2, 1);
            m.RowSwap(2, 1);
            var exp = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 4, 5 },
                    { 4, 8 },
                }
            );
            Assert.AreEqual(exp, m);
        }
    }
}
