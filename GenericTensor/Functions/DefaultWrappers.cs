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
using System.Numerics;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    // If you do not want to determine which of classical wrappers to use, use this one,
    // it will determine it for you

    #region Lazy wrapper

    public struct GenericWrapper<T> : IOperations<T>
    {
        public T Add(T a, T b)
        {
            if (typeof(T) == typeof(int))
                return (T) (object) ((int) (object) a + (int) (object) b);
            else if (typeof(T) == typeof(float))
                return (T) (object) ((float) (object) a + (float) (object) b);
            else if (typeof(T) == typeof(double))
                return (T) (object) ((double) (object) a + (double) (object) b);
            else if (typeof(T) == typeof(Complex))
                return (T) (object) ((Complex) (object) a + (Complex) (object) b);
            else
                throw new NotSupportedException();
        }

        public T Subtract(T a, T b)
        {
            if (typeof(T) == typeof(int))
                return (T) (object) ((int) (object) a - (int) (object) b);
            else if (typeof(T) == typeof(float))
                return (T) (object) ((float) (object) a - (float) (object) b);
            else if (typeof(T) == typeof(double))
                return (T) (object) ((double) (object) a - (double) (object) b);
            else if (typeof(T) == typeof(Complex))
                return (T) (object) ((Complex) (object) a - (Complex) (object) b);
            else
                throw new NotSupportedException();
        }

        public T Multiply(T a, T b)
        {
            if (typeof(T) == typeof(int))
                return (T) (object) ((int) (object) a * (int) (object) b);
            else if (typeof(T) == typeof(float))
                return (T) (object) ((float) (object) a * (float) (object) b);
            else if (typeof(T) == typeof(double))
                return (T) (object) ((double) (object) a * (double) (object) b);
            else if (typeof(T) == typeof(Complex))
                return (T) (object) ((Complex) (object) a * (Complex) (object) b);
            else
                throw new NotSupportedException();
        }

        public T Negate(T a)
        {
            if (typeof(T) == typeof(int))
                return (T) (object) (-(int) (object) a);
            else if (typeof(T) == typeof(float))
                return (T) (object) (-(float) (object) a);
            else if (typeof(T) == typeof(double))
                return (T) (object) (-(double) (object) a);
            else if (typeof(T) == typeof(Complex))
                return (T) (object) (-(Complex) (object) a);
            else
                throw new NotSupportedException();
        }

        public T Divide(T a, T b)
        {
            if (typeof(T) == typeof(int))
                return (T) (object) ((int) (object) a / (int) (object) b);
            else if (typeof(T) == typeof(float))
                return (T) (object) ((float) (object) a / (float) (object) b);
            else if (typeof(T) == typeof(double))
                return (T) (object) ((double) (object) a / (double) (object) b);
            else if (typeof(T) == typeof(Complex))
                return (T) (object) ((Complex) (object) a / (Complex) (object) b);
            else
                throw new NotSupportedException();
        }

        public T CreateOne()
        {
            if (typeof(T) == typeof(int))
                return (T) (object) (int) 1;
            else if (typeof(T) == typeof(float))
                return (T) (object) (float) 1;
            if (typeof(T) == typeof(double))
                return (T) (object) (double) 1;
            if (typeof(T) == typeof(Complex))
                return (T) (object) (Complex) 1;
            else
                throw new NotSupportedException();
        }

        public T CreateZero()
        {
            if (typeof(T) == typeof(int))
                return (T)(object)(int)0;
            else if (typeof(T) == typeof(float))
                return (T)(object)(float)0;
            if (typeof(T) == typeof(double))
                return (T)(object)(double)0;
            if (typeof(T) == typeof(Complex))
                return (T)(object)(Complex)0;
            else
                throw new NotSupportedException();
        }

        public T Copy(T a)
        {
            return a;
        }

        public bool AreEqual(T a, T b)
        {
            return object.ReferenceEquals(a, b) || a is {} && a.Equals(b);
        }

        public bool IsZero(T a)
        {
            if (typeof(T) == typeof(int))
                return a.Equals((T)(object)(int)0);
            else if (typeof(T) == typeof(float))
                return a.Equals((T)(object)(float)0);
            if (typeof(T) == typeof(double))
                return a.Equals((T)(object)(double)0);
            if (typeof(T) == typeof(Complex))
                return a.Equals((T)(object)(Complex)0);
            else
                throw new NotSupportedException();
        }

        public string ToString(T a)
        {
            return a?.ToString() ?? "";
        }

        public byte[] Serialize(T a)
        {
            if (typeof(T) == typeof(int))
                return BitConverter.GetBytes((int) (object) a);
            else if (typeof(T) == typeof(float))
                return BitConverter.GetBytes((float)(object)a);
            else if (typeof(T) == typeof(double))
                return BitConverter.GetBytes((double)(object)a);
            else if (typeof(T) == typeof(Complex))
                return SerializationUtils.SerializeComplex((Complex) (object) a);
            else
                throw new NotSupportedException();
        }

        public T Deserialize(byte[] data)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)BitConverter.ToInt32(data, 0);
            else if (typeof(T) == typeof(float))
                return (T)(object)BitConverter.ToSingle(data, 0);
            if (typeof(T) == typeof(double))
                return (T)(object)BitConverter.ToDouble(data, 0);
            if (typeof(T) == typeof(Complex))
                return (T) (object) SerializationUtils.DeserializeComplex(data);
            else
                throw new NotSupportedException();
        }
    }
    #endregion

    // Classic types you more likely to deal with

    #region Classical primitives
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
        public bool AreEqual(int a, int b) => a == b;
        public bool IsZero(int a) => a == 0;
        public string ToString(int a) => a.ToString();
        public byte[] Serialize(int a) => BitConverter.GetBytes(a);
        public int Deserialize(byte[] data) => BitConverter.ToInt32(data, 0);
    }

    public struct LongWrapper : IOperations<long>
    {
        public long Add(long a, long b) => a + b;
        public long Subtract(long a, long b) => a - b;
        public long Multiply(long a, long b) => a * b;
        public long Negate(long a) => -a;
        public long Divide(long a, long b) => a / b;
        public long CreateOne() => 1;
        public long CreateZero() => 0;
        public long Copy(long a) => a;
        public bool AreEqual(long a, long b) => a == b;
        public bool IsZero(long a) => a == 0;
        public string ToString(long a) => a.ToString();
        public byte[] Serialize(long a) => BitConverter.GetBytes(a);
        public long Deserialize(byte[] data) => BitConverter.ToInt64(data, 0);
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
        public bool AreEqual(float a, float b) => Math.Abs(a - b) < 1e-5;
        public bool IsZero(float a) => Math.Abs(a) < 1e-5;
        public string ToString(float a) => a.ToString();
        public byte[] Serialize(float a) => BitConverter.GetBytes(a);
        public float Deserialize(byte[] data) => BitConverter.ToSingle(data, 0);
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
        public bool AreEqual(double a, double b) => Math.Abs(a - b) < 1e-7;
        public bool IsZero(double a) => Math.Abs(a) < 1e-7;
        public string ToString(double a) => a.ToString();
        public byte[] Serialize(double a) => BitConverter.GetBytes(a);
        public double Deserialize(byte[] data) => BitConverter.ToDouble(data, 0);
    }

    #endregion

    // Those are rarely used types like BigInteger (if you need inifinitely large numbers) and Complex

    #region Complicated primitives
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
        public bool AreEqual(Complex a, Complex b) => a == b;
        public bool IsZero(Complex a) => a == 0;
        public string ToString(Complex a) => a.ToString();
        public byte[] Serialize(Complex a) => SerializationUtils.SerializeComplex(a);
        public Complex Deserialize(byte[] data) => SerializationUtils.DeserializeComplex(data);
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
        public bool AreEqual(BigInteger a, BigInteger b) => a == b;
        public bool IsZero(BigInteger a) => a == 0;
        public string ToString(BigInteger a) => a.ToString();
        public byte[] Serialize(BigInteger a) => a.ToByteArray();
        public BigInteger Deserialize(byte[] data) => new BigInteger(data);
    }
    #endregion
}

