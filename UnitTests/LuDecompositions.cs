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

using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class LuDecompositions
    {
        private void AssertMatrixIsLowerTriangular<T, TW>(GenTensor<T, TW> t) where TW : struct, IOperations<T>
        {
            Assert.IsTrue(t.IsSquareMatrix);
            t.IterateOver2((i, j) =>
            {
                if (i < j) Assert.AreEqual(t[i, j], default(TW).CreateZero());
            });
        }

        private void AssertMatrixIsUpperTriangular<T, TW>(GenTensor<T, TW> t) where TW : struct, IOperations<T>
        {
            Assert.IsTrue(t.IsSquareMatrix);
            t.IterateOver2((i, j) =>
            {
                if (i > j) Assert.AreEqual(t[i, j], default(TW).CreateZero());
            });
        }

        private void TestProvidedLu<T, TW>(T[,] data) where TW : struct, IOperations<T>
        {
            var m = GenTensor<T, TW>.CreateMatrix(data);
            var (lower, upper) = m.LuDecomposition();
            
            AssertMatrixIsLowerTriangular(lower);
            AssertMatrixIsUpperTriangular(upper);

            Assert.AreEqual(GenTensor<T, TW>.MatrixMultiply(lower, upper), m);
        }
        
        private void TestProvidedPlu<T, TW>(T[,] data) where TW : struct, IOperations<T>
        {
            var m = GenTensor<T, TW>.CreateMatrix(data);
            var (p, l, u) = m.PluDecomposition();
            
            AssertMatrixIsLowerTriangular(l);
            AssertMatrixIsUpperTriangular(u);

            Assert.AreEqual(GenTensor<T, TW>.MatrixMultiply(p, m), GenTensor<T, TW>.MatrixMultiply(l, u));
        }

        private static void TestNoLu<T, TW>(T[,] data) where TW : struct, IOperations<T> =>
            Assert.ThrowsException<ImpossibleDecomposition>(() =>
                GenTensor<T, TW>.CreateMatrix(data).LuDecomposition());

        [TestMethod]
        public void LuArbitrary_1() =>
            TestProvidedLu<int, IntWrapper>(new[,]
            {
                { 1, 2, 4 },
                { 3, 8, 14 },
                { 2, 6, 13 },
            });

        [TestMethod]
        public void LuArbitrary_2() =>
            TestProvidedLu<int, IntWrapper>(new[,]
            {
                { -6, -4 },
                { -6, -4 }
            });

        [TestMethod]
        public void LuArbitrary_3() =>
            TestNoLu<int, IntWrapper>(new[,]
            {
                { 0, 0, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            });

        [TestMethod]
        public void PluArbitrary_1() =>
            TestProvidedPlu<int, IntWrapper>(new[,]
            {
                { 1, 2, 4 },
                { 3, 8, 14 },
                { 2, 6, 13 }
            });
        
        [TestMethod]
        public void PluArbitrary_2() =>
            TestProvidedPlu<double, DoubleWrapper>(new[,]
            {
                { 0D, 1, 0 },
                { -8, 8, 1 },
                { 2, -2, 0 }
            });
        
        [TestMethod]
        public void PluArbitrary_3() =>
            TestProvidedPlu<int, IntWrapper>(new[,]
            {
                {1, 0, 4},
                {0, 0, 1},
                {3, 2, 13}
            });
    }
}