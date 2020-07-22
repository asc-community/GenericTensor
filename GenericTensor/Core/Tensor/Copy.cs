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
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        /// <summary>
        /// Copies a tensor calling each cell with a .Copy()
        /// </summary>
        public Tensor<TWrapper, TPrimitive> Copy(bool copyElements)
        {
            var res = new Tensor<TWrapper, TPrimitive>(Shape);
            if (!copyElements)
            {
                foreach (var index in res.IterateOverElements())
                    res.SetCell((TWrapper)this.GetCell(index).Forward(), index);
            }
            else
                foreach (var index in res.IterateOverElements())
                    res.SetCell((TWrapper)this.GetCell(index).Copy(), index);
            return res;
        }

        /// <summary>
        /// Forwards the tensor so TPrimitives are not copied,
        /// but TWrappers are
        /// </summary>
        /// <returns></returns>
        public Tensor<TWrapper, TPrimitive> Forward()
        {
            var res = new Tensor<TWrapper, TPrimitive>(Shape);
            foreach (var index in res.IterateOverElements())
                res.SetCell((TWrapper)this.GetCell(index).Forward(), index);
            return res;
        }

        internal void Assign(Tensor<TWrapper, TPrimitive> tensor)
        {
            foreach (var (index, value) in tensor.Iterate())
                this[index] = value;
        }
    }
}
