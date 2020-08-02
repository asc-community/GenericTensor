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
    public class VectorProduct
    {
        public VectorProduct()
        {
            
        }

        [TestMethod]
        public void TestVectorDotProduct1()
        {
            var v1 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v2 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5, 3);
            Assert.AreEqual(16, GenTensor<int, IntegerWrapper>.VectorDotProduct(v1, v2));
        }

        [TestMethod]
        public void TestVectorDotProduct2()
        {
            var v1 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v2 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5);
            Assert.ThrowsException<InvalidShapeException>(() => GenTensor<int, IntegerWrapper>.VectorDotProduct(v1, v2));
        }

        [TestMethod]
        public void TestTensorVectorDotProduct1()
        {
            var v1_1 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v1_2 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v2_1 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5, 3);
            var v2_2 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5, 3);
            var v1 = GenTensor<int, IntegerWrapper>.Stack(v1_1, v1_2);
            var v2 = GenTensor<int, IntegerWrapper>.Stack(v2_1, v2_2);
            Assert.AreEqual(GenTensor<int, IntegerWrapper>.CreateVector(16, 16), GenTensor<int, IntegerWrapper>.TensorVectorDotProduct(v1, v2));
        }

        [TestMethod]
        public void TestTensorVectorDotProduct2()
        {
            var v1_1 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v1_2 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v2_1 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5);
            var v2_2 = GenTensor<int, IntegerWrapper>.CreateVector(3, 5);
            var v1 = GenTensor<int, IntegerWrapper>.Stack(v1_1, v1_2);
            var v2 = GenTensor<int, IntegerWrapper>.Stack(v2_1, v2_2);
            Assert.ThrowsException<InvalidShapeException>(() => GenTensor<int, IntegerWrapper>.TensorVectorDotProduct(v1, v2));
        }

        [TestMethod]
        public void TestVectorCrossProduct1()
        {
            var v1 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v2 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5, 3);
            Assert.AreEqual(GenTensor<int, IntegerWrapper>.CreateVector(-9, -12, 11), GenTensor<int, IntegerWrapper>.VectorCrossProduct(v1, v2));
        }

        [TestMethod]
        public void TestTensorVectorCrossProduct1()
        {
            var v1_1 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var v1_2 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5, 3);
            var v2_1 = GenTensor<int, IntegerWrapper>.CreateVector(-3, 5, 3);
            var v2_2 = GenTensor<int, IntegerWrapper>.CreateVector(1, 2, 3);
            var T1 = GenTensor<int, IntegerWrapper>.Stack(v1_1, v1_2);
            var T2 = GenTensor<int, IntegerWrapper>.Stack(v2_1, v2_2);

            var exp = GenTensor<int, IntegerWrapper>.CreateVector(-9, -12, 11);
            var res = GenTensor<int, IntegerWrapper>.Stack(exp, GenTensor<int, IntegerWrapper>.PiecewiseMultiply(exp, -1));
            Assert.AreEqual(res, GenTensor<int, IntegerWrapper>.TensorVectorCrossProduct(T1, T2));
        }
    }
}
