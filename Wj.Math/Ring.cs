﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class Ring<T> where T : IEquatable<T>
    {
        private Group<T> _group;
        private Func<T, T, T> _multiply;

        public Ring(Group<T> group, Func<T, T, T> multiply)
        {
            _group = group;
            _multiply = multiply;
        }

        public Group<T> Group
        {
            get { return _group; }
        }

        public Func<T, T, T> Multiply
        {
            get { return _multiply; }
        }

        /// <summary>
        /// Returns the ideal generated by a single element r.
        /// </summary>
        public Group<T> Ideal(T r)
        {
            return _group.Subgroup(new FiniteSet<T>(_group.Set.Select(x => _multiply(r, x))));
        }

        /// <summary>
        /// Returns the quotient ring R/I.
        /// </summary>
        public Ring<Coset<T>> Quotient(Group<T> i)
        {
            return new Ring<Coset<T>>(_group.Quotient(i), (x, y) => x.Cosets[_multiply(x.Representative, y.Representative)]);
        }
    }
}
