using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Wj.Math
{
    /// <summary>
    /// Represents a formal power series f with rational coefficients a[n] for n=0,1,2,....
    /// </summary>
    public class Series
    {
        public const int InfinityOrder = int.MaxValue;
        public const int InfinityDegree = int.MaxValue;
        public const int MinusInfinityDegree = int.MinValue;

        public const int DefaultDisplayTerms = 10;

        public static readonly Series One = new Series((n, s) => n == 0 ? Rational.One : Rational.Zero, order: 0, degree: 0);
        public static readonly Series Zero = new Series((n, s) => Rational.Zero, order: InfinityOrder, degree: MinusInfinityDegree);
        public static readonly Series Z = new Series((n, s) => n == 1 ? Rational.One : Rational.Zero, order: 1, degree: 1);

        #region Internal Functions

        /// <summary>
        /// Adds two values.
        /// </summary>
        /// <param name="x">An integer that may also be +infinity or -infinity.</param>
        /// <param name="y">An integer that may also be +infinity or -infinity.</param>
        /// <returns>An integer that may also be +infinity or -infinity.</returns>
        private static int ExtendedAdd(int x, int y)
        {
            if (x == int.MinValue || y == int.MinValue)
                return int.MinValue;
            else if (x == int.MaxValue || y == int.MaxValue)
                return int.MaxValue;
            else
                return x + y;
        }

        /// <summary>
        /// Subtracts two values.
        /// </summary>
        /// <param name="x">An integer that may also be +infinity or -infinity.</param>
        /// <param name="y">An integer that may not be +infinity or -infinity.</param>
        /// <returns>An integer that may also be +infinity or -infinity.</returns>
        private static int ExtendedSubtract(int x, int y)
        {
            if (x == int.MaxValue)
                return int.MaxValue;
            else if (x == int.MinValue)
                return int.MinValue;
            else
                return x - y;
        }

        /// <summary>
        /// Multiplies two values.
        /// </summary>
        /// <param name="x">An integer that may also be +infinity or -infinity.</param>
        /// <param name="y">An integer that may not be +infinity or -infinity.</param>
        /// <returns>An integer that may also be +infinity or -infinity.</returns>
        private static int ExtendedMultiply(int x, int y)
        {
            if (x == int.MinValue && y == int.MinValue)
                return int.MaxValue;
            else if (x == int.MinValue || y == int.MinValue)
                return int.MinValue;
            else if (x == int.MaxValue || y == int.MaxValue)
                return int.MaxValue;
            else
                return x * y;
        }

        private static BigInteger Multinomial(int[] n)
        {
            BigInteger sum = 0;

            for (int i = 0; i < n.Length; i++)
                sum += n[i];

            sum = Discrete.Factorial(sum);

            for (int i = 0; i < n.Length; i++)
                sum /= Discrete.Factorial(n[i]);

            return sum;
        }

        #endregion

        #region Basic Operators

        // Nothing
        public static Series operator +(Series f)
        {
            return f;
        }

        public static Series Negate(Series f)
        {
            return new Series((n, s) => -f[n], order: f.Order, degree: f.Degree);
        }
        public static Series operator -(Series f)
        {
            return Negate(f);
        }

        public static Series Add(Series f, Series g)
        {
            return new Series((n, s) => f[n] + g[n], order: System.Math.Min(f.Order, g.Order), degree: System.Math.Max(f.Degree, g.Degree));
        }
        public static Series operator +(Series f, Series g)
        {
            return Add(f, g);
        }

        public static Series Subtract(Series f, Series g)
        {
            return new Series((n, s) => f[n] - g[n], order: System.Math.Min(f.Order, g.Order), degree: System.Math.Max(f.Degree, g.Degree));
        }
        public static Series operator -(Series f, Series g)
        {
            return Subtract(f, g);
        }

        public static Series Multiply(Series f, Series g)
        {
            return new Series((n, s) =>
            {
                int low = 0;
                int high = n;
                Rational c = Rational.Zero;

                if (low < f.Order)
                    low = f.Order;
                if (high > f.Degree)
                    high = f.Degree;

                if (g.Order == InfinityOrder)
                    high = -1;
                else if (high > n - g.Order)
                    high = n - g.Order;

                if (g.Degree == MinusInfinityDegree)
                    low = InfinityDegree;
                else if (g.Degree != InfinityDegree && low < n - g.Degree)
                    low = n - g.Degree;

                for (int i = low; i <= high; i++)
                {
                    c += f[i] * g[n - i];
                }

                return c;
            }, order: ExtendedAdd(f.Order, g.Order), degree: ExtendedAdd(f.Degree, g.Degree), cache: true);
        }
        public static Series operator *(Series f, Series g)
        {
            return Multiply(f, g);
        }

        public static Series Divide(Series f, Series g)
        {
            return Multiply(f, g.Inv());
        }
        public static Series operator /(Series f, Series g)
        {
            return Divide(f, g);
        }

        #endregion

        #region Convenience Operators

        public static Series Add(Series f, Rational k)
        {
            return new Series((n, s) =>
            {
                if (n == 0)
                    return f[0] + k;
                else
                    return f[n];
            }, order: 0, degree: System.Math.Max(f.Degree, 0));
        }
        public static Series operator +(Series f, Rational k)
        {
            return Add(f, k);
        }
        public static Series operator +(Rational k, Series f)
        {
            return Add(f, k);
        }

        public static Series Subtract(Series f, Rational k)
        {
            return new Series((n, s) =>
            {
                if (n == 0)
                    return f[0] - k;
                else
                    return f[n];
            }, order: 0, degree: System.Math.Max(f.Degree, 0));
        }
        public static Series operator -(Series f, Rational k)
        {
            return Subtract(f, k);
        }
        public static Series Subtract(Rational k, Series f)
        {
            return new Series((n, s) =>
            {
                if (n == 0)
                    return k - f[0];
                else
                    return -f[n];
            }, order: 0, degree: System.Math.Max(f.Degree, 0));
        }
        public static Series operator -(Rational k, Series f)
        {
            return Subtract(k, f);
        }

        public static Series Multiply(Series f, Rational k)
        {
            if (k == Rational.Zero)
                return Zero;
            else
                return new Series((n, s) => f[n] * k, order: f.Order, degree: f.Degree);
        }
        public static Series operator *(Series f, Rational k)
        {
            return Multiply(f, k);
        }
        public static Series operator *(Rational k, Series f)
        {
            return Multiply(f, k);
        }

        public static Series Divide(Series f, Rational k)
        {
            return new Series((n, s) => f[n] / k, order: f.Order, degree: f.Degree);
        }
        public static Series operator /(Series f, Rational k)
        {
            return Divide(f, k);
        }

        #endregion

        #region Cast Operators

        public static implicit operator Series(Rational k)
        {
            if (k == Rational.Zero)
                return Zero;

            return new Series((n, s) => n == 0 ? k : Rational.Zero, order: 0, degree: 0);
        }

        #endregion

        #region Functions

        /// <summary>
        /// Generates the series f(g), where g has nonzero order.
        /// </summary>
        /// <param name="f">A series.</param>
        /// <param name="g">A series with nonzero order.</param>
        /// <returns>The series f(g).</returns>
        public static Series Compose(Series f, Series g)
        {
            return new Series((n, s) =>
            {
                if (n == 0)
                    return f[0];

                int high = n;
                Rational c = Rational.Zero;

                if (high > f.Degree)
                    high = f.Degree;

                for (int i = 1; i <= high; i++)
                {
                    c += f[i] * PowCoefficient(g, i, n, true);
                }

                return c;
            }, ExtendedMultiply(f.Order, g.Order), ExtendedMultiply(f.Degree, g.Degree));
        }

        /// <summary>
        /// Computes the coefficient of z^n in f^k, where k is a nonnegative integer.
        /// </summary>
        /// <param name="f">A series.</param>
        /// <param name="k">A nonnegative integer exponent.</param>
        /// <param name="n">The coefficient index.</param>
        /// <returns>The coefficient of z^n in f^k.</returns>
        public static Rational PowCoefficient(Series f, int k, int n, bool nonZeroOrder = false)
        {
            if (k == 0)
                return n == 0 ? Rational.One : Rational.Zero;

            Rational c = Rational.Zero;
            int[] buffer = new int[System.Math.Min(n, k) + 1];
            int[] input = new int[n + 1];

            foreach (var partition in Discrete.Partitions(n, k, buffer))
            {
                if (nonZeroOrder && partition.Count != k)
                    continue;

                Rational p = Rational.One;
                int i;

                Array.Clear(input, 0, n + 1);

                for (i = 0; i < partition.Count; i++)
                {
                    p *= f[partition.Array[partition.Offset + i]];
                    input[partition.Array[partition.Offset + i]]++;
                }

                input[0] = k - partition.Count;

                for (; i < k; i++)
                {
                    p *= f[0];
                }

                p *= Multinomial(input);
                c += p;
            }

            return c;
        }

        #endregion

        #region Basic Series

        public static Series ZPow(int k)
        {
            return new Series((n, s) => n == k ? Rational.One : Rational.Zero, order: k, degree: k);
        }

        /// <summary>
        /// Generates the series (1+az)^r.
        /// </summary>
        public static Series Binomial(Rational a, Rational r)
        {
            int degree = InfinityDegree;

            if (r.IsInteger && r.Top >= 0 && r.Top <= int.MaxValue)
                degree = (int)r.Top;

            if (a == Rational.One)
                return new Series((n, s) => Discrete.Binomial(r, n), 0, degree);
            else
                return new Series((n, s) => Rational.Pow(a, n) * Discrete.Binomial(r, n), 0, degree);
        }

        #endregion

        private Func<int, Series, Rational> _compute;
        private int _order;
        private int _degree;
        private Rational[] _cache;
        private int _cacheValidTo;

        public Series(Func<int, Series, Rational> compute, int order = 0, int degree = InfinityDegree, bool cache = false)
        {
            _compute = compute;
            _order = order;
            _degree = degree;

            if (cache)
            {
                _cache = new Rational[2];
            }

            _cacheValidTo = -1;
        }

        /// <summary>
        /// The order ord(f) of the series. If there exists an integer m such that a[m]!=0
        /// and a[n]=0 for all n&lt;m, we define ord(f) = m. If f=0 then we define
        /// ord(f)=infinity (represented here by InfiniteOrder).
        ///
        /// If ord(f) is currently unknown, then a lower bound for ord(f) is returned.
        /// </summary>
        public int Order
        {
            get { return _order; }
        }

        /// <summary>
        /// The degree deg(f) of the series. If there exists an integer m such that a[m]!=0
        /// and a[n]=0 for all n&gt;m, we define deg(f) = m. If f=0 then we define
        /// deg(f)=-infinity (represented here by MinusInfinityDegree). If f has infinitely
        /// many nonzero coefficients then we define deg(f)=infinity (represented here by
        /// InfinityDegree).
        ///
        /// If deg(f) is currently unknown, then an upper bound for deg(f) is returned.
        /// </summary>
        public int Degree
        {
            get { return _degree; }
        }

        /// <summary>
        /// Gets the coefficient a[n].
        /// </summary>
        /// <param name="n">The index of the coefficient.</param>
        /// <returns>The coefficient a[n].</returns>
        public Rational this[int n]
        {
            get { return this.At(n); }
        }

        /// <summary>
        /// Gets the coefficient a[n].
        /// </summary>
        /// <param name="n">The index of the coefficient.</param>
        /// <returns>The coefficient a[n].</returns>
        public Rational At(int n)
        {
            if (_cache != null)
            {
                if (_cacheValidTo >= n)
                    return _cache[n];

                Rational a = _compute(n, this);

                if (_cache != null && _cacheValidTo == n - 1)
                {
                    EnsureCache(n);
                    _cache[n] = a;
                    _cacheValidTo = n;
                }

                return a;
            }
            else
            {
                return _compute(n, this);
            }
        }

        /// <summary>
        /// Determines whether f is a unit (invertible).
        /// </summary>
        /// <returns>Whether f is invertible.</returns>
        public bool IsUnit()
        {
            return this.At(0) != Rational.Zero;
        }

        public string ToString(int displayTerms)
        {
            int low = _order;
            int high = System.Math.Min(_degree, _order + displayTerms - 1);
            StringBuilder sb = new StringBuilder();

            for (int i = low; i <= high; i++)
            {
                Rational a = this[i];

                if (a == Rational.Zero)
                    continue;

                if (a > 0)
                {
                    sb.Append('+');

                    if (a != Rational.One || i == 0)
                        sb.Append(a.ToString());
                }
                else if (a == Rational.MinusOne)
                {
                    sb.Append('-');
                }
                else
                {
                    sb.Append(a.ToString());
                }

                if (i > 0)
                {
                    sb.Append('z');

                    if (i > 1)
                    {
                        sb.Append('^');
                        sb.Append(i.ToString());
                    }
                }
            }

            if (sb.Length == 0)
                sb.Append('0');
            else if (sb[0] == '+')
                sb.Remove(0, 1);

            if (high < _degree)
                sb.Append("+...");

            return sb.ToString();
        }

        public override string ToString()
        {
            return this.ToString(DefaultDisplayTerms);
        }

        #region Cache

        private void EnsureCache(int n)
        {
            if (_cache.Length < n + 1)
            {
                int newLength = _cache.Length * 2;

                if (newLength < n + 1)
                    newLength = n + 1;

                Array.Resize(ref _cache, newLength);
            }
        }

        public void ComputeTo(int n)
        {
            if (_cache == null)
                return;

            EnsureCache(n);

            for (int i = _cacheValidTo + 1; i <= n; i++)
            {
                _cache[i] = _compute(i, this);
            }

            _cacheValidTo = n;
        }

        public bool IsComputedTo(int n)
        {
            return _cacheValidTo >= n;
        }

        #endregion

        #region Functions

        public Series Differentiate()
        {
            int order;
            int degree;

            if (_order == 0)
                order = 0;
            else
                order = ExtendedSubtract(_order, 1);

            if (_degree == 0)
                degree = MinusInfinityDegree;
            else
                degree = ExtendedSubtract(_degree, 1);

            return new Series((n, s) => (n + 1) * this[n + 1], order: order, degree: degree);
        }

        public Series Integrate()
        {
            return new Series((n, s) =>
            {
                if (n == 0)
                    return 0;
                else
                    return this[n - 1] / n;
            }, order: ExtendedAdd(_order, 1), degree: ExtendedAdd(_degree, 1));
        }

        public Series Inv()
        {
            // We have the following recursive formula for the coefficients b[n] of
            // the multiplicative inverse f^-1:
            //
            // b[0] = a[0]^-1
            // b[1] = -a[0]^-1(a[1]b[0])
            // b[2] = -a[0]^-1(a[1]b[1]+a[2]b[0])
            // b[3] = -a[0]^-1(a[1]b[2]+a[2]b[1]+a[3]b[0])
            // ...
            //
            // We also have an explicit formula for b[n]:
            //
            // b[n] = a[0]^-1(1-c[n,1]+c[n,2]-c[n,3]+...+c[n,n]),
            // c[n,k] = a[0]^-k sum(multinomial*a[i[1]]...a[i[k]])
            // where the sum is taken over all partitions (i[1],...,i[k]) of n
            // into k parts, and multinomial is the multinomial coefficient for the
            // partition.

            return new Series((n, s) =>
            {
                Rational a0inv = Rational.Inv(this[0]);

                if (n == 0)
                    return a0inv;

                // If the previous terms have all been computed, use the recursive
                // formula. Otherwise, use the explicit formula.
                if (s.IsComputedTo(n - 1))
                {
                    int high = n;
                    Rational c = Rational.Zero;

                    if (high > _degree)
                        high = _degree;

                    for (int i = 1; i <= high; i++)
                    {
                        c += this[i] * s[n - i];
                    }

                    return -a0inv * c;
                }
                else
                {
                    Rational m = a0inv;
                    Rational c = Rational.Zero;

                    for (int i = 1; i <= n; i++)
                    {
                        m *= -a0inv;
                        c += m * PowCoefficient(this, i, n, true);
                    }

                    return c;
                }
            }, order: 0, degree: _degree != 0 ? InfinityDegree : 0, cache: true);
        }

        public Series Pow(int k)
        {
            if (k == 0)
                return One;
            else if (k > 0)
                return new Series((n, s) => PowCoefficient(this, k, n), ExtendedMultiply(this.Order, k), ExtendedMultiply(this.Degree, k));
            else
                return this.Pow(-k).Inv();
        }

        #endregion
    }
}
