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
using System.Collections.Generic;
using System.Text;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    public class TensorIntWrapper : ITensorElement<int>
    {
        private int val;
        public int GetValue() => val;
        public void SetValue(int newValue) => this.val = newValue;

        public override string ToString()
            => val.ToString();

        public ITensorElement<int> Copy()
        {
            var res = new TensorIntWrapper();
            res.SetValue(val);
            return res;
        }

        public ITensorElement<int> Forward()
        {
            var res = new TensorIntWrapper();
            res.SetValue(val);
            return res;
        }

        public void SetZero()
            => SetValue(0);

        public void SetOne()
            => SetValue(1);

        public void Add(ITensorElement<int> other)
        {
            val += other.GetValue();
        }

        public void Multiply(ITensorElement<int> other)
        {
            val *= other.GetValue();
        }

        public void Subtract(ITensorElement<int> other)
        {
            val -= other.GetValue();
        }

        public void Divide(ITensorElement<int> other)
        {
            val /= other.GetValue();
        }

        public void Negate()
        {
            val *= -1;
        }

    }

    public class TensorFloatWrapper : ITensorElement<float>
    {
        private float val;
        public float GetValue() => val;
        public void SetValue(float newValue) => this.val = newValue;

        public override string ToString()
            => val.ToString();

        public ITensorElement<float> Copy()
        {
            var res = new TensorFloatWrapper();
            res.SetValue(val);
            return res;
        }

        public ITensorElement<float> Forward()
        {
            var res = new TensorFloatWrapper();
            res.SetValue(val);
            return res;
        }

        public void SetZero()
            => SetValue(0);

        public void SetOne()
            => SetValue(1);

        public void Add(ITensorElement<float> other)
        {
            val += other.GetValue();
        }

        public void Multiply(ITensorElement<float> other)
        {
            val *= other.GetValue();
        }

        public void Subtract(ITensorElement<float> other)
        {
            val -= other.GetValue();
        }

        public void Divide(ITensorElement<float> other)
        {
            val /= other.GetValue();
        }

        public void Negate()
        {
            val *= -1;
        }

    }


    public class TensorInt : Tensor<TensorIntWrapper, int>
    {
        public TensorInt(params int[] dimensions) : base(dimensions) { }
    }
}
