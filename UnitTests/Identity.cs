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
    using TS = GenTensor<int, IntegerWrapper>;
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
            var t = TS.CreateIdentityTensor(new []{4, 5}, 8);
            var k = t.Copy(false);
            k.TransposeMatrix();
            Assert.AreEqual(t, k);
        }
    }
}
