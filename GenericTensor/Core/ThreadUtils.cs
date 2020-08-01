using System;

namespace GenericTensor.Core
{
    public enum Threading
    {
        Single,
        Multi,
        Auto
    }

    internal static class ThreadUtils
    {
        internal static T GetOrDefault<T>(ref T field, Func<T> Default) where T : new()
        {
            if (field is null)
                field = Default();
            return field;
        }
    }
}
