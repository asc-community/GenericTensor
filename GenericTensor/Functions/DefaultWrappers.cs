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
using System.Globalization;
using System.Numerics;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    public struct IntWrapper : IOperations<int>
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;
        public int Negate(int a) => -a;
        public int Divide(int a, int b) => a / b;
        public int CreateOne() => 1;
        public int CreateZero() => 0;
        public int Copy(int a) => a;
        public int Forward(int a) => a;
        public bool AreEqual(int a, int b) => a == b;
        public bool IsZero(int a) => a == 0;
        public string ToString(int a) => a.ToString();
    }



    public struct FloatWrapper : IOperations<float>
    {
        public float Add(float a, float b) => a + b;
        public float Subtract(float a, float b) => a - b;
        public float Multiply(float a, float b) => a * b;
        public float Negate(float a) => -a;
        public float Divide(float a, float b) => a / b;
        public float CreateOne() => 1;
        public float CreateZero() => 0;
        public float Copy(float a) => a;
        public float Forward(float a) => a;
        public bool AreEqual(float a, float b) => Math.Abs(a - b) < 1e-5;
        public bool IsZero(float a) => Math.Abs(a) < 1e-5;
        public string ToString(float a) => a.ToString();
    }



    public struct DoubleWrapper : IOperations<double>
    {
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Negate(double a) => -a;
        public double Divide(double a, double b) => a / b;
        public double CreateOne() => 1;
        public double CreateZero() => 0;
        public double Copy(double a) => a;
        public double Forward(double a) => a;
        public bool AreEqual(double a, double b) => Math.Abs(a - b) < 1e-7;
        public bool IsZero(double a) => Math.Abs(a) < 1e-7;
        public string ToString(double a) => a.ToString();
    }



    public struct ComplexWrapper : IOperations<Complex>
    {
        public Complex Add(Complex a, Complex b) => a + b;
        public Complex Subtract(Complex a, Complex b) => a - b;
        public Complex Multiply(Complex a, Complex b) => a * b;
        public Complex Negate(Complex a) => -a;
        public Complex Divide(Complex a, Complex b) => a / b;
        public Complex CreateOne() => 1;
        public Complex CreateZero() => 0;
        public Complex Copy(Complex a) => a;
        public Complex Forward(Complex a) => a;
        public bool AreEqual(Complex a, Complex b) => a == b;
        public bool IsZero(Complex a) => a == 0;
        public string ToString(Complex a) => a.ToString();
    }



    public struct BigIntWrapper : IOperations<BigInteger>
    {
        public BigInteger Add(BigInteger a, BigInteger b) => a + b;
        public BigInteger Subtract(BigInteger a, BigInteger b) => a - b;
        public BigInteger Multiply(BigInteger a, BigInteger b) => a * b;
        public BigInteger Negate(BigInteger a) => -a;
        public BigInteger Divide(BigInteger a, BigInteger b) => a / b;
        public BigInteger CreateOne() => 1;
        public BigInteger CreateZero() => 0;
        public BigInteger Copy(BigInteger a) => a;
        public BigInteger Forward(BigInteger a) => a;
        public bool AreEqual(BigInteger a, BigInteger b) => a == b;
        public bool IsZero(BigInteger a) => a == 0;
        public string ToString(BigInteger a) => a.ToString();
    }
}

