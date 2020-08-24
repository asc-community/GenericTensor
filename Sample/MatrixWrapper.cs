/*
 *
 * This file shows how the user of this library is supposed to write their own
 * wrappers for GenTensors.
 *
 */

using GenericTensor.Core;

namespace Sample
{
    /// <summary>
    /// This class is only supposed to be used for matrix multiplication
    /// </summary>
    public sealed class MyMatrix
    {
        // if you want to use a built-in type with the same operations as they provide,
        // you can use default wrappers like IntWrapper, FloatWrapper, DoubleWrapper,
        // and other in DefaultWrappers.cs
        private readonly GenTensor<double, MyCustomDoubleTypeWrapper> inner;

        public MyMatrix(int width, int height)
        // Many constructors and functions may be directly redirected in one
        // defined in GenericTensor, but you are free to write your own new method
            : this(GenTensor<double, MyCustomDoubleTypeWrapper>.CreateMatrix(width, height))
        {
            
        }

        private MyMatrix(GenTensor<double, MyCustomDoubleTypeWrapper> newInner)
            => inner = newInner;

        public double this[int x, int y]
        {
            // You may add bound check here, but they all are checked for you in inner tensor
            get => inner[x, y];
            set => inner[x, y] = value;
        }

        // MatrixMultiply only requires Add, Multiply, and CreateZero defined for your primitives
        public static MyMatrix operator *(MyMatrix a, MyMatrix b)
            => new MyMatrix(GenTensor<double, MyCustomDoubleTypeWrapper>.MatrixMultiply(a.inner, b.inner));


        // this will require ToString to be defined in your primitive wrapper
        public override string ToString()
            => inner.ToString();


        // Make sure that all required operations are implemented
        // as there is no compile-time check on whether a method is implemented
        // and it will throw NotImplementedException if not
        private struct MyCustomDoubleTypeWrapper : IOperations<double>
        {
            public double Add(double a, double b) => a + b;
            public double Multiply(double a, double b) => a * b;
            public double CreateZero() => 0;
            
            // It's better to somehow define it as it will make your debug-time easier :)
            public string ToString(double a) => a.ToString();
        }
    }
}
