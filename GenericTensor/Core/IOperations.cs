using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GenericTensor.Core
{
    public interface IOperations<T>
    {
        T Add(T a, T b) => throw AskForDefiningException();
        T Subtract(T a, T b) => throw AskForDefiningException();
        T Multiply(T a, T b) => throw AskForDefiningException();
        T Negate(T a) => throw AskForDefiningException();
        T Divide(T a, T b) => throw AskForDefiningException();
        T CreateOne() => throw AskForDefiningException();
        T CreateZero() => throw AskForDefiningException();
        T Copy(T a) => throw AskForDefiningException();
        bool AreEqual(T a, T b) => throw AskForDefiningException();
        bool IsZero(T a) => throw AskForDefiningException();
        string ToString(T a) => throw AskForDefiningException();
        byte[] Serialize(T a) => throw AskForDefiningException();
        T Deserialize(byte[] data) => throw AskForDefiningException();

        private static Exception AskForDefiningException()
        {
            var st = new StackTrace(1, false);
            var curr = st.GetFrame(0);
            return new NotImplementedException(curr.GetMethod().Name + " should be implemented");
        }
    }
}
