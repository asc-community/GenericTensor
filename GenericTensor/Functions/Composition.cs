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


namespace GenericTensor.Core
{
    public partial class Tensor<T>
    {
        /// <summary>
        /// Creates a new axis that is put backward
        /// and then sets all elements as children
        /// e. g.
        /// say you have a bunch of tensors {t1, t2, t3} with shape of [2 x 4]
        /// Stack(t1, t2, t3) => T
        /// where T is a tensor of shape of [3 x 2 x 4]
        /// </summary>
        public static Tensor<T> Stack(params Tensor<T>[] elements)
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
            var res = new Tensor<T>(newShape);
            for (int i = 0; i < elements.Length; i++)
                res.SetSubtensor(elements[i], i);
            return res;
        }

        /// <summary>
        /// Borrowed from here: https://github.com/ZacharyPatten/Towel/blob/master/Sources/Towel/Mathematics/Matrix.cs#L365
        /// </summary>
        private static void GetCofactor(Tensor<T> a, Tensor<T> temp, int rowId,
            int colId, int diagLength)
        {
            int i = 0, j = 0;
            for (int row = 0; row < diagLength; row++)
            {
                for (int col = 0; col < diagLength; col++)
                {
                    if (row != rowId && col != colId)
                    {
                        temp[i, j++] = a[row, col];
                        if (j == diagLength - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
        }
    }
}
