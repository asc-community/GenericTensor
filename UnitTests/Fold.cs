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
    public class Fold
    {
        // 3 x 2 x 2 x 2
        private readonly GenTensor<int, IntWrapper> frog = GenTensor<int, IntWrapper>.CreateTensor(new[,,,] {
                {
                    { { 1, 2 }, { 3, 4 } },
                    { { 10, 20 }, { 30, 40 } },
                },
                {
                    { { 100, 200 }, { 300, 400 } },
                    { { 1000, 2000 }, { 3000, 4000 } },
                },
                {
                    { { 10000, 20000 }, { 30000, 40000 } },
                    { { 100000, 200000 }, { 300000, 400000 } },
                }
            });

        [TestMethod]
        public void SumAx0()
        {
            var acc = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(2, 2, 2), id => 0);
            GenTensor<int, IntWrapper>.Fold(new HonkPerf.NET.Core.PureValueDelegate<int, int, int>((a, b) => a + b), acc, frog, axis: 0);
            var expected =
                GenTensor<int, IntWrapper>.CreateTensor(new[,,] {
                    { { 10101, 20202 }, { 30303, 40404 } },
                    { { 101010, 202020 }, { 303030, 404040 } },
                });
            Assert.AreEqual(expected, acc);
        }

        [TestMethod]
        public void SumAx1()
        {
            var acc = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 2, 2), id => 0);
            GenTensor<int, IntWrapper>.Fold(new HonkPerf.NET.Core.PureValueDelegate<int, int, int>((a, b) => a + b), acc, frog, axis: 1);
            var expected =
                GenTensor<int, IntWrapper>.CreateTensor(new[, ,] {
                    {
                        { 11, 22 }, { 33, 44 }
                    },
                    {
                        { 1100, 2200 }, { 3300, 4400 }
                    },
                    {
                        { 110000, 220000 }, { 330000, 440000 }
                    }
                });
            Assert.AreEqual(expected, acc);
        }

        [TestMethod]
        public void SumAx2()
        {
            var acc = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 2, 2), id => 0);
            GenTensor<int, IntWrapper>.Fold(new HonkPerf.NET.Core.PureValueDelegate<int, int, int>((a, b) => a + b), acc, frog, axis: 2);
            var expected = 
                GenTensor<int, IntWrapper>.CreateTensor(new [,,] {
                    {
                        { 4, 6 },
                        { 40, 60 }
                    },
                    {
                        { 400, 600 },
                        { 4000, 6000 }
                    },
                    {
                        { 40000, 60000 },
                        { 400000, 600000 }
                    }
                });
            Assert.AreEqual(expected, acc);
        }

        [TestMethod]
        public void SumAx3()
        {
            var acc = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 2, 2), id => 0);
            GenTensor<int, IntWrapper>.Fold(new HonkPerf.NET.Core.PureValueDelegate<int, int, int>((a, b) => a + b), acc, frog, axis: 3);
            var expected =
                GenTensor<int, IntWrapper>.CreateTensor(new[, ,] {
                    {
                        { 3, 7 },
                        { 30, 70 }
                    },
                    {
                        { 300, 700 },
                        { 3000, 7000 }
                    },
                    {
                        { 30000, 70000 },
                        { 300000, 700000 }
                    }
                });
            Assert.AreEqual(expected, acc);
        }
    }
}