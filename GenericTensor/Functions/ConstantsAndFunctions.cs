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

        public static TPrimitive Multiply(TWrapper a, TWrapper b)
        {
            var r = a.Forward();
            r.Multiply(b);
            return r.GetValue();
        }

        public static TWrapper MultiplySaveWrapper(TWrapper a, TWrapper b)
        {
            var r = a.Forward();
            r.Multiply(b);
            return (TWrapper)r;
        }

        public static TPrimitive Subtract(TPrimitive a, TPrimitive b)
        {
            var c = Create(a);
            var d = Create(b);
            c.Subtract(d);
            return c.GetValue();
        }

        public static TWrapper SubtractSaveWrapper(TWrapper a, TWrapper b)
        {
            var c = a.Forward();
            c.Subtract(b);
            return (TWrapper)c;
        }

        public static TPrimitive Add(TPrimitive a, TPrimitive b)
        {
            var c = Create(a);
            var d = Create(b);
            c.Add(d);
            return c.GetValue();
        }

        public static TWrapper AddSaveWrapper(TWrapper a, TWrapper b)
        {
            var c = a.Forward();
            c.Add(b);
            return (TWrapper)c;
        }

        public static TPrimitive Divide(TPrimitive a, TPrimitive b)
        {
            var c = Create(a);
            var d = Create(b);
            c.Divide(d);
            return c.GetValue();
        }

        public static TWrapper DivideSaveWrapper(TWrapper a, TWrapper b)
        {
            var c = a.Forward();
            c.Divide(b);
            return (TWrapper)c;
        }
    }
}
