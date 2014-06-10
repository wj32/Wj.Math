using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Wj.Math
{
    /// <summary>
    /// Represents a formal power series f(z) with rational coefficients a[n] for n=0,1,2,....
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
        public static readonly Series Geometric = new Series((n, s) => Rational.One, order: 0, degree: InfinityDegree);

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

        public static implicit operator Series(int k)
        {
            if (k == 0)
                return Zero;

            return new Series((n, s) => n == 0 ? new Rational(k, 1) : Rational.Zero, order: 0, degree: 0);
        }

        public static implicit operator Series(Rational k)
        {
            if (k == Rational.Zero)
                return Zero;

            return new Series((n, s) => n == 0 ? k : Rational.Zero, order: 0, degree: 0);
        }

        #endregion

        #region Functions

        public static Series MultiplyPointwise(Series f, Series g)
        {
            return new Series((n, s) => f[n] * g[n], order: System.Math.Max(f.Order, g.Order), degree: System.Math.Min(f.Degree, g.Degree));
        }

        public static Series DividePointwise(Series f, Series g)
        {
            return new Series((n, s) => f[n] / g[n], order: f.Order, degree: f.Degree);
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

        /// <summary>
        /// Generates the series z^k.
        /// </summary>
        public static Series ZPow(int k)
        {
            return new Series((n, s) => n == k ? Rational.One : Rational.Zero, order: k, degree: k);
        }

        public static Series Polynomial(params Rational[] coefficients)
        {
            int order = InfinityOrder;
            int degree = MinusInfinityDegree;

            for (int i = 0; i < coefficients.Length; i++)
            {
                if (coefficients[i] != Rational.Zero)
                {
                    order = i;
                    break;
                }
            }

            if (order != InfinityOrder)
            {
                degree = order;

                for (int i = coefficients.Length - 1; i > order; i--)
                {
                    if (coefficients[i] != Rational.Zero)
                    {
                        degree = i;
                        break;
                    }
                }
            }

            return new Series((n, s) =>
            {
                if (n > degree)
                    return Rational.Zero;

                return coefficients[n];
            }, order: order, degree: degree);
        }

        /// <summary>
        /// Generates the series (1+z)^r.
        /// </summary>
        public static Series Binomial(Rational r)
        {
            return Binomial(Rational.One, r);
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
                return new Series((n, s) => Discrete.Binomial(r, n), order: 0, degree: degree);
            else
                return new Series((n, s) => Rational.Pow(a, n) * Discrete.Binomial(r, n), order: 0, degree: degree);
        }

        /// <summary>
        /// The series exp(z).
        /// </summary>
        public static readonly Series Exp = new Series((n, s) => new Rational(1, Discrete.Factorial(n)), order: 0, degree: InfinityDegree);

        /// <summary>
        /// The series sin(z).
        /// </summary>
        public static readonly Series Sin = new Series(
            (n, s) => (n % 2 != 0) ? Discrete.AlternateSign(new Rational(1, Discrete.Factorial(n)), (n - 1) / 2) : Rational.Zero,
            order: 0, degree: InfinityDegree);

        /// <summary>
        /// The series cos(z).
        /// </summary>
        public static readonly Series Cos = new Series(
            (n, s) => (n % 2 == 0) ? Discrete.AlternateSign(new Rational(1, Discrete.Factorial(n)), n / 2) : Rational.Zero,
            order: 0, degree: InfinityDegree);

        /// <summary>
        /// The series arctan(z).
        /// </summary>
        public static readonly Series ArcTan = new Series(
            (n, s) => (n % 2 != 0) ? Discrete.AlternateSign(new Rational(1, n), (n - 1) / 2) : Rational.Zero,
            order: 0, degree: InfinityDegree);

        /// <summary>
        /// The series log(1+z).
        /// </summary>
        public static readonly Series Log1 = new Series((n, s) =>
        {
            if (n == 0)
                return Rational.Zero;

            return Discrete.AlternateSign(new Rational(1, n), n + 1);
        }, order: 1, degree: InfinityDegree);

        #endregion

        private Func<int, Series, Rational> _compute;
        private int _order;
        private int _degree;
        private Rational[] _cache;
        private int _cacheValidTo;

        public Series(Series s, bool cache)
        {
            _compute = s._compute;
            _order = s._order;
            _degree = s._degree;

            if (cache && s._cache != null)
            {
                _cache = s._cache;
                _cacheValidTo = s._cacheValidTo;
            }
            else
            {
                if (cache)
                {
                    _cache = new Rational[2];
                }

                _cacheValidTo = -1;
            }
        }

        public Series(Func<int, Series, Rational> compute, int order = 0, int degree = InfinityDegree, bool cache = false)
        {
            if (order > degree)
            {
                order = InfinityDegree;
                degree = MinusInfinityDegree;
            }

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
        /// The order ord(f(z)) of the series. If there exists an integer m such that a[m]!=0
        /// and a[n]=0 for all n&lt;m, we define ord(f(z)) = m. If f(z)=0 then we define
        /// ord(f(z))=infinity (represented here by InfiniteOrder).
        ///
        /// If ord(f(z)) is currently unknown, then a lower bound for ord(f(z)) is returned.
        /// </summary>
        public int Order
        {
            get { return _order; }
        }

        /// <summary>
        /// The degree deg(f(z)) of the series. If there exists an integer m such that a[m]!=0
        /// and a[n]=0 for all n&gt;m, we define deg(f(z)) = m. If f(z)=0 then we define
        /// deg(f(z))=-infinity (represented here by MinusInfinityDegree). If f(z) has infinitely
        /// many nonzero coefficients then we define deg(f(z))=infinity (represented here by
        /// InfinityDegree).
        ///
        /// If deg(f(z)) is currently unknown, then an upper bound for deg(f(z)) is returned.
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

        public string ToString(int displayTerms)
        {
            int low = this.Order;
            int high = System.Math.Min(this.Degree, this.Order + displayTerms - 1);
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

            if (high < this.Degree)
                sb.Append("+...");

            return sb.ToString();
        }

        public override string ToString()
        {
            return this.ToString(DefaultDisplayTerms);
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
        /// Determines whether f(z) is a unit (invertible).
        /// </summary>
        /// <returns>Whether f(z) is invertible.</returns>
        public bool IsUnit()
        {
            return this[0] != Rational.Zero;
        }

        public Polynomial<Rational, RationalField> ToPolynomial(int degree)
        {
            return this.ToPolynomial(degree, Polynomial<Rational, RationalField>.X);
        }

        public Polynomial<Rational, RationalField> ToPolynomial(int degree, Polynomial<Rational, RationalField> x)
        {
            int low = this.Order;
            int high = degree;

            if (x.Terms.Length == 1)
            {
                var terms = new List<Term<Rational>>();
                var leadingTerm = x.Terms[0];

                for (int i = low; i <= high; i++)
                {
                    var term = new Term<Rational>(x.Degree * i, this[i] * Rational.Pow(leadingTerm.Coeff, i));
                    Polynomial<Rational, RationalField>.Add(terms, term);
                }

                return new Polynomial<Rational, RationalField>(terms);
            }
            else
            {
                Polynomial<Rational, RationalField> result = Polynomial<Rational, RationalField>.Zero;
                int i;

                for (i = high; i >= low; i--)
                {
                    result *= x;
                    result += this[i];
                }

                for (; i >= 0; i--)
                {
                    result *= x;
                }

                return result;
            }
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

        #region Operations

        /// <summary>
        /// Generates the series f(g(z)), where g(z) has nonzero order.
        /// </summary>
        /// <param name="g">A series with nonzero order.</param>
        /// <returns>The series f(g(z)).</returns>
        public Series Compose(Series g)
        {
            if (g.IsUnit())
                throw new ArgumentException("g must have nonzero order");

            return new Series((n, s) =>
            {
                if (n == 0)
                    return this[0];

                int high = n;
                Rational c = Rational.Zero;

                if (high > this.Degree)
                    high = this.Degree;

                for (int i = 1; i <= high; i++)
                {
                    c += this[i] * PowCoefficient(g, i, n, true);
                }

                return c;
            }, order: ExtendedMultiply(this.Order, g.Order), degree: ExtendedMultiply(this.Degree, g.Degree), cache: true);
        }

        /// <summary>
        /// Generates the composition inverse of f(z), i.e. the series g(z) such that
        /// f(g(z)) = z. f(z) must have order 1.
        /// </summary>
        /// <returns>The series g(z).</returns>
        public Series CompositionInv()
        {
            if (this[0] != Rational.Zero || this[1] == Rational.Zero)
                throw new InvalidOperationException("The series has no composition inverse.");

            Series h = this.DivideZPow(1).Inv();

            return new Series((n, s) =>
            {
                if (n == 0)
                    return Rational.Zero;

                return PowCoefficient(h, n, n - 1, false) / n;
            }, order: 1, degree: this.Degree == 1 ? 1 : InfinityDegree);
        }

        public Series Differentiate()
        {
            int order;
            int degree;

            if (this.Order == 0)
                order = 0;
            else
                order = ExtendedSubtract(this.Order, 1);

            if (this.Degree == 0)
                degree = MinusInfinityDegree;
            else
                degree = ExtendedSubtract(this.Degree, 1);

            return new Series((n, s) => (n + 1) * this[n + 1], order: order, degree: degree);
        }

        public Series Integrate()
        {
            return new Series((n, s) =>
            {
                if (n == 0)
                    return Rational.Zero;
                else
                    return this[n - 1] / n;
            }, order: ExtendedAdd(this.Order, 1), degree: ExtendedAdd(this.Degree, 1));
        }

        public Series Inv()
        {
            // We have the following recursive formula for the coefficients b[n] of
            // the multiplicative inverse f(z)^-1:
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

                    if (high > this.Degree)
                        high = this.Degree;

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
            }, order: 0, degree: this.Degree != 0 ? InfinityDegree : 0, cache: true);
        }

        /// <summary>
        /// Generates the series f(z)^k.
        /// </summary>
        /// <param name="k">An integer.</param>
        /// <returns>The series f(z)^k.</returns>
        public Series Pow(int k)
        {
            if (k == 0)
                return One;
            else if (k > 0)
                return new Series((n, s) => PowCoefficient(this, k, n), order: ExtendedMultiply(this.Order, k), degree: ExtendedMultiply(this.Degree, k), cache: true);
            else
                return this.Pow(-k).Inv();
        }

        /// <summary>
        /// Generates the series f(z^k).
        /// </summary>
        /// <param name="k">A positive integer.</param>
        /// <returns>The series f(z^k).</returns>
        public Series ComposeZPow(int k)
        {
            return new Series((n, s) =>
            {
                if (n % k == 0)
                    return this[n / k];
                else
                    return Rational.Zero;
            }, order: ExtendedMultiply(this.Order, k), degree: ExtendedMultiply(this.Degree, k));
        }

        /// <summary>
        /// Generates the series z^(-k)f(z). Coefficients a[0],...,a[k-1] are discarded.
        /// </summary>
        /// <param name="k">A nonnegative integer.</param>
        /// <returns>The series z^(-k)f(z).</returns>
        public Series DivideZPow(int k)
        {
            int order;
            int degree;

            if (this.Order < k)
                order = 0;
            else
                order = ExtendedSubtract(this.Order, k);

            if (this.Degree < k)
                degree = MinusInfinityDegree;
            else
                degree = ExtendedSubtract(this.Degree, k);

            return new Series((n, s) => this[n + k], order: order, degree: degree);
        }

        /// <summary>
        /// Generates the series z^kf(z).
        /// </summary>
        /// <param name="k">A nonnegative integer.</param>
        /// <returns>The series z^kf(z).</returns>
        public Series MultiplyZPow(int k)
        {
            return new Series((n, s) =>
            {
                if (n >= k)
                    return this[n - k];
                else
                    return Rational.Zero;
            }, order: ExtendedAdd(this.Order, k), degree: ExtendedAdd(this.Degree, k));
        }

        public Series Truncate(int degree)
        {
            int order;

            if (this.Order > degree)
                order = InfinityDegree;
            else
                order = System.Math.Min(this.Order, degree);

            return new Series((n, s) =>
            {
                if (n <= degree)
                    return this[n];
                else
                    return Rational.Zero;
            }, order: order, degree: System.Math.Min(this.Degree, degree));
        }

        #endregion
    }
}
