using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
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

        public IEnumerable<(int[] index, TPrimitive value)> Iterate(int offsetFromLeft = 0)
        {
            foreach (var ind in IterateOver(0))
                yield return (ind, this[ind]);
        }

        internal TWrapper GetFlattenedWrapper(params int[] indecies)
        {
            var actualIndex = GetFlattenedIndexWithCheck(indecies);
            return Data[actualIndex];
        }

        internal TWrapper GetWrapperSilent(params int[] indecies)
        {
            var actualIndex = GetFlattenedIndexSilent(indecies);
            return Data[actualIndex];
        }

        public TPrimitive this[params int[] indecies]
        {
            get => GetFlattenedWrapper(indecies).GetValue();
            set => GetFlattenedWrapper(indecies).SetValue(value);
        }

        public void SetCell(TWrapper newWrapper, params int[] indecies)
        {
            var actualIndex = GetFlattenedIndexWithCheck(indecies);
            Data[actualIndex] = newWrapper;
        }

        public TWrapper GetCell(params int[] indecies)
        {
            var actualIndex = GetFlattenedIndexWithCheck(indecies);
            return Data[actualIndex];
        }

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

        public IEnumerable<int[]> IterateOverMatrices()
            => IterateOver(2);

        public IEnumerable<int[]> IterateOverVectors()
            => IterateOver(1);

        public IEnumerable<int[]> IterateOverElements()
            => IterateOver(0);
    }
}
