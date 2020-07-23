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

using GenericTensor;
using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using System.Linq;

namespace Sample
{
    class Program
    { 
        static void Main(string[] args)
        {
            var t1 = TensorInt.CreateMatrix(new[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            var t2 = TensorInt.CreateMatrix(new[,] { { 3, 2, 1 }, { 6, 5, 4 }, { 9, 8, 7 } });

            for(int i = 0; true; i++)
            {
                var t3 = TensorInt.MatrixDotProduct(t1, t2);
                (t1, t2) = (t2, t3);
            }
        }
    }
}
