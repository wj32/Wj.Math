using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public class Graph<T>
        where T : IEquatable<T>, IComparable<T>
    {
        public static Graph<int> CompleteGraph(int n)
        {
            HashSet<int> vertices = new HashSet<int>();
            HashSet<UnorderedPair<int>> edges = new HashSet<UnorderedPair<int>>();

            for (int i = 1; i < n; i++)
            {
                vertices.Add(i);

                for (int j = i + 1; j <= n; j++)
                {
                    edges.Add(new UnorderedPair<int>(i, j));
                }
            }

            vertices.Add(n);

            return new Graph<int>(vertices, edges);
        }

        private HashSet<T> _vertices;
        private HashSet<UnorderedPair<T>> _edges;

        public Graph(IEnumerable<T> vertices, IEnumerable<UnorderedPair<T>> edges)
        {
            _vertices = new HashSet<T>(vertices);
            _edges = new HashSet<UnorderedPair<T>>(edges);
        }

        private Graph(HashSet<T> vertices, HashSet<UnorderedPair<T>> edges, bool copy)
        {
            if (!copy)
            {
                _vertices = vertices;
                _edges = edges;
            }
            else
            {
                _vertices = new HashSet<T>(vertices);
                _edges = new HashSet<UnorderedPair<T>>(edges);
            }
        }

        public ISet<T> Vertices
        {
            get { return _vertices; }
        }

        public ISet<UnorderedPair<T>> Edges
        {
            get { return _edges; }
        }

        public int TotalDegree
        {
            get { return _edges.Count * 2; }
        }

        public bool AddVertex(T vertex)
        {
            return _vertices.Add(vertex);
        }

        public bool AddEdge(UnorderedPair<T> edge)
        {
            return _edges.Add(edge);
        }

        public int CountConnectedComponents()
        {
            int componentCount = 0;
            int componentNumber = 1;
            Dictionary<T, int> components = new Dictionary<T, int>();
            HashSet<UnorderedPair<int>> merged = new HashSet<UnorderedPair<int>>();

            foreach (UnorderedPair<T> edge in _edges)
            {
                bool containsFirst = components.ContainsKey(edge.First);
                bool containsSecond = components.ContainsKey(edge.Second);

                if (!containsFirst && !containsSecond)
                {
                    // New edge; create a new component.
                    components.Add(edge.First, componentNumber);
                    components.Add(edge.Second, componentNumber);
                    componentNumber++;
                    componentCount++;
                }
                else if (containsFirst && containsSecond)
                {
                    if (components[edge.First] != components[edge.Second])
                    {
                        UnorderedPair<int> record = new UnorderedPair<int>(components[edge.First], components[edge.Second]);

                        // Merge two components.
                        if (!merged.Contains(record))
                        {
                            merged.Add(record);
                            componentCount--;
                        }
                    }
                }
                else if (containsFirst)
                {
                    if (components.ContainsKey(edge.Second))
                        components[edge.Second] = components[edge.First];
                    else
                        components.Add(edge.Second, components[edge.First]);
                }
                else if (containsSecond)
                {
                    if (components.ContainsKey(edge.First))
                        components[edge.First] = components[edge.Second];
                    else
                        components.Add(edge.First, components[edge.Second]);
                }
            }

            return componentCount + (_vertices.Count - components.Count);
        }

        public bool DeleteEdge(UnorderedPair<T> edge)
        {
            return _edges.Remove(edge);
        }

        public bool DeleteVertex(T vertex)
        {
            if (!_vertices.Remove(vertex))
                return false;

            _edges.RemoveWhere(pair => pair.First.Equals(vertex) || pair.Second.Equals(vertex));

            return true;
        }

        public int GetDegree(T vertex)
        {
            int degree = 0;

            foreach (UnorderedPair<T> edge in _edges)
            {
                if (edge.First.Equals(vertex) || edge.Second.Equals(vertex))
                    degree++;
            }

            return degree;
        }

        public T[] GetNeighbors(T vertex)
        {
            List<T> neighbors = new List<T>();

            foreach (UnorderedPair<T> edge in _edges)
            {
                if (edge.First.Equals(vertex))
                    neighbors.Add(edge.Second);
                else if (edge.Second.Equals(vertex))
                    neighbors.Add(edge.First);
            }

            return neighbors.ToArray();
        }

        public Graph<U> Relabel<U>(Func<T, U> bijection)
            where U : IEquatable<U>, IComparable<U>
        {
            HashSet<U> newVertices = new HashSet<U>();
            HashSet<UnorderedPair<U>> newEdges = new HashSet<UnorderedPair<U>>();

            foreach (T vertex in _vertices)
                newVertices.Add(bijection(vertex));
            foreach (UnorderedPair<T> edge in _edges)
                newEdges.Add(new UnorderedPair<U>(bijection(edge.First), bijection(edge.Second)));

            return new Graph<U>(newVertices, newEdges, false);
        }
    }
}
