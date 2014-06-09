using System;
using System.Collections.Generic;
using System.Linq;
using BigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Wj.Math
{
    public static class Discrete
    {
        #region Factorial, Binomial Coefficients and Related Numbers

        private static readonly ulong[] _factorials = new ulong[]
        {
            1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600,
            6227020800, 87178291200, 1307674368000
        };

        public static BigInteger AlternateSign(BigInteger n, BigInteger k)
        {
            if (k.IsEven)
                return n;
            else
                return -n;
        }

        public static BigInteger AlternateSign(BigInteger n, int k)
        {
            if (k % 2 == 0)
                return n;
            else
                return -n;
        }

        public static Rational AlternateSign(Rational q, BigInteger k)
        {
            if (k.IsEven)
                return q;
            else
                return -q;
        }

        public static Rational FallingFactorial(Rational q, BigInteger n)
        {
            Rational result = Rational.One;

            if (n > 0)
            {
                do
                {
                    result *= q;
                    q--;
                } while (--n != 0);
            }
            else
            {
                while (n != 0)
                {
                    q++;
                    result /= q;
                    n++;
                }
            }

            return result;
        }

        public static Rational RisingFactorial(Rational q, BigInteger n)
        {
            Rational result = Rational.One;

            if (n > 0)
            {
                do
                {
                    result *= q;
                    q++;
                } while (--n != 0);
            }
            else
            {
                while (n != 0)
                {
                    q--;
                    result /= q;
                    n++;
                }
            }

            return result;
        }

        public static BigInteger Factorial(BigInteger n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException("n");
            if (n > long.MaxValue)
                throw new OverflowException();

            long i = (long)n;

            if (i < _factorials.LongLength)
                return _factorials[i];

            BigInteger product;

            product = _factorials[_factorials.Length - 1];

            for (long j = _factorials.LongLength; j <= i; j++)
                product *= j;

            return product;
        }

        public static Rational Binomial(Rational n, BigInteger k)
        {
            // As defined in Kronenburg (2011).

            if (k >= 0)
            {
                return FallingFactorial(n, k) / Factorial(k);
            }
            else
            {
                if (n >= 0)
                    return 0;
                else if (k <= n && n.IsInteger)
                    return AlternateSign(Binomial(-k - 1, n.Top - k), n.Top - k);
                else
                    return 0;
            }
        }

        public static BigInteger Multinomial(params BigInteger[] n)
        {
            BigInteger sum = 0;

            for (int i = 0; i < n.Length; i++)
                sum += n[i];

            sum = Factorial(sum);

            for (int i = 0; i < n.Length; i++)
                sum /= Factorial(n[i]);

            return sum;
        }

        public static BigInteger Subfactorial(BigInteger n)
        {
            BigInteger sum = 0;
            BigInteger term = 1;
            int counter = n.IsEven ? 0 : -1;

            for (BigInteger i = n; !i.IsZero; i--)
            {
                sum += AlternateSign(term, counter);
                term *= i;
                counter++;
            }

            return sum + term;
        }

        public static BigInteger Catalan(BigInteger n)
        {
            return (FallingFactorial(2 * n, n) / Factorial(n + 1)).Top;
        }

        #endregion

        #region Stirling Numbers, Eulerian Numbers and Related Numbers

        public static BigInteger Eulerian(BigInteger n, BigInteger k)
        {
            if (n > int.MaxValue)
                throw new OverflowException();
            if (k > n)
                return 0;

            int smallN = (int)n;
            BigInteger sum = 0;

            for (BigInteger j = 0; j <= k; j++)
                sum += AlternateSign(Binomial(n + 1, j).Top * BigInteger.Pow(k + 1 - j, smallN), j);

            return sum;
        }

        public static BigInteger Eulerian2(BigInteger n, BigInteger k)
        {
            return EvaluateRecurrence<Tuple<BigInteger, BigInteger>, BigInteger>(new Tuple<BigInteger, BigInteger>(n, k), null, (key, fetch) =>
                {
                    if (key.Item2 >= key.Item1)
                        return new Tuple<BigInteger, bool>(0, false);
                    if (key.Item2 == 0)
                        return new Tuple<BigInteger, bool>(1, false);

                    return new Tuple<BigInteger, bool>(
                        (key.Item2 + 1) * fetch(new Tuple<BigInteger, BigInteger>(key.Item1 - 1, key.Item2)) +
                        (2 * key.Item1 - key.Item2 - 1) * fetch(new Tuple<BigInteger, BigInteger>(key.Item1 - 1, key.Item2 - 1)),
                        true);
                });
        }

        /// <summary>
        /// Unsigned Stirling number of the first kind [n k].
        /// </summary>
        public static BigInteger Stirling1(BigInteger n, BigInteger k)
        {
            if (n.Sign < 0 && k.Sign < 0)
                return Stirling2(-k, -n);

            return EvaluateRecurrence<Tuple<BigInteger, BigInteger>, BigInteger>(new Tuple<BigInteger, BigInteger>(n, k), null, (key, fetch) =>
            {
                if (key.Item2 > key.Item1)
                    return new Tuple<BigInteger, bool>(0, false);
                if (key.Item2 == 0)
                    return new Tuple<BigInteger, bool>(key.Item1.IsZero ? 1 : 0, false);

                return new Tuple<BigInteger, bool>(
                    (key.Item1 - 1) * fetch(new Tuple<BigInteger, BigInteger>(key.Item1 - 1, key.Item2)) +
                    fetch(new Tuple<BigInteger, BigInteger>(key.Item1 - 1, key.Item2 - 1)),
                    true);
            });
        }

        /// <summary>
        /// Number of surjective functions from N to K where |N|=n and |K|=k, k!{n k}.
        /// </summary>
        public static BigInteger Stirling2KF(BigInteger n, BigInteger k)
        {
            if (n > int.MaxValue)
                throw new OverflowException();
            if (n < 0 || k < 0)
                return 0;
            if (k > n)
                return 0;

            int smallN = (int)n;
            BigInteger sum = 0;

            for (BigInteger j = 0; j <= k; j++)
                sum += AlternateSign(Binomial(k, j).Top * BigInteger.Pow(j, smallN), k - j);

            return sum;
        }

        /// <summary>
        /// Stirling number of the second kind {n k}.
        /// </summary>
        public static BigInteger Stirling2(BigInteger n, BigInteger k)
        {
            if (n.Sign < 0 && k.Sign < 0)
                return Stirling1(-k, -n);

            return Stirling2KF(n, k) / Factorial(k);
        }

        public static BigInteger Bell(BigInteger n)
        {
            BigInteger sum = 0;

            for (BigInteger k = 0; k <= n; k++)
                sum += Stirling2(n, k);

            return sum;
        }

        #endregion

        #region Bernoulli Numbers and Euler Numbers

        public static Rational Bernoulli(BigInteger m)
        {
            Rational sum = 0;

            for (BigInteger k = 0; k <= m; k++)
            {
                sum += AlternateSign(new Rational(Stirling2KF(m, k), k + 1), k);
            }

            return sum;
        }

        public static Polynomial<Rational, RationalField> Bernoulli(int n, Polynomial<Rational, RationalField> x)
        {
            Polynomial<Rational, RationalField> p = Polynomial<Rational, RationalField>.Zero;

            for (int k = 0; k <= n; k++)
                p += Discrete.Binomial(n, k) * Discrete.Bernoulli(k) * x.Pow(n - k);

            return p;
        }

        public static Polynomial<Rational, RationalField> SumOfPthPowers(int p, Polynomial<Rational, RationalField> x)
        {
            return (Bernoulli(p + 1, x + x.Field.One) - Bernoulli(p + 1, Polynomial<Rational, RationalField>.Zero)) / (p + 1);
        }

        public static BigInteger Euler(int n)
        {
            if (n % 2 != 0)
                return 0;

            n /= 2;

            Rational[,] m = new Rational[n, n];
            BigInteger bottom = 1;
            BigInteger counter = 0;

            for (int i = 0; i <= n; i++)
            {
                Rational entry = new Rational(1, bottom);

                for (int j = 0; j < n; j++)
                {
                    int x = i + j - 1;
                    int y = j;

                    if (x >= 0 && x < n && y >= 0 && y < n)
                        m[x, y] = entry;
                }

                if (i != n)
                {
                    counter++;
                    bottom *= counter;
                    counter++;
                    bottom *= counter;
                }
            }

            return AlternateSign((bottom * (new Matrix<Rational, RationalSpace>(m)).Determinant()).Top, n);
        }

        #endregion

        #region Fibonacci Numbers and Lucas Numbers

        private static Matrix<BigInteger, IntegerSpace> FibonacciMatrix(int n)
        {
            Matrix<BigInteger, IntegerSpace> m = new Matrix<BigInteger, IntegerSpace>(new BigInteger[,] { { 1, 1 }, { 1, 0 } });

            return m.Pow(n);
        }

        public static BigInteger Fibonacci(int n)
        {
            if (n < 0)
                return AlternateSign(Fibonacci(-n), n + 1);

            return FibonacciMatrix(n)[0, 1];
        }

        public static BigInteger Lucas(int n)
        {
            if (n < 0)
                return AlternateSign(Lucas(-n), n);

            var matrix = FibonacciMatrix(n);

            return matrix[0, 1] + 2 * matrix[1, 1];
        }

        #endregion

        #region Harmonic Numbers

        public static Rational Harmonic(BigInteger n)
        {
            Rational sum = 0;

            for (BigInteger k = 1; k <= n; k++)
                sum += Rational.Inv(k);

            return sum;
        }

        public static Rational Harmonic(BigInteger n, int m)
        {
            Rational sum = 0;

            for (BigInteger k = 1; k <= n; k++)
                sum += Rational.Inv(Rational.Pow(k, m));

            return sum;
        }

        #endregion

        #region Power Series

        public static Polynomial<Rational, RationalField> PowerSeries(int n0, int n, Polynomial<Rational, RationalField> x, Func<int, Rational> coefficient)
        {
            if (x.Terms.Length == 1)
            {
                List<Term<Rational>> terms = new List<Term<Rational>>();
                Term<Rational> leadingTerm = x.Terms[0];

                for (int k = n0; k <= n; k++)
                {
                    Term<Rational> term = new Term<Rational>(x.Degree * k, coefficient(k) * Rational.Pow(leadingTerm.Coeff, k));

                    Polynomial<Rational, RationalField>.Add(terms, term);
                }

                return new Polynomial<Rational, RationalField>(terms);
            }
            else
            {
                Polynomial<Rational, RationalField> result = Polynomial<Rational, RationalField>.Zero;

                for (int k = n; k >= n0; k--)
                {
                    result *= x;
                    result += coefficient(k);
                }

                return result;
            }
        }

        /// <summary>
        /// Returns a power series for exp(x).
        /// </summary>
        public static Polynomial<Rational, RationalField> Exp(int n, Polynomial<Rational, RationalField> x)
        {
            return PowerSeries(0, n, x, k => new Rational(1, Factorial(k)));
        }

        /// <summary>
        /// Returns a power series for (1+x)^q.
        /// </summary>
        public static Polynomial<Rational, RationalField> BinomialPower(int n, Polynomial<Rational, RationalField> x, Rational q)
        {
            return PowerSeries(0, n, x, k => Binomial(q, k));
        }

        /// <summary>
        /// Returns a power series for log(1-x).
        /// </summary>
        public static Polynomial<Rational, RationalField> Log(int n, Polynomial<Rational, RationalField> x)
        {
            return -PowerSeries(1, n, x, k => Rational.Inv(k));
        }

        public static Polynomial<Rational, RationalField> Sin(int n, Polynomial<Rational, RationalField> x)
        {
            return PowerSeries(1, n, x, k => (k % 2 != 0) ? AlternateSign(new Rational(1, Factorial(k)), (k - 1) / 2) : 0);
        }

        public static Polynomial<Rational, RationalField> Cos(int n, Polynomial<Rational, RationalField> x)
        {
            return PowerSeries(0, n, x, k => (k % 2 == 0) ? AlternateSign(new Rational(1, Factorial(k)), k / 2) : 0);
        }

        public static Polynomial<Rational, RationalField> Tan(int n, Polynomial<Rational, RationalField> x)
        {
            return PowerSeries(1, n, x, delegate(int k)
            {
                if (k % 2 == 0)
                    return 0;

                int j = (k + 1) / 2;
                BigInteger fourPower = BigInteger.Pow(4, j);

                return AlternateSign(Bernoulli(2 * j) * fourPower * (1 - fourPower), j) / Factorial(2 * j);
            });
        }

        #endregion

        #region Recurrences

        public static TValue EvaluateRecurrence<TKey, TValue>(TKey target, IDictionary<TKey, TValue> initial, Func<TKey, Func<TKey, TValue>, Tuple<TValue, bool>> recurrence)
        {
            Dictionary<TKey, TValue> cache;
            Func<TKey, TValue> fetch = null;

            if (initial != null)
                cache = new Dictionary<TKey, TValue>(initial);
            else
                cache = new Dictionary<TKey, TValue>();

            fetch = key =>
            {
                TValue value;
                Tuple<TValue, bool> result;

                if (cache.TryGetValue(key, out value))
                    return value;

                result = recurrence(key, fetch);

                if (result.Item2)
                    cache.Add(key, result.Item1);

                return result.Item1;
            };

            return fetch(target);
        }

        #endregion

        #region Functions

        public static bool IsInjective<T, U>(FiniteSet<T> domain, FiniteSet<U> codomain, Func<T, U> map)
            where T : IEquatable<T>
            where U : IEquatable<U>
        {
            if (domain.Size > codomain.Size)
                return false;

            HashSet<U> range = new HashSet<U>();

            foreach (T x in domain)
            {
                U y = map(x);

                if (range.Contains(y))
                    return false;

                range.Add(y);
            }

            return true;
        }

        public static bool IsSurjective<T, U>(FiniteSet<T> domain, FiniteSet<U> codomain, Func<T, U> map)
            where T : IEquatable<T>
            where U : IEquatable<U>
        {
            if (domain.Size < codomain.Size)
                return false;

            HashSet<U> range = new HashSet<U>();

            foreach (T element in domain)
                range.Add(map(element));

            return range.Count == codomain.Size;
        }

        public static bool IsBijective<T, U>(FiniteSet<T> domain, FiniteSet<U> codomain, Func<T, U> map)
            where T : IEquatable<T>
            where U : IEquatable<U>
        {
            if (domain.Size != codomain.Size)
                return false;

            return IsSurjective(domain, codomain, map);
        }

        public static Func<U, T> Inverse<T, U>(FiniteSet<T> domain, Func<T, U> bijection) where T : IEquatable<T>
        {
            Dictionary<U, T> inverseMap = new Dictionary<U, T>();

            foreach (T element in domain)
                inverseMap.Add(bijection(element), element);

            return u => inverseMap[u];
        }

        #endregion

        #region Partitions

        private static BigInteger Partition(int n, int parts, bool allParts)
        {
            // P(k,n) = sum_{j=0}^{k} P(k-n,n-j).
            return EvaluateRecurrence<Tuple<int, int, bool>, BigInteger>(new Tuple<int, int, bool>(n, parts, allParts), null, (key, fetch) =>
                {
                    if (key.Item1 < 0 || key.Item2 < 0)
                        return new Tuple<BigInteger, bool>(0, false);

                    if (key.Item3)
                    {
                        if (key.Item1 == 0)
                            return new Tuple<BigInteger, bool>(1, false);

                        BigInteger sum = 0;

                        for (int i = 1; i <= key.Item1; i++)
                            sum += fetch(new Tuple<int, int, bool>(key.Item1, i, false));

                        return new Tuple<BigInteger, bool>(sum, false);
                    }
                    else
                    {
                        if (key.Item1 + key.Item2 == 0)
                            return new Tuple<BigInteger, bool>(1, false);
                        if (key.Item2 == 0 || key.Item2 > key.Item1)
                            return new Tuple<BigInteger, bool>(0, false);
                        if (key.Item2 == 1 || key.Item1 - key.Item2 <= 1)
                            return new Tuple<BigInteger, bool>(1, false);
                        if (key.Item1 == key.Item2 + 2)
                            return new Tuple<BigInteger, bool>(2, false);

                        BigInteger sum = 0;

                        for (int j = 0; j <= key.Item1; j++)
                            sum += fetch(new Tuple<int, int, bool>(key.Item1 - key.Item2, key.Item2 - j, false));

                        return new Tuple<BigInteger, bool>(sum, true);
                    }
                });
        }

        public static BigInteger Partition(int n)
        {
            // P(n) = sum_{k=1}{n} (-1)^(k+1) (P(n-k(3k-1)/2)+P(n-k(3k+1)/2))
            return EvaluateRecurrence<int, BigInteger>(n, null, (key, fetch) =>
                {
                    if (key == 0)
                        return new Tuple<BigInteger, bool>(1, true);
                    if (key == 1)
                        return new Tuple<BigInteger, bool>(1, true);

                    BigInteger sum = 0;

                    for (int k = 1; k <= key; k++)
                    {
                        sum += AlternateSign(fetch(key - k * (3 * k - 1) / 2) + fetch(key - k * (3 * k + 1) / 2), k + 1);
                    }

                    return new Tuple<BigInteger, bool>(sum, true);
                });
        }

        public static BigInteger Partition(int n, int parts)
        {
            return Partition(n, parts, false);
        }

        public static IEnumerable<ArraySegment<int>> Partitions(int n, int atMostParts, int[] buffer)
        {
            // From http://jeromekelleher.net/partitions.php

            if (buffer.Length < System.Math.Min(n, atMostParts) + 1)
                throw new ArgumentException("buffer");

            int[] a = buffer;
            int k = 1;
            int y = n - 1;
            int x;
            int j;

            if (n == 0)
            {
                yield return new ArraySegment<int>(a, 0, 0);
                yield break;
            }
            else if (atMostParts == 1)
            {
                a[0] = n;
                yield return new ArraySegment<int>(a, 0, 1);
                yield break;
            }

            while (k != 0)
            {
                x = a[k - 1] + 1;
                k--;

                while (x * 2 <= y && k + 2 < atMostParts)
                {
                    a[k] = x;
                    y -= x;
                    k++;
                }

                j = k + 1;

                while (x <= y)
                {
                    a[k] = x;
                    a[j] = y;
                    yield return new ArraySegment<int>(a, 0, k + 2);
                    x++;
                    y--;
                }

                a[k] = x + y;
                y = x + y - 1;

                yield return new ArraySegment<int>(a, 0, k + 1);
            }
        }

        public static IEnumerable<int[]> Partitions(int n, int atMostParts)
        {
            int[] buffer = new int[System.Math.Min(n, atMostParts) + 1];

            return Partitions(n, atMostParts, buffer).Select(segment =>
            {
                int[] result = new int[segment.Count];
                Array.Copy(segment.Array, segment.Offset, result, 0, segment.Count);
                return result;
            });
        }

        public static IEnumerable<int[]> Partitions(int n)
        {
            return Partitions(n, int.MaxValue);
        }

        #endregion

        #region Primes

        #region First 1000 Primes

        private static readonly int[] _primes = new int[]
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89,
            97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191,
            193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283,
            293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401,
            409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509,
            521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631,
            641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751,
            757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877,
            881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997, 1009,
            1013, 1019, 1021, 1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091, 1093, 1097,
            1103, 1109, 1117, 1123, 1129, 1151, 1153, 1163, 1171, 1181, 1187, 1193, 1201, 1213, 1217,
            1223, 1229, 1231, 1237, 1249, 1259, 1277, 1279, 1283, 1289, 1291, 1297, 1301, 1303, 1307,
            1319, 1321, 1327, 1361, 1367, 1373, 1381, 1399, 1409, 1423, 1427, 1429, 1433, 1439, 1447,
            1451, 1453, 1459, 1471, 1481, 1483, 1487, 1489, 1493, 1499, 1511, 1523, 1531, 1543, 1549,
            1553, 1559, 1567, 1571, 1579, 1583, 1597, 1601, 1607, 1609, 1613, 1619, 1621, 1627, 1637,
            1657, 1663, 1667, 1669, 1693, 1697, 1699, 1709, 1721, 1723, 1733, 1741, 1747, 1753, 1759,
            1777, 1783, 1787, 1789, 1801, 1811, 1823, 1831, 1847, 1861, 1867, 1871, 1873, 1877, 1879,
            1889, 1901, 1907, 1913, 1931, 1933, 1949, 1951, 1973, 1979, 1987, 1993, 1997, 1999, 2003,
            2011, 2017, 2027, 2029, 2039, 2053, 2063, 2069, 2081, 2083, 2087, 2089, 2099, 2111, 2113,
            2129, 2131, 2137, 2141, 2143, 2153, 2161, 2179, 2203, 2207, 2213, 2221, 2237, 2239, 2243,
            2251, 2267, 2269, 2273, 2281, 2287, 2293, 2297, 2309, 2311, 2333, 2339, 2341, 2347, 2351,
            2357, 2371, 2377, 2381, 2383, 2389, 2393, 2399, 2411, 2417, 2423, 2437, 2441, 2447, 2459,
            2467, 2473, 2477, 2503, 2521, 2531, 2539, 2543, 2549, 2551, 2557, 2579, 2591, 2593, 2609,
            2617, 2621, 2633, 2647, 2657, 2659, 2663, 2671, 2677, 2683, 2687, 2689, 2693, 2699, 2707,
            2711, 2713, 2719, 2729, 2731, 2741, 2749, 2753, 2767, 2777, 2789, 2791, 2797, 2801, 2803,
            2819, 2833, 2837, 2843, 2851, 2857, 2861, 2879, 2887, 2897, 2903, 2909, 2917, 2927, 2939,
            2953, 2957, 2963, 2969, 2971, 2999, 3001, 3011, 3019, 3023, 3037, 3041, 3049, 3061, 3067,
            3079, 3083, 3089, 3109, 3119, 3121, 3137, 3163, 3167, 3169, 3181, 3187, 3191, 3203, 3209,
            3217, 3221, 3229, 3251, 3253, 3257, 3259, 3271, 3299, 3301, 3307, 3313, 3319, 3323, 3329,
            3331, 3343, 3347, 3359, 3361, 3371, 3373, 3389, 3391, 3407, 3413, 3433, 3449, 3457, 3461,
            3463, 3467, 3469, 3491, 3499, 3511, 3517, 3527, 3529, 3533, 3539, 3541, 3547, 3557, 3559,
            3571, 3581, 3583, 3593, 3607, 3613, 3617, 3623, 3631, 3637, 3643, 3659, 3671, 3673, 3677,
            3691, 3697, 3701, 3709, 3719, 3727, 3733, 3739, 3761, 3767, 3769, 3779, 3793, 3797, 3803,
            3821, 3823, 3833, 3847, 3851, 3853, 3863, 3877, 3881, 3889, 3907, 3911, 3917, 3919, 3923,
            3929, 3931, 3943, 3947, 3967, 3989, 4001, 4003, 4007, 4013, 4019, 4021, 4027, 4049, 4051,
            4057, 4073, 4079, 4091, 4093, 4099, 4111, 4127, 4129, 4133, 4139, 4153, 4157, 4159, 4177,
            4201, 4211, 4217, 4219, 4229, 4231, 4241, 4243, 4253, 4259, 4261, 4271, 4273, 4283, 4289,
            4297, 4327, 4337, 4339, 4349, 4357, 4363, 4373, 4391, 4397, 4409, 4421, 4423, 4441, 4447,
            4451, 4457, 4463, 4481, 4483, 4493, 4507, 4513, 4517, 4519, 4523, 4547, 4549, 4561, 4567,
            4583, 4591, 4597, 4603, 4621, 4637, 4639, 4643, 4649, 4651, 4657, 4663, 4673, 4679, 4691,
            4703, 4721, 4723, 4729, 4733, 4751, 4759, 4783, 4787, 4789, 4793, 4799, 4801, 4813, 4817,
            4831, 4861, 4871, 4877, 4889, 4903, 4909, 4919, 4931, 4933, 4937, 4943, 4951, 4957, 4967,
            4969, 4973, 4987, 4993, 4999, 5003, 5009, 5011, 5021, 5023, 5039, 5051, 5059, 5077, 5081,
            5087, 5099, 5101, 5107, 5113, 5119, 5147, 5153, 5167, 5171, 5179, 5189, 5197, 5209, 5227,
            5231, 5233, 5237, 5261, 5273, 5279, 5281, 5297, 5303, 5309, 5323, 5333, 5347, 5351, 5381,
            5387, 5393, 5399, 5407, 5413, 5417, 5419, 5431, 5437, 5441, 5443, 5449, 5471, 5477, 5479,
            5483, 5501, 5503, 5507, 5519, 5521, 5527, 5531, 5557, 5563, 5569, 5573, 5581, 5591, 5623,
            5639, 5641, 5647, 5651, 5653, 5657, 5659, 5669, 5683, 5689, 5693, 5701, 5711, 5717, 5737,
            5741, 5743, 5749, 5779, 5783, 5791, 5801, 5807, 5813, 5821, 5827, 5839, 5843, 5849, 5851,
            5857, 5861, 5867, 5869, 5879, 5881, 5897, 5903, 5923, 5927, 5939, 5953, 5981, 5987, 6007,
            6011, 6029, 6037, 6043, 6047, 6053, 6067, 6073, 6079, 6089, 6091, 6101, 6113, 6121, 6131,
            6133, 6143, 6151, 6163, 6173, 6197, 6199, 6203, 6211, 6217, 6221, 6229, 6247, 6257, 6263,
            6269, 6271, 6277, 6287, 6299, 6301, 6311, 6317, 6323, 6329, 6337, 6343, 6353, 6359, 6361,
            6367, 6373, 6379, 6389, 6397, 6421, 6427, 6449, 6451, 6469, 6473, 6481, 6491, 6521, 6529,
            6547, 6551, 6553, 6563, 6569, 6571, 6577, 6581, 6599, 6607, 6619, 6637, 6653, 6659, 6661,
            6673, 6679, 6689, 6691, 6701, 6703, 6709, 6719, 6733, 6737, 6761, 6763, 6779, 6781, 6791,
            6793, 6803, 6823, 6827, 6829, 6833, 6841, 6857, 6863, 6869, 6871, 6883, 6899, 6907, 6911,
            6917, 6947, 6949, 6959, 6961, 6967, 6971, 6977, 6983, 6991, 6997, 7001, 7013, 7019, 7027,
            7039, 7043, 7057, 7069, 7079, 7103, 7109, 7121, 7127, 7129, 7151, 7159, 7177, 7187, 7193,
            7207, 7211, 7213, 7219, 7229, 7237, 7243, 7247, 7253, 7283, 7297, 7307, 7309, 7321, 7331,
            7333, 7349, 7351, 7369, 7393, 7411, 7417, 7433, 7451, 7457, 7459, 7477, 7481, 7487, 7489,
            7499, 7507, 7517, 7523, 7529, 7537, 7541, 7547, 7549, 7559, 7561, 7573, 7577, 7583, 7589,
            7591, 7603, 7607, 7621, 7639, 7643, 7649, 7669, 7673, 7681, 7687, 7691, 7699, 7703, 7717,
            7723, 7727, 7741, 7753, 7757, 7759, 7789, 7793, 7817, 7823, 7829, 7841, 7853, 7867, 7873,
            7877, 7879, 7883, 7901, 7907, 7919
        };

        #endregion

        public static IEnumerable<BigInteger> Factors(BigInteger n)
        {
            BigInteger limit = ApproximateSqrt(n);
            HashSet<BigInteger> seen = new HashSet<BigInteger>();

            for (BigInteger k = 1; k <= limit; k++)
            {
                if (n % k == 0)
                {
                    if (!seen.Contains(k))
                    {
                        yield return k;
                        seen.Add(k);
                    }

                    if (!seen.Contains(n / k))
                    {
                        yield return n / k;
                        seen.Add(n / k);
                    }
                }
            }
        }

        /// <summary>
        /// Returns an integer k close to sqrt(n) such that sqrt(n)&lt;=k.
        /// </summary>
        public static BigInteger ApproximateSqrt(BigInteger n)
        {
            if (n <= uint.MaxValue)
                return (uint)System.Math.Ceiling(System.Math.Sqrt((ulong)n));

            Rational x = new Rational(n, 65535); // sqrt(x) <= x/65535 when x >= 4294836225.

            for (int i = 0; i < 4; i++)
            {
                x = (x + n / x) / 2;
                x = (x.Top + x.Bottom - 1) / x.Bottom; // take ceiling of x
            }

            return x.Top;
        }

        public static bool IsPrime(BigInteger n)
        {
            int largestPrime = _primes[_primes.Length - 1];

            if (n <= largestPrime)
                return Array.BinarySearch(_primes, (int)n) >= 0;

            BigInteger limit = ApproximateSqrt(n);

            for (int i = 0; i < _primes.Length - 1; i++)
            {
                BigInteger prime = _primes[i];

                if (prime > limit)
                    return true;
                if (n % prime == 0)
                    return false;
            }

            for (BigInteger k = largestPrime + 1; k <= limit; k++)
            {
                if (n % k == 0)
                    return false;
            }

            return true;
        }

        public static IEnumerable<BigInteger> Primes(BigInteger limit)
        {
            BigInteger largestPrime = _primes[_primes.Length - 1];

            for (int i = 0; i < _primes.Length; i++)
            {
                if (_primes[i] > limit)
                    break;

                yield return _primes[i];
            }

            for (BigInteger k = largestPrime + 1; k <= limit; k++)
            {
                if (IsPrime(k))
                    yield return k;
            }
        }

        public static IEnumerable<Pair<BigInteger, int>> PrimeFactors(BigInteger n)
        {
            if (n <= 0)
                yield break;

            BigInteger limit = ApproximateSqrt(n);

            foreach (BigInteger prime in Primes(limit))
            {
                int count = 0;

                while (n % prime == 0)
                {
                    n /= prime;
                    count++;
                }

                if (count != 0)
                {
                    yield return new Pair<BigInteger, int>(prime, count);
                }

                if (n == 1)
                    break;
            }

            if (n != 1)
                yield return new Pair<BigInteger, int>(n, 1);
        }

        /// <summary>
        /// Returns phi(n), the totient function.
        /// </summary>
        public static BigInteger Totient(BigInteger n)
        {
            BigInteger result = 1;

            if (n <= 0)
                return 0;

            foreach (var factor in PrimeFactors(n))
                result *= BigInteger.Pow(factor.First, factor.Second - 1) * (factor.First - 1);

            return result;
        }

        #endregion
    }
}
