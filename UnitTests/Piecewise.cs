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
    using TS = GenTensor<int, IntWrapper>;

    [TestClass]
    public class Piecewise
    {
        GenTensor<int, IntWrapper> GetA()
        {
            var res = GenTensor<int, IntWrapper>.CreateMatrix(
                new [,]
                {
                    {1, 2},
                    {3, 4}
                }
                );
            return res;
        }

        GenTensor<int, IntWrapper> GetB()
        {
            var res = GenTensor<int, IntWrapper>.CreateMatrix(
                new [,]
                {
                    {6, 7},
                    {8, 9}
                }
            );
            return res;
        }

        GenTensor<int, IntWrapper> GetT()
        {
            var res = GenTensor<int, IntWrapper>.CreateTensor(
                new [,,]
                {
                    {
                        {1, 2},
                        {3, 4}
                    },
                    {
                        {5, 6},
                        {7, 8}
                    },
                }
            );
            return res;
        }

        GenTensor<int, IntWrapper> GetV()
        {
            var res = GenTensor<int, IntWrapper>.CreateTensor(
                new []
                {
                    1, 2, 3
                }
            );
            return res;
        }

        #region One thread

        [TestMethod]
        public void T1()
        {
            Assert.AreEqual(GenTensor<int, IntWrapper>.CreateVector(2, 4, 6), GenTensor<int, IntWrapper>.PiecewiseAdd(GetV(), GetV()));
        }

        [TestMethod]
        public void T3()
        {
            Assert.AreEqual(
                    GenTensor<int, IntWrapper>.CreateTensor(
                        new [,,]
                        {
                            {
                                {2, 4},
                                {6, 8}
                            },
                            {
                                {10, 12},
                                {14, 16}
                            },
                        }
                    ),
                    GenTensor<int, IntWrapper>.PiecewiseAdd(GetT(), GetT())
                );
        }

        [TestMethod]
        public void AddMat()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseAdd(GetA(), GetB()),
                GenTensor<int, IntWrapper>.CreateMatrix(
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
                GenTensor<int, IntWrapper>.PiecewiseSubtract(GetA(), GetB()),
                GenTensor<int, IntWrapper>.CreateMatrix(
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
                GenTensor<int, IntWrapper>.PiecewiseMultiply(GetA(), GetB()),
                GenTensor<int, IntWrapper>.CreateMatrix(
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
                GenTensor<int, IntWrapper>.PiecewiseDivide(GetA(), GetB()),
                GenTensor<int, IntWrapper>.CreateMatrix(
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
                GenTensor<int, IntWrapper>.PiecewiseAdd(GetA(), 2),
                GenTensor<int, IntWrapper>.CreateMatrix(
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
                GenTensor<int, IntWrapper>.PiecewiseSubtract(GetA(), 2),
                GenTensor<int, IntWrapper>.CreateMatrix(
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
                GenTensor<int, IntWrapper>.PiecewiseMultiply(GetA(), 2),
                GenTensor<int, IntWrapper>.CreateMatrix(
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
                GenTensor<int, IntWrapper>.PiecewiseDivide(GetA(), 2),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {0, 1},
                        {1, 2}
                    }
                ));
        }

        [TestMethod]
        public void SumOf4D()
        {
            var t4 = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(2, 2, 2, 2),
                ids => ids[0] + ids[1] + ids[2] + ids[3]);
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseMultiply(t4, 2),
                GenTensor<int, IntWrapper>.PiecewiseAdd(t4, t4)
                );
        }

        #endregion

        #region Parallel

        [TestMethod]
        public void T1Par()
        {
            Assert.AreEqual(GenTensor<int, IntWrapper>.CreateVector(2, 4, 6), GenTensor<int, IntWrapper>.PiecewiseAdd(GetV(), GetV(), Threading.Multi));
        }

        [TestMethod]
        public void T3Par()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.CreateTensor(
                    new [,,]
                    {
                        {
                            {2, 4},
                            {6, 8}
                        },
                        {
                            {10, 12},
                            {14, 16}
                        },
                    }
                ),
                GenTensor<int, IntWrapper>.PiecewiseAdd(GetT(), GetT(), Threading.Multi)
            );
        }

        [TestMethod]
        public void AddMatPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseAdd(GetA(), GetB(), Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {7,  9},
                        {11, 13}
                    }
                )
                );
        }

        [TestMethod]
        public void SubMatPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseSubtract(GetA(), GetB(), Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {-5,  -5},
                        {-5, -5}
                    }
                )
            );
        }

        [TestMethod]
        public void MpMatPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseMultiply(GetA(), GetB(), Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {6,  14},
                        {24, 36}
                    }
                )
            );
        }

        [TestMethod]
        public void DivMatPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseDivide(GetA(), GetB(), Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {0, 0},
                        {0, 0}
                    }
                )
            );
        }

        [TestMethod]
        public void AddElPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseAdd(GetA(), 2, Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {3, 4},
                        {5, 6}
                    }
                ));
        }

        [TestMethod]
        public void SubElPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseSubtract(GetA(), 2, Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {-1, 0},
                        {1,  2}
                    }
                ));
        }

        [TestMethod]
        public void MulElPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseMultiply(GetA(), 2, Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {2, 4},
                        {6, 8}
                    }
                ));
        }

        [TestMethod]
        public void DivElPar()
        {
            Assert.AreEqual(
                GenTensor<int, IntWrapper>.PiecewiseDivide(GetA(), 2, Threading.Multi),
                GenTensor<int, IntWrapper>.CreateMatrix(
                    new [,]
                    {
                        {0, 1},
                        {1, 2}
                    }
                ));
        }

        #endregion
    }
}
