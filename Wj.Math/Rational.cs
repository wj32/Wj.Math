using System;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Wj.Math
{
    public struct Rational :
        IComparable<Rational>,
        IComparable<BigInteger>,
        IEquatable<Rational>,
        IEquatable<BigInteger>
    {
        public const ulong SingleBase = 10000000;
        public const ulong DoubleBase = 10000000000000000;
        public static readonly Rational One = new Rational(1, 1, true);
        public static readonly Rational Zero = new Rational(0, 1, true);

        #region Basic Operators

        // Nothing
        public static Rational operator +(Rational q)
        {
            return q;
        }

        public static Rational Negate(Rational q)
        {
            return new Rational(-q.Top, q.Bottom, true);
        }
        public static Rational operator -(Rational q)
        {
            return Negate(q);
        }

        public static Rational Add(Rational q1, Rational q2)
        {
            if (q1.Bottom == q2.Bottom)
                return new Rational(q1.Top + q2.Top, q1.Bottom);

            return new Rational(q1.Top * q2.Bottom + q1.Bottom * q2.Top, q1.Bottom * q2.Bottom);
        }
        public static Rational operator +(Rational q1, Rational q2)
        {
            return Add(q1, q2);
        }

        public static Rational Subtract(Rational q1, Rational q2)
        {
            if (q1.Bottom == q2.Bottom)
                return new Rational(q1.Top - q2.Top, q1.Bottom);

            return new Rational(q1.Top * q2.Bottom - q1.Bottom * q2.Top, q1.Bottom * q2.Bottom);
        }
        public static Rational operator -(Rational q1, Rational q2)
        {
            return Subtract(q1, q2);
        }

        public static Rational Multiply(Rational q1, Rational q2)
        {
            return new Rational(q1.Top * q2.Top, q1.Bottom * q2.Bottom);
        }
        public static Rational operator *(Rational q1, Rational q2)
        {
            return Multiply(q1, q2);
        }

        public static Rational Divide(Rational q1, Rational q2)
        {
            return new Rational(q1.Top * q2.Bottom, q1.Bottom * q2.Top);
        }
        public static Rational operator /(Rational q1, Rational q2)
        {
            return Divide(q1, q2);
        }

        public static Rational operator ++(Rational q)
        {
            return new Rational(q.Top + q.Bottom, q.Bottom);
        }

        public static Rational operator --(Rational q)
        {
            return new Rational(q.Top - q.Bottom, q.Bottom);
        }

        #endregion

        #region Convenience Operators

        public static Rational Add(Rational q, BigInteger k)
        {
            return new Rational(q.Top + k * q.Bottom, q.Bottom);
        }
        public static Rational operator +(Rational q, BigInteger k)
        {
            return Add(q, k);
        }
        public static Rational operator +(BigInteger k, Rational q)
        {
            return Add(q, k);
        }

        public static Rational Subtract(Rational q, BigInteger k)
        {
            return new Rational(q.Top - k * q.Bottom, q.Bottom);
        }
        public static Rational operator -(Rational q, BigInteger k)
        {
            return Subtract(q, k);
        }
        public static Rational Subtract(BigInteger k, Rational q)
        {
            return new Rational(k * q.Bottom - q.Top, q.Bottom);
        }
        public static Rational operator -(BigInteger k, Rational q)
        {
            return Subtract(k, q);
        }

        public static Rational Multiply(Rational q, BigInteger k)
        {
            return new Rational(k * q.Top, q.Bottom);
        }
        public static Rational operator *(Rational q, BigInteger k)
        {
            return Multiply(q, k);
        }
        public static Rational operator *(BigInteger k, Rational q)
        {
            return Multiply(q, k);
        }

        public static Rational Divide(Rational q, BigInteger k)
        {
            return new Rational(q.Top, k * q.Bottom);
        }
        public static Rational operator /(Rational q, BigInteger k)
        {
            return Divide(q, k);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Rational q1, Rational q2)
        {
            return q1.Equals(q2);
        }
        public static bool operator !=(Rational q1, Rational q2)
        {
            return !q1.Equals(q2);
        }
        public static bool operator <(Rational q1, Rational q2)
        {
            return q1.CompareTo(q2) < 0;
        }
        public static bool operator <=(Rational q1, Rational q2)
        {
            return q1.CompareTo(q2) <= 0;
        }
        public static bool operator >(Rational q1, Rational q2)
        {
            return q1.CompareTo(q2) > 0;
        }
        public static bool operator >=(Rational q1, Rational q2)
        {
            return q1.CompareTo(q2) >= 0;
        }

        public static bool operator ==(Rational q1, BigInteger k2)
        {
            return q1.Equals(k2);
        }
        public static bool operator !=(Rational q1, BigInteger k2)
        {
            return !q1.Equals(k2);
        }
        public static bool operator <(Rational q1, BigInteger k2)
        {
            return q1.CompareTo(k2) < 0;
        }
        public static bool operator <=(Rational q1, BigInteger k2)
        {
            return q1.CompareTo(k2) <= 0;
        }
        public static bool operator >(Rational q1, BigInteger k2)
        {
            return q1.CompareTo(k2) > 0;
        }
        public static bool operator >=(Rational q1, BigInteger k2)
        {
            return q1.CompareTo(k2) >= 0;
        }

        #endregion

        #region Cast Operators

        public static implicit operator Rational(sbyte top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(byte top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(short top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(ushort top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(int top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(uint top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(long top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(ulong top)
        {
            return new Rational(top, 1, true);
        }

        public static implicit operator Rational(BigInteger top)
        {
            return new Rational(top, 1, true);
        }

        public static explicit operator Rational(float top)
        {
            return new Rational((BigInteger)(top * (float)SingleBase), SingleBase);
        }

        public static explicit operator Rational(double top)
        {
            return new Rational((BigInteger)(top * (double)DoubleBase), DoubleBase);
        }

        public static explicit operator Rational(decimal top)
        {
            return (Rational)(double)top;
        }

        public static explicit operator float(Rational q)
        {
            return (float)q.Top / (float)q.Bottom;
        }

        public static explicit operator double(Rational q)
        {
            return (double)q.Top / (double)q.Bottom;
        }

        public static explicit operator decimal(Rational q)
        {
            return (decimal)q.Top / (decimal)q.Bottom;
        }

        #endregion

        #region Conversion

        public static Rational FromDouble(double re, ulong digits = DoubleBase)
        {
            return new Rational((BigInteger)(re * (double)digits), digits);
        }

        #endregion

        #region Functions

        public static Rational Inv(Rational q)
        {
            return new Rational(q.Bottom, q.Top, true);
        }

        public static Rational Pow(Rational b, int n)
        {
            Rational result = Rational.One;

            while (n != 0)
            {
                if (n % 2 != 0)
                    result *= b;

                if (n == 1)
                    break;

                n /= 2;
                b *= b;
            }

            return result;
        }

        #endregion

        private BigInteger _top;
        private BigInteger _bottom;

        public Rational(BigInteger top)
            : this(top, 1)
        { }

        public Rational(BigInteger top, BigInteger bottom)
            : this(top, bottom, false)
        { }

        private Rational(BigInteger top, BigInteger bottom, bool reduced)
        {
            _top = top;
            _bottom = bottom;

            if (bottom < 0)
            {
                _top = -_top;
                _bottom = -_bottom;
            }

            if (!reduced)
            {
                BigInteger gcd = BigInteger.GreatestCommonDivisor(_top, _bottom);

                if (gcd != BigInteger.One)
                {
                    _top /= gcd;
                    _bottom /= gcd;
                }
            }
        }

        public BigInteger Top
        {
            get { return _top; }
        }

        public BigInteger Bottom
        {
            get { if (_bottom != 0) return _bottom; else return 1; }
        }

        public bool IsInteger
        {
            get { return this.Bottom == 1; }
        }

        public int CompareTo(Rational other)
        {
            BigInteger n1 = this.Top * other.Bottom;
            BigInteger n2 = this.Bottom * other.Top;

            return n1.CompareTo(n2);
        }

        public int CompareTo(BigInteger other)
        {
            BigInteger n1 = this.Top;
            BigInteger n2 = this.Bottom * other;

            return n1.CompareTo(n2);
        }

        public bool Equals(Rational other)
        {
            BigInteger n1 = this.Top * other.Bottom;
            BigInteger n2 = this.Bottom * other.Top;

            return n1.Equals(n2);
        }

        public bool Equals(BigInteger other)
        {
            if (this.Bottom != 1)
                return false;

            return this.Top == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is Rational)
                return this.Equals((Rational)obj);
            else if (obj is BigInteger)
                return this.Equals((BigInteger)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _top.GetHashCode() + 7 * _bottom.GetHashCode();
        }

        public override string ToString()
        {
            if (this.Bottom == 1)
                return this.Top.ToString();
            else
                return this.Top.ToString() + "/" + this.Bottom.ToString();
        }
    }
}
