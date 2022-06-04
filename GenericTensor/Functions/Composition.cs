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

namespace GenericTensor.Functions
{
    internal static class Composition<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        public static GenTensor<T, TWrapper> Stack(params GenTensor<T, TWrapper>[] elements)
        {
            #if ALLOW_EXCEPTIONS
            if (elements.Length < 1)
                throw new InvalidShapeException("Shoud be at least one element to stack");
            #endif
            var desiredShape = elements[0].Shape;
            #if ALLOW_EXCEPTIONS
            for (int i = 1; i < elements.Length; i++)
                if (elements[i].Shape != desiredShape)
                    throw new InvalidShapeException($"Tensors in {nameof(elements)} should be of the same shape");
            #endif
            var newShape = new int[desiredShape.Count + 1];
            newShape[0] = elements.Length;
            for (int i = 1; i < newShape.Length; i++)
                newShape[i] = desiredShape[i - 1];
            var res = new GenTensor<T, TWrapper>(newShape);
            for (int i = 0; i < elements.Length; i++)
                res.SetSubtensor(elements[i], i);
            return res;
        }

        public static GenTensor<T, TWrapper> Concat(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape.SubShape(1, 0) != b.Shape.SubShape(1, 0))
                throw new InvalidShapeException("Excluding the first dimension, all others should match");
            #endif

            if (a.IsVector)
            {
                var resultingVector = GenTensor<T, TWrapper>.CreateVector(a.Shape.shape[0] + b.Shape.shape[0]);
                for (int i = 0; i < a.Shape.shape[0]; i++)
                    resultingVector.SetValueNoCheck(a.GetValueNoCheck(i), i);

                for (int i = 0; i < b.Shape.shape[0]; i++)
                    resultingVector.SetValueNoCheck(b.GetValueNoCheck(i), i + a.Shape.shape[0]);

                return resultingVector;
            }
            else
            {
                var newShape = a.Shape.Copy();
                newShape.shape[0] = a.Shape.shape[0] + b.Shape.shape[0];

                var res = new GenTensor<T, TWrapper>(newShape);
                for (int i = 0; i < a.Shape.shape[0]; i++)
                    res.SetSubtensor(a.GetSubtensor(i), i);

                for (int i = 0; i < b.Shape.shape[0]; i++)
                    res.SetSubtensor(b.GetSubtensor(i), i + a.Shape.shape[0]);

                return res;
            }
        }
        
        public static void Fold<TCollapse, U, UWrapper>(TCollapse collapse, GenTensor<U, UWrapper> acc, GenTensor<T, TWrapper> t, int axis)
            where TCollapse : struct, HonkPerf.NET.Core.IValueDelegate<U, T, U>
            where UWrapper : struct, IOperations<U>
        {
            for (int i = axis; i > 0; i--)
                t.Transpose(i, i - 1);
            for (int i = 0; i < t.Shape[0]; i++)
            {
                var sub = t.GetSubtensor(i);
                foreach (var (id, value) in sub.Iterate())
                    acc[id] = collapse.Invoke(acc[id], value);
            }
            for (int i = 0; i < axis; i++)
                t.Transpose(i, i + 1);
        }
    }
}
    