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
                    var bytesInt = new Span<byte>(bytes, currId, 4);
                    var res = BitConverter.ToInt32(bytesInt);
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
                    var bytesn = new Span<byte>(bytes, currId, numberOfBytes);
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
            var realBytes = new Span<byte>(data, 0, sizeof(double));
            var imagBytes = new Span<byte>(data, sizeof(double), sizeof(double));
            return new Complex(
                BitConverter.ToDouble(realBytes),
                BitConverter.ToDouble(imagBytes)
            );
        }
    }
}
