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
    public class MatrixProduct
    {
        public MatrixProduct()
        {
            BuiltinTypeInitter.InitForInt();
        }

        [TestMethod]
        public void MatrixDotProduct1()
        {
            var A = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 12, -1, 1 },
                    { 0,   1, 4 },
                });
            var B = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 1, -1 },
                    { 0,  1 },
                    { 3,  0 },
                    { 0,  3 },
                });

            A.TransposeMatrix();
            B.TransposeMatrix();
            Assert.AreEqual(GenTensor<int>.MatrixMultiply(A, B), GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 12, 0, 36, 0 },
                    { -2, 1, -3, 3 },
                    {-3,  4, 3,  12}
                }
            ));
        }

        [TestMethod]
        public void MatrixTensorMp()
        {
            var a = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 1, 2 },
                    { 3, 4 }
                }
                );
            var b = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 5, 7 },
                    { 6, 8 }
                }
            );
            var T1 = GenTensor<int>.Stack(a, b);

            var c = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { -3, 2 },
                    { 3, 5 }
                }
            );

            var d = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { -3, 2 },
                    { 23, 5 }
                }
            );

            var T2 = GenTensor<int>.Stack(c, d);

            var exp1 = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 3, 12 },
                    { 3, 26 }
                }
            );

            var exp2 = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 146, 45 },
                    { 166, 52 }
                }
            );

            var exp = GenTensor<int>.Stack(exp1, exp2);

            Assert.AreEqual(exp, GenTensor<int>.TensorMatrixMultiply(T1, T2));
        }

        [TestMethod]
        public void MatrixDotProduct1Par()
        {
            var A = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 12, -1, 1 },
                    { 0,   1, 4 },
                });
            var B = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 1, -1 },
                    { 0,  1 },
                    { 3,  0 },
                    { 0,  3 },
                });

            A.TransposeMatrix();
            B.TransposeMatrix();
            Assert.AreEqual(GenTensor<int>.MatrixMultiply(A, B, Threading.Multi), GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 12, 0, 36, 0 },
                    { -2, 1, -3, 3 },
                    {-3,  4, 3,  12}
                }
            ));
        }

        [TestMethod]
        public void MatrixTensorMpPar()
        {
            var a = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 1, 2 },
                    { 3, 4 }
                }
                );
            var b = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 5, 7 },
                    { 6, 8 }
                }
            );
            var T1 = GenTensor<int>.Stack(a, b);

            var c = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { -3, 2 },
                    { 3, 5 }
                }
            );

            var d = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { -3, 2 },
                    { 23, 5 }
                }
            );

            var T2 = GenTensor<int>.Stack(c, d);

            var exp1 = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 3, 12 },
                    { 3, 26 }
                }
            );

            var exp2 = GenTensor<int>.CreateMatrix(
                new [,]
                {
                    { 146, 45 },
                    { 166, 52 }
                }
            );

            var exp = GenTensor<int>.Stack(exp1, exp2);

            Assert.AreEqual(exp, GenTensor<int>.TensorMatrixMultiply(T1, T2, Threading.Multi));
        }
    }
}
