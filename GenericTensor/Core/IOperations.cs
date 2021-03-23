using System;
using System.Diagnostics;

namespace GenericTensor.Core
{
    /// <summary>
    /// This the structure responsible for operations on the
    /// elements on your tensor
    /// </summary>
    /// <typeparam name="T">
    /// The type of primitive in your tensor.
    /// </typeparam>
    public interface IOperations<T>
    {
        /// <summary>
        /// Rules of adding elements. Must return a new one.
        /// Should not modify the old ones.
        /// </summary>
        /// <returns>
        /// A primitive of the same type
        /// </returns>
        T Add(T a, T b);

        /// <summary>
        /// Rules of subtracting elements. Must return a new one.
        /// Should not modify the old ones.
        /// </summary>
        /// <returns>
        /// A primitive of the same type
        /// </returns>
        T Subtract(T a, T b);

        /// <summary>
        /// Rules of multiplying elements. Must return a new one.
        /// Should not modify the old ones.
        /// </summary>
        /// <returns>
        /// A primitive of the same type
        /// </returns>
        T Multiply(T a, T b);

        /// <summary>
        /// Rules of multiplying an element by -1. Must return a new one.
        /// Should not modify the old ones.
        /// </summary>
        /// <returns>
        /// A primitive of the same type
        /// </returns>
        T Negate(T a);

        /// <summary>
        /// Rules of dividing elements. Must return a new one.
        /// Should not modify the old ones.
        /// </summary>
        /// <returns>
        /// A primitive of the same type
        /// </returns>
        T Divide(T a, T b);

        /// <returns>
        /// 1 (one). A primitive of the same type
        /// </returns>
        T CreateOne();

        /// <returns>
        /// 0 (zero). A primitive of the same type
        /// </returns>
        T CreateZero();

        /// <returns>
        /// If your elements are mutable, it
        /// might be useful to be able to copy
        /// them as well.
        /// </returns>
        T Copy(T a);

        /// <summary>
        /// Determines whether the instances
        /// of your objects are equal
        /// </summary>
        bool AreEqual(T a, T b);

        /// <summary>
        /// Whether the given instance is zero
        /// </summary>
        bool IsZero(T a);

        /// <summary>
        /// Get the string representation of the instance
        /// </summary>
        string ToString(T a);

        /// <summary>
        /// Rules of serialization of one instance
        /// </summary>
        byte[] Serialize(T a);

        /// <summary>
        /// Rules of deserialization of one instance
        /// </summary>
        T Deserialize(byte[] data);
    }
}
