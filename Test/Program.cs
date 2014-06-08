using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wj.Math;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //DFTForm form = new DFTForm();

            //System.Windows.Forms.Application.EnableVisualStyles();
            //System.Windows.Forms.Application.Run(form);

            TestSeries();
        }

        static void Stuff()
        {
            var mySet = new FiniteSet<int>(0, 1, 2, 3, 4, 5);
            var myOtherSet = new FiniteSet<string>("asdf", "123", "b", "z", "94", "0");
            var myOtherSetList = myOtherSet.ToList();

            var p = Permutation<int>.FromMappings(mySet, new Dictionary<int, int> { { 0, 4 }, { 1, 2 }, { 2, 0 }, { 3, 5 }, { 4, 1 }, { 5, 3 } });

            var permutations = Permutation<int>.SetPermutations(mySet).ToArray();

            var evenPermutations = (from a in permutations where a.Sign == 1 select a).ToArray();
            var onePermutations = (from a in permutations where a.Count == 1 select a).ToArray();

            var p2 = Permutation<string>.FromPermutation(myOtherSet, p, i => myOtherSetList[i]);

            var myGroup = new Group<int>(mySet, (a, b) => (a + b) % 6, null);
            var mySubgroup = myGroup.Subgroup(new FiniteSet<int>(0, 3));
            Func<int, Permutation<int>> outerIsomorphism;
            var myGroupP = myGroup.ToPermutationGroup(out outerIsomorphism);
            var mySubgroupP = Group<Permutation<int>>.FromGroup(mySubgroup, outerIsomorphism);

            var s3 = Group.SymmetricGroup(3);
            var s4 = Group.SymmetricGroup(4);
            var s6 = Group.SymmetricGroup(6);

            var graphEdgeList = new List<UnorderedPair<int>>();

            for (int i = 1; i <= 4; i++)
            {
                for (int j = i + 1; j <= 4; j++)
                    graphEdgeList.Add(new UnorderedPair<int>(i, j));
            }

            var graphEdges = new FiniteSet<UnorderedPair<int>>(graphEdgeList);
            Func<Permutation<int>, UnorderedPair<int>, UnorderedPair<int>> graphEdgeAction = (g, e) => new UnorderedPair<int>(g[e.First], g[e.Second]);

            var actionGroup = s4.ActionGroup(graphEdgeAction, graphEdges);
            var cycleIndex = Group.CycleIndex(actionGroup);
            Func<int, Polynomial<Rational, RationalField>> xFunction = k => Polynomial<Rational, RationalField>.X.Pow(k) + (Rational)1;
            var cycleIndexAnswer = cycleIndex.Evaluate(xFunction);

            SomeExample2();
            SomeExample();
            TestMatrix();
        }

        static Rational AverageLength<T>(Dictionary<T, Vector<int>> code, IEnumerable<Pair<T, Rational>> sourceSymbols) where T : IEquatable<T>
        {
            Rational sum = 0;

            foreach (var pair in sourceSymbols)
            {
                sum += code[pair.First].Length * pair.Second;
            }

            return sum;
        }

        static void SomeExample2()
        {

            var radix4Symbols = new Pair<int, Rational>[]
            {
                new Pair<int, Rational>(1, new Rational(22, 100)),
                new Pair<int, Rational>(2, new Rational(20, 100)),
                new Pair<int, Rational>(3, new Rational(18, 100)),
                new Pair<int, Rational>(4, new Rational(15, 100)),
                new Pair<int, Rational>(5, new Rational(10, 100)),
                new Pair<int, Rational>(6, new Rational(8, 100)),
                new Pair<int, Rational>(7, new Rational(5, 100)),
                new Pair<int, Rational>(8, new Rational(2, 100)),
                new Pair<int, Rational>(9, new Rational(0, 100)),
                new Pair<int, Rational>(10, new Rational(0, 100))
            };
            var radix4Huffman = Coding.Huffman(radix4Symbols, 4);

            var sourceSymbols = new Pair<int, Rational>[]
            {
                new Pair<int, Rational>(1, new Rational(27, 40)),
                new Pair<int, Rational>(2, new Rational(9, 40)),
                new Pair<int, Rational>(3, new Rational(3, 40)),
                new Pair<int, Rational>(4, new Rational(1, 40))
            };
            var source2 = Coding.Power(sourceSymbols, 2).ToList();
            var source3 = Coding.Power(sourceSymbols, 3).ToList();
            var source4 = Coding.Power(sourceSymbols, 4).ToList();
            var source5 = Coding.Power(sourceSymbols, 5).ToList();
            var source6 = Coding.Power(sourceSymbols, 6).ToList();

            var huffmanCode = Coding.Huffman(sourceSymbols);
            var huffmanCode2 = Coding.Huffman(source2);
            //var huffmanCode3 = Coding.Huffman(source3);
            //var huffmanCode4 = Coding.Huffman(source4);
            //var huffmanCode5 = Coding.Huffman(source5);
            //var huffmanCode6 = Coding.Huffman(source6);

            var blockSource = new Pair<int, Rational>[]
            {
                new Pair<int, Rational>(1, new Rational(1, 2)),
                new Pair<int, Rational>(2, new Rational(1, 2))
            };

            var blockSource4 = Coding.Power(blockSource, 4);
            var blockHuffman = Coding.Huffman(blockSource4);

            var fibonacciSource = new List<Pair<int, Rational>>();
            int fibonacciCount = 10;
            var fibonacciDenom = Discrete.Fibonacci(fibonacciCount + 2) - 1;

            for (int i = 1; i <= fibonacciCount; i++)
            {
                fibonacciSource.Add(new Pair<int, Rational>(i, new Rational(Discrete.Fibonacci(i), fibonacciDenom)));
            }

            var fibonacciHuffman = Coding.Huffman(fibonacciSource);

            var encoding = Coding.ArithmeticEncode(new int[] { 3, 1, 2, 1, 1 }, sourceSymbols, 4);

            var output = Coding.Lz78Encode("foo bar ra fo obar foo foo foo foo bar bar rab rab ra fbar".ToList()).ToList();

            var aSource = new Pair<int, Rational>[]
            {
                new Pair<int, Rational>(1, new Rational(2, 5)),
                new Pair<int, Rational>(2, new Rational(1, 5)),
                new Pair<int, Rational>(3, new Rational(1, 5)),
                new Pair<int, Rational>(4, new Rational(1, 5))
            };

            var aOutput = Coding.ArithmeticEncode(new int[] { 2, 1, 3, 4 }, aSource, 4);
        }

        static void SomeExample()
        {
            var s6 = Group.SymmetricGroup(6);
            var setX = new FiniteSet<int>(1, 2, 3, 4, 5, 6);
            var perm = Permutation<int>.FromDisjointCycles(setX, new Cycle<int>[] { new Cycle<int>(new int[] { 1, 2, 4, 6 }), new Cycle<int>(new int[] { 3, 5 }) });
            Func<Permutation<int>, int, int> action = (p, k) => p[k];

            var s6subgroup = s6.GeneratedSubgroup(perm);

            var s3 = Group.SymmetricGroup(3);
            var s3subgroup = s3.GeneratedSubgroup(Permutation<int>.FromCycle(s3.Identity.Set, new Cycle<int>(new int[] { 1, 2 })));
            Func<Permutation<int>, Group<Permutation<int>>, Group<Permutation<int>>> conjugationAction =
                (p, h) => s3.Subgroup(Group<Permutation<int>>.RightProduct(s3, Group<Permutation<int>>.LeftProduct(s3, p, h.Set), s3.Inverse(p)));

            var orders2 = Group.CyclicGroup(15).Set.Select(x => Group.CyclicGroup(15).OrderOf(x)).ToArray();
            var orders1 = Group.DihedralGroup(15).Set.Select(x => Group.DihedralGroup(15).OrderOf(x)).ToArray();

            var group = Group.DirectProduct(Group.CyclicGroup(3), Group.DihedralGroup(5));
            var orders = group.Set.Select(x => group.OrderOf(x)).ToArray();

            var group3 = Group.DirectProduct(Group.CyclicGroup(5), Group.DihedralGroup(3));
            var orders3 = group3.Set.Select(x => group3.OrderOf(x)).ToArray();

            var d14 = Group.DihedralGroup(14);
            var d7z2 = Group.DirectProduct(Group.DihedralGroup(7), Group.CyclicGroup(2));

            Func<Pair<Pair<int, bool>, int>, Pair<int, bool>> isomorphism =
                g => g.Second == 1 ? new Pair<int, bool>((g.First.First * 2 + 7) % 14, g.First.Second) : new Pair<int, bool>(g.First.First * 2, g.First.Second);

            var s6subgroup2 = s6.StabilizerSubgroup(action, 6);

            var group222 = Group.DirectProduct(Group.DirectProduct(Group.CyclicGroup(2), Group.CyclicGroup(2)), Group.DihedralGroup(4)).ToPermutationGroup();
            var group222orders = group222.Set.Select(x => group222.OrderOf(x)).ToArray();

            var ring = new Ring<int>(Group.CyclicGroup(10), (x, y) => (x * y) % 10);

            Func<int, int> cyclicH = k => (4 * k) % 8;
        }

        static void TestMatrix()
        {
            var v1 = new Matrix<double, RealSpace>(new double[] { 1, 2, 3 });
            var v2 = new Matrix<double, RealSpace>(new double[] { 5, -1, 1 });
            var v3 = v1 + v2;
            var m9 = new Matrix<double, RealSpace>(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });

            var m = Matrix<double, RealSpace>.FromColumns(new Matrix<double, RealSpace>[] { v1, v2, v3 });
            var nullSpace = new List<Matrix<double, RealSpace>>();

            var eigenvalue = m.Eigenvalues()[0];
            var eigenvector = m.Eigenspace(eigenvalue)[0];
            var newVector = m * eigenvector;
            var newVectorExpected = eigenvalue * eigenvector;

            var mi = new Matrix<Complex, ComplexSpace>(new Complex[,] { { 1, 3, -1 }, { 1, 1, 3 }, { -2, 1, 0 } });
            Matrix<Complex, ComplexSpace> p;
            Matrix<Complex, ComplexSpace> d;

            mi.Eigendecomposition(out p, out d);

            var mi2 = new Matrix<double, RealSpace>(new double[,] { { 1, 1 }, { 1, 0 } });
            Matrix<double, RealSpace> p2;
            Matrix<double, RealSpace> d2;

            mi2.Eigendecomposition(out p2, out d2);

            var mi3 = new Matrix<double, RealSpace>(new double[,] { { 0.1, 0.2, 0.3, 0.4 }, { 0.9, 0, 0, 0 }, { 0, 0.8, 0, 0 }, { 0, 0, 0.7, 0.6 } });

            Matrix<double, RealSpace> q;
            Matrix<double, RealSpace> r;

            mi3.QrDecomposition(out q, out r);

            var mi4 = new Matrix<Rational, RationalSpace>(new Rational[,] { { 1, 1 }, { 1, 0 } });

            var mi3rat = Matrix<Rational, RationalSpace>.FromMatrix(mi3, x => (Rational)x);

            var mi5 = new Matrix<System.Numerics.BigInteger, IntegerSpace>(new System.Numerics.BigInteger[,] { { 14, 3, 100, 8 } });
            Matrix<System.Numerics.BigInteger, IntegerSpace> sLeft;
            Matrix<System.Numerics.BigInteger, IntegerSpace> sRight;

            var smithNormal = mi5.SmithNormalForm(out sLeft, out sRight);

            Matrix<Rational, RationalSpace> lower, upper, permute;

            mi3rat.LupDecomposition(out lower, out upper, out permute);

            var a = new Matrix<Rational, RationalSpace>(new Rational[,] { { 5, 4, 2, 1 }, { 0, 1, -1, -1 }, { -1, -1, 3, 0 }, { 1, 1, -1, 2 } });
            var charPoly = Polynomial<Complex, ComplexField>.FromPolynomial(a.CharacteristicPolynomial<RationalField>(), rat => (double)rat);

            var b = new Matrix<Rational, RationalSpace>(new Rational[,]
            {
                { 4, 1, 0, -1 },
                { 0, new Rational(7, 2), new Rational(1, 2), 0 },
                { 0, new Rational(-1, 2), new Rational(9, 2), 0 },
                { 0, new Rational(-1, 2), new Rational(1, 2), 4 }
            });
            Matrix<Rational, RationalSpace> jordanP;
            var jordanD = b.JordanNormalForm(new Rational[] { 4 }, out jordanP, null);

            Matrix<Rational, RationalSpace> jordanP2;
            var jordanD2 = a.JordanNormalForm(new Rational[] { 1, 2, 4 }, out jordanP2, null);

            var mi3Cp = new Matrix<Complex, ComplexSpace>(new Complex[,] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 } });
            Matrix<Complex, ComplexSpace> mi3P;
            Matrix<Complex, ComplexSpace> mi3D;

            mi3D = mi3Cp.JordanNormalForm(out mi3P);

            var test2 = new Matrix<Rational, RationalSpace>(new Rational[4, 4] { { 7, 4, 5, 1 }, { -15, -10, -15, -3 }, { 0, 0, 5, 0 }, { 56, 52, 51, 15 } });
            Matrix<Rational, RationalSpace> test2P, test2D;

            test2D = test2.RationalCanonicalForm<RationalField>(out test2P, null);
            test2D = test2.JordanNormalForm(new Rational[] { 2, 5 }, out test2P, null);
        }

        static void TestSeries()
        {
            Series z = Series.One + 4;
        }
    }
}
