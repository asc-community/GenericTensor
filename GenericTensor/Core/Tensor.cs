using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public class Tensor<TWrapper, TPrimitive> where TWrapper : ITensorElement<TPrimitive>, new()
    {
        private readonly DataArray<TWrapper> Data;
        public List<int> Shape { get; }
        private readonly List<int> Blocks = new List<int>(); // 3 x 4 x 5
        private readonly List<int> AxesOrder = new List<int>();
        public int Volume => Data.Count;

        private void BlockRecompute()
        {
            int len = 1;
            Blocks.Clear();
            for (int i = Shape.Count - 1; i >= 0; i--)
            {
                Blocks.Add(len);
                len *= Shape[i];
            }
            Blocks.Reverse();
        }

        protected Tensor(List<int> dimensions, List<int> blocks, List<int> axesOrder, DataArray<TWrapper> data)
        {
            Shape = dimensions;
            AxesOrder = axesOrder;
            Blocks = blocks;
            Data = data;
        }

        public Tensor(params int[] dimensions)
        {
            Shape = dimensions.ToList();
            int len = 1;
            for (int i = 0; i < dimensions.Length; i++)
            {
                len *= dimensions[i];
                AxesOrder.Add(i);
            }
            var data = new TWrapper[len];
            for (int i = 0; i < len; i++)
                data[i] = new TWrapper();
            Data = new DataArray<TWrapper>(data);
            BlockRecompute();
        }

        private int GetFlattenedIndex(int[] indecies)
        {
            var res = 0;
            for (int i = 0; i < indecies.Length; i++)
                res += Blocks[AxesOrder[i]] * indecies[i];
            return res;
        }

        public void Transpose(int axis1, int axis2)
        {
            (AxesOrder[axis1], AxesOrder[axis2]) = (AxesOrder[axis2], AxesOrder[axis1]);
            (Shape[axis1], Shape[axis2]) = (Shape[axis2], Shape[axis1]);
        }

        public Tensor<TWrapper, TPrimitive> GetSubtensor(params int[] indecies)
        {
            if (indecies.Length == 0)
                return this;
            var currIndex = indecies[0];
            (int begin, int step, int end) inds;

            inds.begin = GetFlattenedIndex(new int[] {currIndex}); // first element of the reduced tensor
            var newIds = new List<int>{currIndex};
            for (int i = 1; i < Shape.Count; i++)
                newIds[i - 1] = Shape[i] - 1;
            inds.end = GetFlattenedIndex(newIds.ToArray()) + 1; // last element of the reduced tensor
            inds.step = (inds.begin - inds.end) / (Volume / Shape[0]);

            var newDa = new DataArray<TWrapper>(Data, inds.begin, inds.step, inds.end);

            // BEFORE
            // Axes:   1   0   2
            // Blocks: 20  5   1
            // Shape:  6   4   5

            // AFTER
            // Axes:   0   2
            // Blocks: 20  1
            // Shape:  6   5
            // AxesOrder[0] points to the first axis

            var newShape = new ArraySegment<int>(Shape.ToArray(), 1, Shape.Count - 1);
            
            var newBlocks = Blocks.Select(c => c).ToList();
            var newAxesOrder = AxesOrder.Select(c => c).ToList();
            var badAxis = AxesOrder.IndexOf(0);
            for (int i = 0; i < badAxis; i++)
                newBlocks[i] /= newBlocks[badAxis];
            newBlocks.RemoveAt(badAxis);
            newAxesOrder.RemoveAt(badAxis);
            newAxesOrder = newAxesOrder.Select(c => c - 1).ToList();
            
            
            //var newAxesOrder = ;
            // 0, 2, 1
            // 2, 1

            var aff = new Tensor<TWrapper, TPrimitive>(newShape.ToList(), newBlocks, newAxesOrder, newDa);
            if (indecies.Length == 1)
                return aff;
            else
                return aff.GetSubtensor(new ArraySegment<int>(indecies, 1, indecies.Length - 1).ToArray());
        }

        // Axes matches = {1, 0, 2}
        // {0,  0,  0}, 0,  0,  0
        // {0}, 0, {0}, 0, {0}, 0
        public TPrimitive this[params int[] indecies]
        {
            get
            {
                if (indecies.Length == Shape.Count)
                {
                    var actualIndex = GetFlattenedIndex(indecies);
                    if (actualIndex >= Data.Count)
                        throw new IndexOutOfRangeException();
                    return Data[actualIndex].GetValue();
                }
                if (indecies.Length > Shape.Count)
                    throw new IndexOutOfRangeException();
                throw new NotImplementedException();
            }
            set
            {
                if (indecies.Length == Shape.Count)
                {
                    var actualIndex = GetFlattenedIndex(indecies);
                    if (actualIndex >= Data.Count)
                        throw new IndexOutOfRangeException();
                    Data[actualIndex].SetValue(value);
                    return;
                }
                if (indecies.Length > Shape.Count)
                    throw new IndexOutOfRangeException();
                throw new NotImplementedException();
            }
        }

        public bool IsMatrix => Shape.Count == 2;
        public bool IsVector => Shape.Count == 1;

        public override string ToString()
        {
            if (IsMatrix)
            {
                var rows = new List<string>();
                for (int i = 0; i < Shape[0]; i++)
                {
                    var s = "";
                    for (int j = 0; j < Shape[1]; j++)
                    {
                        var count = 8 - this[i, j].ToString().Length;
                        count = Math.Max(0, count);
                        s += this[i, j].ToString();
                        for (int k = 0; k < count; k++)
                            s += " ";
                    }
                    rows.Add(s);
                }
                return string.Join("\n", rows);
            }
            if (IsVector)
                return string.Join(" ", Data.Select(c => c.ToString()));
            return "<T>";
        }
    }
}
