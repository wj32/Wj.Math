using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public static class Coding
    {
        private class HuffmanNode<T>
        {
            public bool Source;
            public int SymbolIndex;
            public Rational Probability;
            public HuffmanNode<T>[] Children;

            public void Traverse(bool sourceOnly, Action<HuffmanNode<T>> action)
            {
                if (!sourceOnly || this.Source)
                    action(this);

                if (this.Children != null)
                {
                    foreach (var child in this.Children)
                        child.Traverse(sourceOnly, action);
                }
            }
        }

        public static double Entropy<T>(IEnumerable<Pair<T, Rational>> sourceSymbols) where T : IEquatable<T>
        {
            double sum = 0;

            foreach (var pair in sourceSymbols)
            {
                sum -= (double)pair.Second * System.Math.Log((double)pair.Second, 2);
            }

            return sum;
        }

        public static IEnumerable<Pair<Vector<T>, Rational>> Power<T>(IEnumerable<Pair<T, Rational>> sourceSymbols, int n) where T : IEquatable<T>
        {
            if (n == 1)
            {
                foreach (var pair in sourceSymbols)
                    yield return new Pair<Vector<T>, Rational>(new Vector<T>(new T[] { pair.First }), pair.Second);

                yield break;
            }

            var previous = Power(sourceSymbols, n - 1);

            foreach (var pair in sourceSymbols)
            {
                foreach (var previousPair in previous)
                {
                    T[] newBits = new T[n];

                    Array.Copy(previousPair.First.Bits, newBits, n - 1);
                    newBits[n - 1] = pair.First;

                    yield return new Pair<Vector<T>, Rational>(new Vector<T>(newBits), previousPair.Second * pair.Second);
                }
            }
        }

        public static Dictionary<T, Vector<int>> Huffman<T>(IEnumerable<Pair<T, Rational>> sourceSymbols, int radix = 2) where T : IEquatable<T>
        {
            List<Pair<T, Rational>> sortedSymbols = sourceSymbols.ToList();
            List<int>[] codeSymbols = new List<int>[sortedSymbols.Count];
            List<HuffmanNode<T>> nodes = new List<HuffmanNode<T>>();
            Dictionary<T, Vector<int>> outputSymbols = new Dictionary<T, Vector<int>>();

            if (radix != 2 && sortedSymbols.Count % (radix - 1) != 1)
                throw new ArgumentException("Invalid number of symbols for radix");

            sortedSymbols.Sort((s1, s2) => -s1.Second.CompareTo(s2.Second));

            for (int i = 0; i < sortedSymbols.Count; i++)
            {
                HuffmanNode<T> symbolNode = new HuffmanNode<T>();

                symbolNode.Source = true;
                symbolNode.SymbolIndex = i;
                symbolNode.Probability = sortedSymbols[i].Second;
                nodes.Add(symbolNode);

                codeSymbols[i] = new List<int>();
            }

            while (nodes.Count >= 2)
            {
                HuffmanNode<T>[] children = new HuffmanNode<T>[radix];
                HuffmanNode<T> child = new HuffmanNode<T>();
                int insertIndex;

                nodes.CopyTo(nodes.Count - radix, children, 0, radix);

                child.Source = false;
                child.Probability = children.Aggregate(Rational.Zero, (x, node) => x + node.Probability);
                child.Children = children;

                nodes.RemoveRange(nodes.Count - radix, radix);

                for (insertIndex = 0; insertIndex < nodes.Count; insertIndex++)
                {
                    if (nodes[insertIndex].Probability <= child.Probability)
                        break;
                }

                nodes.Insert(insertIndex, child);

                for (int i = 0; i < radix; i++)
                    children[i].Traverse(true, node => codeSymbols[node.SymbolIndex].Add(i));
            }

            for (int i = 0; i < codeSymbols.Length; i++)
                outputSymbols.Add(sortedSymbols[i].First, new Vector<int>(codeSymbols[i], true));

            return outputSymbols;
        }

        public static Vector<int> Encode<T>(IEnumerable<T> input, Dictionary<T, Vector<int>> code)
        {
            List<int> output = new List<int>();

            foreach (T symbol in input)
                output.AddRange(code[symbol]);

            return new Vector<int>(output.ToArray());
        }

        public static IEnumerable<T> Decode<T>(Vector<int> input, Dictionary<T, Vector<int>> code)
        {
            return null;
        }

        private static Dictionary<T, Pair<Rational, Rational>> CreateIntervalDictionary<T>(IEnumerable<Pair<T, Rational>> sourceSymbols) where T : IEquatable<T>
        {
            var intervals = new Dictionary<T, Pair<Rational, Rational>>();
            Rational left = 0;

            foreach (var pair in sourceSymbols)
            {
                intervals.Add(pair.First, new Pair<Rational, Rational>(left, left + pair.Second));
                left += pair.Second;
            }

            return intervals;
        }

        private static Pair<T, Pair<Rational, Rational>> FindInterval<T>(Dictionary<T, Pair<Rational, Rational>> intervals, Rational q) where T : IEquatable<T>
        {
            foreach (var pair in intervals)
            {
                if (q >= pair.Value.First && q < pair.Value.Second)
                    return new Pair<T, Pair<Rational, Rational>>(pair.Key, pair.Value);
            }

            throw new ArgumentException();
        }

        public static Pair<Rational, Rational> ArithmeticEncode<T>(IEnumerable<T> input, IEnumerable<Pair<T, Rational>> sourceSymbols, T stopSymbol) where T : IEquatable<T>
        {
            var intervals = CreateIntervalDictionary(sourceSymbols);
            Rational left = 0;
            Pair<Rational, Rational> output = new Pair<Rational, Rational>(0, 1);

            foreach (var symbol in input.Concat(Enumerable.Repeat(stopSymbol, 1)))
            {
                Pair<Rational, Rational> interval = intervals[symbol];
                Rational length = output.Second - output.First;

                output = new Pair<Rational, Rational>(output.First + interval.First * length, output.First + interval.Second * length);
            }

            return output;
        }

        public static IEnumerable<T> ArithmeticDecode<T>(Rational input, IEnumerable<Pair<T, Rational>> sourceSymbols, T stopSymbol) where T : IEquatable<T>
        {
            var intervals = CreateIntervalDictionary(sourceSymbols);
            T symbol;

            do
            {
                var pair = FindInterval(intervals, input);

                symbol = pair.First;
                input = (input - pair.Second.First) / (pair.Second.Second - pair.Second.First);
                yield return symbol;
            } while (!symbol.Equals(stopSymbol));
        }

        private static Pair<int, int> MatchPrefix<T>(List<List<T>> dictionary, List<T> input, int index) where T : IEquatable<T>
        {
            int maximumLength = 0;
            int maximumOrdinal = 0;

            for (int i = 0; i < dictionary.Count; i++)
            {
                List<T> entry = dictionary[i];
                bool match = true;

                if (entry.Count > input.Count - index)
                    continue;

                for (int j = 0; j < entry.Count; j++)
                {
                    if (!input[index + j].Equals(entry[j]))
                    {
                        match = false;
                        break;
                    }
                }

                if (match && maximumLength < entry.Count)
                {
                    maximumLength = entry.Count;
                    maximumOrdinal = i + 1;
                }
            }

            return new Pair<int,int>(maximumOrdinal, maximumLength);
        }

        public static IEnumerable<Pair<int, T>> Lz78Encode<T>(List<T> input) where T : IEquatable<T>
        {
            List<List<T>> dictionary = new List<List<T>>();
            int index = 0;

            while (index < input.Count)
            {
                var match = MatchPrefix(dictionary, input, index);
                T next;

                if (index + match.Second < input.Count)
                    next = input[index + match.Second];
                else
                    next = default(T);

                yield return new Pair<int, T>(match.First, next);

                if (match.First != 0)
                    dictionary.Add(dictionary[match.First - 1].Concat(Enumerable.Repeat(next, 1)).ToList());
                else
                    dictionary.Add(Enumerable.Repeat(next, 1).ToList());

                index += match.Second + 1;
            }
        }

        public static IEnumerable<T> Lz78Decode<T>(IEnumerable<Pair<int, T>> input) where T : IEquatable<T>
        {
            List<List<T>> dictionary = new List<List<T>>();

            foreach (var pair in input)
            {
                if (pair.First == 0)
                {
                    dictionary.Add(Enumerable.Repeat(pair.Second, 1).ToList());
                    yield return pair.Second;
                }
                else
                {
                    List<T> entry = dictionary[pair.First - 1];
                    List<T> newEntry = entry.Concat(Enumerable.Repeat(pair.Second, 1)).ToList();

                    dictionary.Add(newEntry);

                    foreach (T t in newEntry)
                        yield return t;
                }
            }
        }

        public static Matrix<int, ModuloSpace<TInteger2>> Hamming(int n, int k)
        {
            int m = n - k;
            Matrix<int, ModuloSpace<TInteger2>> matrix = new Matrix<int, ModuloSpace<TInteger2>>(m, n);
            int r = 2;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    
                }

                r *= 2;
            }

            return matrix;
        }
    }
}
