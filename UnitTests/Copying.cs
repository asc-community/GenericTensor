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
    public class Copy
    {
        [TestMethod]
        public void NoChangeInShapeAfterTranspose1()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(new[,] { { 1, 2 } });
            Assert.AreEqual(m.Shape[0], 1);
            Assert.AreEqual(m.Shape[1], 2);
            var n = m.Copy(false);
            n.TransposeMatrix();
            Assert.AreEqual(m.Shape[0], 1);
            Assert.AreEqual(m.Shape[1], 2);
        }

        [TestMethod]
        public void NoChangeInShapeAfterTranspose2()
        {
            var m = GenTensor<int, IntWrapper>.CreateTensor(new[,,] { { { 1, 2 } } });
            Assert.AreEqual(m.Shape[0], 1);
            Assert.AreEqual(m.Shape[1], 1);
            Assert.AreEqual(m.Shape[2], 2);
            var n = m.Copy(false);
            n.Transpose(0, 2);
            Assert.AreEqual(m.Shape[0], 1);
            Assert.AreEqual(m.Shape[1], 1);
            Assert.AreEqual(m.Shape[2], 2);

            Assert.AreEqual(n.Shape[0], 2);
            Assert.AreEqual(n.Shape[1], 1);
            Assert.AreEqual(n.Shape[2], 1);
        }
    }
}
