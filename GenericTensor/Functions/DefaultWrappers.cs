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
    }
    public class TensorInt : Tensor<TensorIntWrapper, int> { }
}
