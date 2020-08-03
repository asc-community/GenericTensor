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


using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static partial class VectorProduct<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        public static GenTensor<T, TWrapper> TensorVectorDotProduct(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b)
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape.SubShape(0, 1) != b.Shape.SubShape(0, 1))
                throw new InvalidShapeException("Other dimensions of tensors should be equal");
            #endif
            var resTensor = new GenTensor<T, TWrapper>(a.Shape.SubShape(0, 1));
            foreach (var index in resTensor.IterateOverElements())
            {
                var scal = VectorDotProduct(a.GetSubtensor(index), b.GetSubtensor(index));
                resTensor.SetValueNoCheck(scal, index);
            }
            return resTensor;
        }

        public static T VectorDotProduct(GenTensor<T, TWrapper> a,
            GenTensor<T, TWrapper> b)
        {
            #if ALLOW_EXCEPTIONS
            if (!a.IsVector || !b.IsVector)
                throw new InvalidShapeException($"{nameof(a)} and {nameof(b)} should be vectors");
            if (a.Shape[0] != b.Shape[0])
                throw new InvalidShapeException($"{nameof(a)}'s length should be the same as {nameof(b)}'s");
            #endif
            var res = default(TWrapper).CreateZero();
            for (int i = 0; i < a.Shape[0]; i++)
            {
                res = default(TWrapper).Add(res,
                    default(TWrapper).Multiply(a.GetValueNoCheck(i), b.GetValueNoCheck(i)));
            }
            return res;
        }
    }
}
