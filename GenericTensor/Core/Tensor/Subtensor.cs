using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        private int LinOffset;
        public Tensor<TWrapper, TPrimitive> GetSubtensor(params int[] indecies)
        {
            if (indecies.Length == 0)
                return this;
            var currIndex = indecies[0];
            var newLinIndexDelta = GetFlattenedIndexWithCheck(new []{currIndex});
            var newBlocks = Blocks.Select(c => c).ToList();
            var rootAxis = AxesOrder[0];
            newBlocks.RemoveAt(rootAxis);
            var newAxesOrder = AxesOrder.Select(c => c).ToList();
            for (int i = 0; i < newAxesOrder.Count; i++)
                if (newAxesOrder[i] > rootAxis)
                    newAxesOrder[i] -= 1;
            newAxesOrder.RemoveAt(0);
            var newShape = new TensorShape(new ArraySegment<int>(Shape.ToArray(), 1, Shape.Count - 1).ToArray());
            var aff = new Tensor<TWrapper, TPrimitive>(newShape, newBlocks, newAxesOrder, Data);
            aff.LinOffset = LinOffset + newLinIndexDelta;
            if (indecies.Length == 1)
                return aff;
            else
                return aff.GetSubtensor(new ArraySegment<int>(indecies, 1, indecies.Length - 1).ToArray());
        }
    }

}
