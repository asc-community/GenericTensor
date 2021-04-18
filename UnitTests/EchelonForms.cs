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
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class EchelonForms
    {
        [TestMethod]
        public void REF1()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 35 }
                });
            var reForm = m.RowEchelonFormSafeDivision();
            var e = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1,  3,  1,  9 },
                    { 0, -2, -2, -8 },
                    { 0,  0,  0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REF2()
        {
            var m = GenTensor<float, FloatWrapper>.CreateMatrix(
                new float[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 35 }
                });
            var reForm = m.RowEchelonFormSafeDivision();
            var e = GenTensor<float, FloatWrapper>.CreateMatrix(
                new float[,]
                {
                    { 1,  3,  1,  9 },
                    { 0, -2, -2, -8 },
                    { 0,  0,  0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REF3()
        {
            var m = GenTensor<float, FloatWrapper>.CreateMatrix(
                new float[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 35 }
                });
            var reForm = m.RowEchelonFormSimple();
            var e = GenTensor<float, FloatWrapper>.CreateMatrix(
                new float[,]
                {
                    { 1,  3,  1,  9 },
                    { 0, -2, -2, -8 },
                    { 0,  0,  0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFOnes1()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 35 }
                });
            var reForm = m.RowEchelonFormLeadingOnesSafeDivision();
            var e = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1,  3,  1,  9 },
                    { 0,  1,  1,  4 },
                    { 0,  0,  0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFOnes2()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 35 }
                });
            var reForm = m.RowEchelonFormLeadingOnesSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  3,  1,  9 },
                    { 0,  1,  1,  4 },
                    { 0,  0,  0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFOnes3()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 25 },
                    { 5, 6,  78, 23 }
                });
            var reForm = m.RowEchelonFormLeadingOnesSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  3,  1,  9 },
                    { 0,  1,  1,  4 },
                    { 0,  0,  1,  7.0/41 },
                    { 0,  0,  0,  1 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFOnes4()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 8,  3 },
                    { 9,  1 },
                    { 3,  7 },
                    { 5,  6 }
                });
            var reForm = m.RowEchelonFormLeadingOnesSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  3.0 / 8 },
                    { 0,  1 },
                    { 0,  0 },
                    { 0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFOnes5()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  3,  0,  9 },
                    { 0,  0,  0,  1 },
                    { 3, 11,  0, 25 },
                });
            var reForm = m.RowEchelonFormLeadingOnesSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1, 3, 0,  9 },
                    { 0, 1, 0, -1 },
                    { 0, 0, 0,  1 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFOnes6()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 0, 0, 0, 0, 0 },
                    { 1, 2, 0, 3, 5 },
                    { 0, 0, 0, 0, 0 }
                });
            var reForm = m.RowEchelonFormLeadingOnesSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1, 2, 0, 3, 5 },
                    { 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFOnes7()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 0, 0, 0, 0, 1 },
                    { 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 1 }
                });
            var reForm = m.RowEchelonFormLeadingOnesSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 0, 0, 0, 0, 1 },
                    { 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void RREF1()
        {
            var m = GenTensor<float, FloatWrapper>.CreateMatrix(
                new float[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 35 }
                });
            var reForm = m.ReducedRowEchelonFormSimple();
            var e = GenTensor<float, FloatWrapper>.CreateMatrix(
                new float[,]
                {
                    { 1,  0, -2, -3 },
                    { 0,  1,  1,  4 },
                    { 0,  0,  0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void RREF2()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 25 },
                    { 5, 6,  78, 23 }
                });
            var reForm = m.ReducedRowEchelonFormSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  0,  0,  0 },
                    { 0,  1,  0,  0 },
                    { 0,  0,  1,  0 },
                    { 0,  0,  0,  1 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFF3()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 2,  3 },
                    { 4,  6 },
                    { 6,  3 },
                    { 5,  7 }
                });
            var reForm = m.ReducedRowEchelonFormSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 1,  0 },
                    { 0,  1 },
                    { 0,  0 },
                    { 0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFF4()
        {
            var m = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 4, 1, 0 },
                });
            var reForm = m.ReducedRowEchelonFormSimple();
            var e = GenTensor<double, DoubleWrapper>.CreateMatrix(
                new double[,]
                {
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 },
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void RREF1SD()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 35 }
                });
            var reForm = m.ReducedRowEchelonFormSafeDivision();
            var e = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1,  0, -2, -3 },
                    { 0,  1,  1,  4 },
                    { 0,  0,  0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void RREF2SD()
        {
            var m = GenTensor<BigInteger, BigIntWrapper>.CreateMatrix(
                new BigInteger[,]
                {
                    { 1,  3,  1,  9 },
                    { 1,  1, -1,  1 },
                    { 3, 11,  5, 25 },
                    { 5, 6,  78, 23 }
                });
            var reForm = m.ReducedRowEchelonFormSafeDivision();
            var e = GenTensor<BigInteger, BigIntWrapper>.CreateMatrix(
                new BigInteger[,]
                {
                    { 1,  0,  0,  0 },
                    { 0,  1,  0,  0 },
                    { 0,  0,  1,  0 },
                    { 0,  0,  0,  1 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFF3SD()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 2,  3 },
                    { 4,  6 },
                    { 6,  3 },
                    { 5,  7 }
                });
            var reForm = m.ReducedRowEchelonFormSafeDivision();
            var e = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1,  0 },
                    { 0,  1 },
                    { 0,  0 },
                    { 0,  0 }
                });
            Assert.AreEqual(e, reForm);
        }

        [TestMethod]
        public void REFF4SD()
        {
            var m = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 4, 1, 0 },
                });
            var reForm = m.ReducedRowEchelonFormSafeDivision();
            var e = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 },
                });
            Assert.AreEqual(e, reForm);
        }

    }
}
