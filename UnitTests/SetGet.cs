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


using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GetSet
    {
        public GetSet()
        {
            BuiltinTypeInitter.InitForInt();
        }

        private GenTensor<int> GetT() => new GenTensor<int>(2, 3, 4);

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
