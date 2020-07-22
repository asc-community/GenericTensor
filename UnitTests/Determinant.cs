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
    using TS = Tensor<TensorIntWrapper, int>;

    [TestClass]
    public class Determinant
    {
        [TestMethod]
        public void SimpleLaplace1()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantLaplace());
        }

        [TestMethod]
        public void SimpleLaplace2()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantLaplace());
        }

        [TestMethod]
        public void SimpleGaussian1()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantGaussian());
        }

        [TestMethod]
        public void SimpleGaussian2()
        {
            var M = Tensor<TensorIntWrapper, int>.CreateMatrix(new int[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussian());
        }
    }
}
