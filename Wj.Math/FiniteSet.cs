using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class FiniteSet<T> : IEnumerable<T>, IEquatable<FiniteSet<T>> where T : IEquatable<T>
    {
        public static bool operator ==(FiniteSet<T> s1, FiniteSet<T> s2)
        {
            if (object.ReferenceEquals(s1, s2))
                return true;
            if ((object)s1 == null)
                return false;
            if ((object)s2 == null || s1.GetHashCode() != s2.GetHashCode())
                return false;

            return s1.Equals(s2);
        }

        public static bool operator !=(FiniteSet<T> s1, FiniteSet<T> s2)
        {
            if (object.ReferenceEquals(s1, s2))
                return false;
            if ((object)s1 == null)
                return true;
            if ((object)s2 == null || s1.GetHashCode() != s2.GetHashCode())
                return true;

            return !s1.Equals(s2);
        }

        private HashSet<T> _set;
        private int _hashCode;

        internal FiniteSet(HashSet<T> set)
        {
            _set = set;
            this.ComputeHashCode();
        }

        public FiniteSet(IEnumerable<T> elements)
        {
            _set = new HashSet<T>(elements);
            this.ComputeHashCode();
        }

        public FiniteSet(params T[] elements)
        {
            _set = new HashSet<T>(elements);
            this.ComputeHashCode();
        }

        private void ComputeHashCode()
        {
            _hashCode = 0;

            foreach (T element in _set)
                _hashCode ^= element.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is FiniteSet<T>)
                return this.Equals((FiniteSet<T>)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('{');

            foreach (T element in _set.Take(50))
            {
                sb.Append(element.ToString());
                sb.Append(", ");
            }

            if (_set.Count <= 50)
            {
                if (sb[sb.Length - 1] == ' ')
                    sb.Remove(sb.Length - 2, 2);
            }
            else
            {
                sb.Append("...");
            }

            sb.Append('}');

            return sb.ToString();
        }

        public int Size
        {
            get { return _set.Count; }
        }

        public bool IsMember(T t)
        {
            return _set.Contains(t);
        }

        public bool IsSubsetOf(FiniteSet<T> superset)
        {
            if (superset is FiniteSet<T>)
            {
                return _set.IsSubsetOf(((FiniteSet<T>)superset)._set);
            }
            else
            {
                foreach (T element in _set)
                {
                    if (!superset.IsMember(element))
                        return false;
                }

                return true;
            }
        }

        public bool IsEqualTo(FiniteSet<T> other)
        {
            if (other is FiniteSet<T>)
            {
                return _set.SetEquals(((FiniteSet<T>)other)._set);
            }
            else
            {
                return this.IsSubsetOf(other) && other.IsSubsetOf(this);
            }
        }

        public IEnumerable<FiniteSet<T>> Partition(Func<T, T, bool> equivalence)
        {
            List<FiniteSet<T>> partitions = new List<FiniteSet<T>>();

            foreach (T element in _set)
            {
                bool inPartition = false;

                for (int i = 0; i < partitions.Count; i++)
                {
                    T elementInPartition = partitions[i]._set.First();

                    if (equivalence(element, elementInPartition))
                    {
                        partitions[i]._set.Add(element);
                        inPartition = true;
                        break;
                    }
                }

                if (!inPartition)
                {
                    partitions.Add(new FiniteSet<T>(element));
                }
            }

            return partitions;
        }

        public IEnumerable<FiniteSet<T>> Partition(Func<T, FiniteSet<T>> equivalenceClass)
        {
            HashSet<T> remaining = new HashSet<T>(_set);

            while (remaining.Count != 0)
            {
                FiniteSet<T> c = equivalenceClass(remaining.First());

                foreach (T element in c)
                    remaining.Remove(element);

                yield return c;
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEquatable<FiniteSet<T>> Members

        public bool Equals(FiniteSet<T> other)
        {
            if (other == null)
                return false;

            return this.IsEqualTo(other);
        }

        #endregion
    }
}
