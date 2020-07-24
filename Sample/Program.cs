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

public static class Samples
{
    public static void CreatingMatrix()
    {
        var myMatrix = GenTensor<float>.CreateMatrix(
            new float[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            }
            );
        Console.WriteLine(myMatrix);
    }

    public static void CreatingMatrixAndMultiply()
    {
        var myMatrix = GenTensor<float>.CreateMatrix(
            new float[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            }
        );
        Console.WriteLine(GenTensor<float>.MatrixDotProduct(myMatrix, myMatrix));
    }
}


class Program
{
    public static GenTensor<T> Divide<T>(GenTensor<T> a, GenTensor<T> b)
    {
        b = b.Forward();
        b.InvertMatrix();
        return GenTensor<T>.MatrixDotProduct(a, b);
    }

    static void Main(string[] args)
    {
        BuiltinTypeInitter.InitForFloat();
        //Samples.CreatingMatrixAndMultiply();
        var myMatrix = GenTensor<float>.CreateMatrix(
            new float[,]
            {
                {6,  1, 1},
                {4, -2, 5},
                {2,  8, 7}
            }
        );

        var myMatrix1 = GenTensor<float>.CreateMatrix(
            new float[,]
            {
                {6,  1, 1},
                {4, -1, 5},
                {2,  8, 7}
            }
        );
        Console.WriteLine(
            Divide(myMatrix, myMatrix1)
            );
    }
}
