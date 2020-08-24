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


using System.Numerics;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Serialization
    {
        public void CircleTest<T, TWrapper>(GenTensor<T, TWrapper> tensor) 
            where TWrapper : struct, IOperations<T>
        {
            var serialized = tensor.Serialize();
            var deserialized = GenTensor<T, TWrapper>.Deserialize(serialized);
            Assert.AreEqual(tensor, deserialized);
        }

        [TestMethod]
        public void TestVecInt()
            => CircleTest(GenTensor<int, IntWrapper>.CreateVector(3, 4, 5));

        [TestMethod]
        public void TestVecFloat()
            => CircleTest(GenTensor<float, FloatWrapper>.CreateVector(3.5f, 4, -5.1f, 6.4f));

        [TestMethod]
        public void TestVecDouble()
            => CircleTest(GenTensor<double, DoubleWrapper>.CreateVector(3.5d, 4, -5.1d, 6.4d));

        [TestMethod]
        public void TestMatrixInt()
            => CircleTest(GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                }));

        [TestMethod]
        public void TestMatrixFloat()
            => CircleTest(GenTensor<float, FloatWrapper>.CreateMatrix(
                new[,]
                {
                    {1f, 2f, 3.3f},
                    {4, 5.5f, 6.6f}
                }));

        [TestMethod]
        public void TestMatrixDouble()
            => CircleTest(GenTensor<double, DoubleWrapper>.CreateMatrix(
                new[,]
                {
                    {1d, 2d, 3.3d},
                    {4, 5.5d, 6.6d}
                }));

        [TestMethod]
        public void TestTensorInt()
            => CircleTest(GenTensor<int, IntWrapper>.CreateTensor(
                new[,,]
                {
                    {
                        {1, 3, 3},
                        {4, 5, 6}
                    },
                    {
                        {-4, 2, 3},
                        {7, 8, 11}
                    }
                }
                ));

        [TestMethod]
        public void TestTensorFloat()
            => CircleTest(GenTensor<float, FloatWrapper>.CreateTensor(
                new[, ,]
                {
                    {
                        {1.1f, 3f, 3f},
                        {4f, 5.5f, 6f}
                    },
                    {
                        {-4f, 2.5f, 3f},
                        {7f, 8f, 11.4f}
                    }
                }
            ));

        [TestMethod]
        public void TestTensorDouble()
            => CircleTest(GenTensor<double, DoubleWrapper>.CreateTensor(
                new[, ,]
                {
                    {
                        {1.1d, 3d, 3d},
                        {4d, 5.5d, 6d}
                    },
                    {
                        {-4d, 2.5d, 3d},
                        {7d, 8d, 11.4d}
                    }
                }
            ));

        [TestMethod]
        public void TestTensorComplex()
            => CircleTest(GenTensor<Complex, ComplexWrapper>.CreateTensor(
                new[, ,]
                {
                    {
                        {new Complex(4, 6), 3d, 3d},
                        {4d, 5.5d, new Complex(-1, 5)}
                    },
                    {
                        {-4d, new Complex(5, 6), 3d},
                        {7d, 8d, new Complex(4, 5.4d)}
                    }
                }
            ));
    }
}
