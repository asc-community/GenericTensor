namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> Stack(params Tensor<TWrapper, TPrimitive>[] elements)
        {
            if (elements.Length < 1)
                throw new InvalidShapeException("Shoud be at least one element to stack");
            var desiredShape = elements[0].Shape;
            for (int i = 1; i < elements.Length; i++)
                if (elements[i].Shape != desiredShape)
                    throw new InvalidShapeException($"Tensors in {nameof(elements)} should be of the same shape");
            var newShape = new int[desiredShape.Count + 1];
            newShape[0] = elements.Length;
            for (int i = 1; i < newShape.Length; i++)
                newShape[i] = desiredShape[i - 1];
            var res = new Tensor<TWrapper, TPrimitive>(newShape);
            for (int i = 0; i < elements.Length; i++)
                res.SetSubtensor(elements[i], i);
            return res;
        }

        /// <summary>
        /// Borrowed from here: https://github.com/ZacharyPatten/Towel/blob/master/Sources/Towel/Mathematics/Matrix.cs#L365
        /// </summary>
        private static void GetCofactor(Tensor<TWrapper, TPrimitive> a, Tensor<TWrapper, TPrimitive> temp, int rowId,
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
