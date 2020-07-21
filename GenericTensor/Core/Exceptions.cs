using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Core
{
    public class InvalidShapeException : ArgumentException
    {
        public InvalidShapeException(string msg) : base(msg) {}
        public InvalidShapeException() : base() {}
    }
}
