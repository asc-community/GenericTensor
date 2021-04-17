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


using GenericTensor.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Functions
{
    internal static class ElementaryRowOperations<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        public static void RowMultiply(GenTensor<T, TWrapper> t, int rowId, T coef)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            #endif
            for (int i = 0; i < t.Shape[1]; i++)
                t.SetValueNoCheck(default(TWrapper).Multiply(coef, t.GetValueNoCheck(rowId, i)), rowId, i);
        }

        public static void RowAdd(GenTensor<T, TWrapper> t, int dstRowId, int srcRowId, T coef)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            #endif
            for (int i = 0; i < t.Shape[1]; i++)
                t.SetValueNoCheck(
                    default(TWrapper).Add(
                        t.GetValueNoCheck(srcRowId, i),
                        default(TWrapper).Multiply(coef, t.GetValueNoCheck(srcRowId, i))
                        ), 
                    dstRowId, i);
        }

        public static void RowSwap(GenTensor<T, TWrapper> t, int row1Id, int row2Id)
        {
            #if ALLOW_EXCEPTIONS
            if (!t.IsMatrix)
                throw new InvalidShapeException("this should be matrix");
            #endif
            for (int i = 0; i < t.Shape[1]; i++)
            {
                var tmp = t.GetValueNoCheck(row1Id, i);
                t.SetValueNoCheck(t.GetValueNoCheck(row2Id, i), row1Id, i);
                t.SetValueNoCheck(tmp, row2Id, i);
            }
        }
    }
}
