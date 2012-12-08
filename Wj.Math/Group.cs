using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class Group<T> where T : IEquatable<T>
    {
        #region Group Functions

        public static Group<T> FromGroup<U>(Group<U> group, Func<U, T> isomorphism) where U : IEquatable<U>
        {
            Dictionary<T, U> inverse = new Dictionary<T, U>();

            foreach (U element in group.Set)
                inverse.Add(isomorphism(element), element);

            return new Group<T>(
                new FiniteSet<T>(group.Set.Select(isomorphism)),
                (t1, t2) => isomorphism(group.Product(inverse[t1], inverse[t2])),
                t => isomorphism(group.Inverse(inverse[t])),
                isomorphism(group.Identity)
                );
        }

        public static FiniteSet<T> LeftProduct(Group<T> group, T t, FiniteSet<T> set)
        {
            return new FiniteSet<T>(set.Select(x => group.Product(t, x)));
        }

        public static FiniteSet<T> RightProduct(Group<T> group, FiniteSet<T> set, T t)
        {
            return new FiniteSet<T>(set.Select(x => group.Product(x, t)));
        }

        public static bool IsSubgroup(Group<T> group, FiniteSet<T> subset)
        {
            if (!subset.IsSubsetOf(group.Set))
                return false;

            foreach (T a in subset)
            {
                foreach (T b in subset)
                {
                    T inverse = group.Inverse(b);

                    if (!subset.IsMember(group.Product(a, inverse)))
                        return false;
                }
            }

            return true;
        }

        public static bool IsSubgroup(Group<T> group, Group<T> subset)
        {
            return IsSubgroup(group, subset.Set);
        }

        public static bool IsNormal(Group<T> group, FiniteSet<T> subset)
        {
            foreach (T g in group.Set)
            {
                T inverse = group.Inverse(g);

                foreach (T n in subset)
                {
                    if (!subset.IsMember(group.Product(group.Product(g, n), inverse)))
                        return false;
                }
            }

            return true;
        }

        public static bool IsNormal(Group<T> group, Group<T> subset)
        {
            return IsNormal(group, subset.Set);
        }

        #endregion

        private FiniteSet<T> _set;
        private Func<T, T, T> _product;
        private T _identity;
        private Func<T, T> _inverse;

        public Group(FiniteSet<T> set, Func<T, T, T> product, Func<T, T> inverse)
        {
            _set = set;
            _product = product;
            _identity = this.FindIdentity();

            if (inverse != null)
                _inverse = inverse;
            else
                _inverse = this.DefaultInverse;
        }

        public Group(FiniteSet<T> set, Func<T, T, T> product, Func<T, T> inverse, T identity)
        {
            _set = set;
            _product = product;
            _identity = identity;

            if (inverse != null)
                _inverse = inverse;
            else
                _inverse = this.DefaultInverse;
        }

        public FiniteSet<T> Set
        {
            get { return _set; }
        }

        public Func<T, T, T> Product
        {
            get { return _product; }
        }

        public T Identity
        {
            get { return _identity; }
        }

        public Func<T, T> Inverse
        {
            get { return _inverse; }
        }

        public int Order
        {
            get { return _set.Size; }
        }

        public bool IsTrivial
        {
            get { return this.Order == 1; }
        }

        private T DefaultInverse(T t)
        {
            foreach (T element in _set)
            {
                if (_product(t, element).Equals(_identity))
                    return element;
            }

            throw new ArgumentException("No inverse found.");
        }

        private T FindIdentity()
        {
            foreach (T candidate in _set)
            {
                bool isIdentity = true;

                foreach (T element in _set)
                {
                    if (!_product(element, candidate).Equals(element))
                    {
                        isIdentity = false;
                        break;
                    }
                }

                if (isIdentity)
                    return candidate;
            }

            throw new ArgumentException("No identity found.");
        }

        #region Basic Functions

        public T Commutator(T x, T y)
        {
            return _product(_inverse(x), _product(_inverse(y), _product(x, y)));
        }

        public FiniteSet<T> ConjugacyClass(T t)
        {
            return new FiniteSet<T>(_set.Select(x => _product(_product(x, t), _inverse(x))));
        }

        public IEnumerable<FiniteSet<T>> ConjugacyClasses()
        {
            return _set.Partition(this.ConjugacyClass);
        }

        /// <summary>
        /// Returns t*x*t^{-1}.
        /// </summary>
        public T Conjugate(T t, T x)
        {
            return _product(t, _product(x, _inverse(t)));
        }

        /// <summary>
        /// Returns t*H*t^{-1}.
        /// </summary>
        public Group<T> Conjugate(T t, Group<T> h)
        {
            return Subgroup(RightProduct(this, LeftProduct(this, t, h.Set), _inverse(t)), false);
        }

        public bool IsAbelian()
        {
            return this.ConjugacyClasses().Count() == this.Order;
        }

        /// <summary>
        /// Returns the order of the element t.
        /// </summary>
        public int OrderOf(T t)
        {
            int order = 1;
            T current = t;

            while (!current.Equals(_identity))
            {
                current = _product(current, t);
                order++;
            }

            return order;
        }

        public T Pow(T t, int n)
        {
            if (n == 0)
                return _identity;

            T b = t;

            if (n < 0)
            {
                b = _inverse(b);
                n = -n;
            }

            T result = b;

            n--;

            while (n != 0)
            {
                if (n % 2 != 0)
                    result = _product(result, b);

                if (n == 1)
                    break;

                n /= 2;
                b = _product(b, b);
            }

            return result;
        }

        /// <summary>
        /// Returns a permutation group on {1,2,...,|G|} isomorphic to this group G.
        /// </summary>
        public Group<Permutation<int>> ToPermutationGroup()
        {
            Func<T, Permutation<int>> isomorphism;

            return this.ToPermutationGroup(out isomorphism);
        }

        /// <summary>
        /// Returns a permutation group on {1,2,...,|G|} isomorphic to this group G.
        /// </summary>
        /// <param name="isomorphism">An isomorphism from G to the permutation group.</param>
        public Group<Permutation<int>> ToPermutationGroup(out Func<T, Permutation<int>> isomorphism)
        {
            Dictionary<T, int> bijection = new Dictionary<T, int>();
            Dictionary<int, T> bijectionInverse = new Dictionary<int, T>();
            int counter = 1;

            foreach (T element in _set)
            {
                bijection.Add(element, counter);
                bijectionInverse.Add(counter, element);
                counter++;
            }

            return this.ToPermutationGroup(t => bijection[t], k => bijectionInverse[k], out isomorphism);
        }

        /// <summary>
        /// Returns a permutation group on {1,2,...,|G|} isomorphic to this group G.
        /// </summary>
        /// <param name="bijection">A bijection from G to {1,2,...,|G|}.</param>
        /// <param name="bijectionInverse">The inverse of <paramref name="bijection"/>.</param>
        /// <param name="isomorphism">An isomorphism from G to the permutation group.</param>
        public Group<Permutation<int>> ToPermutationGroup(Func<T, int> bijection, Func<int, T> bijectionInverse, out Func<T, Permutation<int>> isomorphism)
        {
            if (bijectionInverse == null)
                bijectionInverse = Discrete.Inverse(_set, bijection);

            var set = new FiniteSet<int>(Enumerable.Range(1, _set.Size));

            isomorphism = x => Permutation<int>.FromFunction(set, k => bijection(_product(x, bijectionInverse(k))));

            var permutations = new FiniteSet<Permutation<int>>(_set.Select(isomorphism));

            return new Group<Permutation<int>>(permutations, Permutation<int>.Product, Permutation<int>.Inverse, Permutation<int>.Identity(set));
        }

        #endregion

        #region Subgroups

        public Group<T> Subgroup(FiniteSet<T> subset)
        {
            return Subgroup(subset, true);
        }

        private Group<T> Subgroup(FiniteSet<T> subset, bool test)
        {
            if (test && !IsSubgroup(this, subset))
                return null;

            return new Group<T>(subset, _product, _inverse, _identity);
        }

        /// <summary>
        /// Returns the center Z(G) of the group.
        /// </summary>
        public Group<T> Center()
        {
            return this.Subgroup(new FiniteSet<T>(_set.Where(z => _set.All(g => _product(z, g).Equals(_product(g, z))))), false);
        }

        public Group<T> Centralizer(T t)
        {
            return this.Subgroup(new FiniteSet<T>(_set.Where(g => _product(g, t).Equals(_product(t, g)))), false);
        }

        public Group<T> Centralizer(FiniteSet<T> subset)
        {
            return this.Subgroup(new FiniteSet<T>(_set.Where(g => subset.All(s => _product(g, s).Equals(_product(s, g))))), false);
        }

        public Group<T> Normalizer(FiniteSet<T> subset)
        {
            return this.Subgroup(new FiniteSet<T>(_set.Where(g => LeftProduct(this, g, subset).IsEqualTo(RightProduct(this, subset, g)))), false);
        }

        public Group<T> CommutatorSubgroup()
        {
            HashSet<T> set = new HashSet<T>();

            foreach (T x in _set)
            {
                T inverse = _inverse(x);

                foreach (T y in _set)
                {
                    set.Add(_product(inverse, _product(_inverse(y), _product(x, y))));
                }
            }

            return this.Subgroup(new FiniteSet<T>(set), false);
        }

        public Group<T> GeneratedSubgroup(T t)
        {
            HashSet<T> subset = new HashSet<T>();
            T current = t;

            subset.Add(_identity);

            while (!current.Equals(_identity))
            {
                subset.Add(current);
                current = _product(current, t);
            }

            return this.Subgroup(new FiniteSet<T>(subset), false);
        }

        #endregion

        #region Quotient Groups

        private IEnumerable<Coset<T>> Cosets(Group<T> h, Func<T, FiniteSet<T>> partition)
        {
            var setOfCosets = _set.Partition(partition);
            var cosets = new Dictionary<T, Coset<T>>();
            var list = new List<Coset<T>>();

            foreach (FiniteSet<T> set in setOfCosets)
            {
                var coset = new Coset<T>(this, h, cosets, set, set.First());

                foreach (T element in set)
                    cosets.Add(element, coset);

                list.Add(coset);
            }

            return list;
        }

        public IEnumerable<Coset<T>> LeftCosets(Group<T> h)
        {
            return this.Cosets(h, t => LeftProduct(this, t, h.Set));
        }

        public IEnumerable<Coset<T>> RightCosets(Group<T> h)
        {
            return this.Cosets(h, t => RightProduct(this, h.Set, t));
        }

        /// <summary>
        /// Gets a left coset of H in the group, i.e. tH.
        /// </summary>
        /// <param name="h">A subgroup.</param>
        public Coset<T> LeftCoset(T t, Group<T> h)
        {
            return LeftCosets(h).First().Cosets[t];
        }

        /// <summary>
        /// Gets a right coset of H in the group, i.e. Ht.
        /// </summary>
        /// <param name="h">A subgroup.</param>
        public Coset<T> RightCoset(Group<T> h, T t)
        {
            return RightCosets(h).First().Cosets[t];
        }

        /// <summary>
        /// Returns the quotient group G/N.
        /// </summary>
        /// <param name="n">A normal subgroup N.</param>
        public Group<Coset<T>> Quotient(Group<T> n)
        {
            FiniteSet<Coset<T>> cosets = new FiniteSet<Coset<T>>(this.LeftCosets(n));

            return new Group<Coset<T>>(cosets, Coset<T>.Product, Coset<T>.Inverse, cosets.First().Cosets[_identity]);
        }

        #endregion

        #region Group Actions

        public Group<Permutation<U>> ActionGroup<U>(Func<T, U, U> action, FiniteSet<U> set) where U : IEquatable<U>
        {
            return new Group<Permutation<U>>(
                new FiniteSet<Permutation<U>>(_set.Select(g => Permutation<U>.FromFunction(set, x => action(g, x)))),
                Permutation<U>.Product,
                Permutation<U>.Inverse,
                Permutation<U>.Identity(set)
                );
        }

        public FiniteSet<U> Orbit<U>(Func<T, U, U> action, U x) where U : IEquatable<U>
        {
            return new FiniteSet<U>(_set.Select(g => action(g, x)));
        }

        public IEnumerable<FiniteSet<U>> Orbits<U>(Func<T, U, U> action, FiniteSet<U> set) where U : IEquatable<U>
        {
            return set.Partition(x => Orbit(action, x));
        }

        public Group<T> StabilizerSubgroup<U>(Func<T, U, U> action, U x)
        {
            return this.Subgroup(new FiniteSet<T>(_set.Where(g => action(g, x).Equals(x))), false);
        }

        #endregion

        #region Automorphisms

        public bool IsAutomorphism(Func<T, T> bijection)
        {
            return Group.IsHomomorphism(this, this, bijection);
        }

        public Group<Permutation<T>> AutomorphismGroup()
        {
            // TODO: Optimize.
            return new Group<Permutation<T>>(
                new FiniteSet<Permutation<T>>(Permutation<T>.SetPermutations(_set).Where(permutation => this.IsAutomorphism(permutation.Evaluate))),
                Permutation<T>.Product,
                Permutation<T>.Inverse,
                Permutation<T>.Identity(_set)
                );
        }

        public Group<Permutation<T>> InnerAutomorphismGroup()
        {
            return new Group<Permutation<T>>(
                new FiniteSet<Permutation<T>>(_set.Select(g => Permutation<T>.FromFunction(_set, x => _product(_inverse(g), _product(x, g))))),
                Permutation<T>.Product,
                Permutation<T>.Inverse,
                Permutation<T>.Identity(_set)
                );
        }

        public Group<Coset<Permutation<T>>> OuterAutomorphismGroup()
        {
            return this.AutomorphismGroup().Quotient(this.InnerAutomorphismGroup());
        }

        #endregion
    }

    public class Coset<T> : IEquatable<Coset<T>> where T : IEquatable<T>
    {
        public static Coset<T> Product(Coset<T> s1, Coset<T> s2)
        {
            return s1._cosets[s1.Group.Product(s1.Representative, s2.Representative)];
        }

        public static Coset<T> Inverse(Coset<T> s)
        {
            return s._cosets[s.Group.Inverse(s.Representative)];
        }

        private Group<T> _group;
        private Group<T> _subgroup;
        private Dictionary<T, Coset<T>> _cosets;
        private FiniteSet<T> _members;
        private T _representative;
        private int _hashCode;

        internal Coset(Group<T> group, Group<T> subgroup, Dictionary<T, Coset<T>> cosets, FiniteSet<T> members, T representative)
        {
            _group = group;
            _subgroup = subgroup;
            _cosets = cosets;
            _members = members;
            _representative = representative;
            _hashCode = _members.GetHashCode();
        }

        public Group<T> Group
        {
            get { return _group; }
        }

        public Group<T> Subgroup
        {
            get { return _subgroup; }
        }

        internal Dictionary<T, Coset<T>> Cosets
        {
            get { return _cosets; }
        }

        public FiniteSet<T> Members
        {
            get { return _members; }
        }

        public T Representative
        {
            get { return _representative; }
        }

        public override bool Equals(object obj)
        {
            if (obj is Coset<T>)
                return this.Equals((Coset<T>)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override string ToString()
        {
            return "[" + _representative.ToString() + "H]";
        }

        #region IEquatable<Coset<T>> Members

        public bool Equals(Coset<T> other)
        {
            if (other == null)
                return false;

            if (_cosets == other._cosets)
                return this.Representative.Equals(other.Representative);

            return _members.IsEqualTo(other._members);
        }

        #endregion
    }
}
