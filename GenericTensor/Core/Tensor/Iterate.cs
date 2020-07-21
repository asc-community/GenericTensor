using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        private void NextIndex(int[] indecies, int id)
        {
            indecies[id]++;
            if (indecies[id] == Shape[id])
            {
                indecies[id] = 0;
                NextIndex(indecies, id - 1);
            }
        }

        public IEnumerable<(int[] index, TPrimitive value)> Iterate()
        {
            var indecies = new int[Shape.Count];
            while (indecies[0] != Shape[0]) // for tensor 4 x 3 x 2 the first violating index would be 5  0  0 
            {
                yield return (indecies, this[indecies]);
                NextIndex(indecies, indecies.Length - 1);
            }
        }
    }
}
