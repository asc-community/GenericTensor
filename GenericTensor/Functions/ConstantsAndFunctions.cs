#region copyright
/*
 * MIT License
 * 
 * Copyright (c) 2020-2021 WhiteBlackGoose
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
using GenericTensor.Core;

namespace GenericTensor.Functions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class ConstantsAndFunctionsForwarder<T, TWrapper> where TWrapper : struct, IOperations<T>

    {
        static Y AskForDefining<Y>([System.Runtime.CompilerServices.CallerMemberName] string methodName = "") =>
            throw new NotImplementedException(
                $"This operation requires ConstantsAndFunctions<{typeof(T)}>." +
                $"{methodName} to be defined.");

        // 1
        public static Func<T> CreateOne = () => AskForDefining<T>();

        // 0
        public static Func<T> CreateZero = () => AskForDefining<T>();

        // +
        public static Func<T, T, T> Add = (a, b) => AskForDefining<T>();

        // -
        public static Func<T, T, T> Subtract = (a, b) => AskForDefining<T>();

        // *
        public static Func<T, T, T> Multiply = (a, b) => AskForDefining<T>();

        // /
        public static Func<T, T, T> Divide = (a, b) => AskForDefining<T>();

        // copying (deep copying)
        public static Func<T, T> Copy = a => AskForDefining<T>();

        // forwarding (only copy the wrapper if have one => shallow copying)
        public static Func<T, T> Forward = a => AskForDefining<T>();

        // ==
        public static Func<T, T, bool> AreEqual = (a, b) => AskForDefining<bool>();

        // -
        public static Func<T, T> Negate = a => AskForDefining<T>();

        // == 0
        public static Func<T, bool> IsZero = a => AskForDefining<bool>();

        // .ToString()
        public new static Func<T, string> ToString = a => AskForDefining<string>();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
