using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class GetMatrixFactoryTest
    {
        private static Func<int, GenTensor<int, IntWrapper>> GetMatrix = GetMatrixFactoryHelper.Get();

        [DataTestMethod]
        [DataRow(5)]
        [DataRow(3)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(4)]
        public void TestMatrix(int diagLength)
        {
            Assert.AreEqual(new TensorShape(diagLength, diagLength), GetMatrix(diagLength).Shape);
            Assert.AreEqual(new TensorShape(diagLength, diagLength), GetMatrix(diagLength).Shape);
        }
    }

    static class GetMatrixFactoryHelper
    {
        internal static Func<int, GenTensor<int, IntWrapper>> Get()
        {
            var assem = typeof(GenTensor<,>).Assembly;
            var type = assem.GetType("GenericTensor.Core.TestSquareMatrixFactoryExposed");
            var method = type.GetMethod("TestGetMatrixExposed", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            return (d => (GenTensor<int, IntWrapper>)method.Invoke(null, new object[]{ d }));
        }
    }
}
