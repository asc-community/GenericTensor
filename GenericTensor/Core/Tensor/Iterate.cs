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
using System.Runtime.CompilerServices;
using System.Text;

namespace GenericTensor.Core
{
    public partial class GenTensor<T>
    {
        private void NextIndex(int[] indecies, int id)
        {
            if (id == -1)
                return;
            indecies[id]++;
            if (indecies[id] == Shape[id])
            {
                indecies[id] = 0;
                NextIndex(indecies, id - 1);
            }
        }

        /// <summary>
        /// Iterate over array of indecies and a value in TPrimitive
        /// </summary>
        public IEnumerable<(int[] index, T value)> Iterate()
        {
            foreach (var ind in IterateOver(0))
                yield return (ind, this.GetValueNoCheck(ind));
        }

        /// <summary>
        /// Element-wise indexing,
        /// for example suppose you have a t = Tensor[2 x 3 x 4] of int-s
        /// A correct way to index it would be
        /// t[0, 0, 1] or t[1, 2, 3],
        /// but neither of t[0, 1] (Use GetSubtensor for this) and t[4, 5, 6] (IndexOutOfRange)
        /// </summary>
        public T this[int x, int y, int z, params int[] indecies]
        {
            get => Data[GetFlattenedIndexWithCheck(x, y, z, indecies)];
            set => Data[GetFlattenedIndexWithCheck(x, y, z, indecies)] = value;
        }

        /// <summary>
        /// Element-wise indexing,
        /// for example suppose you have a t = Tensor[2 x 3 x 4] of int-s
        /// A correct way to index it would be
        /// t[0, 0, 1] or t[1, 2, 3],
        /// but neither of t[0, 1] (Use GetSubtensor for this) and t[4, 5, 6] (IndexOutOfRange)
        /// </summary>
        public T this[int x, int y, int z]
        {
            get => Data[GetFlattenedIndexWithCheck(x, y, z)];
            set => Data[GetFlattenedIndexWithCheck(x, y, z)] = value;
        }

        /// <summary>
        /// Element-wise indexing,
        /// for example suppose you have a t = Tensor[2 x 3 x 4] of int-s
        /// A correct way to index it would be
        /// t[0, 0, 1] or t[1, 2, 3],
        /// but neither of t[0, 1] (Use GetSubtensor for this) and t[4, 5, 6] (IndexOutOfRange)
        /// </summary>
        public T this[int x, int y]
        {
            get => Data[GetFlattenedIndexWithCheck(x, y)];
            set => Data[GetFlattenedIndexWithCheck(x, y)] = value;
        }

        /// <summary>
        /// Element-wise indexing,
        /// for example suppose you have a t = Tensor[2 x 3 x 4] of int-s
        /// A correct way to index it would be
        /// t[0, 0, 1] or t[1, 2, 3],
        /// but neither of t[0, 1] (Use GetSubtensor for this) and t[4, 5, 6] (IndexOutOfRange)
        /// </summary>
        public T this[int x]
        {
            get => Data[GetFlattenedIndexWithCheck(x)];
            set => Data[GetFlattenedIndexWithCheck(x)] = value;
        }

        public T this[int[] inds]
        {
            get => Data[GetFlattenedIndexWithCheck(inds)];
            set => Data[GetFlattenedIndexWithCheck(inds)] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x)
         => Data[GetFlattenedIndexSilent(x)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x, int y)
            => Data[GetFlattenedIndexSilent(x, y)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x, int y, int z)
            => Data[GetFlattenedIndexSilent(x, y, z)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x, int y, int z, int[] indecies)
            => Data[GetFlattenedIndexSilent(x, y, z, indecies)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int[] indecies)
            => Data[GetFlattenedIndexSilent(indecies)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x)
            => Data[GetFlattenedIndexSilent(x)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x, int y)
            => Data[GetFlattenedIndexSilent(x, y)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x, int y, int z)
            => Data[GetFlattenedIndexSilent(x, y, z)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x, int y, int z, int[] other)
            => Data[GetFlattenedIndexSilent(x, y, z, other)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int[] indecies)
            => Data[GetFlattenedIndexSilent(indecies)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x)
            => Data[GetFlattenedIndexSilent(x)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x, int y)
            => Data[GetFlattenedIndexSilent(x, y)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x, int y, int z)
            => Data[GetFlattenedIndexSilent(x, y, z)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x, int y, int z, int[] indecies)
            => Data[GetFlattenedIndexSilent(x, y, z, indecies)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int[] indecies)
            => Data[GetFlattenedIndexSilent(indecies)] = valueCreator();

        /// <summary>
        /// If you need to set your wrapper to the tensor directly, use this function
        /// </summary>
        public void SetCell(T newWrapper, params int[] indecies)
        {
            var actualIndex = GetFlattenedIndexWithCheck(indecies);
            Data[actualIndex] = newWrapper;
        }

        /// <summary>
        /// Get a pointer to the wrapper in your tensor
        /// You can call its methods or set its fields, so that it will be applied to the tensor's element
        /// </summary>
        public T GetCell(params int[] indecies)
        {
            var actualIndex = GetFlattenedIndexWithCheck(indecies);
            return Data[actualIndex];
        }

        /// <summary>
        /// Allows to iterate on lower-dimensions,
        /// so that, for example, in tensor of [2 x 3 x 4]
        /// and offsetFromLeft = 1
        /// while iterating you will get the following arrays:
        /// {0, 0}
        /// {0, 1}
        /// {0, 2}
        /// {1, 0}
        /// {1, 1}
        /// {1, 2}
        /// </summary>
        /// <param name="offsetFromLeft"></param>
        /// <returns></returns>
        public IEnumerable<int[]> IterateOver(int offsetFromLeft)
        {
            int Sum(int[] arr)
            {
                int s = 0;
                for (int i = 0; i < arr.Length; i++)
                    s += arr[i];
                return s;
            }

            var indecies = new int[Shape.Count - offsetFromLeft];
            do
            {
                yield return indecies;
                NextIndex(indecies, indecies.Length - 1);
            } while (Sum(indecies) != 0); // for tensor 4 x 3 x 2 the first violating index would be 5  0  0 
        }

        /// <summary>
        /// IterateOver where yourTensor[index] is always a matrix
        /// </summary>
        public IEnumerable<int[]> IterateOverMatrices()
            => IterateOver(2);

        /// <summary>
        /// IterateOver where yourTensor[index] is always a vector
        /// </summary>
        public IEnumerable<int[]> IterateOverVectors()
            => IterateOver(1);

        /// <summary>
        /// IterateOver where yourTensor[index] is always an element
        /// </summary>
        public IEnumerable<int[]> IterateOverElements()
            => IterateOver(0);

        public void IterateOver1(Action<int> react)
        {
            for (int x = 0; x < Shape[0]; x++)
                react(x);
        }

        public void IterateOver2(Action<int, int> react)
        {
            for (int x = 0; x < Shape[0]; x++)
            for (int y = 0; y < Shape[1]; y++)
                react(x, y);
        }

        public void IterateOver3(Action<int, int, int> react)
        {
            for (int x = 0; x < Shape[0]; x++)
            for (int y = 0; y < Shape[1]; y++)
            for (int z = 0; z < Shape[2]; z++)
                react(x, y, z);
        }
    }
}
