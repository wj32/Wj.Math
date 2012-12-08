using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class Cycle<T> :
        IEquatable<Cycle<T>>,
        IEnumerable<T>
        where T : IEquatable<T>
    {
        public static bool operator ==(Cycle<T> c1, Cycle<T> c2)
        {
            if (object.ReferenceEquals(c1, c2))
                return true;
            if ((object)c1 == null)
                return false;

            return c1.Equals(c2);
        }

        public static bool operator !=(Cycle<T> c1, Cycle<T> c2)
        {
            if (object.ReferenceEquals(c1, c2))
                return false;
            if ((object)c1 == null)
                return true;

            return !c1.Equals(c2);
        }

        public static Cycle<T> Inverse(Cycle<T> cycle)
        {
            T[] elements = new T[cycle.Length];

            if (elements.Length > 0)
            {
                elements[0] = cycle._elements[0];

                for (int i = 1; i < elements.Length; i++)
                    elements[i] = cycle._elements[elements.Length - i];
            }

            return new Cycle<T>(elements, false);
        }

        private T[] _elements;

        private Cycle(T[] elements, bool copy)
        {
            if (copy)
            {
                _elements = new T[elements.Length];
                elements.CopyTo(_elements, 0);
            }
            else
            {
                _elements = elements;
            }
        }

        public Cycle(T element)
        {
            _elements = new T[] { element };
        }

        public Cycle(T[] elements)
            : this(elements, true)
        { }

        public Cycle(IEnumerable<T> elements)
        {
            _elements = elements.ToArray();
        }

        public int Length
        {
            get { return _elements.Length; }
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0)
                {
                    return _elements[index % _elements.Length];
                }
                else
                {
                    int newIndex = (index % _elements.Length) + _elements.Length;

                    if (newIndex == _elements.Length)
                        return _elements[0];
                    else
                        return _elements[newIndex];
                }
            }
        }

        public int Sign
        {
            get { return (_elements.Length % 2) * 2 - 1; }
        }

        public bool IsDisjointWith(Cycle<T> other)
        {
            return _elements.Intersect(other).Count() != 0;
        }

        public bool Equals(Cycle<T> other)
        {
            if (other == null)
                return false;
            if (this.Length != other.Length)
                return false;

            T reference = this[0];
            int i;

            for (i = 0; i < other.Length; i++)
            {
                if (other[i].Equals(reference))
                    break;
            }

            if (i == other.Length)
                return false;

            i++;

            for (int j = 1; j < this.Length; j++)
            {
                if (!other[i].Equals(this[j]))
                    return false;

                i++;
            }

            return true;
        }

        public override bool Equals(object other)
        {
            if (other is Cycle<T>)
                return Equals((Cycle<T>)other);
            else
                return false;
        }

        public override int GetHashCode()
        {
            int code = this.Length;

            for (int i = 0; i < _elements.Length; i++)
                code += _elements[i].GetHashCode();

            return code;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('(');

            for (int i = 0; i < _elements.Length; i++)
            {
                string text = _elements[i].ToString();

                if (text.Contains(' '))
                {
                    sb.Append('"');
                    sb.Append(text);
                    sb.Append('"');
                }
                else
                {
                    sb.Append(text);
                }

                sb.Append(' ');
            }

            if (sb[sb.Length - 1] == ' ')
                sb.Remove(sb.Length - 1, 1);

            sb.Append(')');

            return sb.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_elements).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }

    public class Permutation<T> :
        IEquatable<Permutation<T>>,
        IEnumerable<Cycle<T>>
        where T : IEquatable<T>
    {
        public static bool operator ==(Permutation<T> p1, Permutation<T> p2)
        {
            if (object.ReferenceEquals(p1, p2))
                return true;
            if ((object)p1 == null)
                return false;

            return p1.Equals(p2);
        }

        public static bool operator !=(Permutation<T> p1, Permutation<T> p2)
        {
            if (object.ReferenceEquals(p1, p2))
                return false;
            if ((object)p1 == null)
                return true;

            return !p1.Equals(p2);
        }

        public static Permutation<T> FromCycle(FiniteSet<T> set, Cycle<T> cycle)
        {
            return new Permutation<T>(set, new Cycle<T>[] { cycle });
        }

        public static Permutation<T> FromDisjointCycles(FiniteSet<T> set, IEnumerable<Cycle<T>> cycles)
        {
            return new Permutation<T>(set, cycles);
        }

        public static Permutation<T> FromFunction(FiniteSet<T> set, Func<T, T> bijection)
        {
            HashSet<T> seen = new HashSet<T>();
            List<Cycle<T>> cycles = new List<Cycle<T>>();
            List<T> currentCycle = new List<T>();

            foreach (T element in set)
            {
                if (!seen.Contains(element))
                {
                    T x = element;
                    T y;

                    currentCycle.Add(x);

                    while (true)
                    {
                        y = bijection(x);

                        if (currentCycle.Count != 0 && currentCycle[0].Equals(y))
                            break;

                        currentCycle.Add(y);
                        x = y;
                    }

                    seen.UnionWith(currentCycle);
                    cycles.Add(new Cycle<T>(currentCycle));
                    currentCycle.Clear();
                }
            }

            return new Permutation<T>(set, cycles);
        }

        public static Permutation<T> FromMappings(FiniteSet<T> set, IDictionary<T, T> mappings)
        {
            return FromFunction(set, x => mappings[x]);
        }

        public static Permutation<T> FromPermutation<U>(FiniteSet<T> set, Permutation<U> permutation, Func<U, T> bijection) where U : IEquatable<U>
        {
            return new Permutation<T>(set, permutation.Select(cycle => new Cycle<T>(cycle.Select(u => bijection(u)))));
        }

        private static IEnumerable<Cycle<T>> OneCycleGenerator(FiniteSet<T> set)
        {
            foreach (T element in set)
                yield return new Cycle<T>(element);
        }

        public static Permutation<T> Identity(FiniteSet<T> set)
        {
            return FromDisjointCycles(set, OneCycleGenerator(set));
        }

        public static Permutation<T> Product(Permutation<T> p1, Permutation<T> p2)
        {
            Dictionary<T, T> map1 = p1.GetMappings();
            Dictionary<T, T> map2 = p2.GetMappings();

            return FromFunction(p1.Set, x => map1[map2[x]]);
        }

        public static Permutation<T> Inverse(Permutation<T> p)
        {
            return new Permutation<T>(p.Set, p.Select(cycle => Cycle<T>.Inverse(cycle)));
        }

        public static IEnumerable<Permutation<T>> SetPermutations(FiniteSet<T> set)
        {
            int n = set.Size;
            Dictionary<int, T> map = new Dictionary<int, T>();
            Dictionary<T, int> inverseMap = new Dictionary<T, int>();
            int[] permutation = new int[n];
            int index = 0;

            foreach (T element in set)
            {
                map.Add(index, element);
                inverseMap.Add(element, index);
                permutation[index] = index;
                index++;
            }

            yield return Identity(set);

            // Generate permutations in lexicographic order.
            while (true)
            {
                int k;
                bool foundK = false;
                int l;
                int temp;

                for (k = n - 2; k >= 0; k--)
                {
                    if (permutation[k] < permutation[k + 1])
                    {
                        foundK = true;
                        break;
                    }
                }

                if (!foundK)
                    break;

                for (l = n - 1; l >= 0; l--)
                {
                    if (permutation[k] < permutation[l])
                        break;
                }

                temp = permutation[k];
                permutation[k] = permutation[l];
                permutation[l] = temp;

                for (int i = 0; i < (n - (k + 1)) / 2; i++)
                {
                    temp = permutation[k + 1 + i];
                    permutation[k + 1 + i] = permutation[n - i - 1];
                    permutation[n - i - 1] = temp;
                }

                yield return Permutation<T>.FromFunction(set, x => map[permutation[inverseMap[x]]]);
            }
        }

        private FiniteSet<T> _set;
        private HashSet<Cycle<T>> _cycles;
        private int _sign;

        private Permutation(FiniteSet<T> set, IEnumerable<Cycle<T>> cycles)
        {
            _set = set;
            _cycles = new HashSet<Cycle<T>>();

            HashSet<T> seen = new HashSet<T>();
            int totalLength = 0;
            int oddCycles = 0;

            foreach (Cycle<T> cycle in cycles)
            {
                totalLength += cycle.Length;
                _cycles.Add(cycle);
                seen.UnionWith(cycle);

                if (cycle.Sign == -1)
                    oddCycles++;
            }

            _sign = ((oddCycles + 1) % 2) * 2 - 1;

            foreach (T element in set)
            {
                if (!seen.Contains(element))
                {
                    totalLength++;
                    _cycles.Add(new Cycle<T>(element));
                }
            }

            if (totalLength != set.Size)
                throw new ArgumentException("Cycles are not disjoint.");
        }

        public FiniteSet<T> Set
        {
            get { return _set; }
        }

        public int Count
        {
            get { return _cycles.Count; }
        }

        public bool IsIdentity
        {
            get { return _cycles.Count == _set.Size; }
        }

        public int Sign
        {
            get { return _sign; }
        }

        public T this[T t]
        {
            get { return this.Evaluate(t); }
        }

        public T Evaluate(T t)
        {
            foreach (Cycle<T> cycle in _cycles)
            {
                for (int i = 0; i < cycle.Length; i++)
                {
                    if (cycle[i].Equals(t))
                        return cycle[i + 1];
                }
            }

            throw new ArgumentException();
        }

        public Dictionary<T, T> GetMappings()
        {
            Dictionary<T, T> mappings = new Dictionary<T, T>();

            foreach (Cycle<T> cycle in _cycles)
            {
                for (int i = 0; i < cycle.Length; i++)
                    mappings.Add(cycle[i], cycle[i + 1]);
            }

            return mappings;
        }

        public bool IsInvolution()
        {
            foreach (Cycle<T> cycle in _cycles)
            {
                if (cycle.Length > 2)
                    return false;
            }

            return true;
        }

        public bool Equals(Permutation<T> other)
        {
            if (other == null)
                return false;

            return _cycles.SetEquals(other._cycles);
        }

        public override bool Equals(object other)
        {
            if (other is Permutation<T>)
                return this.Equals((Permutation<T>)other);
            else
                return false;
        }

        public override int GetHashCode()
        {
            int code = _cycles.Count;

            foreach (Cycle<T> cycle in _cycles)
                code ^= cycle.GetHashCode();

            return code;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Cycle<T> cycle in _cycles)
                sb.Append(cycle.ToString());

            return sb.ToString();
        }

        public IEnumerator<Cycle<T>> GetEnumerator()
        {
            return ((IEnumerable<Cycle<T>>)_cycles).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _cycles.GetEnumerator();
        }
    }

    public static class Permutation
    {
        public static Permutation<int> FromString(string s)
        {
            return FromString(null, s);
        }

        public static Permutation<int> FromString(int n, string s)
        {
            return FromString(new FiniteSet<int>(Enumerable.Range(1, n)), s);
        }

        public static Permutation<int> FromString(FiniteSet<int> set, string s)
        {
            HashSet<int> usedSet = null;

            if (set == null)
                usedSet = new HashSet<int>();

            Permutation<int> permutation = null;
            List<int> currentCycle = new List<int>();
            StringBuilder currentInteger = new StringBuilder();
            int bracket = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ')')
                {
                    if (bracket == 0)
                        throw new ArgumentException("Mismatched ')' bracket.");

                    bracket--;

                    if (currentInteger.Length != 0)
                    {
                        currentCycle.Add(int.Parse(currentInteger.ToString()));
                        currentInteger.Clear();
                    }

                    if (currentCycle.Count == 0)
                        throw new ArgumentException("Empty cycle encountered.");

                    Cycle<int> cycle = new Cycle<int>(currentCycle);

                    if (usedSet != null)
                        usedSet.UnionWith(currentCycle);

                    if (set != null)
                    {
                        if (permutation == null)
                            permutation = Permutation<int>.FromCycle(set, cycle);
                        else
                            permutation = Permutation<int>.Product(permutation, Permutation<int>.FromCycle(set, cycle));
                    }
                    else
                    {
                        if (permutation == null)
                        {
                            permutation = Permutation<int>.FromCycle(new FiniteSet<int>((IEnumerable<int>)usedSet), cycle);
                        }
                        else
                        {
                            var newSet = new FiniteSet<int>((IEnumerable<int>)usedSet);

                            if (permutation.Set.Size < usedSet.Count)
                                permutation = Permutation<int>.FromFunction(newSet, x => permutation.Set.IsMember(x) ? permutation[x] : x);

                            permutation = Permutation<int>.Product(permutation, Permutation<int>.FromCycle(newSet, cycle));
                        }
                    }

                    currentCycle.Clear();
                }
                else if (s[i] == '-' || char.IsNumber(s, i))
                {
                    if (bracket == 0)
                        throw new ArgumentException("Invalid character at index " + i.ToString() + ".");

                    currentInteger.Append(s[i]);
                }
                else if (char.IsWhiteSpace(s, i))
                {
                    if (currentInteger.Length != 0)
                    {
                        currentCycle.Add(int.Parse(currentInteger.ToString()));
                        currentInteger.Clear();
                    }
                }
                else if (s[i] == '(')
                {
                    if (bracket != 0)
                        throw new ArgumentException("Too many '(' brackets.");

                    bracket++;
                }
                else
                {
                    throw new ArgumentException("Invalid character at index " + i.ToString() + ".");
                }
            }

            if (bracket != 0)
                throw new ArgumentException("Missing ')'.");

            return permutation;
        }
    }
}
