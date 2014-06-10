using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public struct Term<T> :
        IEquatable<Term<T>>
        where T : IEquatable<T>
    {
        public static implicit operator Term<T>(T coeff)
        {
            return new Term<T>(0, coeff);
        }

        private int _deg;
        private T _coeff;

        public Term(int deg, T coeff)
        {
            _deg = deg;
            _coeff = coeff;
        }

        public int Deg
        {
            get { return _deg; }
            set { _deg = value; }
        }

        public T Coeff
        {
            get { return _coeff; }
            set { _coeff = value; }
        }

        public bool Equals(Term<T> t)
        {
            return this.Deg == t.Deg && this.Coeff.Equals(t.Coeff);
        }

        public override int GetHashCode()
        {
            return _deg.GetHashCode() ^ _coeff.GetHashCode();
        }
    }

    public class Polynomial<T, TField> :
        IEnumerable<Term<T>>,
        IEquatable<Polynomial<T, TField>>,
        IEquatable<T>
        where T : IEquatable<T>
        where TField : IField<T>, new()
    {
        public static readonly Polynomial<T, TField> One = new Polynomial<T, TField>(0, (new TField()).One);
        public static readonly Polynomial<T, TField> X = new Polynomial<T, TField>(1, (new TField()).One);
        public static readonly Polynomial<T, TField> Zero = new Polynomial<T, TField>();

        #region Internal Functions

        internal static void Add(List<Term<T>> terms, Term<T> t)
        {
            IField<T> field = (new TField()).Instance;
            int index;

            index = terms.FindIndex((t_) => t_.Deg == t.Deg);

            if (index != -1)
            {
                T newCoeff = field.Add(terms[index].Coeff, t.Coeff);

                if (!field.IsZero(newCoeff))
                    terms[index] = new Term<T>(t.Deg, newCoeff);
                else
                    terms.RemoveAt(index);
            }
            else if (!field.IsZero(t.Coeff))
            {
                terms.Add(t);
            }
        }

        internal static void Simplify(List<Term<T>> terms)
        {
            IField<T> field = (new TField()).Instance;
            Dictionary<int, T> termsDic = new Dictionary<int, T>();

            foreach (var t in terms)
            {
                if (!termsDic.ContainsKey(t.Deg))
                    termsDic.Add(t.Deg, field.Zero);

                termsDic[t.Deg] = field.Add(termsDic[t.Deg], t.Coeff);
            }

            terms.Clear();

            foreach (var pair in termsDic)
            {
                if (!field.IsZero(pair.Value))
                    terms.Add(new Term<T>(pair.Key, pair.Value));
            }
        }

        #endregion

        #region Basic Operators

        // Nothing
        public static Polynomial<T, TField> operator +(Polynomial<T, TField> p)
        {
            return p;
        }

        public static Polynomial<T, TField> Negate(Polynomial<T, TField> p)
        {
            var terms = p.CloneList();

            for (int i = 0; i < terms.Count; i++)
                terms[i] = new Term<T>(terms[i].Deg, p.Field.Negate(terms[i].Coeff));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator -(Polynomial<T, TField> p)
        {
            return Negate(p);
        }

        public static Polynomial<T, TField> Add(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            var terms = p1.CloneList();

            foreach (Term<T> t in p2)
                Add(terms, t);

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator +(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            return Add(p1, p2);
        }

        public static Polynomial<T, TField> Subtract(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            var terms = p1.CloneList();

            foreach (Term<T> t in p2)
                Add(terms, new Term<T>(t.Deg, p1.Field.Negate(t.Coeff)));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator -(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            return Subtract(p1, p2);
        }

        public static Polynomial<T, TField> Multiply(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            List<Term<T>> terms = new List<Term<T>>();

            foreach (Term<T> t1 in p1)
            {
                foreach (Term<T> t2 in p2)
                {
                    Add(terms, new Term<T>(t1.Deg + t2.Deg, p1.Field.Multiply(t1.Coeff, t2.Coeff)));
                }
            }

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator *(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            return Multiply(p1, p2);
        }

        public static Polynomial<T, TField> Divide(Polynomial<T, TField> p1, Polynomial<T, TField> p2, out Polynomial<T, TField> rem)
        {
            IField<T> field = p1.Field;
            T leadingCoeff;
            T[] dividList;
            T[] divisList;
            T[] quotList;
            T[] workList;

            if (p2.Degree > p1.Degree)
            {
                rem = p1;

                return Polynomial<T, TField>.Zero;
            }

            if (p2.Degree == 0)
            {
                rem = new Polynomial<T, TField>();

                return Divide(p1, p2[0].Coeff);
            }

            dividList = p1.ToCoefficientList();
            divisList = p2.ToCoefficientListForDivisor(out leadingCoeff);
            quotList = new T[p1.Degree - p2.Degree + 1];
            workList = new T[divisList.Length];

            if (!field.IsOne(leadingCoeff))
            {
                // Fix the dividend.
                for (int i = 0; i < dividList.Length; i++)
                    dividList[i] = field.Divide(dividList[i], leadingCoeff);
            }

            // Initialize the work list.
            Array.Copy(dividList, workList, workList.Length);

            for (int i = 0; i < quotList.Length; i++)
            {
                // Save the quotient cofficient.
                quotList[i] = workList[0];

                // Shift left, take in from dividend.
                Array.Copy(workList, 1, workList, 0, workList.Length - 1);
                workList[workList.Length - 1] = dividList[i + workList.Length];

                // Copy the divisor and multiply with the last quotient
                // coefficient. Add this to the work list. Note that this
                // is all done in one step.
                for (int j = 0; j < workList.Length; j++)
                    workList[j] = field.Add(workList[j], field.Multiply(divisList[j], quotList[i]));
            }

            if (!field.IsOne(leadingCoeff))
            {
                // Fix the remainder.
                for (int i = 0; i < workList.Length; i++)
                    workList[i] = field.Multiply(workList[i], leadingCoeff);
            }

            rem = FromCoefficientList(workList);

            return FromCoefficientList(quotList);
        }
        public static Polynomial<T, TField> Divide(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            Polynomial<T, TField> rem;

            return Divide(p1, p2, out rem);
        }
        public static Polynomial<T, TField> operator /(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            return Divide(p1, p2);
        }
        public static Polynomial<T, TField> operator %(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            Polynomial<T, TField> rem;

            Divide(p1, p2, out rem);

            return rem;
        }

        public static Polynomial<T, TField> operator ++(Polynomial<T, TField> p)
        {
            return p + Polynomial<T, TField>.One;
        }

        public static Polynomial<T, TField> operator --(Polynomial<T, TField> p)
        {
            return p - Polynomial<T, TField>.One;
        }

        #endregion

        #region Convenience Operators

        public static Polynomial<T, TField> Add(Polynomial<T, TField> p, Term<T> t)
        {
            var terms = p.CloneList();

            Add(terms, t);

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator +(Polynomial<T, TField> p, Term<T> t)
        {
            return Add(p, t);
        }
        public static Polynomial<T, TField> operator +(Term<T> t, Polynomial<T, TField> p)
        {
            return Add(p, t);
        }

        public static Polynomial<T, TField> Subtract(Polynomial<T, TField> p, Term<T> t)
        {
            var terms = p.CloneList();

            Add(terms, new Term<T>(t.Deg, p.Field.Negate(t.Coeff)));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator -(Polynomial<T, TField> p, Term<T> t)
        {
            return Subtract(p, t);
        }
        public static Polynomial<T, TField> Subtract(Term<T> t, Polynomial<T, TField> p)
        {
            var terms = new List<Term<T>>();

            if (!p.Field.IsZero(t.Coeff))
                terms.Add(t);

            foreach (Term<T> t_ in p)
                Add(terms, new Term<T>(t_.Deg, p.Field.Negate(t_.Coeff)));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator -(Term<T> t, Polynomial<T, TField> p)
        {
            return Subtract(t, p);
        }

        public static Polynomial<T, TField> Multiply(Polynomial<T, TField> p, Term<T> t)
        {
            var terms = p.CloneList();

            for (int i = 0; i < terms.Count; i++)
                terms[i] = new Term<T>(terms[i].Deg + t.Deg, p.Field.Multiply(terms[i].Coeff, t.Coeff));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator *(Polynomial<T, TField> p, Term<T> t)
        {
            return Multiply(p, t);
        }
        public static Polynomial<T, TField> operator *(Term<T> t, Polynomial<T, TField> p)
        {
            return Multiply(p, t);
        }

        public static Polynomial<T, TField> Multiply(Polynomial<T, TField> p, int n)
        {
            var terms = p.CloneList();

            for (int i = 0; i < terms.Count; i++)
                terms[i] = new Term<T>(terms[i].Deg, p.Field.Multiply(terms[i].Coeff, n));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator *(Polynomial<T, TField> p, int n)
        {
            return Multiply(p, n);
        }
        public static Polynomial<T, TField> operator *(int n, Polynomial<T, TField> p)
        {
            return Multiply(p, n);
        }

        public static Polynomial<T, TField> Divide(Polynomial<T, TField> p, T t)
        {
            var terms = p.CloneList();

            for (int i = 0; i < terms.Count; i++)
                terms[i] = new Term<T>(terms[i].Deg, p.Field.Divide(terms[i].Coeff, t));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator /(Polynomial<T, TField> p, T t)
        {
            return Divide(p, t);
        }

        public static Polynomial<T, TField> Divide(Polynomial<T, TField> p, int n)
        {
            var terms = p.CloneList();

            for (int i = 0; i < terms.Count; i++)
                terms[i] = new Term<T>(terms[i].Deg, p.Field.Divide(terms[i].Coeff, n));

            return new Polynomial<T, TField>(terms);
        }
        public static Polynomial<T, TField> operator /(Polynomial<T, TField> p, int n)
        {
            return Divide(p, n);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            if (object.ReferenceEquals(p1, p2))
                return true;
            if ((object)p1 == null)
                return false;

            return p1.Equals(p2);
        }
        public static bool operator !=(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            if (object.ReferenceEquals(p1, p2))
                return false;
            if ((object)p1 == null)
                return true;

            return !p1.Equals(p2);
        }

        public static bool operator ==(Polynomial<T, TField> p, T t)
        {
            if ((object)p == null)
                return false;

            return p.Equals(t);
        }
        public static bool operator !=(Polynomial<T, TField> p, T t)
        {
            if ((object)p == null)
                return true;

            return !p.Equals(t);
        }

        #endregion

        #region Cast Operators

        public static implicit operator Polynomial<T, TField>(T t)
        {
            if (!(new TField()).IsZero(t))
                return new Polynomial<T, TField>(0, t);
            else
                return Polynomial<T, TField>.Zero;
        }

        public static implicit operator Polynomial<T, TField>(Term<T> t)
        {
            if (!(new TField()).IsZero(t.Coeff))
                return new Polynomial<T, TField>(t.Deg, t.Coeff);
            else
                return Polynomial<T, TField>.Zero;
        }

        #endregion

        #region Conversion

        public static Polynomial<T, TField> FromCoefficientList(params T[] list)
        {
            List<Term<T>> terms = new List<Term<T>>(list.Length);

            for (int i = 0; i < list.Length; i++)
            {
                terms.Add(new Term<T>(list.Length - i - 1, list[i]));
            }

            Simplify(terms);

            return new Polynomial<T, TField>(terms);
        }

        public static Polynomial<T, TField> FromPolynomial<UField>(Polynomial<T, UField> p) where UField : IField<T>, new()
        {
            return new Polynomial<T, TField>(p.CloneArray());
        }

        public static Polynomial<T, TField> FromPolynomial<U, UField>(Polynomial<U, UField> p, Func<U, T> f) where U : IEquatable<U> where UField : IField<U>, new()
        {
            return new Polynomial<T, TField>(p.Select(term => new Term<T>(term.Deg, f(term.Coeff))));
        }

        #endregion

        #region Functions

        #region Basic

        public static Polynomial<T, TField> Gcd(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            Polynomial<T, TField> t;

            while (p2 != p2.Field.Zero)
            {
                t = p2;
                p2 = (p1 % p2).MakeMonic();
                p1 = t;
            }

            return p1;
        }

        public static Polynomial<T, TField> Interpolate(T[] inputs, T[] outputs)
        {
            // Newton interpolation polynomial.

            IField<T> field = (new TField()).Instance;
            T[][] levels;
            Polynomial<T, TField> p = new Polynomial<T, TField>();

            if (inputs.Length != outputs.Length)
                throw new ArgumentException("The number of inputs must be equal to the number of outputs.");

            if (inputs.Length == 0)
                return Polynomial<T, TField>.Zero;
            else if (inputs.Length == 1)
                return outputs[0];

            levels = new T[inputs.Length][];
            levels[0] = outputs;

            for (int i = 1; i < inputs.Length; i++)
            {
                T[] level = new T[inputs.Length - i];

                for (int j = 0; j < inputs.Length - i; j++)
                {
                    level[j] = field.Divide(field.Subtract(levels[i - 1][j + 1], levels[i - 1][j]), field.Subtract(inputs[i + j], inputs[j]));
                }

                levels[i] = level;
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                Polynomial<T, TField> tp = levels[i][0];

                for (int j = 0; j < i; j++)
                    tp *= new Polynomial<T, TField>(new Term<T>[] { new Term<T>(1, field.One), new Term<T>(0, field.Negate(inputs[j])) });

                p += tp;
            }

            return p;
        }

        public static Polynomial<T, TField> Lcm(Polynomial<T, TField> p1, Polynomial<T, TField> p2)
        {
            return (p1 / Gcd(p1, p2)) * p2;
        }

        #endregion

        #region Generators

        public static Polynomial<T, TField> ChebyshevT(int n, Polynomial<T, TField> x)
        {
            if (n == 0)
                return Polynomial<T, TField>.One;
            else if (n == 1)
                return x;
            else
                return (ChebyshevT(n - 1, x) * x * 2) - ChebyshevT(n - 2, x);
        }

        public static Polynomial<T, TField> ChebyshevU(int n, Polynomial<T, TField> x)
        {
            if (n == 0)
                return Polynomial<T, TField>.One;
            else if (n == 1)
                return x * 2;
            else
                return (ChebyshevU(n - 1, x) * x * 2) - ChebyshevU(n - 2, x);
        }

        public static Polynomial<T, TField> Fibonacci(int n, Polynomial<T, TField> x)
        {
            if (n == 1)
                return Polynomial<T, TField>.One;
            else if (n == 2)
                return x;
            else if (n > 2)
                return Fibonacci(n - 1, x) * x + Fibonacci(n - 2, x);
            else
                throw new ArgumentException("n");
        }

        public static Polynomial<T, TField> HermitePhys(int n, Polynomial<T, TField> x)
        {
            if (n == 0)
                return Polynomial<T, TField>.One;
            else
                return
                    (HermitePhys(n - 1, x) * x * 2) -
                    HermitePhys(n - 1, x).Differentiate();
        }

        public static Polynomial<T, TField> HermiteProb(int n, Polynomial<T, TField> x)
        {
            if (n == 0)
                return Polynomial<T, TField>.One;
            else
                return (HermiteProb(n - 1, x) * x) - HermiteProb(n - 1, x).Differentiate();
        }

        public static Polynomial<T, TField> Laguerre(int n, int a, Polynomial<T, TField> x)
        {
            if (n == 0)
            {
                return Polynomial<T, TField>.One;
            }
            else if (n == 1)
            {
                return -x + x.Field.Multiply(x.Field.One, a + 1);
            }
            else
            {
                Polynomial<T, TField> p1 = Laguerre(n - 1, a, x) * (n + a);
                Polynomial<T, TField> p2 = Laguerre(n - 1, a + 1, x) * x;

                return (p1 - p2) / n;
            }
        }

        public static Polynomial<T, TField> Legendre(int n, Polynomial<T, TField> x)
        {
            if (n == 0)
            {
                return Polynomial<T, TField>.One;
            }
            else if (n == 1)
            {
                return x;
            }
            else
            {
                return
                    ((Legendre(n - 1, x) * x * (2 * n - 1)) -
                    (Legendre(n - 2, x) * (n - 1))) / n;
            }
        }

        public static Polynomial<T, TField> Spread(int n, Polynomial<T, TField> x)
        {
            return (x.Field.One - ChebyshevT(n, x.Field.One - x * 2)) / 2;
        }

        #endregion

        #endregion

        private IField<T> _field;
        private Term<T>[] _terms;
        private int _degree;

        internal Polynomial(Term<T>[] terms)
        {
            _terms = terms;
            this.Initialize();
        }

        internal Polynomial(List<Term<T>> terms)
        {
            _terms = terms.ToArray();
            this.Initialize();
        }

        internal Polynomial(int deg, T coeff)
        {
            _terms = new Term<T>[1];
            _terms[0] = new Term<T>(deg, coeff);
            this.Initialize();
        }

        public Polynomial()
        {
            _terms = new Term<T>[0];
            this.Initialize();
        }

        public Polynomial(IEnumerable<Term<T>> terms)
        {
            List<Term<T>> list = new List<Term<T>>(terms);

            Simplify(list);
            _terms = list.ToArray();

            this.Initialize();
        }

        public Polynomial(params T[] coefficients)
        {
            Polynomial<T, TField> p;

            p = FromCoefficientList(coefficients);
            _terms = p.Terms;
            _degree = p.Degree;
        }

        public Term<T> this[int index]
        {
            get { return _terms[index]; }
        }

        public IField<T> Field
        {
            get { return _field; }
        }

        public int Degree
        {
            get { return _degree; }
        }

        internal Term<T>[] Terms
        {
            get { return _terms; }
        }

        private Term<T>[] CloneArray()
        {
            Term<T>[] array = new Term<T>[_terms.Length];

            Array.Copy(_terms, array, _terms.Length);

            return array;
        }

        private List<Term<T>> CloneList()
        {
            return new List<Term<T>>(_terms);
        }

        public T CoefficientAt(int deg)
        {
            foreach (Term<T> t in _terms)
            {
                if (t.Deg == deg)
                    return t.Coeff;
            }

            return _field.Zero;
        }

        public bool Equals(Polynomial<T, TField> p)
        {
            if ((object)p == null)
                return false;
            if (this.Degree != p.Degree)
                return false;

            T[] list1 = this.ToCoefficientList();
            T[] list2 = p.ToCoefficientList();

            for (int i = 0; i < list1.Length; i++)
            {
                if (!list1[i].Equals(list2[i]))
                    return false;
            }

            return true;
        }

        public bool Equals(T t)
        {
            return this.Degree == 0 && (_terms.Length > 0 ? _terms[0].Coeff.Equals(t) : _field.IsZero(t));
        }

        public override bool Equals(object obj)
        {
            if (obj is Polynomial<T, TField>)
                return this.Equals((Polynomial<T, TField>)obj);
            else
                return false;
        }

        public T Evaluate(T x)
        {
            T y = _field.Zero;

            foreach (Term<T> t in _terms)
            {
                y = _field.Add(y, _field.Multiply(t.Coeff, _field.Pow(x, t.Deg)));
            }

            return y;
        }

        public T[] Evaluate(T[] x)
        {
            T[] y = new T[x.Length];

            for (int i = 0; i < x.Length; i++)
                y[i] = this.Evaluate(x[i]);

            return y;
        }

        public Polynomial<T, TField> Evaluate(Polynomial<T, TField> x)
        {
            Polynomial<T, TField> y = Polynomial<T, TField>.Zero;

            foreach (Term<T> t in _terms)
            {
                y += t.Coeff * x.Pow(t.Deg);
            }

            return y;
        }

        public IEnumerator<Term<T>> GetEnumerator()
        {
            foreach (Term<T> t in _terms)
                yield return t;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _terms.GetEnumerator();
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            foreach (Term<T> t in _terms)
                hashCode ^= t.GetHashCode();

            return hashCode;
        }

        private void Initialize()
        {
            int degree = 0;

            foreach (Term<T> t in _terms)
            {
                if (t.Deg > degree)
                    degree = t.Deg;
            }

            _degree = degree;
            _field = (new TField()).Instance;
        }

        public T LeadingCoefficient()
        {
            return this.CoefficientAt(_degree);
        }

        public T[] ToCoefficientList()
        {
            T[] list = new T[_degree + 1];

            foreach (Term<T> t in _terms)
                list[_degree - t.Deg] = t.Coeff;

            return list;
        }

        private T[] ToCoefficientListForDivisor(out T leadingCoeff)
        {
            T[] list = new T[_degree - 1 + 1];
            T lc = _field.Zero;

            foreach (Term<T> t in _terms)
            {
                if (t.Deg < _degree)
                    list[_degree - t.Deg - 1] = _field.Negate(t.Coeff);
                else if (t.Deg == _degree)
                    lc = t.Coeff;
            }

            for (int i = 0; i < list.Length; i++)
                list[i] = _field.Divide(list[i], lc);

            leadingCoeff = lc;

            return list;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            List<Term<T>> terms = new List<Term<T>>(_terms);

            terms.Sort((t1, t2) => -t1.Deg.CompareTo(t2.Deg));

            foreach (Term<T> t in terms)
            {
                string coeffPart;
                string varPart;

                if (typeof(T) == typeof(Complex))
                {
                    Complex coeff;

                    coeff = Complex.RoundToEpsilon((Complex)(object)t.Coeff);

                    if (coeff.Im != 0)
                    {
                        coeffPart = "+(" + coeff.ToString() + ")";
                    }
                    else if (coeff.Re == 1 && t.Deg != 0)
                    {
                        coeffPart = "+";
                    }
                    else if (coeff.Re == -1 && t.Deg != 0)
                    {
                        coeffPart = "-";
                    }
                    else if (coeff.Re != 0)
                    {
                        if (coeff.Re >= 0)
                            coeffPart = "+" + coeff.Re.ToString();
                        else if (coeff.Re < 0)
                            coeffPart = "-" + (-coeff.Re).ToString();
                        else
                            coeffPart = "";
                    }
                    else
                    {
                        // Coefficient is zero. Shouldn't happen, but
                        // since it did, we skip the term.
                        continue;
                    }
                }
                else
                {
                    coeffPart = t.Coeff.ToString();

                    if (!coeffPart.StartsWith("+") && !coeffPart.StartsWith("-"))
                        coeffPart = "+" + coeffPart;
                }

                if (t.Deg == 0)
                    varPart = "";
                else if (t.Deg == 1)
                    varPart = "x";
                else
                    varPart = "x^" + t.Deg.ToString();

                sb.Append(coeffPart + varPart);
            }

            if (sb.Length == 0)
                return "0";

            if (sb.Length > 0 && sb[0] == '+')
                sb.Remove(0, 1);

            return sb.ToString();
        }

        #region Functions

        #region Basic

        public Polynomial<T, TField> Differentiate()
        {
            List<Term<T>> terms = new List<Term<T>>();

            foreach (Term<T> t in _terms)
            {
                if (t.Deg > 0)
                    Add(terms, new Term<T>(t.Deg - 1, _field.Multiply(t.Coeff, t.Deg)));
            }

            return new Polynomial<T, TField>(terms);
        }

        public Polynomial<T, TField> Integrate()
        {
            List<Term<T>> terms = new List<Term<T>>();

            foreach (Term<T> t in _terms)
            {
                Add(terms, new Term<T>(t.Deg + 1, _field.Divide(t.Coeff, t.Deg + 1)));
            }

            return new Polynomial<T, TField>(terms);
        }

        public Polynomial<T, TField> MakeMonic()
        {
            T leadingCoeff;

            return this.MakeMonic(out leadingCoeff);
        }

        public Polynomial<T, TField> MakeMonic(out T leadingCoeff)
        {
            T c;

            c = this.LeadingCoefficient();
            leadingCoeff = c;

            return this / c;
        }

        public Polynomial<T, TField> Pow(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException("n");

            Polynomial<T, TField> b = this;
            Polynomial<T, TField> result = Polynomial<T, TField>.One;

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

        public Polynomial<T, TField> RemoveMultipleRoots()
        {
            Polynomial<T, TField> multiplePart;

            return this.RemoveMultipleRoots(out multiplePart);
        }

        public Polynomial<T, TField> RemoveMultipleRoots(out Polynomial<T, TField> multiplePart)
        {
            Polynomial<T, TField> gcd = Gcd(this, this.Differentiate());

            multiplePart = gcd;

            return this / gcd;
        }

        #endregion

        #endregion
    }
}
