using System;
using System.Collections;
using System.Collections.Generic;

namespace CLArgsParser
{
    public static class StringSliceExtension
    {
        public static Slice Slice(this string str, int start, int end) => new Slice(str, start, end);

        public static Slice ToSlice(this string str) => (Slice)str;
    }

    public readonly struct Slice : IEnumerable<char>, IEnumerable, IComparable<Slice>, IComparable, IEquatable<Slice>
    {
        public string Str => _str[Start..End];
        public char this[int i] => _str[i + Start];
        public int Length => End - Start;
        public int Start { get; init; }
        public int End { get; init; }

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

            if (Length < other.Length)
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
            if (Length < other.Length)
                return -1;
            if (other.Length < Length)
                return 1;
            else
                return 0;
        }

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is Slice))
                throw new InvalidCastException();
            return CompareTo((Slice)obj);
        }

        public static bool operator ==(in Slice a, in Slice b)
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

        public static bool operator ==(in Slice a, string b)
        {
            if (a.Length != b.Length)
                return false;
            else if (a.CompareTo((Slice)b) == 0)
                return true;
            else
                return false;
        }

        public static bool operator !=(in Slice a, in Slice b) => !(a == b);

        public static bool operator !=(in Slice a, string b) => !(a == b);

        public static implicit operator string(in Slice slice) => slice.Str;

        public static explicit operator Slice(string str) => new Slice(str, 0, str.Length);

        public IEnumerator<char> GetEnumerator()
        {
            for (int position = Start; position < End; position++)
                yield return _str[position];
        }

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
                if (_str[start] != ' ' && _str[start] != '\r' && _str[start] != '\n')
                    break;

            for (end = End - 1; start < end; end--)
                if (_str[end] != ' ' && _str[end] != '\r' && _str[end] != '\n')
                    break;

            return new(_str, start, end + 1);
        }

        public Slice TrimStart()
        {
            for (int i = Start; i < End; i++)
                if (_str[i] != ' ' && _str[i] != '\r' && _str[i] != '\n')
                    return new(_str, i, End);
            return this;
        }

        public Slice TrimEnd()
        {
            for (int i = End - 1; Start <= i; i--)
                if (_str[i] != ' ' && _str[i] != '\r' && _str[i] != '\n')
                    return new(_str, Start, i + 1);
            return this;
        }

        public Slice SliceFromSlice(int start, int end) => new(_str, Start + start, Start + end);

        public Slice SubSlice(int startindex) => new(_str, Start + startindex, End);

        public Slice SubSlice(int startindex, int length) => new(_str, Start + startindex, Start + startindex + length);

        public Slice ExpendStart(int AddLength) => new(_str, Start - AddLength, End);

        public Slice ExpendEnd(int AddLength) => new(_str, Start, End + AddLength);
    }
}