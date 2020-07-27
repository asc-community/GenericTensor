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

namespace GenericTensor.Core
{
    public partial class GenTensor<T>
    {
        private void NextIndex(int[] indices, int id)
        {
            if (id == -1)
                return;
            indices[id]++;
            if (indices[id] == Shape[id])
            {
                indices[id] = 0;
                NextIndex(indices, id - 1);
            }
        }

        /// <summary>
        /// Iterate over array of indices and a value in TPrimitive
        /// </summary>
        public IEnumerable<(int[] Index, T Value)> Iterate()
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
        public T this[int x, int y, int z, params int[] indices]
        {
            get => _data[GetFlattenedIndexWithCheck(x, y, z, indices)];
            set => _data[GetFlattenedIndexWithCheck(x, y, z, indices)] = value;
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
            get => _data[GetFlattenedIndexWithCheck(x, y, z)];
            set => _data[GetFlattenedIndexWithCheck(x, y, z)] = value;
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
            get => _data[GetFlattenedIndexWithCheck(x, y)];
            set => _data[GetFlattenedIndexWithCheck(x, y)] = value;
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
            get => _data[GetFlattenedIndexWithCheck(x)];
            set => _data[GetFlattenedIndexWithCheck(x)] = value;
        }

        public T this[int[] inds]
        {
            get => _data[GetFlattenedIndexWithCheck(inds)];
            set => _data[GetFlattenedIndexWithCheck(inds)] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x)
         => _data[GetFlattenedIndexSilent(x)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x, int y)
            => _data[GetFlattenedIndexSilent(x, y)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x, int y, int z)
            => _data[GetFlattenedIndexSilent(x, y, z)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int x, int y, int z, int[] indices)
            => _data[GetFlattenedIndexSilent(x, y, z, indices)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValueNoCheck(int[] indices)
            => _data[GetFlattenedIndexSilent(indices)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x)
            => _data[GetFlattenedIndexSilent(x)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x, int y)
            => _data[GetFlattenedIndexSilent(x, y)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x, int y, int z)
            => _data[GetFlattenedIndexSilent(x, y, z)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int x, int y, int z, int[] other)
            => _data[GetFlattenedIndexSilent(x, y, z, other)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(T value, int[] indices)
            => _data[GetFlattenedIndexSilent(indices)] = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x)
            => _data[GetFlattenedIndexSilent(x)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x, int y)
            => _data[GetFlattenedIndexSilent(x, y)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x, int y, int z)
            => _data[GetFlattenedIndexSilent(x, y, z)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int x, int y, int z, int[] indices)
            => _data[GetFlattenedIndexSilent(x, y, z, indices)] = valueCreator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueNoCheck(Func<T> valueCreator, int[] indices)
            => _data[GetFlattenedIndexSilent(indices)] = valueCreator();

        /// <summary>
        /// If you need to set your wrapper to the tensor directly, use this function
        /// </summary>
        public void SetCell(T newWrapper, params int[] indices)
        {
            var actualIndex = GetFlattenedIndexWithCheck(indices);
            _data[actualIndex] = newWrapper;
        }

        /// <summary>
        /// Get a pointer to the wrapper in your tensor
        /// You can call its methods or set its fields, so that it will be applied to the tensor's element
        /// </summary>
        public T GetCell(params int[] indices)
        {
            var actualIndex = GetFlattenedIndexWithCheck(indices);
            return _data[actualIndex];
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
            static int Sum(int[] arr)
            {
                int s = 0;
                for (int i = 0; i < arr.Length; i++)
                    s += arr[i];
                return s;
            }

            var indices = new int[Shape.Count - offsetFromLeft];
            do
            {
                yield return indices;
                NextIndex(indices, indices.Length - 1);
            } while (Sum(indices) != 0); // for tensor 4 x 3 x 2 the first violating index would be 5  0  0 
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
