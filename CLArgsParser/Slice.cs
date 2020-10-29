using System;
using System.Collections;
using System.Collections.Generic;

namespace CLArgsParser
{
    public static class StringSliceExtension
    {
        public static Slice Slice(this string str, int start, int end) => new Slice(str, start, end);

        public static Slice ToSlice(this string str) => str.Slice(0, str.Length);
    }

    public struct Slice : IEnumerable<char>, IEnumerable, IComparable<Slice>, IComparable, IEquatable<Slice>
    {
        public string Str => _str[Start..End];
        public char this[int i] => _str[i + Start];
        public int Length => End - Start;

        public readonly int Start;
        public readonly int End;
        private readonly string _str;

        public Slice(string str, int start, int end)
        {
            if (start < 0 || end < start)
                throw new IndexOutOfRangeException();
            if (str.Length - start < (end - start))
                throw new IndexOutOfRangeException();

            _str = str;
            Start = start;
            End = end;
        }

        public override string ToString() => Str;

        public int CompareTo(Slice other)
        {
            Slice center;
            Slice outer;

            if (other.Length < Length)
            {
                center = this;
                outer = other;
            }
            else
            {
                center = other;
                outer = this;
            }

            int index = 0;
            foreach (var item in center)
            {
                if (item < outer[index])
                    return -1;
                else if (outer[index] < item)
                    return 1;
                index += 1;
            }
            return 0;
        }

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is Slice))
                throw new InvalidCastException();
            return CompareTo((Slice)obj);
        }

        public static bool operator ==(Slice a, Slice b)
        {
            if (ReferenceEquals(a._str, b._str) &&
                a.Start == b.Start &&
                a.End == b.End)
                return true;
            else if (a.Length != b.Length)
                return false;
            else if (a.CompareTo(b) == 0)
                return true;
            else
                return false;
        }

        public static bool operator ==(Slice a, string b)
        {
            if (a.Length != b.Length)
                return false;
            else if (a.CompareTo(b.Slice(0, b.Length)) == 0)
                return true;
            else
                return false;
        }

        public static bool operator !=(Slice a, Slice b) => !(a == b);

        public static bool operator !=(Slice a, string b) => !(a == b);

        public IEnumerator<char> GetEnumerator() => new SliceEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(Slice other) => this == other;

        public override int GetHashCode() => HashCode.Combine(_str, Start, End);

        public override bool Equals(object obj)
        {
            if (!(obj is Slice))
                return false;
            return this == ((Slice)obj);
        }

        public Slice Trim()
        {
            int start;
            int end;

            for (start = Start; start < End; start++)
            {
                if (_str[start] != ' ')
                    break;
            }

            for (end = End; Start < end; end--)
            {
                if (_str[end - 1] != ' ')
                    break;
            }

            if (start == Start && end == End)
                return this;
            else
                return new Slice(_str, start, end);
        }

        public Slice TrimStart()
        {
            for (int i = Start; i < End; i++)
            {
                if (_str[i] != ' ')
                    return new Slice(_str, i, End);
            }
            return this;
        }

        public Slice TrimEnd()
        {
            for (int i = End - 1; Start <= i; i--)
            {
                if (_str[i] != ' ')
                    return new Slice(_str, Start, i);
            }
            return this;
        }

        public Slice SliceFromSlice(int start, int end) => new Slice(_str, Start + start, Start + end);

        public Slice SubSlice(int startindex) => new Slice(_str, Start + startindex, End);

        public Slice SubSlice(int startindex, int length) => new Slice(_str, Start + startindex, Start + startindex + length);

        public Slice ExpendStart(int AddLength) => new Slice(_str, Start - AddLength, End);

        public Slice ExpendEnd(int AddLength) => new Slice(_str, Start, End + AddLength);

        private struct SliceEnumerator : IEnumerator, IEnumerator<char>
        {
            public char Current => _slice[_position];
            object IEnumerator.Current => Current;

            private Slice _slice;
            private int _position;

            public SliceEnumerator(Slice slice)
            {
                _slice = slice;
                _position = _slice.Start - 1;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                _position += 1;
                if (_slice.End <= _position)
                {
                    Reset();
                    return false;
                }
                else
                    return true;
            }

            public void Reset()
            {
                _position = _slice.Start - 1;
            }
        }
    }
}