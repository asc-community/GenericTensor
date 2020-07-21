using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public Tensor<TWrapper, TPrimitive> Copy(bool copyElements)
        {
            var res = new Tensor<TWrapper, TPrimitive>(Shape);
            if (!copyElements)
                res.Data = Data.Clone() as TWrapper[];
            else
                res.Data = Data.Select(c => (TWrapper) c.Copy()).ToArray(); 
            return res;
        }

        internal void Assign(Tensor<TWrapper, TPrimitive> tensor)
        {
            foreach (var (index, value) in tensor.Iterate())
                this[index] = value;
        }
    }
}
