using System;
using System.Diagnostics;

namespace GenericTensor.Core
{
    public interface IOperations<T>
    {
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Negate(T a);
        T Divide(T a, T b);
        T CreateOne();
        T CreateZero();
        T Copy(T a);
        bool AreEqual(T a, T b);
        bool IsZero(T a);
        string ToString(T a);
        byte[] Serialize(T a);
        T Deserialize(byte[] data);
    }
}
