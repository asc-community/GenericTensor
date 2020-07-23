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
    using TS = Tensor<int>;

    [TestClass]
    public class Piecewise
    {
        public Piecewise()
        {
            BuiltinTypeInitter.InitForInt();
        }

        Tensor<int> GetA()
        {
            var res = Tensor<int>.CreateMatrix(
                new [,]
                {
                    {1, 2},
                    {3, 4}
                }
                );
            return res;
        }

        Tensor<int> GetB()
        {
            var res = Tensor<int>.CreateMatrix(
                new [,]
                {
                    {6, 7},
                    {8, 9}
                }
            );
            return res;
        }

        [TestMethod]
        public void AddMat()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseAdd(GetA(), GetB()),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {7,  9},
                        {11, 13}
                    }
                )
                );
        }

        [TestMethod]
        public void SubMat()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseSubtract(GetA(), GetB()),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {-5,  -5},
                        {-5, -5}
                    }
                )
            );
        }

        [TestMethod]
        public void MpMat()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseMultiply(GetA(), GetB()),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {6,  14},
                        {24, 36}
                    }
                )
            );
        }

        [TestMethod]
        public void DivMat()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseDivide(GetA(), GetB()),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {0, 0},
                        {0, 0}
                    }
                )
            );
        }

        [TestMethod]
        public void AddEl()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseAdd(GetA(), 2),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {3, 4},
                        {5, 6}
                    }
                ));
        }

        [TestMethod]
        public void SubEl()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseSubtract(GetA(), 2),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {-1, 0},
                        {1,  2}
                    }
                ));
        }

        [TestMethod]
        public void MulEl()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseMultiply(GetA(), 2),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {2, 4},
                        {6, 8}
                    }
                ));
        }

        [TestMethod]
        public void DivEl()
        {
            Assert.AreEqual(
                Tensor<int>.PiecewiseDivide(GetA(), 2),
                Tensor<int>.CreateMatrix(
                    new [,]
                    {
                        {0, 1},
                        {1, 2}
                    }
                ));
        }
    }
}
