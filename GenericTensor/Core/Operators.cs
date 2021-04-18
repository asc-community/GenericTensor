#region copyright
/*
 * MIT License
 * 
 * Copyright (c) 2020-2021 WhiteBlackGoose
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


namespace GenericTensor.Core
{
    public partial class GenTensor<T, TWrapper> : System.IEquatable<GenTensor<T, TWrapper>>
    {
        /// <summary>
        /// A tensor is a matrix if has two dimensions, e. g. [3 x 4]
        /// </summary>
        public bool IsMatrix => Shape.Count == 2;

        /// <summary>
        /// A tensor is a vector if has one dimension
        /// </summary>
        public bool IsVector => Shape.Count == 1;

        /// <summary>
        /// Determines wether one is a matrix AND its width and height are equal
        /// </summary>
        public bool IsSquareMatrix => IsMatrix && Shape.shape[0] == Shape.shape[1];

        /// <summary>
        /// Calls your default(TWrapper).Equals
        /// Be sure to override it when using this function or ==, != operators
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is null || obj is not GenTensor<T, TWrapper> ten)
                return false;
            return Equals(ten);
        }

        /// <summary>
        /// Calls your default(TWrapper).Equals
        /// Be sure to override it when using this function or ==, != operators
        /// </summary>
        public bool Equals(GenTensor<T, TWrapper> obj)
        {
            if (obj is null)
                return false;
            if (obj.Shape != Shape)
                return false;
            foreach (var (index, _) in obj.Iterate())
                if (!default(TWrapper).AreEqual(this.GetValueNoCheck(index), obj.GetValueNoCheck(index)))
                    return false;
            return true;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool operator ==(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => object.ReferenceEquals(a, b) || (a is {} && a.Equals(b));

        public static bool operator !=(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b)
            => !(a == b);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
