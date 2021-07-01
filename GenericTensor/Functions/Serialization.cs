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


using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using GenericTensor.Core;

namespace GenericTensor.Functions
{
    internal static class Serializer<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
        public static byte[] Serialize(GenTensor<T, TWrapper> tensor)
        {
            var bb = new SerializationUtils.ByteBuilder();
            bb.AddInt(tensor.Shape.Length);
            foreach (var sh in tensor.Shape.shape)
                bb.AddInt(sh);
            foreach (var (_, value) in tensor.Iterate())
            {
                var ser = default(TWrapper).Serialize(value);
                bb.AddInt(ser.Length);
                bb.AddBytes(ser);
            }
            return bb.ToArray();
        }

        public static GenTensor<T, TWrapper> Deserialize(byte[] data)
        {
            var bp = new SerializationUtils.ByteParser(data);
            var dimCount = bp.PopInt();
            var dimensions = new int[dimCount];
            for (int i = 0; i < dimCount; i++)
                dimensions[i] = bp.PopInt();
            var res = new GenTensor<T, TWrapper>(dimensions);
            foreach (var index in res.IterateOver(0))
            {
                var serializedCellLength = bp.PopInt();
                var serData = bp.PopBytes(serializedCellLength);
                var unser = default(TWrapper).Deserialize(serData);
                res.SetValueNoCheck(unser, index);
            }
            return res;
        }
    }

    // Span<byte> is the only thing used from
    // System.Memory, so why not to replace
    // it with my own ByteSpan, since I use one
    // single method of Span<byte>, which makes it
    // not worth making an external dependency.
    internal struct ByteSpan
    {
        private readonly byte[] arr;
        private readonly int start;
        private readonly int length;
        public ByteSpan(byte[] arr, int start, int length)
            => (this.arr, this.start, this.length) = (arr, start, length);
        public byte[] ToArray()
        {
            var res = new byte[length];
            Array.Copy(arr, start, res, 0, length);
            return res;
        }
    }

    internal static class SerializationUtils
    {
        // TODO: replace with an existing solution
        internal class ByteBuilder
        {
            private readonly List<byte> bytes = new List<byte>();

            public void AddInt(int val)
                => bytes.AddRange(BitConverter.GetBytes(val));

            public void AddBytes(byte[] data)
                => bytes.AddRange(data);

            public byte[] ToArray()
                => bytes.ToArray();
        }


        // TODO: replace with an existing solution
        internal class ByteParser
        {
            private readonly byte[] bytes;
            private int currId = 0;

            public ByteParser(byte[] data)
                => bytes = data;

            public int PopInt()
            {
                try
                {
                    var bytesInt = new ByteSpan(bytes, currId, 4);
                    var res = BitConverter.ToInt32(bytesInt.ToArray(), 0);
                    currId += sizeof(int);
                    return res;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new InvalidDataException("End of array reached", e);
                }
            }

            public byte[] PopBytes(int numberOfBytes)
            {
                try
                {
                    var bytesn = new ByteSpan(bytes, currId, numberOfBytes);
                    currId += numberOfBytes;
                    return bytesn.ToArray();
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new InvalidDataException("End of array reached", e);
                }
            }
        }

        // TODO: There surely is a faster method than that
        public static byte[] SerializeComplex(Complex c)
        {
            var list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(c.Real));
            list.AddRange(BitConverter.GetBytes(c.Imaginary));
            return list.ToArray();
        }

        // TODO: There surely is a faster method than that
        public static Complex DeserializeComplex(byte[] data)
        {
            var realBytes = new ByteSpan(data, 0, sizeof(double));
            var imagBytes = new ByteSpan(data, sizeof(double), sizeof(double));
            return new Complex(
                BitConverter.ToDouble(realBytes.ToArray(), 0),
                BitConverter.ToDouble(imagBytes.ToArray(), 0)
            );
        }
    }
}
