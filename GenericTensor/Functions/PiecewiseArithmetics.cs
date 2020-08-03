﻿#region copyright
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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    public interface IZipOperator<T>
    {
        T Operation(T a, T b);
    }

    internal static class PiecewiseArithmetics<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        

        public static GenTensor<T, TWrapper> Zip<TOperator>(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading = Threading.Single) where TOperator : struct, IZipOperator<T>
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape != b.Shape)
                throw new InvalidShapeException("Arguments should be of the same shape");
            #endif
            var res = new GenTensor<T, TWrapper>(a.Shape);

            var parallel = threading == Threading.Multi || (threading == Threading.Auto && a.Volume > 850);

            if (!parallel)
            {
                switch (res.Shape.shape.Length)
                {

                    case 1:
                        for (int x = 0; x < res.Shape.shape[0]; x++)
                            res.data[x] = default(TWrapper).Forward(
                                default(TOperator).Operation(a.GetValueNoCheck(x), b.GetValueNoCheck(x)));
                        break;
                    case 2:
                    {
                        for (int x = 0; x < res.Shape.shape[0]; x++)
                        {
                            var currId = x * res.blocks[0];
                            for (int y = 0; y < res.Shape.shape[1]; y++)
                            {
                                res.data[currId] = default(TWrapper).Forward(
                                    default(TOperator).Operation(a.GetValueNoCheck(x, y), b.GetValueNoCheck(x, y)));
                                currId += 1;
                            }
                        }
                    }
                        break;
                    case 3:
                    {
                        for (int x = 0; x < res.Shape.shape[0]; x++)
                        for (int y = 0; y < res.Shape.shape[1]; y++)
                        {
                            var currId = x * res.blocks[0] + y * res.blocks[1];
                            for (int z = 0; z < res.Shape.shape[2]; z++)
                            {
                                res.data[currId] = default(TWrapper).Forward(
                                    default(TOperator).Operation(a.GetValueNoCheck(x, y, z),
                                        b.GetValueNoCheck(x, y, z)));
                                currId++;
                            }
                        }
                    }
                        break;
                    default:
                        foreach (var index in res.IterateOverElements())
                            res.SetValueNoCheck(default(TWrapper).Forward(
                                    default(TOperator).Operation(a.GetValueNoCheck(index), b.GetValueNoCheck(index))),
                                index);
                        break;
                }
            }
            else
            {
                if (res.Shape.shape.Length == 1)
                    for (int x = 0; x < res.Shape.shape[0]; x++)
                        res.data[x] = default(TWrapper).Forward(
                            default(TOperator).Operation(a.GetValueNoCheck(x), b.GetValueNoCheck(x)));
                else if (res.Shape.shape.Length == 2)
                    Parallel.For(0, res.Shape.shape[0], x =>
                    {
                        var currId = x * res.blocks[0];
                        for (int y = 0; y < res.Shape.shape[1]; y++)
                        {
                            res.data[currId] = default(TWrapper).Forward(
                                default(TOperator).Operation(a.GetValueNoCheck(x, y), b.GetValueNoCheck(x, y)));
                            currId++;
                        }
                    });
                else if (res.Shape.shape.Length == 3)
                    Parallel.For(0, res.Shape.shape[0], x =>
                    {
                        for (int y = 0; y < res.Shape.shape[1]; y++)
                        {
                            var currId = x * res.blocks[0] + y * res.blocks[1];
                            for (int z = 0; z < res.Shape.shape[2]; z++)
                            {
                                res.data[currId] = default(TWrapper).Forward(
                                    default(TOperator).Operation(a.GetValueNoCheck(x, y, z),
                                        b.GetValueNoCheck(x, y, z)));
                                currId++;
                            }
                        }
                    });
                else
                    foreach (var index in res.IterateOverElements())
                        res.SetValueNoCheck(default(TWrapper).Forward(
                            default(TOperator).Operation(a.GetValueNoCheck(index), b.GetValueNoCheck(index))), index);
            }
            return res;
        }

        internal struct AddWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Add(a, b);
        }

        internal struct SubtractWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Subtract(a, b);
        }

        internal struct MultiplyWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Multiply(a, b);
        }

        internal struct DivideWrapper : IZipOperator<T>
        {
            public T Operation(T a, T b) => default(TWrapper).Divide(a, b);
        }

        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => Zip<AddWrapper>(a, b, threading);

        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => Zip<SubtractWrapper>(a, b, threading);

        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => Zip<MultiplyWrapper>(a, b, threading);

        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b, Threading threading)
            => Zip<DivideWrapper>(a, b, threading);

        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Add(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Subtract(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseSubtract(
            T a, GenTensor<T, TWrapper> b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(b.Shape, ind => 
                default(TWrapper).Subtract(a, b[ind]), threading);

        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Multiply(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseDivide(GenTensor<T, TWrapper> a,
            T b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(a.Shape, ind => 
                default(TWrapper).Divide(a[ind], b), threading);

        public static GenTensor<T, TWrapper> PiecewiseDivide(
            T a, GenTensor<T, TWrapper> b, Threading threading)
            => Constructors<T, TWrapper>.CreateTensor(b.Shape, ind => 
                default(TWrapper).Divide(a, b[ind]), threading);
    }
}
