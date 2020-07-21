using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public bool IsMatrix => Shape.Count == 2;
        public bool IsVector => Shape.Count == 1;
        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Tensor<TWrapper, TPrimitive> ten))
                return false;
            if (ten.Shape != Shape)
                return false;
            foreach (var (index, _) in ten.Iterate())
                if (!this[index].Equals(ten[index]))
                    return false;
            return true;
        }

        public static bool operator ==(Tensor<TWrapper, TPrimitive> a, Tensor<TWrapper, TPrimitive> b)
            => a.Equals(b);

        public static bool operator !=(Tensor<TWrapper, TPrimitive> a, Tensor<TWrapper, TPrimitive> b)
            => a.Equals(b);

        public static Tensor<TWrapper, TPrimitive> CreateVector(params TPrimitive[] elements)
        {
            var res = new Tensor<TWrapper, TPrimitive>(elements.Length);
            for (int i = 0; i < elements.Length; i++)
                res[i] = elements[i];
            return res;
        }
    }
}
