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
    public static class BuiltinTypeInitter
    {
        public static void InitForInt()
        {
            ConstantsAndFunctions<int>.Add = (a, b) => a + b;
            ConstantsAndFunctions<int>.Subtract = (a, b) => a - b;
            ConstantsAndFunctions<int>.Multiply = (a, b) => a * b;
            ConstantsAndFunctions<int>.Divide = (a, b) => a / b;
            ConstantsAndFunctions<int>.CreateZero = () => 0;
            ConstantsAndFunctions<int>.CreateOne = () => 1;
            ConstantsAndFunctions<int>.Default = () => 0;
            ConstantsAndFunctions<int>.AreEqual = (a, b) => a == b;
            ConstantsAndFunctions<int>.Negate = a => -a;
            ConstantsAndFunctions<int>.IsZero = a => a == 0;
            ConstantsAndFunctions<int>.Copy = a => a;
        }

        public static void InitForFloat()
        {
            ConstantsAndFunctions<float>.Add = (a, b) => a + b;
            ConstantsAndFunctions<float>.Subtract = (a, b) => a - b;
            ConstantsAndFunctions<float>.Multiply = (a, b) => a * b;
            ConstantsAndFunctions<float>.Divide = (a, b) => a / b;
            ConstantsAndFunctions<float>.CreateZero = () => 0;
            ConstantsAndFunctions<float>.CreateOne = () => 1;
            ConstantsAndFunctions<float>.Default = () => 0;
            ConstantsAndFunctions<float>.AreEqual = (a, b) => a == b;
            ConstantsAndFunctions<float>.Negate = a => -a;
            ConstantsAndFunctions<float>.IsZero = a => a == 0;
            ConstantsAndFunctions<float>.Copy = a => a;
        }
    }
}
