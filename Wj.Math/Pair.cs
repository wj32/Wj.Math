using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public struct Pair<T, U> : IEquatable<Pair<T, U>> where T : IEquatable<T> where U : IEquatable<U>
    {
        private T _first;
        private U _second;

        public Pair(T first, U second)
        {
            _first = first;
            _second = second;
        }

        public T First
        {
            get { return _first; }
        }

        public U Second
        {
            get { return _second; }
        }

        public override bool Equals(object obj)
        {
            if (obj is Pair<T, U>)
                return this.Equals((Pair<T, U>)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (_first.GetHashCode() * 3) ^ _second.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + _first.ToString() + ", " + _second.ToString() + ")";
        }

        #region IEquatable<Pair<T,U>> Members

        public bool Equals(Pair<T, U> other)
        {
            return _first.Equals(other._first) && _second.Equals(other._second);
        }

        #endregion
    }

    public struct Pair<T, U, V> : IEquatable<Pair<T, U, V>>
        where T : IEquatable<T>
        where U : IEquatable<U>
        where V : IEquatable<V>
    {
        private T _item1;
        private U _item2;
        private V _item3;

        public Pair(T item1, U item2, V item3)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
        }

        public T Item1
        {
            get { return _item1; }
        }

        public U Item2
        {
            get { return _item2; }
        }

        public V Item3
        {
            get { return _item3; }
        }

        public override bool Equals(object obj)
        {
            if (obj is Pair<T, U, V>)
                return this.Equals((Pair<T, U, V>)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (_item1.GetHashCode() * 7) ^ (_item2.GetHashCode() * 3) ^ _item3.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + _item1.ToString() + ", " + _item2.ToString() + ", " + _item3.ToString() + ")";
        }

        #region IEquatable<Pair<T,U,V>> Members

        public bool Equals(Pair<T, U, V> other)
        {
            return _item1.Equals(other._item1) && _item2.Equals(other._item2) && _item3.Equals(other._item3);
        }

        #endregion
    }
}
