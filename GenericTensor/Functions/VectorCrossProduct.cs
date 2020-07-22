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


using System;
using System.Collections.Generic;
using System.Text;
using GenericTensor.Functions;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        /// <summary>
        /// Finds a perpendicular vector to two given
        /// TODO: So far only implemented for 3D vectors
        /// </summary>
        public static Tensor<TWrapper, TPrimitive> VectorCrossProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (!a.IsVector || !b.IsVector)
                throw new InvalidShapeException($"Both {nameof(a)} and {nameof(b)} should be vectors");
            if (a.Shape[0] != b.Shape[0])
                throw new InvalidShapeException($"Length of {nameof(a)} and {nameof(b)} should be equal");
            if (a.Shape[0] != 3)
                throw new NotImplementedException("Other than vectors of the length of 3 aren't supported for VectorCrossProduct yet");
            return Tensor<TWrapper, TPrimitive>.CreateVector(
                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[1], b[2]),
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[2], b[1])),

                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[2], b[0]),
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[0], b[2])),

                ConstantsAndFunctions<TWrapper, TPrimitive>.Subtract(
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[0], b[1]),
                    ConstantsAndFunctions<TWrapper, TPrimitive>.Multiply(a[1], b[0]))
            );
        }

        /// <summary>
        /// Calls VectorCrossProduct for every vector in the tensor
        /// </summary>
        public static Tensor<TWrapper, TPrimitive> TensorVectorCrossProduct(Tensor<TWrapper, TPrimitive> a,
            Tensor<TWrapper, TPrimitive> b)
        {
            if (a.Shape != b.Shape)
                throw new InvalidShapeException($"Pre-shapes of {nameof(a)} and {nameof(b)} should be equal");
            var res = new Tensor<TWrapper, TPrimitive>(a.Shape);
            foreach (var index in a.IterateOverVectors())
                res.SetSubtensor(
                    VectorCrossProduct(a.GetSubtensor(index), b.GetSubtensor(index)),
                    index
                    );
            return res;
        }
    }
}
