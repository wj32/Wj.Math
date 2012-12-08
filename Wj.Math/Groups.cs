using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public static class Group
    {
        #region Standard Groups

        /// <summary>
        /// Returns the symmetric group on {1,2,...,n} of order n!.
        /// </summary>
        public static Group<Permutation<int>> SymmetricGroup(int n)
        {
            FiniteSet<int> set = new FiniteSet<int>(Enumerable.Range(1, n));

            return SymmetricGroup(set);
        }

        /// <summary>
        /// Returns the symmetric group Sym(S) of order |S|!.
        /// </summary>
        public static Group<Permutation<T>> SymmetricGroup<T>(FiniteSet<T> set) where T : IEquatable<T>
        {
            FiniteSet<Permutation<T>> permutations = new FiniteSet<Permutation<T>>(Permutation<T>.SetPermutations(set));

            return new Group<Permutation<T>>(permutations, Permutation<T>.Product, Permutation<T>.Inverse, Permutation<T>.Identity(set));
        }

        /// <summary>
        /// Returns the alternating group on {1,2,...,n} of order n!/2.
        /// </summary>
        public static Group<Permutation<int>> AlternatingGroup(int n)
        {
            FiniteSet<int> set = new FiniteSet<int>(Enumerable.Range(1, n));

            return AlternatingGroup(set);
        }

        /// <summary>
        /// Returns the alternating group Alt(S) of order |S|!/2.
        /// </summary>
        public static Group<Permutation<T>> AlternatingGroup<T>(FiniteSet<T> set) where T : IEquatable<T>
        {
            FiniteSet<Permutation<T>> permutations = new FiniteSet<Permutation<T>>(Permutation<T>.SetPermutations(set).Where(a => a.Sign == 1));

            return new Group<Permutation<T>>(permutations, Permutation<T>.Product, Permutation<T>.Inverse, Permutation<T>.Identity(set));
        }

        /// <summary>
        /// Returns the cyclic group Z_n of order n.
        /// </summary>
        public static Group<int> CyclicGroup(int n)
        {
            return new Group<int>(new FiniteSet<int>(Enumerable.Range(0, n)), (a, b) => (a + b) % n, a => (n - a) % n, 0);
        }

        /// <summary>
        /// Returns the cyclic group on the vertices {1,2,...,n} of an n-gon.
        /// </summary>
        public static Group<Permutation<int>> CyclicGroupP(int n)
        {
            FiniteSet<int> set = new FiniteSet<int>(Enumerable.Range(1, n));
            HashSet<Permutation<int>> permutations = new HashSet<Permutation<int>>();
            Permutation<int> generator = Permutation<int>.FromCycle(set, new Cycle<int>(Enumerable.Range(1, n)));
            Permutation<int> current = Permutation<int>.Identity(set);

            for (int i = 0; i < n; i++)
            {
                permutations.Add(current);
                current = Permutation<int>.Product(current, generator);
            }

            return new Group<Permutation<int>>(new FiniteSet<Permutation<int>>(permutations), Permutation<int>.Product, Permutation<int>.Inverse, Permutation<int>.Identity(set));
        }

        /// <summary>
        /// Returns the Z_2 group with boolean type and False as the identity.
        /// </summary>
        public static Group<bool> Z2GroupB()
        {
            return new Group<bool>(new FiniteSet<bool>(false, true), (a, b) => a ^ b, a => a, false);
        }

        /// <summary>
        /// Returns the dihedral group D_n of order 2n.
        /// </summary>
        /// <remarks>
        /// Each element of the group is a tuple (r,f), where r is the rotation component
        /// and f is the reflection component. If f is True, the element is a reflection.
        /// </remarks>
        public static Group<Pair<int, bool>> DihedralGroup(int n)
        {
            Group<int> cyclic = CyclicGroup(n);

            return SemidirectProduct(cyclic, Z2GroupB(), (f, r) => f ? cyclic.Inverse(r) : r);
        }

        private static Permutation<int> NGonReflection(FiniteSet<int> set)
        {
            List<Cycle<int>> cycles = new List<Cycle<int>>();
            int n = set.Size;
            int k = 1;

            if (n % 2 != 0)
            {
                cycles.Add(new Cycle<int>(k));
                k++;
                n++;
            }

            while (k <= n / 2)
            {
                cycles.Add(new Cycle<int>(new int[] { k, n - k + 1 }));
                k++;
            }

            return Permutation<int>.FromDisjointCycles(set, cycles);
        }

        /// <summary>
        /// Returns the dihedral group on the vertices {1,2,...,n} of an n-gon.
        /// </summary>
        public static Group<Permutation<int>> DihedralGroupP(int n)
        {
            FiniteSet<int> set = new FiniteSet<int>(Enumerable.Range(1, n));
            HashSet<Permutation<int>> permutations = new HashSet<Permutation<int>>();
            Permutation<int> generator = Permutation<int>.FromCycle(set, new Cycle<int>(Enumerable.Range(1, n)));
            Permutation<int> current = Permutation<int>.Identity(set);
            Permutation<int> reflection = NGonReflection(set);

            for (int i = 0; i < n; i++)
            {
                permutations.Add(current);
                permutations.Add(Permutation<int>.Product(current, reflection));
                current = Permutation<int>.Product(current, generator);
            }

            return new Group<Permutation<int>>(new FiniteSet<Permutation<int>>(permutations), Permutation<int>.Product, Permutation<int>.Inverse, Permutation<int>.Identity(set));
        }

        #endregion

        #region Homomorphisms

        public static bool IsHomomorphism<T, U>(Group<T> group1, Group<U> group2, Func<T, U> map)
            where T : IEquatable<T>
            where U : IEquatable<U>
        {
            return group1.Set.All(x => group1.Set.All(y => map(group1.Product(x, y)).Equals(group2.Product(map(x), map(y)))));
        }

        public static Group<T> Kernel<T, U>(Group<T> group1, Group<U> group2, Func<T, U> homomorphism)
            where T : IEquatable<T>
            where U : IEquatable<U>
        {
            return group1.Subgroup(new FiniteSet<T>(group1.Set.Where(x => homomorphism(x).Equals(group2.Identity))));
        }

        #endregion

        #region Group Products

        /// <summary>
        /// Returns the direct product G_1 x G_2.
        /// </summary>
        public static Group<Pair<T, U>> DirectProduct<T, U>(Group<T> group1, Group<U> group2)
            where T : IEquatable<T>
            where U : IEquatable<U>
        {
            return new Group<Pair<T, U>>(
                FiniteSet.CartesianProduct(group1.Set, group2.Set),
                (g1, g2) => new Pair<T, U>(group1.Product(g1.First, g2.First), group2.Product(g1.Second, g2.Second)),
                g => new Pair<T, U>(group1.Inverse(g.First), group2.Inverse(g.Second)),
                new Pair<T, U>(group1.Identity, group2.Identity)
                );
        }

        /// <summary>
        /// Returns the semidirect product N x_f H.
        /// </summary>
        public static Group<Pair<T, U>> SemidirectProduct<T, U>(Group<T> n, Group<U> h, Func<U, T, T> f)
            where T : IEquatable<T>
            where U : IEquatable<U>
        {
            return new Group<Pair<T, U>>(
                FiniteSet.CartesianProduct(n.Set, h.Set),
                (g1, g2) => new Pair<T, U>(n.Product(g1.First, f(g1.Second, g2.First)), h.Product(g1.Second, g2.Second)),
                g => { U inverse = h.Inverse(g.Second); return new Pair<T, U>(f(inverse, n.Inverse(g.First)), inverse); },
                new Pair<T, U>(n.Identity, h.Identity)
                );
        }

        #endregion

        #region Cycle Index

        private static FiniteSet<Pair<int, int>> PermutationStructure<T>(Permutation<T> permutation) where T : IEquatable<T>
        {
            Dictionary<int, int> structure = new Dictionary<int, int>();

            foreach (Cycle<T> cycle in permutation)
            {
                if (!structure.ContainsKey(cycle.Length))
                    structure.Add(cycle.Length, 0);

                structure[cycle.Length]++;
            }

            return new FiniteSet<Pair<int, int>>(structure.Select(pair => new Pair<int, int>(pair.Key, pair.Value)));
        }

        public static GroupCycleIndex CycleIndex<T>(Group<Permutation<T>> permutationGroup) where T : IEquatable<T>
        {
            var structure = new Dictionary<FiniteSet<Pair<int, int>>, int>();

            foreach (var element in permutationGroup.Set)
            {
                var elementStructure = PermutationStructure(element);

                if (!structure.ContainsKey(elementStructure))
                    structure.Add(elementStructure, 0);

                structure[elementStructure]++;
            }

            var cycleIndex = new GroupCycleIndex();

            cycleIndex.Order = permutationGroup.Order;
            cycleIndex.Terms = new List<GroupCycleIndex.IndexTerm>();

            foreach (var pair in structure)
            {
                cycleIndex.Terms.Add(new GroupCycleIndex.IndexTerm { Coefficient = pair.Value, Factors = pair.Key });
            }

            return cycleIndex;
        }

        #endregion
    }

    public struct GroupCycleIndex
    {
        public struct IndexTerm
        {
            public int Coefficient;
            public FiniteSet<Pair<int, int>> Factors;
        }

        public int Order;
        public List<IndexTerm> Terms;

        public Polynomial<Rational, RationalField> Evaluate(Func<int, Polynomial<Rational, RationalField>> x)
        {
            Polynomial<Rational, RationalField> polynomial = Polynomial<Rational, RationalField>.Zero;

            foreach (var term in this.Terms)
            {
                Polynomial<Rational, RationalField> component = (Rational)term.Coefficient;

                foreach (var factor in term.Factors)
                {
                    component *= x(factor.First).Pow(factor.Second);
                }

                polynomial += component;
            }

            return polynomial / this.Order;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('(');

            foreach (var term in this.Terms)
            {
                if (term.Coefficient > 1)
                {
                    sb.Append(term.Coefficient);
                    sb.Append('*');
                }

                foreach (var factor in term.Factors)
                {
                    sb.Append("x[");
                    sb.Append(factor.First);
                    sb.Append("]");

                    if (factor.Second > 1)
                    {
                        sb.Append('^');
                        sb.Append(factor.Second);
                    }

                    sb.Append('*');
                }

                if (sb[sb.Length - 1] == '*')
                    sb.Remove(sb.Length - 1, 1);

                sb.Append('+');
            }

            if (sb[sb.Length - 1] == '+')
                sb.Remove(sb.Length - 1, 1);

            sb.Append(")/");
            sb.Append(this.Order.ToString());

            return sb.ToString();
        }
    }
}
