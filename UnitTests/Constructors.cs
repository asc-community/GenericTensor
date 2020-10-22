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
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class Constructors
    {
        
        void AssertTensor<T, TWrapper>(IEnumerable<T> expected, GenTensor<T, TWrapper> actual) where TWrapper : struct, IOperations<T> =>
            CollectionAssert.AreEqual(expected.ToList(), actual.Iterate().Select(tup => tup.Value).ToList());
        [TestMethod]
        public void CreateMatrix()
        {
            var x = GenTensor<int, IntWrapper>.CreateMatrix(
                new[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                    { 5, 6 }
                }
            );
            Assert.IsFalse(x.IsVector);
            Assert.IsTrue(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 3, 2 }, x.Shape.ToArray());
            Assert.AreEqual(6, x.Volume);
            AssertTensor(Enumerable.Range(1, 6), x);
        }

        [TestMethod]
        public void CreateVector()
        {
            var x = GenTensor<int, IntWrapper>.CreateVector(new[] { 1, 2, 3, 4 });
            Assert.IsTrue(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 4 }, x.Shape.ToArray());
            Assert.AreEqual(4, x.Volume);
            AssertTensor(Enumerable.Range(1, 4), x);
        }
        [TestMethod]
        public void CreateTensor1D()
        {
            var x = GenTensor<int, IntWrapper>.CreateTensor(new[] { 1, 2, 3, 4 });
            Assert.IsTrue(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 4 }, x.Shape.ToArray());
            Assert.AreEqual(4, x.Volume);
            AssertTensor(Enumerable.Range(1, 4), x);
        }
        [TestMethod]
        public void CreateTensor2D()
        {
            var x = GenTensor<int, IntWrapper>.CreateTensor(
                new[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                    { 5, 6 }
                });
            Assert.IsFalse(x.IsVector);
            Assert.IsTrue(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 3, 2 }, x.Shape.ToArray());
            Assert.AreEqual(6, x.Volume);
            AssertTensor(Enumerable.Range(1, 6), x);
        }
        [TestMethod]
        public void CreateTensor3D()
        {
            var x = GenTensor<int, IntWrapper>.CreateTensor(
                new[, ,]
                {
                    { {  1,  2,  3 }, {  4,  5,  6 } },
                    { {  7,  8,  9 }, { 10, 11, 12 } },
                    { { 13, 14, 15 }, { 16, 17, 18 } },
                    { { 19, 20, 21 }, { 22, 23, 24 } },
                });
            Assert.IsFalse(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 4, 2, 3 }, x.Shape.ToArray());
            Assert.AreEqual(24, x.Volume);
            AssertTensor(Enumerable.Range(1, 24), x);
        }
        [TestMethod]
        public void CreateTensor4D()
        {
            var x = GenTensor<int, IntWrapper>.CreateTensor(
                new[, , ,]
                {
                    { { {  1,  2,  3 }, {  4,  5,  6 } },
                      { {  7,  8,  9 }, { 10, 11, 12 } },
                      { { 13, 14, 15 }, { 16, 17, 18 } },
                      { { 19, 20, 21 }, { 22, 23, 24 } }, },
                    { { { 25, 26, 27 }, { 28, 29, 30 } },
                      { { 31, 32, 33 }, { 34, 35, 36 } },
                      { { 37, 38, 39 }, { 40, 41, 42 } },
                      { { 43, 44, 45 }, { 46, 47, 48 } }, },
                });
            Assert.IsFalse(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 2, 4, 2, 3 }, x.Shape.ToArray());
            Assert.AreEqual(48, x.Volume);
            AssertTensor(Enumerable.Range(1, 48), x);
            Assert.AreEqual(x.Shape.DimensionCount, 4);
        }
        [TestMethod]
        public void CreateTensor5D()
        {
            var x = GenTensor<int, IntWrapper>.CreateTensor(
                new[, , , ,]
                { {
                    { { {  1,  2,  3 }, {  4,  5,  6 } },
                      { {  7,  8,  9 }, { 10, 11, 12 } },
                      { { 13, 14, 15 }, { 16, 17, 18 } },
                      { { 19, 20, 21 }, { 22, 23, 24 } }, },
                    { { { 25, 26, 27 }, { 28, 29, 30 } },
                      { { 31, 32, 33 }, { 34, 35, 36 } },
                      { { 37, 38, 39 }, { 40, 41, 42 } },
                      { { 43, 44, 45 }, { 46, 47, 48 } }, },
                } });
            Assert.IsFalse(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 1, 2, 4, 2, 3 }, x.Shape.ToArray());
            Assert.AreEqual(48, x.Volume);
            AssertTensor(Enumerable.Range(1, 48), x);
            Assert.AreEqual(x.Shape.DimensionCount, 5);
        }
        [TestMethod]
        public void CreateTensor6D()
        {
            var x = GenTensor<int, IntWrapper>.CreateTensor(new int[0, 0, 0, 0, 0, 0]);
            Assert.IsFalse(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 0, 0, 0, 0, 0, 0 }, x.Shape.ToArray());
            Assert.AreEqual(0, x.Volume);
        }
        [TestMethod]
        public void CreateTensor_NonStandardArray()
        {
            // array has indices [-9, -2], [-9, -1], [-9, 0], [-8, -2], [-8, -1], [-8, 0]
            var array = System.Array.CreateInstance(typeof(double), new[] { 2, 3 }, new[] { -9, -2 });
            array.SetValue(9.2, -9, -2);
            array.SetValue(9.1, -9, -1);
            array.SetValue(9.0, -9,  0);
            array.SetValue(8.2, -8, -2);
            array.SetValue(8.1, -8, -1);
            array.SetValue(8.0, -8,  0);
            var x = GenTensor<double, DoubleWrapper>.CreateTensor(array);
            Assert.IsFalse(x.IsVector);
            Assert.IsTrue(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 2, 3 }, x.Shape.ToArray());
            Assert.AreEqual(6, x.Volume);
            AssertTensor(new[] { 9.2, 9.1, 9.0, 8.2, 8.1, 8.0 }, x);
        }

        static IEnumerable<object[]> CreateUninitializedMatrixData =>
            new[]
            {
                new GenTensor<BigInteger, BigIntWrapper>(4, 4),
                GenTensor<BigInteger, BigIntWrapper>.CreateMatrix(4, 4),
                GenTensor<BigInteger, BigIntWrapper>.CreateMatrix(new BigInteger[4, 4]),
                GenTensor<BigInteger, BigIntWrapper>.CreateTensor(new BigInteger[4, 4])
            }.Select(tensor => new[] { tensor }).ToArray();
        [DataTestMethod]
        [DynamicData(nameof(CreateUninitializedMatrixData))]
        public void CreateUninitializedMatrix(GenTensor<BigInteger, BigIntWrapper> x)
        {
            Assert.IsFalse(x.IsVector);
            Assert.IsTrue(x.IsMatrix);
            Assert.IsTrue(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 4, 4 }, x.Shape.ToArray());
            Assert.AreEqual(16, x.Volume);
            AssertTensor(Enumerable.Repeat(BigInteger.Zero, 16), x);
        }

        static IEnumerable<object[]> CreateUninitializedVectorData =>
            new[]
            {
                new GenTensor<Complex, ComplexWrapper>(5),
                GenTensor<Complex, ComplexWrapper>.CreateVector(5),
                GenTensor<Complex, ComplexWrapper>.CreateVector(new Complex[5]),
                GenTensor<Complex, ComplexWrapper>.CreateTensor(new Complex[5])
            }.Select(tensor => new[] { tensor }).ToArray();
        [DataTestMethod]
        [DynamicData(nameof(CreateUninitializedVectorData))]
        public void CreateUninitializedVector(GenTensor<Complex, ComplexWrapper> x)
        {
            Assert.IsTrue(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { 5 }, x.Shape.ToArray());
            Assert.AreEqual(5, x.Volume);
            AssertTensor(Enumerable.Repeat(Complex.Zero, 5), x);
        }
        
        [TestMethod]
        public void IndexingAndAssigning2D()
        {
            var t2Actual = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 4), ids => 0);
            var t2Expected = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 4), ids => ids[0] + ids[1]);
            t2Actual.IterateOver2((a, b) => { t2Actual[a, b] = a + b; });
            Assert.AreEqual(t2Expected, t2Actual);
        }

        [TestMethod]
        public void IndexingAndAssigning3D()
        {
            var t3Actual = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 4, 5), ids => 0);
            var t3Expected = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 4, 5), ids => ids[0] + ids[1] + ids[2]);
            t3Actual.IterateOver3((a, b, c) => { t3Actual[a, b, c] = a + b + c; });
            Assert.AreEqual(t3Expected, t3Actual);
        }

        [TestMethod]
        public void IndexingAndAssigning4D()
        {
            var t4Actual = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 4, 5, 6), ids => 0);
            var t4Expected = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(3, 4, 5, 6), ids => ids[0] + ids[1] + ids[2] + ids[3]);
            foreach (var (ids, _) in t4Actual.Iterate())
                t4Actual[ids[0], ids[1], ids[2], ids[3]] = ids.Sum();
            Assert.AreEqual(t4Expected, t4Actual);
        }
    }
}
