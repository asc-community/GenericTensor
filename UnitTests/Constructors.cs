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
        public void CreateTensor0D()
        {
            var x = new GenTensor<int, IntWrapper>(new TensorShape(System.Array.Empty<int>()));
            void Check(int value)
            {
                Assert.IsFalse(x.IsVector);
                Assert.IsFalse(x.IsMatrix);
                Assert.IsFalse(x.IsSquareMatrix);
                CollectionAssert.AreEqual(System.Array.Empty<int>(), x.Shape.ToArray());
                Assert.AreEqual(1, x.Volume);
                AssertTensor(Enumerable.Range(value, 1), x);
                Assert.AreEqual(value, x[System.Array.Empty<int>()]);
                Assert.AreEqual(value, x.GetCell(System.Array.Empty<int>()));
                Assert.AreEqual(value, x.GetValueNoCheck(System.Array.Empty<int>()));
                CollectionAssert.AreEqual(System.Array.Empty<int>(), x.blocks);
                CollectionAssert.AreEqual(new[] { value }, x.data);
                Assert.AreEqual(0, x.LinOffset);
                CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0, 4, 0, 0, 0, (byte)value, 0, 0, 0 }, x.Serialize());
            }
            Check(0);
            x[System.Array.Empty<int>()] = 3;
            Check(3);
            x.SetValueNoCheck(2, System.Array.Empty<int>());
            Check(2);
            x.SetCell(1, System.Array.Empty<int>());
            Check(1);
            x = x.Forward();
            Check(1);
            x = (GenTensor<int, IntWrapper>)x.Clone();
            Check(1);
            x = GenTensor<int, IntWrapper>.Deserialize(new byte[] { 0, 0, 0, 0, 4, 0, 0, 0, 9, 0, 0, 0 });
            Check(9);
            //x = GenTensor<int, IntWrapper>.PiecewiseAdd(x, x);
            //Check(18);
            //x = GenTensor<int, IntWrapper>.PiecewiseMultiply(x, x);
            //Check(324);
            //x = GenTensor<int, IntWrapper>.PiecewiseDivide(x, x);
            //Check(1);
            //x = GenTensor<int, IntWrapper>.PiecewiseSubtract(x, x);
            //Check(0);
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
        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, 0)]
        [DataRow(1, 0, 0, 0, 0, 0)]
        [DataRow(1, 0, 0, 0, 0, 1)]
        [DataRow(1, 0, 0, 9, 0, 1)]
        [DataRow(0, 0, 0, 0, 0, 1)]
        [DataRow(9, 0, 0, 0, 0, 0)]
        [DataRow(9, 0, 0, 0, 0, 9)]
        [DataRow(9, 0, 0, 1, 0, 9)]
        [DataRow(0, 0, 0, 0, 0, 9)]
        [DataRow(9, 1, 0, 1, 1, 9)]
        public void CreateTensor6DEmpty(int d1, int d2, int d3, int d4, int d5, int d6)
        {
            var x = GenTensor<int, IntWrapper>.CreateTensor(new int[d1, d2, d3, d4, d5, d6]);
            Assert.IsFalse(x.IsVector);
            Assert.IsFalse(x.IsMatrix);
            Assert.IsFalse(x.IsSquareMatrix);
            CollectionAssert.AreEqual(new[] { d1, d2, d3, d4, d5, d6 }, x.Shape.ToArray());
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
        
        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(0, 4)]
        [DataRow(3, 0)]
        [DataRow(3, 4)]
        public void IndexingAndAssigning2D(int d1, int d2)
        {
            var t2Actual = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(d1, d2), ids => 0);
            var t2Expected = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(d1, d2), ids => ids[0] + ids[1]);
            t2Actual.IterateOver2((a, b) => { t2Actual[a, b] = a + b; });
            Assert.AreEqual(t2Expected, t2Actual);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(0, 0, 5)]
        [DataRow(0, 4, 0)]
        [DataRow(0, 4, 5)]
        [DataRow(3, 0, 0)]
        [DataRow(3, 0, 5)]
        [DataRow(3, 4, 0)]
        [DataRow(3, 4, 5)]
        public void IndexingAndAssigning3D(int d1, int d2, int d3)
        {
            var t3Actual = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(d1, d2, d3), ids => 0);
            var t3Expected = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(d1, d2, d3), ids => ids[0] + ids[1] + ids[2]);
            t3Actual.IterateOver3((a, b, c) => { t3Actual[a, b, c] = a + b + c; });
            Assert.AreEqual(t3Expected, t3Actual);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0)]
        [DataRow(0, 0, 0, 6)]
        [DataRow(0, 0, 5, 0)]
        [DataRow(0, 0, 5, 6)]
        [DataRow(0, 4, 0, 0)]
        [DataRow(0, 4, 0, 6)]
        [DataRow(0, 4, 5, 0)]
        [DataRow(0, 4, 5, 6)]
        [DataRow(3, 0, 0, 0)]
        [DataRow(3, 0, 0, 6)]
        [DataRow(3, 0, 5, 0)]
        [DataRow(3, 0, 5, 6)]
        [DataRow(3, 4, 0, 0)]
        [DataRow(3, 4, 0, 6)]
        [DataRow(3, 4, 5, 0)]
        [DataRow(3, 4, 5, 6)]
        public void IndexingAndAssigning4D(int d1, int d2, int d3, int d4)
        {
            var t4Actual = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(d1, d2, d3, d4), ids => 0);
            var t4Expected = GenTensor<int, IntWrapper>.CreateTensor(new TensorShape(d1, d2, d3, d4), ids => ids[0] + ids[1] + ids[2] + ids[3]);
            foreach (var (ids, _) in t4Actual.Iterate())
                t4Actual[ids[0], ids[1], ids[2], ids[3]] = ids.Sum();
            Assert.AreEqual(t4Expected, t4Actual);
        }
    }
}
