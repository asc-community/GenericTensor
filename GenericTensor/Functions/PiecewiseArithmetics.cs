using System;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public static Tensor<TWrapper, TPrimitive> Zip(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b, Func<TWrapper, TWrapper, TWrapper> operation)
        {
            if (a.Shape != b.Shape)
                throw new InvalidShapeException("Arguments should be of the same shape");
            var res = new Tensor<TWrapper, TPrimitive>(a.Shape);
            foreach (var index in res.IterateOverElements())
                res.SetCell(operation(a.GetCell(index), b.GetCell(index)), index);
            return res;
        }

        public static Tensor<TWrapper, TPrimitive> PiecewiseAdd(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.AddSaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseSubtract(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.SubtractSaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseMultiply(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.MultiplySaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseDivide(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
            => Zip(a, b, ConstantsAndFunctions<TWrapper, TPrimitive>.DivideSaveWrapper);

        public static Tensor<TWrapper, TPrimitive> PiecewiseAdd(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Add(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseSubtract(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseSubtract(
            TPrimitive a, Tensor<TWrapper, TPrimitive> b)
            => CreateTensor(b.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(a, b[ind]));

        public static Tensor<TWrapper, TPrimitive> PiecewiseMultiply(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseDivide(Tensor<TWrapper, TPrimitive> a,
            TPrimitive b)
            => CreateTensor(a.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Divide(a[ind], b));

        public static Tensor<TWrapper, TPrimitive> PiecewiseDivide(
            TPrimitive a, Tensor<TWrapper, TPrimitive> b)
            => CreateTensor(b.Shape, ind => 
                ConstantsAndFunctions<TWrapper, TPrimitive>.Divide(a, b[ind]));
    }
}
