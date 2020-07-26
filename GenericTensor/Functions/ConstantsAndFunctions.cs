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

namespace GenericTensor.Functions
{
    public static class ConstantsAndFunctions<T>
    {
        static Y AskForDefining<Y>(string methodName)
        {
            throw new NotImplementedException(
                $"This operation requires ConstantsAndFunctions<{typeof(T)}>." +
                $"{methodName} to be defined.");
        }

        // 1
        public static Func<T> CreateOne = () => AskForDefining<T>("CreateOne");

        // 0
        public static Func<T> CreateZero = () => AskForDefining<T>("CreateZero");

        // +
        public static Func<T, T, T> Add = (a, b) => AskForDefining<T>("Add");

        // -
        public static Func<T, T, T> Subtract = (a, b) => AskForDefining<T>("Sutbract");

        // *
        public static Func<T, T, T> Multiply = (a, b) => AskForDefining<T>("Multiply");

        // /
        public static Func<T, T, T> Divide = (a, b) => AskForDefining<T>("Divide");

        // copying
        public static Func<T, T> Copy = a => AskForDefining<T>("Copy");

        // forwarding (only copy the wrapper if have one)
        public static Func<T, T> Forward = a => AskForDefining<T>("Forward");

        // ==
        public static Func<T, T, bool> AreEqual = (a, b) => AskForDefining<bool>("AreEqual");

        // -
        public static Func<T, T> Negate = a => AskForDefining<T>("Negate");

        // == 0
        public static Func<T, bool> IsZero = a => AskForDefining<bool>("IsZero");

        // .ToSting()
        public new static Func<T, string> ToString = a => AskForDefining<string>("ToString");
    }
}
