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
    public class Inverse
    {
        public Inverse()
        {
            BuiltinTypeInitter.InitForFloat();
        }

        [TestMethod]
        public void Test1()
        {
            var A = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            var B = A.Copy(true);
            B.InvertMatrix();
            Assert.AreEqual(
                GenTensor<float, FloatWrapper>.CreateIdentityMatrix(3),
                GenTensor<float, FloatWrapper>.MatrixMultiply(A, B)
            );
        }

        [TestMethod]
        public void Test2()
        {
            var A = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {1, 2},
                {3, 4}
            });
            var B = A.Copy(true);
            B.InvertMatrix();
            Assert.AreEqual(
                GenTensor<float, FloatWrapper>.CreateIdentityMatrix(2),
                GenTensor<float, FloatWrapper>.MatrixMultiply(A, B)
            );
        }

        [TestMethod]
        public void Division()
        {
            var A = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });

            var B = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -1, 5},
                {2,  8, 7}
            });

            var res = GenTensor<float, FloatWrapper>.MatrixDivide(A, B);
            Assert.AreEqual(
                GenTensor<float, FloatWrapper>.MatrixMultiply(res, B),
                A
                );
        }

        [TestMethod]
        public void TensorDivision()
        {
            var A = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });

            var B = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -1, 5},
                {2,  8, 7}
            });

            var T1 = GenTensor<float, FloatWrapper>.Stack(A, B);
            var T2 = GenTensor<float, FloatWrapper>.Stack(B, A);

            var res = GenTensor<float, FloatWrapper>.TensorMatrixDivide(T1, T2);
            Assert.AreEqual(
                res.GetSubtensor(0), GenTensor<float, FloatWrapper>.MatrixDivide(A, B)
                );
            Assert.AreEqual(
                    res.GetSubtensor(1), GenTensor<float, FloatWrapper>.MatrixDivide(B, A)
                );
        }

        [TestMethod]
        public void TensorMatrixInverse()
        {
            var A = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });

            var B = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -1, 5},
                {2,  8, 7}
            });

            var T = GenTensor<float, FloatWrapper>.Stack(A, B);
            var K = T.Copy(true);
            T.TensorMatrixInvert();

            Assert.AreEqual(
                GenTensor<float, FloatWrapper>.CreateIdentityTensor(T.Shape.SubShape(0, 2).ToArray(), 3),
                GenTensor<float, FloatWrapper>.TensorMatrixMultiply(T, K)
                );
        }
    }
}
