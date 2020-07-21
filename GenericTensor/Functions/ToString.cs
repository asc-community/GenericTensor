using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
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
