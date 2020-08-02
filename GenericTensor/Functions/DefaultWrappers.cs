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
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    public struct IntegerWrapper : IOperations<int>
    {

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Negate(int a)
        {
            return -a;
        }

        public int Divide(int a, int b)
        {
            return a / b;
        }

        public int CreateOne()
        {
            return 1;
        }

        public int CreateZero()
        {
            return 0;
        }

        public int Copy(int a)
        {
            return a;
        }

        public int Forward(int a)
        {
            return a;
        }

        public bool AreEqual(int a, int b)
        {
            return a == b;
        }

        public bool IsZero(int a)
        {
            return a == 0;
        }

        public string ToString(int a)
        {
            return a.ToString();
        }
    }

    public struct FloatWrapper : IOperations<float>
    {

        public float Add(float a, float b)
        {
            return a + b;
        }

        public float Subtract(float a, float b)
        {
            return a - b;
        }

        public float Multiply(float a, float b)
        {
            return a * b;
        }

        public float Negate(float a)
        {
            return -a;
        }

        public float Divide(float a, float b)
        {
            return a / b;
        }

        public float CreateOne()
        {
            return 1;
        }

        public float CreateZero()
        {
            return 0;
        }

        public float Copy(float a)
        {
            return a;
        }

        public float Forward(float a)
        {
            return a;
        }

        public bool AreEqual(float a, float b)
        {
            return Math.Abs(a - b) < 1e-6;
        }

        public bool IsZero(float a)
        {
            return Math.Abs(a) < 1e-6;
        }

        public string ToString(float a)
        {
            return a.ToString();
        }
    }
}
