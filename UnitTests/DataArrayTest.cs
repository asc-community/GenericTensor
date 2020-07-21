using System;
using GenericTensor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class DataArrayTest
    {
        private static DataArray<int> da;

        public DataArrayTest()
        {
            var arr = new int[100];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = i;
            da = new DataArray<int>(arr);
        }

        [TestMethod]
        public void Equality()
        {
            Assert.AreEqual(da, da);
        }

        [TestMethod]
        public void Count()
        {
            Assert.AreEqual(da.Count, 100);
        }

        [TestMethod]
        public void Iter()
        {
            for (int i = 0; i < da.Count; i++)
                Assert.AreEqual(da[i], i);
        }

        [TestMethod]
        public void CountSlice1()
        {
            Assert.AreEqual(da.Slice(0, da.Count, 1), da);
        }

        [TestMethod]
        public void CountSlice2()
        {
            Assert.AreEqual(da.Slice(1, da.Count, 1).Count, da.Count - 1);
        }

        [TestMethod]
        public void CountSlice3()
        {
            Assert.AreEqual(da.Slice(0, da.Count, 2).Count, da.Count / 2);
        }

        [TestMethod]
        public void CountSlice4()
        {
            Assert.AreEqual(da.Slice(2, da.Count - 2, 2).Count, da.Count / 2 - 2);
        }

        [TestMethod]
        public void Slice1()
        {
            var sl = da.Slice(0, da.Count, 100);
            Assert.AreEqual(sl, new DataArray<int>(0));
        }

        [TestMethod]
        public void Slice2()
        {
            var sl = da.Slice(0, da.Count, 30);
            Assert.AreEqual(sl, new DataArray<int>(0, 30, 60, 90));
        }

        [TestMethod]
        public void Slice3()
        {
            var sl = da.Slice(0, 30, 3);
            Assert.AreEqual(sl.Count, 10);
            for (int i = 0; i < 10; i++)
                Assert.AreEqual(sl[i], i * 3);
        }

        [TestMethod]
        public void DoubleSlice1()
        {
            var sl = da.Slice(0, 30, 3);
            var sl2 = sl.Slice(2, 5, 2);
            Assert.AreEqual(sl2[0], 6);
            Assert.AreEqual(sl2[1], 12);
        }

        [TestMethod]
        public void Enum()
        {
            var d = da.Slice(30, 50, 15);
            foreach (var f in d)
                Assert.IsTrue(true);
        }
    }
}
