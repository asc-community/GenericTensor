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

        void ITensorElement<int>.Add(ITensorElement<int> other)
        {
            val += other.GetValue();
        }

        void ITensorElement<int>.Multiply(ITensorElement<int> other)
        {
            val *= other.GetValue();
        }

        void ITensorElement<int>.Subtract(ITensorElement<int> other)
        {
            val -= other.GetValue();
        }

        void ITensorElement<int>.Divide(ITensorElement<int> other)
        {
            val /= other.GetValue();
        }

        void ITensorElement<int>.Negate()
        {
            val *= -1;
        }

    }


    public class TensorInt : Tensor<TensorIntWrapper, int>
    {
        public TensorInt(params int[] dimensions) : base(dimensions) { }
    }
}
