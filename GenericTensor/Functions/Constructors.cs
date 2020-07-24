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
using System.Runtime.CompilerServices;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class GenTensor<T>
    {
        /// <summary>
        /// Creates a tensor whose all matrices are identity matrices
        /// 1 is achieved with TWrapper.SetOne()
        /// 0 is achieved with TWrapper.SetZero()
        /// </summary>
        public static GenTensor<T> CreateIdentityTensor(int[] dimensions, int finalMatrixDiag)
        {
            var newDims = new int[dimensions.Length + 2];
            for (int i = 0; i < dimensions.Length; i++)
                newDims[i] = dimensions[i];
            newDims[newDims.Length - 2] = newDims[newDims.Length - 1] = finalMatrixDiag;
            var res = new GenTensor<T>(newDims);
            foreach (var index in res.IterateOverMatrices())
            {
                var iden = CreateIdentityMatrix(finalMatrixDiag);
                res.SetSubtensor(iden, index);
            }
            return res;
        }

        /// <summary>
        /// Creates an indentity matrix whose width and height are equal to diag
        /// 1 is achieved with TWrapper.SetOne()
        /// 0 is achieved with TWrapper.SetZero()
        /// </summary>
        public static GenTensor<T> CreateIdentityMatrix(int diag)
        {
            var res = new GenTensor<T>(diag, diag);
            for (int i = 0; i < res.Data.Length; i++)
                res.Data[i] = ConstantsAndFunctions<T>.CreateZero();

            for (int i = 0; i < diag; i++)
                res.SetValueNoCheck(ConstantsAndFunctions<T>.CreateOne, i, i);
            return res;
        }

        /// <summary>
        /// Creates a vector from an array of primitives
        /// Its length will be equal to elements.Length
        /// </summary>
        public static GenTensor<T> CreateVector(params T[] elements)
        {
            var res = new GenTensor<T>(elements.Length);
            for (int i = 0; i < elements.Length; i++)
                res[i] = elements[i];
            return res;
        }

        private static (int height, int width) ExtractAndCheck<T>(T[,] data)
        {
            var width = data.GetLength(0);
            #if ALLOW_EXCEPTIONS
            if (width <= 0)
                throw new InvalidShapeException();
            #endif
            var height = data.GetLength(1);
            #if ALLOW_EXCEPTIONS
            if (height <= 0)
                throw new InvalidShapeException();
            #endif
            return (width, height);
        }

        /// <summary>
        /// Creates a matrix from a two-dimensional array of primitives
        /// for example
        /// var M = Tensor.CreateMatrix(new[,]
        /// {
        ///     {1, 2},
        ///     {3, 4}
        /// });
        /// where yourData.GetLength(0) is Shape[0]
        /// yourData.GetLength(1) is Shape[1]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateMatrix(T[,] data)
        {
            var (width, height) = GenTensor<T>.ExtractAndCheck(data);
            var res = new GenTensor<T>(width, height);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                res.SetValueNoCheck(data[x, y], x, y);
            return res;
        }


        /// <summary>
        /// Creates a matrix of width and height size
        /// and iterator for each pair of coordinate
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateMatrix(int width, int height, Func<int, int, T> stepper)
        {
            var res = GenTensor<T>.CreateMatrix(width, height);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                res.SetValueNoCheck(stepper(x, y), x, y);
            return res;
        }

        /// <summary>
        /// Creates a matrix of width and height size
        /// </summary>
        public static GenTensor<T> CreateMatrix(int width, int height)
            => new GenTensor<T>(width, height);

        /// <summary>
        /// Creates a tensor of given size with iterator over its indecies
        /// (its only argument is an array of integers which are indecies of the tensor)
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenTensor<T> CreateTensor(TensorShape shape, Func<int[], T> operation)
        {
            var res = new GenTensor<T>(shape);
            foreach (var ind in res.IterateOverElements())
                res[ind] = operation(ind);
            return res;
        }
    }
}
