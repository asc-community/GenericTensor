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
            var newLinIndexDelta = GetFlattenedIndexSilent(new []{currIndex});
            var newBlocks = Blocks.Select(c => c).ToList();
            var rootAxis = AxesOrder[0];
            newBlocks.RemoveAt(rootAxis);
            var newAxesOrder = AxesOrder.Select(c => c).ToList();
            for (int i = 0; i < newAxesOrder.Count; i++)
                if (newAxesOrder[i] > rootAxis)
                    newAxesOrder[i] -= 1;
            newAxesOrder.RemoveAt(0);
            var newShape = Shape.CutOffset1();
            var aff = new Tensor<TWrapper, TPrimitive>(newShape, newBlocks, newAxesOrder, Data);
            aff.LinOffset = newLinIndexDelta;
            var sfdgd = aff.ToString();
            if (indecies.Length == 1)
                return aff;
            else
                return aff.GetSubtensor(new ArraySegment<int>(indecies, 1, indecies.Length - 1).ToArray());
        }

        public void SetSubtensor(Tensor<TWrapper, TPrimitive> sub, params int[] indecies)
        {
            if (indecies.Length >= Shape.Count)
                throw new InvalidShapeException($"{nameof(indecies.Length)} should be less than {nameof(Shape.Length)}");
            for (int i = 0; i < indecies.Length; i++)
                if (indecies[i] < 0 || indecies[i] >= Shape[i])
                    throw new IndexOutOfRangeException();
            if (Shape.Count - indecies.Length != sub.Shape.Count)
                throw new InvalidShapeException($"Number of {nameof(sub.Shape.Length)} + {nameof(indecies.Length)} should be equal to {Shape.Count}");
            var thisSub = GetSubtensor(indecies);
            if (thisSub.Shape != sub.Shape)
                throw new InvalidShapeException($"{nameof(sub.Shape)} must be equal to {nameof(Shape)}");
            thisSub.Assign(sub);
        }
    }

}
