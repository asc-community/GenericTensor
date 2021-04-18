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
    }
}
