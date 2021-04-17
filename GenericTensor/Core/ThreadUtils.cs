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

namespace GenericTensor.Core
{
    /// <summary>
    /// Use this enum to set the mode of execution
    /// </summary>
    public enum Threading
    {
        /// <summary>
        /// Will guarantee the single-thread execution
        /// </summary>
        Single,

        /// <summary>
        /// Will unconditionally run in multithreading mode,
        /// using as many cores as possible (in normal priority)
        /// </summary>
        Multi,

        /// <summary>
        /// Will select the necessary mode depending on the input.
        /// Is recommended for cases where the performance is
        /// needed, but you do not want to manage it manually
        /// </summary>
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
