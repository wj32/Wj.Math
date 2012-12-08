using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public struct Complex :
        IEquatable<Complex>,
        IEquatable<float>,
        IEquatable<double>,
        IEquatable<decimal>
    {
        public static readonly Complex I = new Complex(0, 1);
        public static readonly Complex One = new Complex(1, 0);
        public static readonly Complex PracticalEpsilon = new Complex(RealField.Epsilon, RealField.Epsilon);
        public static readonly int PracticalEpsilonDigits = RealField.EpsilonDigits;
        public static readonly Complex Zero = new Complex(0, 0);

        #region Basic Operators

        // Nothing
        public static Complex operator +(Complex c)
        {
            return c;
        }

        public static Complex Negate(Complex c)
        {
            return new Complex(-c.Re, -c.Im);
        }
        public static Complex operator -(Complex c)
        {
            return Negate(c);
        }

        public static Complex Conjugate(Complex c)
        {
            return new Complex(c.Re, -c.Im);
        }
        public static Complex operator ~(Complex c)
        {
            return Conjugate(c);
        }

        public static Complex Add(Complex c1, Complex c2)
        {
            return new Complex(c1.Re + c2.Re, c1.Im + c2.Im);
        }
        public static Complex operator +(Complex c1, Complex c2)
        {
            return Add(c1, c2);
        }

        public static Complex Subtract(Complex c1, Complex c2)
        {
            return new Complex(c1.Re - c2.Re, c1.Im - c2.Im);
        }
        public static Complex operator -(Complex c1, Complex c2)
        {
            return Subtract(c1, c2);
        }

        public static Complex Multiply(Complex c1, Complex c2)
        {
            return new Complex(
                (c1.Re * c2.Re) - (c1.Im * c2.Im),
                (c1.Re * c2.Im) + (c1.Im * c2.Re)
                );
        }
        public static Complex operator *(Complex c1, Complex c2)
        {
            return Multiply(c1, c2);
        }

        public static Complex Divide(Complex c1, Complex c2)
        {
            double d = (c2.Re * c2.Re) + (c2.Im * c2.Im);

            return new Complex(
                ((c1.Re * c2.Re) + (c1.Im * c2.Im)) / d,
                ((c1.Im * c2.Re) - (c1.Re * c2.Im)) / d
                );
        }
        public static Complex operator /(Complex c1, Complex c2)
        {
            return Divide(c1, c2);
        }

        #endregion

        #region Convenience Operators

        public static Complex Add(Complex c, double d)
        {
            return new Complex(c.Re + d, c.Im);
        }
        public static Complex operator +(Complex c, double d)
        {
            return Add(c, d);
        }
        public static Complex operator +(double d, Complex c)
        {
            return Add(c, d);
        }

        public static Complex Subtract(Complex c, double d)
        {
            return new Complex(c.Re - d, c.Im);
        }
        public static Complex operator -(Complex c, double d)
        {
            return Subtract(c, d);
        }
        public static Complex Subtract(double d, Complex c)
        {
            return new Complex(d - c.Re, c.Im);
        }
        public static Complex operator -(double d, Complex c)
        {
            return Subtract(d, c);
        }

        public static Complex Multiply(Complex c, double d)
        {
            return new Complex(c.Re * d, c.Im * d);
        }
        public static Complex operator *(Complex c, double d)
        {
            return Multiply(c, d);
        }
        public static Complex operator *(double d, Complex c)
        {
            return Multiply(c, d);
        }

        public static Complex Divide(Complex c, double d)
        {
            return new Complex(c.Re / d, c.Im / d);
        }
        public static Complex operator /(Complex c, double d)
        {
            return Divide(c, d);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Complex c1, Complex c2)
        {
            return c1.Equals(c2);
        }
        public static bool operator !=(Complex c1, Complex c2)
        {
            return !c1.Equals(c2);
        }

        public static bool operator ==(Complex c, float f)
        {
            return c.Equals(f);
        }
        public static bool operator !=(Complex c, float f)
        {
            return !c.Equals(f);
        }

        public static bool operator ==(Complex c, double d)
        {
            return c.Equals(d);
        }
        public static bool operator !=(Complex c, double d)
        {
            return !c.Equals(d);
        }

        #endregion

        #region Cast Operators

        public static implicit operator Complex(sbyte re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(byte re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(short re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(ushort re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(int re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(uint re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(long re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(ulong re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(float re)
        {
            return new Complex(re, 0);
        }

        public static implicit operator Complex(double re)
        {
            return new Complex(re, 0);
        }

        public static explicit operator Complex(decimal re)
        {
            return new Complex((double)re, 0);
        }

        #endregion

        #region Conversion

        public static Complex FromPolar(double mag, double arg)
        {
            return mag * (new Complex(System.Math.Cos(arg), System.Math.Sin(arg)));
        }

        public static Complex RoundToEpsilon(Complex c)
        {
            return RoundToEpsilon(c, PracticalEpsilonDigits);
        }

        public static Complex RoundToEpsilon(Complex c, int digits)
        {
            return new Complex(
                System.Math.Round(c.Re, digits),
                System.Math.Round(c.Im, digits)
                );
        }

        #endregion

        #region Functions

        #region Basic

        public static Complex Exp(Complex c)
        {
            return FromPolar(System.Math.Exp(c.Re), c.Im);
        }

        public static Complex Inv(Complex c)
        {
            double d = (c.Re * c.Re) + (c.Im * c.Im);

            return new Complex(c.Re / d, -c.Im / d);
        }

        public static Complex Log(Complex c)
        {
            return new Complex(
                0.5 * System.Math.Log((c.Re * c.Re) + (c.Im * c.Im)),
                c.Arg
                );
        }

        public static Complex Log(double d)
        {
            return new Complex(
                0.5 * System.Math.Log(d * d),
                System.Math.Atan2(0, d)
                );
        }

        public static Complex Log(Complex c, Complex newBase)
        {
            return Log(c) / Log(newBase);
        }

        public static Complex Log(Complex c, double newBase)
        {
            return Log(c) / Log(newBase);
        }

        public static Complex Log(double c, Complex newBase)
        {
            return Log(c) / Log(newBase);
        }

        public static Complex Pow(Complex c1, Complex c2)
        {
            double m = (c1.Re * c1.Re) + (c1.Im * c1.Im);
            double arg = c1.Arg;

            return FromPolar(
                System.Math.Pow(m, c2.Re / 2) / System.Math.Exp(c2.Im * arg),
                (0.5 * c2.Im * System.Math.Log(m)) + (c2.Re * arg)
                );
        }

        public static Complex Pow(Complex c, double d)
        {
            return FromPolar(System.Math.Pow(c.Abs, d), c.Arg * d);
        }

        public static Complex Pow(double d, Complex c)
        {
            return FromPolar(System.Math.Pow(d, c.Re), c.Im * System.Math.Log(d));
        }

        public static Complex Sqrt(Complex c)
        {
            double mag = c.Abs;

            if (c.Im >= 0)
            {
                return new Complex(System.Math.Sqrt(mag + c.Re), System.Math.Sqrt(mag - c.Re)) / RealField.Sqrt2;
            }
            else
            {
                return new Complex(System.Math.Sqrt(mag + c.Re), -System.Math.Sqrt(mag - c.Re)) / RealField.Sqrt2;
            }
        }

        public static Complex Sqrt(double d)
        {
            if (d >= 0)
                return new Complex(System.Math.Sqrt(d), 0);
            else
                return new Complex(0, System.Math.Sqrt(-d));
        }

        #endregion

        #region Trigonometric

        public static Complex Acos(Complex c)
        {
            Complex r = Log(c + Sqrt((c * c) - 1));

            return new Complex(r.Im, -r.Re);
        }

        public static Complex Asin(Complex c)
        {
            Complex r = Log(new Complex(-c.Im, c.Re) + Sqrt(1 - (c * c)));

            return new Complex(r.Im, -r.Re);
        }

        public static Complex Atan(Complex c)
        {
            Complex iz = new Complex(-c.Im, c.Re);
            Complex r = Log((1 - iz) / (1 + iz));

            return new Complex(-r.Im / 2, r.Re / 2);
        }

        public static Complex Cos(Complex c)
        {
            double exp2y = System.Math.Exp(c.Im * 2);

            return new Complex(
                System.Math.Cos(c.Re) * (exp2y + 1),
                System.Math.Sin(c.Re) * (1 - exp2y)
                ) / (2 * System.Math.Exp(c.Im));
        }

        public static Complex Sin(Complex c)
        {
            double exp2y = System.Math.Exp(c.Im * 2);

            return new Complex(
                System.Math.Sin(c.Re) * (exp2y + 1),
                System.Math.Cos(c.Re) * (exp2y - 1)
                ) / (2 * System.Math.Exp(c.Im));
        }

        public static Complex Tan(Complex c)
        {
            double exp2y = System.Math.Exp(c.Im * 2);
            double sinx = System.Math.Sin(c.Re);
            double cosx = System.Math.Cos(c.Re);

            return new Complex(
                4 * exp2y * sinx * cosx,
                (exp2y * exp2y) - 1
                ) / (
                ((exp2y + 1) * (exp2y + 1) * cosx * cosx) +
                ((1 - exp2y) * (1 - exp2y) * sinx * sinx)
                );
        }

        #endregion

        #region Hyperbolic

        public static Complex Acosh(Complex c)
        {
            return Log(c + Sqrt((c * c) - 1));
        }

        public static Complex Asinh(Complex c)
        {
            return Log(c + Sqrt((c * c) + 1));
        }

        public static Complex Atanh(Complex c)
        {
            return 0.5 * Log((1 + c) / (1 - c));
        }

        public static Complex Cosh(Complex c)
        {
            return 0.5 * (Exp(c) + Exp(-c));
        }

        public static Complex Sinh(Complex c)
        {
            return 0.5 * (Exp(c) - Exp(-c));
        }

        public static Complex Tanh(Complex c)
        {
            Complex exp2z = Exp(2 * c);

            return (exp2z - 1) / (exp2z + 1);
        }

        #endregion

        #region Special

        public static Complex Fibonacci(Complex c)
        {
            return (Pow(RealField.Phi, c) - Cos(c * System.Math.PI) * Pow(RealField.Phi, -c)) / RealField.Sqrt5;
        }

        public static Complex Gamma(Complex c)
        {
            // The formula is only valid for the right complex half-plane, 
            // so we'll use the reflection formula.
            if (c.Re < 0)
                return System.Math.PI / Sin(System.Math.PI * (1 - c)) / Gamma(1 - c);

            // Original Stirling's approximation.
            //return Sqrt(_2pi / c) *
            //    Pow((c / System.Math.E) * Sqrt(c * Sinh(1 / c) + 1 / (810 * Pow(c, 6))), c);

            // Nemes, 2007.
            return Sqrt(RealField.DoublePi / c) * Pow(RealField.InvE * (c + 1 / (12 * c - (1 / (10 * c)))), c);
        }

        public static Complex LambertW(Complex c)
        {
            Complex x = 0;

            while (true)
            {
                Complex innerPart;
                Complex newX;

                innerPart = x * Exp(x) - c;
                newX = x - innerPart / (Exp(x) * (x + 1) - ((x + 2) * innerPart) / (2 * x + 2));

                if (x.ApproxEquals(newX))
                    break;

                x = newX;
            }

            return x;
        }

        #endregion

        #endregion

        private double _re;
        private double _im;

        public Complex(double re, double im)
        {
            _re = re;
            _im = im;
        }

        public Complex(int re, int im)
        {
            _re = re;
            _im = im;
        }

        public Complex(float re, float im)
        {
            _re = re;
            _im = im;
        }

        public double Re
        {
            get { return _re; }
        }

        public double Im
        {
            get { return _im; }
        }

        public double Abs
        {
            get { return System.Math.Sqrt((_re * _re) + (_im * _im)); }
        }

        public double Arg
        {
            get { return System.Math.Atan2(_im, _re); }
        }

        public bool ApproxEquals(Complex c)
        {
            return this.ApproxEquals(c, PracticalEpsilon);
        }

        public bool ApproxEquals(Complex c, Complex epsilon)
        {
            if (System.Math.Abs(this.Re - c.Re) > epsilon.Re)
                return false;
            if (System.Math.Abs(this.Im - c.Im) > epsilon.Im)
                return false;

            return true;
        }

        public bool Equals(Complex c)
        {
            return this.Re == c.Re && this.Im == c.Im;
        }

        public bool Equals(float f)
        {
            return this.Im == 0 && this.Re == f;
        }

        public bool Equals(double d)
        {
            return this.Im == 0 && this.Re == d;
        }

        public bool Equals(decimal d)
        {
            return this.Im == 0 && (decimal)this.Re == d;
        }

        public override bool Equals(object obj)
        {
            if (obj is Complex)
                return this.Equals((Complex)obj);
            else if (obj is float)
                return this.Equals((float)obj);
            else if (obj is double)
                return this.Equals((double)obj);
            else if (obj is decimal)
                return this.Equals((decimal)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            long re, im;

            re = BitConverter.DoubleToInt64Bits(_re);
            im = BitConverter.DoubleToInt64Bits(_im);

            return (int)(re >> 32) ^ ((int)re) ^ (int)(im >> 32) ^ ((int)im);
        }

        public override string ToString()
        {
            Complex c = RoundToEpsilon(this);

            if (c.Re != 0 && c.Im != 0)
            {
                string im = c.Im >= 0 ? ("+" + c.Im.ToString()) : ("-" + (-c.Im).ToString());

                return c.Re.ToString() + im + "i";
            }
            else if (c.Re != 0)
            {
                return c.Re.ToString();
            }
            else if (c.Im != 0)
            {
                return c.Im.ToString() + "i";
            }
            else
            {
                return "0";
            }
        }
    }
}
