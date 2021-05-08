using GenericTensor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    internal sealed class MyDouble
    {
        public double Value { get; }
        public MyDouble(double v) => Value = v;
        public override string ToString() => Value.ToString();
        public override bool Equals(object obj)
            => obj is MyDouble { Value: var val } && val == Value;
    }

    internal struct MyDoubleWrapper : IOperations<MyDouble>
    {
        public MyDouble Add(MyDouble a, MyDouble b) => new(a.Value + b.Value);
        public MyDouble Subtract(MyDouble a, MyDouble b) => new(a.Value - b.Value);
        public MyDouble Multiply(MyDouble a, MyDouble b) => new(a.Value * b.Value);
        public MyDouble Negate(MyDouble a) => new(-a.Value);
        public MyDouble Divide(MyDouble a, MyDouble b) => new(a.Value / b.Value);
        public MyDouble CreateOne() => new(1);
        public MyDouble CreateZero() => new(0);
        public MyDouble Copy(MyDouble a) => a;
        public bool AreEqual(MyDouble a, MyDouble b) => a.Value == b.Value;
        public bool IsZero(MyDouble a) => a.Value == 0;
        public string ToString(MyDouble a) => a.Value.ToString();
        public byte[] Serialize(MyDouble a) => throw new();
        public MyDouble Deserialize(byte[] data) => throw new();
    }

    [TestClass]
    public class ReferenceWrapperTest
    {
        [TestMethod]
        public void TestValidDetOfDefaultMatrix()
        {
            var m = GenTensor<MyDouble, MyDoubleWrapper>.CreateMatrix(new MyDouble[,]
            {
                { new(1), new(0) },
                { new(0), new(1) }
            });
            var adj = m.Adjoint();
            Assert.AreEqual(m, adj);
            m.InvertMatrix();
            Assert.AreEqual(m, adj);
            Assert.AreEqual(new(1), m.DeterminantGaussianSafeDivision());
            Assert.AreEqual(new(1), m.DeterminantGaussianSimple());
        }
    }
}
