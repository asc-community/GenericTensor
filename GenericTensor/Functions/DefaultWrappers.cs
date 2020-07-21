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
    }

    public class TensorInt : Tensor<TensorIntWrapper, int>
    {
        public TensorInt(params int[] dimensions) : base(dimensions) { }
    }
}
