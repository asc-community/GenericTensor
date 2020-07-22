using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using TS = Tensor<TensorIntWrapper, int>;

    [TestClass]
    public class Piecewise
    {
        Tensor<TensorIntWrapper, int> GetA()
        {
            var res = Tensor<TensorIntWrapper, int>.CreateMatrix(
                new [,]
                {
                    {1, 2},
                    {3, 4}
                }
                );
            return res;
        }

        Tensor<TensorIntWrapper, int> GetB()
        {
            var res = Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseAdd(GetA(), GetB()),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseSubtract(GetA(), GetB()),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseMultiply(GetA(), GetB()),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseDivide(GetA(), GetB()),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseAdd(GetA(), 2),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseSubtract(GetA(), 2),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseMultiply(GetA(), 2),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
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
                Tensor<TensorIntWrapper, int>.PiecewiseDivide(GetA(), 2),
                Tensor<TensorIntWrapper, int>.CreateMatrix(
                    new [,]
                    {
                        {0, 1},
                        {1, 2}
                    }
                ));
        }
    }
}
