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
    [TestClass]
    public class Slicing
    {
        public Slicing()
        {
            BuiltinTypeInitter.InitForFloat();
        }

        [TestMethod]
        public void Test1()
        {
            var A = GenTensor<float>.CreateMatrix(
                new float[,]
                {
                    {1,  2,  3,  4},
                    {5,  6,  7,  8},
                    {9,  10, 11, 12},
                    {13, 14, 15, 16},
                }
                );
            var sl1 = A.Slice(1, 3);
            A.TransposeMatrix();
            var sl2 = A.Slice(1, 3);
            Assert.AreEqual(
                GenTensor<float>.CreateMatrix(
                    new float[,]
                    {
                        {5,  6,  7,  8},
                        {9,  10, 11, 12},
                    }
                    ), sl1
                );
            Assert.AreEqual(
                GenTensor<float>.CreateMatrix(
                    new float[,]
                    {
                        {2,  6,  10,  14},
                        {3,  7,  11,  15},
                    }
                ), sl2
            );
        }
    }
}
