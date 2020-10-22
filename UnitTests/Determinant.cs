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
using System.Numerics;

namespace UnitTests
{
    using TS = GenTensor<int, IntWrapper>;

    [TestClass]
    public class Determinant
    {
        public Determinant()
        {
            
            
        }

        [TestMethod]
        public void SimpleLaplace1()
        {
            var M = GenTensor<int, IntWrapper>.CreateMatrix(new[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantLaplace());
        }

        [TestMethod]
        public void SimpleLaplace2()
        {
            var M = GenTensor<int, IntWrapper>.CreateMatrix(new[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantLaplace());
        }


        // safe division


        [TestMethod]
        public void SimpleGaussian1()
        {
            var M = GenTensor<int, IntWrapper>.CreateMatrix(new[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantGaussianSafeDivision());
        }

        [TestMethod]
        public void SimpleGaussian2()
        {
            var M = GenTensor<int, IntWrapper>.CreateMatrix(new int[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussianSafeDivision());
        }

        [TestMethod]
        public void SimpleGaussian2Long()
        {
            var M = GenTensor<long, LongWrapper>.CreateMatrix(new long[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussianSafeDivision());
        }


        // unsafe


        [TestMethod]
        public void SimpleGaussian3()
        {
            var M = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {1, 2},
                {3, 4}
            });
            Assert.AreEqual(-2, M.DeterminantGaussianSimple());
        }

        [TestMethod]
        public void SimpleGaussian4()
        {
            var M = GenTensor<float, FloatWrapper>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussianSimple());
        }

        [TestMethod]
        public void TensorLaplace()
        {
            var T = GenTensor<float, FloatWrapper>.CreateTensor(new float[,,]
            {
                {
                    {1, 2}, // -2
                    {3, 4}
                },
                {
                    {5, 7}, // -4
                    {7, 9}
                },
                {
                    {3, 1}, // 15
                    {6, 7}
                }
            });
            Assert.AreEqual(
                GenTensor<float, FloatWrapper>.CreateVector(-2, -4, 15),
                T.TensorDeterminantLaplace()
                );
        }

        [TestMethod]
        public void TensorGaussian()
        {
            var T = GenTensor<float, FloatWrapper>.CreateTensor(new float[,,]
            {
                {
                    {1, 2}, // -2
                    {3, 4}
                },
                {
                    {5, 7}, // -4
                    {7, 9}
                },
                {
                    {3, 1}, // 15
                    {6, 7}
                }
            });
            Assert.AreEqual(
                GenTensor<float, FloatWrapper>.CreateVector(-2, -4, 15),
                T.TensorDeterminantGaussianSafeDivision()
            );
        }

        public void SimpleGaussianGWInt()
        {
            var M = GenTensor<int, GenericWrapper<int>>.CreateMatrix(new int[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussianSimple());
        }

        public void SimpleGaussianGWFloat()
        {
            var M = GenTensor<float, GenericWrapper<float>>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussianSimple());
        }

        public void SimpleGaussianGWDouble()
        {
            var M = GenTensor<double, GenericWrapper<double>>.CreateMatrix(new double[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussianSimple());
        }

        public void SimpleGaussianGWComplex()
        {
            var M = GenTensor<Complex, GenericWrapper<Complex>>.CreateMatrix(new Complex[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            Assert.AreEqual(-306, M.DeterminantGaussianSimple());
        }
    }
}
