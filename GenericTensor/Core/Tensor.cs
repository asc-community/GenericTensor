using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GenericTensor.Core
{
    public class Tensor<TWrapper, TPrimitive> where TWrapper : ITensorElement<TPrimitive>, new()
    {
        private readonly TWrapper[] Data;
        public List<int> Shape { get; }
        private readonly List<int> Blocks = new List<int>(); // 3 x 4 x 5
        private readonly List<int> AxesOrder = new List<int>();
        public int Volume => Data.Length;
        private int LinOffset;

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

        protected Tensor(List<int> dimensions, List<int> blocks, List<int> axesOrder, TWrapper[] data)
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
            Data = data;
            LinOffset = 0;
            BlockRecompute();
        }

        private int GetFlattenedIndex(int[] indecies)
        {
            var res = 0;
            for (int i = 0; i < indecies.Length; i++)
                res += Blocks[AxesOrder[i]] * indecies[i];
            return res + LinOffset;
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
            var newLinIndexDelta = GetFlattenedIndex(new []{currIndex});
            var newBlocks = Blocks.Select(c => c).ToList();
            var rootAxis = AxesOrder[0];
            newBlocks.RemoveAt(rootAxis);
            var newAxesOrder = AxesOrder.Select(c => c).ToList();
            for (int i = 0; i < newAxesOrder.Count; i++)
                if (newAxesOrder[i] > rootAxis)
                    newAxesOrder[i] -= 1;
            newAxesOrder.RemoveAt(0);
            var newShape = new ArraySegment<int>(Shape.ToArray(), 1, Shape.Count - 1).ToList();
            var aff = new Tensor<TWrapper, TPrimitive>(newShape, newBlocks, newAxesOrder, Data);
            aff.LinOffset = LinOffset + newLinIndexDelta;
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
                    if (actualIndex >= Data.Length)
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
                    if (actualIndex >= Data.Length)
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
            {
                var els = new List<string>();
                for (int i = 0; i < Shape[0]; i++)
                    els.Add(this[i].ToString());
                return string.Join(" ", els);
            }
            return "<T>";
        }
    }
}
