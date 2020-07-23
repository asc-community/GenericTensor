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

namespace GenericTensor.Core
{
    /// <summary>
    /// This is the interface you are to inherit from
    /// Your class will be the main element of a tensor,
    /// it will be appealed every time you perform any operations
    /// on tensor. You do not have to implement all methods though. For example,
    /// if you are only planning to use matrix product, you might
    /// not need to implement Divide at all
    /// </summary>
    public interface ITensorElement<T>
    {
        public T GetValue();
        public void SetValue(T newValue);
        public ITensorElement<T> Copy();
        public ITensorElement<T> Forward();
        public void SetZero();
        public void SetOne();
        public void Add(ITensorElement<T> other);
        public void Multiply(ITensorElement<T> other);
        public void Subtract(ITensorElement<T> other);
        public void Divide(ITensorElement<T> other);
        public void Negate();
        public bool IsZero(); // if you can't implement it, return false
    }
}
