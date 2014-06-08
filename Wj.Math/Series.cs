using System;
using System.Collections.Generic;
using System.Linq;
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

        public static readonly Series One = new Series(n => n == 0 ? Rational.One : Rational.Zero, order: 0, degree: 0);
        public static readonly Series Zero = new Series(n => Rational.Zero, order: InfinityOrder, degree: MinusInfinityDegree);
        public static readonly Series Z = new Series(n => n == 1 ? Rational.One : Rational.Zero, order: 1, degree: 1);

        #region Internal Functions

        /// <summary>
        /// Adds two values.
        /// </summary>
        /// <param name="x">An integer that may also be +infinity or -infinity.</param>
        /// <param name="y">An integer that may also be +infinity or -infinity.</param>
        /// <returns>An integer that may also be +infinity or -infinity.</returns>
        internal static int ExtendedAdd(int x, int y)
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
        internal static int ExtendedSubtract(int x, int y)
        {
            if (x == int.MaxValue)
                return x;
            else if (x == int.MinValue)
                return x;
            else
                return x - y;
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
            return new Series(n => -f[n], order: f.Order, degree: f.Degree);
        }
        public static Series operator -(Series f)
        {
            return Negate(f);
        }

        public static Series Add(Series f, Series g)
        {
            return new Series(n => f[n] + g[n], order: System.Math.Min(f.Order, g.Order), degree: System.Math.Max(f.Degree, g.Degree));
        }
        public static Series operator +(Series f, Series g)
        {
            return Add(f, g);
        }

        public static Series Subtract(Series f, Series g)
        {
            return new Series(n => f[n] - g[n], order: System.Math.Min(f.Order, g.Order), degree: System.Math.Max(f.Degree, g.Degree));
        }
        public static Series operator -(Series f, Series g)
        {
            return Subtract(f, g);
        }

        public static Series Multiply(Series f, Series g)
        {
            return new Series(n =>
            {
                int low = 0;
                int high = n;
                int i;
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

                for (i = low; i <= high; i++)
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
            return new Series(n =>
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
            return new Series(n =>
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
            return new Series(n =>
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
                return new Series(n => f[n] * k, order: f.Order, degree: f.Degree);
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
            return new Series(n => f[n] / k, order: f.Order, degree: f.Degree);
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

            return new Series(n => n == 0 ? k : Rational.Zero, order: 0, degree: 0);
        }

        #endregion

        private Func<int, Rational> _compute;
        private int _order;
        private int _degree;

        public Series(Func<int, Rational> compute, int order = 0, int degree = InfinityDegree, bool cache = false)
        {
            _compute = compute;
            _order = order;
            _degree = degree;
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
            return _compute(n);
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

            return new Series(n => (n + 1) * this[n + 1], order: order, degree: degree);
        }

        public Series Integrate()
        {
            return new Series(n =>
            {
                if (n == 0)
                    return 0;
                else
                    return this[n - 1] / n;
            }, order: ExtendedAdd(_order, 1), degree: ExtendedAdd(_degree, 1));
        }

        public Series Inv()
        {
            throw new NotImplementedException();
        }

        public Series Pow(int n)
        {
            if (n == 0)
                return One;

            throw new NotImplementedException();
        }

        #endregion
    }
}
