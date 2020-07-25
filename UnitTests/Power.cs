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
    using TS = GenTensor<int>;

    [TestClass]
    public class Power
    {
        public Power()
        {
            BuiltinTypeInitter.InitForFloat();
        }


        [TestMethod]
        public void Test1()
        {
            var M = GenTensor<float>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            var powered = GenTensor<float>.MatrixPower(M, 4);
            var I = GenTensor<float>.CreateIdentityMatrix(3);
            for (int i = 0; i < 4; i++)
                I = GenTensor<float>.MatrixDotProduct(I, M);

            Assert.AreEqual(I, powered);
        }

        [TestMethod]
        public void Test2()
        {
            var M = GenTensor<float>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            var powered = GenTensor<float>.MatrixPower(M, 3);
            var I = GenTensor<float>.CreateIdentityMatrix(3);
            for (int i = 0; i < 3; i++)
                I = GenTensor<float>.MatrixDotProduct(I, M);

            Assert.AreEqual(I, powered);
        }

        [TestMethod]
        public void Test3()
        {
            var M = GenTensor<float>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            var powered = GenTensor<float>.MatrixPower(M, -4);
            M = M.Forward();
            M.InvertMatrix();
            var I = GenTensor<float>.CreateIdentityMatrix(3);
            for (int i = 0; i < 4; i++)
                I = GenTensor<float>.MatrixDotProduct(I, M);

            Assert.AreEqual(I, powered);
        }

        [TestMethod]
        public void Test4Tensor()
        {
            var power = 3;

            var M1 = GenTensor<float>.CreateMatrix(new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            });
            var M2 = GenTensor<float>.CreateMatrix(new float[,]
            {
                {1,  2, 9},
                {2, 3, 5},
                {3,  8, 5}
            });

            Assert.AreEqual(
                GenTensor<float>.Stack(
                    GenTensor<float>.MatrixPower(M1, power),
                    GenTensor<float>.MatrixPower(M2, power)),
                GenTensor<float>.TensorMatrixPower(
                    GenTensor<float>.Stack(M1, M2),
                    power
                    )
            );
        }
    }
}
