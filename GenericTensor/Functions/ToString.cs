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
    public partial class GenTensor<T>
    {
        public override string ToString()
        {
            if (IsMatrix)
            {
                var maxLength = -1;
                var stringArray = new string[Shape[0], Shape[1]];
                for (int i = 0; i < Shape[0]; i++)
                for (int j = 0; j < Shape[1]; j++)
                {
                    stringArray[i, j] = ConstantsAndFunctions<T>.ToString(GetValueNoCheck(i, j));
                    maxLength = Math.Max(maxLength, stringArray[i, j].Length);
                }
                var rows = new List<string>();
                rows.Add("Matrix[" + Shape + "]");
                for (int i = 0; i < Shape[0]; i++)
                {
                    var s = "";
                    for (int j = 0; j < Shape[1]; j++)
                    {
                        var count = maxLength + 3 - stringArray[i, j].Length;
                        count = Math.Max(0, count);
                        s += stringArray[i, j];
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
                    els.Add(ConstantsAndFunctions<T>.ToString(this.GetValueNoCheck(i)));
                return string.Join(" ", els);
            }
            var sb = new StringBuilder();
            sb.Append("Tensor[" + Shape + "] {\n");
            foreach (var index in IterateOverMatrices())
            {
                sb.Append("    ");
                sb.Append(GetSubtensor(index).ToString().Replace("\n", "\n    "));
                sb.Append("\n\n");
            }
            sb.Append("}");
            return sb.ToString();
        }

        // TODO: make it faster
        public override int GetHashCode()
        {
            var res = 0;
            unchecked
            {
                res += Shape.GetHashCode();
                foreach (var (_, value) in this.Iterate())
                    res += value?.GetHashCode() ?? 0;
            }
            return res;
        }
    }
}
