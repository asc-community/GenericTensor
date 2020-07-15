using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Core
{
    public interface ITensorElement<T>
    {
        public T GetValue();
        public void SetValue(T newValue);
    }
}
