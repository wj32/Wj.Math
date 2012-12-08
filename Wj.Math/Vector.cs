using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class Vector<T> : IEquatable<Vector<T>>, IEnumerable<T>
    {
        private T[] _bits;

        public Vector()
        {
            _bits = new T[0];
        }

        public Vector(int length)
        {
            _bits = new T[length];
        }

        public Vector(T[] bits)
        {
            _bits = bits;
        }

        public Vector(List<T> bits, bool reverse)
        {
            if (!reverse)
            {
                _bits = bits.ToArray();
            }
            else
            {
                _bits = new T[bits.Count];

                for (int i = 0; i < bits.Count; i++)
                {
                    _bits[i] = bits[bits.Count - i - 1];
                }
            }
        }

        public int Length
        {
            get { return _bits.Length; }
        }

        public T[] Bits
        {
            get { return _bits; }
        }

        public T this[int index]
        {
            get { return _bits[index]; }
            set { _bits[index] = value; }
        }

        public bool Equals(Vector<T> other)
        {
            if (_bits.Length != other._bits.Length)
                return false;

            for (int i = 0; i < _bits.Length; i++)
            {
                if (!_bits[i].Equals(other._bits[i]))
                    return false;
            }

            return true;
        }

        public void Prepend(T bit)
        {
            Array.Resize(ref _bits, _bits.Length + 1);

            for (int i = _bits.Length - 2; i >= 0; i--)
                _bits[i + 1] = _bits[i];

            _bits[0] = bit;
        }

        public void Append(T bit)
        {
            Array.Resize(ref _bits, _bits.Length + 1);
            _bits[_bits.Length - 1] = bit;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _bits.Length; i++)
                sb.Append(_bits[i]);

            return sb.ToString();
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_bits).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
