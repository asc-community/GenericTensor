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


using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class Composition<T>
    {
        public static GenTensor<T> Stack(params GenTensor<T>[] elements)
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
            var res = new GenTensor<T>(newShape);
            for (int i = 0; i < elements.Length; i++)
                res.SetSubtensor(elements[i], i);
            return res;
        }

        public static GenTensor<T> Concat(GenTensor<T> a, GenTensor<T> b)
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape.SubShape(1, 0) != b.Shape.SubShape(1, 0))
                throw new InvalidShapeException("Excluding the first dimension, all others should match");
            #endif

            if (a.IsVector)
            {
                var resultingVector = GenTensor<T>.CreateVector(a.Shape.shape[0] + b.Shape.shape[0]);
                for (int i = 0; i < a.Shape.shape[0]; i++)
                    resultingVector.SetValueNoCheck(ConstantsAndFunctions<T>.Forward(a.GetValueNoCheck(i)), i);

                for (int i = 0; i < b.Shape.shape[0]; i++)
                    resultingVector.SetValueNoCheck(ConstantsAndFunctions<T>.Forward(b.GetValueNoCheck(i)), i + a.Shape.shape[0]);

                return resultingVector;
            }
            else
            {
                var newShape = a.Shape.Copy();
                newShape.shape[0] = a.Shape.shape[0] + b.Shape.shape[0];

                var res = new GenTensor<T>(newShape);
                for (int i = 0; i < a.Shape.shape[0]; i++)
                    res.SetSubtensor(a.GetSubtensor(i), i);

                for (int i = 0; i < b.Shape.shape[0]; i++)
                    res.SetSubtensor(b.GetSubtensor(i), i + a.Shape.shape[0]);

                return res;
            }
        }
    }
}
