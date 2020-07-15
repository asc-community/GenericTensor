using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GenericTensor.Core
{
    public class DataArray<T> : IEnumerable<T>
    {
        private readonly T[] Data;
        private readonly int Begin;
        private readonly int End;
        private readonly int Step;

        public int Count => (End - Begin) / Step + ((End - Begin) % Step == 0 ? 0 : 1);

        public bool IsReadOnly => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ActualIndex(int index) => index * Step + Begin;
        public T this[int index]
        {
            get
            {
                int actualIndex = ActualIndex(index);
                if (actualIndex >= End)
                    throw new IndexOutOfRangeException();
                return Data[actualIndex];
            }
            set
            {
                int actualIndex = ActualIndex(index);
                if (actualIndex >= End)
                    throw new IndexOutOfRangeException();
                Data[actualIndex] = value;
            }
        }

        public DataArray(params T[] data)
        {
            this.Data = data;
            Begin = 0;
            End = Data.Length;
            Step = 1;
        }

        public DataArray<T> Slice(int begin, int end, int step)
            => new DataArray<T>(this, begin, end, step);

        public DataArray(DataArray<T> dataArray, int begin, int end, int step)
        {
            this.Data = dataArray.Data;
            Begin = dataArray.Begin + begin * dataArray.Step;
            Step = step * dataArray.Step;
            End = dataArray.Begin + end * dataArray.Step;
        }

        public override string ToString()
            => "{ " + string.Join(" ", this) + " }";

        public class DataArrayEnumerator<T> : IEnumerator<T>
        {
            public T Current => that[currId];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                currId += 1;
                return currId < that.Count;
            }

            public void Reset()
            {
                currId = 0 - 1;
            }

            private int currId;
            private DataArray<T> that;
            public DataArrayEnumerator(DataArray<T> that)
            {
                currId = 0 - 1;
                this.that = that;
            }
        }

        public IEnumerator<T> GetEnumerator()
            => new DataArrayEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (!(obj is DataArray<T> da))
                return false;
            if (Count != da.Count)
                return false;
            for (int i = 0; i < Count; i++)
                if (!this[i].Equals(da[i]))
                    return false;
            return true;
        }

        public static bool operator ==(DataArray<T> a, DataArray<T> b)
            => a.Equals(b);

        public static bool operator !=(DataArray<T> a, DataArray<T> b)
            => !a.Equals(b);
    }
}
