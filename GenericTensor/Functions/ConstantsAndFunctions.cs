using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class ConstantsAndFunctions<TWrapper, TPrimitive> where TWrapper : ITensorElement<TPrimitive>, new()
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

        public static TPrimitive Multiply(TPrimitive a, TPrimitive b)
        {
            var c = Create(a);
            var d = Create(b);
            c.Multiply(d);
            return c.GetValue();
        }

        public static TPrimitive Subtract(TPrimitive a, TPrimitive b)
        {
            var c = Create(a);
            var d = Create(b);
            c.Subtract(d);
            return c.GetValue();
        }

        public static TPrimitive Add(TPrimitive a, TPrimitive b)
        {
            var c = Create(a);
            var d = Create(b);
            c.Add(d);
            return c.GetValue();
        }
    }
}
