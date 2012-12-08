using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public struct UnorderedPair<T> : IEquatable<UnorderedPair<T>>
        where T : IEquatable<T>, IComparable<T>
    {
        private T _first;
        private T _second;

        public UnorderedPair(T first, T second)
        {
            if (first.CompareTo(second) < 0)
            {
                _first = first;
                _second = second;
            }
            else
            {
                _first = second;
                _second = first;
            }
        }

        public T First
        {
            get { return _first; }
            set { _first = value; }
        }

        public T Second
        {
            get { return _second; }
            set { _second = value; }
        }

        public bool Equals(UnorderedPair<T> pair)
        {
            return _first.Equals(pair._first) && _second.Equals(pair._second);
        }

        public override int GetHashCode()
        {
            return (_first.GetHashCode() * 3) ^ _second.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is UnorderedPair<T>)
                return this.Equals((UnorderedPair<T>)obj);
            else
                return false;
        }

        public override string ToString()
        {
            return "(" + _first.ToString() + ", " + _second.ToString() + ")";
        }
    }
}
