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
using System.Linq;
using System.Linq.Expressions;
using GenericTensor.Core;
using GenericTensor.Core.Expressions;
using GenericTensor.Functions;

using FuncFrom3 = System.Action<GenericTensor.Core.GenTensor<int, GenericTensor.Functions.IntWrapper>, GenericTensor.Core.GenTensor<int, GenericTensor.Functions.IntWrapper>, GenericTensor.Core.GenTensor<int, GenericTensor.Functions.IntWrapper>>;
using TS = GenericTensor.Core.GenTensor<int, GenericTensor.Functions.IntWrapper>;
public static class Samples
{
    public static void CreatingMatrix()
    {
        var myMatrix = GenTensor<float, FloatWrapper>.CreateMatrix(
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
        var myMatrix = GenTensor<float, FloatWrapper>.CreateMatrix(
            new float[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            }
        );
        var multipled = GenTensor<float, FloatWrapper>.MatrixMultiply(myMatrix, myMatrix);
        Console.WriteLine(GenTensor<float, FloatWrapper>.MatrixMultiply(myMatrix, multipled));
        var t = GenTensor<float, FloatWrapper>.Stack(myMatrix, myMatrix);
        t.Transpose(0, 2);
        Console.WriteLine(t);
    }

    public static GenTensor<T, GenericWrapper<T>> LazyWrapperMultiply<T>(T[,] a, T[,] b)
    {
        return GenTensor<T, GenericWrapper<T>>.MatrixMultiply(
            GenTensor<T, GenericWrapper<T>>.CreateMatrix(a),
            GenTensor<T, GenericWrapper<T>>.CreateMatrix(b)
        );
    }
}


class Program
{
    static TS CreateMatrix(int size)
        => TS.CreateMatrix(size, size, (x, y) => x + y);

    static void Main(string[] args)
    {

        var a = CreateMatrix(30);
        var b = CreateMatrix(30);
        /*
        Console.WriteLine(a);
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine(b);
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine(ExpressionCompiler<int, IntWrapper>.PiecewiseAdd(a, b, false));
        
        //GenTensor<int, IntWrapper>.PiecewiseAdd(a, b, Threading.Multi);
        */
        var res = ExpressionCompiler<int, IntWrapper>.PiecewiseAdd(a, b, true);
        Console.WriteLine(res);
        
    }
}
