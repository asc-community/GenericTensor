using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public struct TensorShape
    {
        private int[] shape;
        public int Length => shape.Length;
        public int DimensionCount => shape.Length;
        public int Count => shape.Length;

        internal TensorShape(int[] shape)
        {
            this.shape = shape;
        }

        internal TensorShape Subshape()
        {
            var newShape = new int[Length - 1];
            for (int i = 0; i < newShape.Length; i++)
                newShape[i] = shape[i + 1];
            return new TensorShape(newShape);
        }

        internal void Swap(int id1, int id2)
            => (shape[id1], shape[id2]) = (shape[id2], shape[id1]);

        internal int this[int axisId] => shape[axisId];

        internal int[] ToArray() => shape;

        public override string ToString()
            => string.Join(" x ", shape.Select(c => c.ToString()));

        public override bool Equals(object obj)
        {
            if (!(obj is TensorShape sh))
                return false;
            if (sh.Length != Length)
                return false;
            for (int i = 0; i < sh.Length; i++)
                if (sh[i] != this[i])
                    return false;
            return true;
        }

        public static bool operator ==(TensorShape a, TensorShape b)
            => a.Equals(b);

        public static bool operator !=(TensorShape a, TensorShape b)
            => !a.Equals(b);
    }
}
