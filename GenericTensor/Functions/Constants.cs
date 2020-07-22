using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class Constants<TWrapper, TPrimitive> where TWrapper : ITensorElement<TPrimitive>, new()
    {
        public static TWrapper Create(TPrimitive value)
        {
            var res = new TWrapper();
            res.SetValue(value);
            return res;
        }

        public static TWrapper CreateOne()
        {
            var res = new TWrapper();
            res.SetOne();
            return res;
        }

        public static TWrapper CreateZero()
        {
            var res = new TWrapper();
            res.SetZero();
            return res;
        }
    }
}
