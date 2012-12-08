using System;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Wj.Math
{
    [Flags]
    public enum ReduceOptions
    {
        /// <summary>
        /// Perform Gauss-Jordan elimination.
        /// </summary>
        Rref = 0x1,
        /// <summary>
        /// Not implemented.
        /// </summary>
        LargePivots = 0x2
    }

    public enum MatrixOperationKind
    {
        /// <summary>
        /// R_I &lt;-&gt; R_J
        /// </summary>
        Swap,
        /// <summary>
        /// R_I &lt;- K*R_I
        /// </summary>
        Multiply,
        /// <summary>
        /// R_I &lt;- R_I + K*R_J
        /// </summary>
        Add
    }

    public struct MatrixOperation<T>
    {
        private MatrixOperationKind _kind;
        private int _i;
        private int _j;
        private T _k;

        public MatrixOperation(MatrixOperationKind kind, int i, int j)
        {
            _kind = kind;
            _i = i;
            _j = j;
            _k = default(T);
        }

        public MatrixOperation(MatrixOperationKind kind, int i, int j, T k)
        {
            _kind = kind;
            _i = i;
            _j = j;
            _k = k;
        }

        public MatrixOperationKind Kind
        {
            get { return _kind; }
        }

        public int I
        {
            get { return _i; }
        }

        public int J
        {
            get { return _j; }
        }

        public T K
        {
            get { return _k; }
        }
    }

    public class Matrix<T, TSpace> :
        IEquatable<T>,
        IEquatable<Matrix<T, TSpace>>
        where T : IEquatable<T>
        where TSpace : ISpace<T>, new()
    {
        #region Internal Functions

        private static bool SameDimension(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                return false;

            return true;
        }

        #endregion

        #region Basic Operators

        // Nothing
        public static Matrix<T, TSpace> operator +(Matrix<T, TSpace> m)
        {
            return m;
        }

        public static Matrix<T, TSpace> Negate(Matrix<T, TSpace> m)
        {
            Matrix<T, TSpace> newMatrix = new Matrix<T, TSpace>(m);

            for (int i = 0; i < newMatrix.Rows; i++)
            {
                for (int j = 0; j < newMatrix.Columns; j++)
                {
                    newMatrix.M[i, j] = m.Field.Negate(newMatrix.M[i, j]);
                }
            }

            return newMatrix;
        }
        public static Matrix<T, TSpace> operator -(Matrix<T, TSpace> m)
        {
            return Negate(m);
        }

        public static Matrix<T, TSpace> Add(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            if (!SameDimension(m1, m2))
                throw new ArgumentException();

            Matrix<T, TSpace> m3 = new Matrix<T, TSpace>(m1);

            for (int i = 0; i < m3.Rows; i++)
            {
                for (int j = 0; j < m3.Columns; j++)
                {
                    m3.M[i, j] = m3.Field.Add(m3.M[i, j], m2.M[i, j]);
                }
            }

            return m3;
        }
        public static Matrix<T, TSpace> operator +(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            return Add(m1, m2);
        }

        public static Matrix<T, TSpace> Subtract(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            if (!SameDimension(m1, m2))
                throw new ArgumentException();

            Matrix<T, TSpace> m3 = new Matrix<T, TSpace>(m1);

            for (int i = 0; i < m3.Rows; i++)
            {
                for (int j = 0; j < m3.Columns; j++)
                {
                    m3.M[i, j] = m3.Field.Subtract(m3.M[i, j], m2.M[i, j]);
                }
            }

            return m3;
        }
        public static Matrix<T, TSpace> operator -(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            return Subtract(m1, m2);
        }

        public static Matrix<T, TSpace> Multiply(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            if (m1.Columns != m2.Rows)
                throw new ArgumentException();

            Matrix<T, TSpace> m3 = new Matrix<T, TSpace>(m1.Rows, m2.Columns);

            for (int i = 0; i < m3.Rows; i++)
            {
                for (int j = 0; j < m3.Columns; j++)
                {
                    T e = m3.Field.Zero;

                    for (int k = 0; k < m1.Columns; k++)
                        e = m3.Field.Add(e, m3.Field.Multiply(m1.M[i, k], m2.M[k, j]));

                    m3.M[i, j] = e;
                }
            }

            return m3;
        }
        public static Matrix<T, TSpace> operator *(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            return Multiply(m1, m2);
        }

        #endregion

        #region Convenience Operators

        public static Matrix<T, TSpace> Multiply(Matrix<T, TSpace> m, T t)
        {
            Matrix<T, TSpace> newMatrix = new Matrix<T, TSpace>(m);

            for (int i = 0; i < newMatrix.Rows; i++)
            {
                for (int j = 0; j < newMatrix.Columns; j++)
                {
                    newMatrix.M[i, j] = m.Field.Multiply(t, newMatrix.M[i, j]);
                }
            }

            return newMatrix;
        }
        public static Matrix<T, TSpace> operator *(Matrix<T, TSpace> m, T t)
        {
            return Multiply(m, t);
        }
        public static Matrix<T, TSpace> operator *(T t, Matrix<T, TSpace> m)
        {
            return Multiply(m, t);
        }

        public static Matrix<T, TSpace> Multiply(Matrix<T, TSpace> m, int n)
        {
            Matrix<T, TSpace> newMatrix = new Matrix<T, TSpace>(m);

            for (int i = 0; i < newMatrix.Rows; i++)
            {
                for (int j = 0; j < newMatrix.Columns; j++)
                {
                    newMatrix.M[i, j] = m.Field.Multiply(newMatrix.M[i, j], n);
                }
            }

            return newMatrix;
        }
        public static Matrix<T, TSpace> operator *(Matrix<T, TSpace> m, int n)
        {
            return Multiply(m, n);
        }
        public static Matrix<T, TSpace> operator *(int n, Matrix<T, TSpace> m)
        {
            return Multiply(m, n);
        }

        public static Matrix<T, TSpace> Divide(Matrix<T, TSpace> m, T t)
        {
            Matrix<T, TSpace> newMatrix = new Matrix<T, TSpace>(m);

            for (int i = 0; i < newMatrix.Rows; i++)
            {
                for (int j = 0; j < newMatrix.Columns; j++)
                {
                    newMatrix.M[i, j] = m.Field.Divide(newMatrix.M[i, j], t);
                }
            }

            return newMatrix;
        }
        public static Matrix<T, TSpace> operator /(Matrix<T, TSpace> m, T t)
        {
            return Divide(m, t);
        }

        public static Matrix<T, TSpace> Divide(Matrix<T, TSpace> m, int n)
        {
            Matrix<T, TSpace> newMatrix = new Matrix<T, TSpace>(m);

            for (int i = 0; i < newMatrix.Rows; i++)
            {
                for (int j = 0; j < newMatrix.Columns; j++)
                {
                    newMatrix.M[i, j] = m.Field.Divide(newMatrix.M[i, j], n);
                }
            }

            return newMatrix;
        }
        public static Matrix<T, TSpace> operator /(Matrix<T, TSpace> m, int n)
        {
            return Divide(m, n);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            if (object.ReferenceEquals(m1, m2))
                return true;
            if ((object)m1 == null)
                return false;

            return m1.Equals(m2);
        }
        public static bool operator !=(Matrix<T, TSpace> m1, Matrix<T, TSpace> m2)
        {
            if (object.ReferenceEquals(m1, m2))
                return false;
            if ((object)m1 == null)
                return true;

            return !m1.Equals(m2);
        }

        public static bool operator ==(Matrix<T, TSpace> m, T t)
        {
            if ((object)m == null)
                return false;

            return m.Equals(t);
        }
        public static bool operator !=(Matrix<T, TSpace> m, T t)
        {
            if ((object)m == null)
                return true;

            return !m.Equals(t);
        }

        #endregion

        #region Basic Matrices

        public static Matrix<T, TSpace> FromColumns(Matrix<T, TSpace>[] columns)
        {
            if (columns.Length == 0)
                throw new ArgumentException();

            int rows = columns[0].Rows;
            int numberOfColumns = 0;

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Rows != rows)
                    throw new ArgumentException();

                numberOfColumns += columns[i].Columns;
            }

            Matrix<T, TSpace> m = new Matrix<T, TSpace>(rows, numberOfColumns);
            int column = 0;

            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    for (int k = 0; k < columns[i].Columns; k++)
                    {
                        m.M[j, column + k] = columns[i].M[j, k];
                    }
                }

                column += columns[i].Columns;
            }

            return m;
        }

        public static Matrix<T, TSpace> FromRows(Matrix<T, TSpace>[] rows)
        {
            if (rows.Length == 0)
                throw new ArgumentException();

            int columns = rows[0].Columns;
            int numberOfRows = 0;

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i].Columns != columns)
                    throw new ArgumentException();

                numberOfRows += rows[i].Rows;
            }

            Matrix<T, TSpace> m = new Matrix<T, TSpace>(numberOfRows, columns);
            int row = 0;

            for (int i = 0; i < rows.Length; i++)
            {
                for (int k = 0; k < rows[i].Rows; k++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        m.M[row + k, j] = rows[i].M[k, j];
                    }
                }

                row += rows[i].Rows;
            }

            return m;
        }

        public static Matrix<T, TSpace> Identity(int n)
        {
            return Scalar(n, (new TSpace()).Field.One);
        }

        public static Matrix<T, TSpace> Scalar(int n, T t)
        {
            Matrix<T, TSpace> m = new Matrix<T, TSpace>(n, n);

            for (int i = 0; i < n; i++)
            {
                m.M[i, i] = t;
            }

            return m;
        }

        public static Matrix<T, TSpace> Zero(int n)
        {
            return new Matrix<T, TSpace>(n, n);
        }

        public static Matrix<T, TSpace> Diagonal(T[] values)
        {
            int n = values.Length;
            Matrix<T, TSpace> m = new Matrix<T, TSpace>(n, n);

            for (int i = 0; i < n; i++)
            {
                m.M[i, i] = values[i];
            }

            return m;
        }

        public static Matrix<T, TSpace> Diagonal(Matrix<T, TSpace>[] entries)
        {
            int row = 0;
            int column = 0;

            for (int i = 0; i < entries.Length; i++)
            {
                row += entries[i].Rows;
                column += entries[i].Columns;
            }

            Matrix<T, TSpace> m = new Matrix<T, TSpace>(row, column);

            row = 0;
            column = 0;

            for (int i = 0; i < entries.Length; i++)
            {
                Matrix<T, TSpace> entry = entries[i];

                for (int j = 0; j < entry.Rows; j++)
                {
                    for (int k = 0; k < entry.Columns; k++)
                        m.M[row + j, column + k] = entry.M[j, k];
                }

                row += entry.Rows;
                column += entry.Columns;
            }

            return m;
        }

        public static Matrix<T, TSpace> JordanBlock(int n, T lambda)
        {
            Matrix<T, TSpace> block = Matrix<T, TSpace>.Scalar(n, lambda);

            for (int i = 0; i < n - 1; i++)
            {
                block.M[i, i + 1] = block.Field.One;
            }

            return block;
        }

        public static Matrix<T, TSpace> Companion<TField>(Polynomial<T, TField> polynomial) where TField : IField<T>, new()
        {
            Matrix<T, TSpace> m = new Matrix<T, TSpace>(polynomial.Degree, polynomial.Degree);

            for (int i = 0; i < polynomial.Degree - 1; i++)
            {
                m.M[i + 1, i] = polynomial.Field.One;
            }

            foreach (Term<T> term in polynomial)
            {
                if (term.Deg != polynomial.Degree)
                    m.M[term.Deg, polynomial.Degree - 1] = polynomial.Field.Negate(term.Coeff);
            }

            return m;
        }

        #endregion

        #region Conversion

        public static Matrix<T, TSpace> FromMatrix<USpace>(Matrix<T, USpace> m) where USpace : ISpace<T>, new()
        {
            return new Matrix<T, TSpace>(m.M);
        }

        public static Matrix<T, TSpace> FromMatrix<U, USpace>(Matrix<U, USpace> m, Func<U, T> f) where U : IEquatable<U> where USpace : ISpace<U>, new()
        {
            T[,] matrix = new T[m.Rows, m.Columns];

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    matrix[i, j] = f(m.M[i, j]);
                }
            }

            return new Matrix<T, TSpace>(matrix);
        }

        #endregion

        #region Matrix Functions

        public static Matrix<T, TSpace> DirectSum(Matrix<T, TSpace> a, Matrix<T, TSpace> b)
        {
            return Diagonal(new Matrix<T, TSpace>[] { a, b });
        }

        public static Matrix<T, TSpace> Solve(Matrix<T, TSpace> a, Matrix<T, TSpace> b, ICollection<Matrix<T, TSpace>> nullSpace)
        {
            if (b != null && (!b.IsVector || b.Rows != a.Rows))
                throw new ArgumentException();

            List<MatrixOperation<T>> operations = null;
            int rank;
            int[] pivotColumns = new int[a.Rows];

            if (b != null)
                operations = new List<MatrixOperation<T>>();

            Matrix<T, TSpace> m = a.Reduce(ReduceOptions.Rref, operations, pivotColumns, out rank);
            Matrix<T, TSpace> x = null;

            if (nullSpace != null)
            {
                int nullity = a.Columns - rank;
                T[,] basis = new T[nullity, a.Columns];
                bool[] pivot = new bool[a.Columns];
                int basisIndex;

                if (a.Field.NeedsInitialize)
                {
                    for (int i = 0; i < nullity; i++)
                    {
                        for (int j = 0; j < a.Columns; j++)
                        {
                            basis[i, j] = a.Field.Zero;
                        }
                    }
                }

                for (int i = 0; i < rank; i++)
                    pivot[pivotColumns[i]] = true;

                for (int i = rank - 1; i >= 0; i--)
                {
                    int pivotColumn = pivotColumns[i];

                    basisIndex = 0;

                    for (int j = 0; j < a.Columns; j++)
                    {
                        if (!pivot[j])
                        {
                            basis[basisIndex, pivotColumn] = a.Field.Negate(m.M[i, j]);
                            basisIndex++;
                        }
                    }
                }

                basisIndex = 0;

                for (int j = 0; j < a.Columns; j++)
                {
                    if (!pivot[j])
                    {
                        Matrix<T, TSpace> v;

                        basis[basisIndex, j] = a.Field.One;

                        v = new Matrix<T, TSpace>(a.Columns, 1);
                        Array.Copy(basis, basisIndex * a.Columns, v.M, 0, a.Columns);
                        nullSpace.Add(v);

                        basisIndex++;
                    }
                }
            }

            if (b != null)
            {
                Matrix<T, TSpace> t = new Matrix<T, TSpace>(b);
                bool inconsistent = false;

                for (int i = 0; i < operations.Count; i++)
                    t.ApplyRowOperation(operations[i]);

                for (int i = rank; i < t.Rows; i++)
                {
                    if (!a.Field.IsApproxZero(t.M[i, 0]))
                    {
                        inconsistent = true;
                        break;
                    }
                }

                // TODO: Potential problems with numerical stability.
                if (!inconsistent)
                {
                    x = new Matrix<T, TSpace>(a.Columns, 1);

                    for (int i = 0; i < rank; i++)
                        x.M[pivotColumns[i], 0] = t.M[i, 0];
                }
            }

            return x;
        }

        #endregion

        #region Vector Functions

        public static Matrix<T, TSpace> Cross(Matrix<T, TSpace> v1, Matrix<T, TSpace> v2)
        {
            return v1.Space.CrossProduct(v1, v2);
        }

        public static T Dot(Matrix<T, TSpace> v1, Matrix<T, TSpace> v2)
        {
            return v1.Space.InnerProduct(v1, v2);
        }

        public static Matrix<T, TSpace> Projection(Matrix<T, TSpace> v, Matrix<T, TSpace> onto)
        {
            return v.Field.Divide(v.Space.InnerProduct(v, onto), v.Space.InnerProduct(onto, onto)) * onto;
        }

        #endregion

        private ISpace<T> _space;
        private IField<T> _field;
        private int _rows;
        private int _columns;
        private T[,] _m;

        public Matrix(int rows, int columns)
            : this(rows, columns, new T[rows, columns])
        {
            if (_field.NeedsInitialize)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        _m[i, j] = _field.Zero;
                    }
                }
            }
        }

        public Matrix(T[] vector)
            : this(vector.Length, 1, new T[vector.Length, 1])
        {
            for (int i = 0; i < vector.Length; i++)
                _m[i, 0] = vector[i];
        }

        public Matrix(T[,] matrix)
            : this(matrix.GetUpperBound(0) - matrix.GetLowerBound(0) + 1, matrix.GetUpperBound(1) - matrix.GetLowerBound(1) + 1, null)
        {
            _m = new T[_rows, _columns];
            Array.Copy(matrix, _m, matrix.Length);
        }

        internal Matrix(int rows, int columns, T[,] m)
        {
            _space = (new TSpace()).Instance;
            _field = _space.Field;
            _rows = rows;
            _columns = columns;
            _m = m;
        }

        internal Matrix(Matrix<T, TSpace> m)
        {
            _space = m.Space;
            _field = m.Field;
            _rows = m.Rows;
            _columns = m.Columns;
            _m = new T[_rows, _columns];
            Array.Copy(m.M, _m, m.M.Length);
        }

        public ISpace<T> Space
        {
            get { return _space; }
        }

        public IField<T> Field
        {
            get { return _field; }
        }

        public int Rows
        {
            get { return _rows; }
        }

        public int Columns
        {
            get { return _columns; }
        }

        public T this[int row, int column]
        {
            get { return _m[row, column]; }
        }

        public bool IsScalar
        {
            get { return _rows == 1 && _columns == 1; }
        }

        public bool IsSquare
        {
            get { return _rows == _columns; }
        }

        public bool IsVector
        {
            get { return _columns == 1; }
        }

        internal T[,] M
        {
            get { return _m; }
        }

        public bool Equals(T other)
        {
            if (_rows != 1 || _columns != 1)
                return false;

            return _m[0, 0].Equals(other);
        }

        public bool Equals(Matrix<T, TSpace> other)
        {
            if ((object)other == null)
                return false;
            if (_rows != other._rows || _columns != other._columns)
                return false;

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (!_m[i, j].Equals(other._m[i, j]))
                        return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix<T, TSpace>)
                return this.Equals((Matrix<T, TSpace>)obj);
            else
                return false;
        }

        public Matrix<T, TSpace> GetColumn(int index)
        {
            Matrix<T, TSpace> column = new Matrix<T, TSpace>(_rows, 1);

            for (int i = 0; i < _rows; i++)
                column.M[i, 0] = _m[i, index];

            return column;
        }

        public override int GetHashCode()
        {
            int code = _rows + 17 * _columns;

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    code += _m[i, j].GetHashCode() << ((i + j) & 3);
                }
            }

            return code;
        }

        public Matrix<T, TSpace> GetRow(int index)
        {
            Matrix<T, TSpace> row = new Matrix<T, TSpace>(1, _columns);

            for (int i = 0; i < _columns; i++)
                row.M[0, i] = _m[index, i];

            return row;
        }

        public T GetScalar()
        {
            if (!this.IsScalar)
                throw new InvalidOperationException();

            return _m[0, 0];
        }

        public bool IsIdentity()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == j)
                    {
                        if (!_field.IsOne(_m[i, j]))
                            return false;
                    }
                    else
                    {
                        if (!_field.IsZero(_m[i, j]))
                            return false;
                    }
                }
            }

            return true;
        }

        public bool IsZero()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (!_field.IsZero(_m[i, j]))
                        return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('{');

            for (int i = 0; i < _rows; i++)
            {
                sb.Append('{');

                for (int j = 0; j < _columns; j++)
                {
                    sb.Append(_m[i, j].ToString());
                    sb.Append(',');
                }

                if (sb[sb.Length - 1] == ',')
                    sb.Remove(sb.Length - 1, 1);

                sb.Append("},");
            }

            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);

            sb.Append('}');

            return sb.ToString();
        }

        #region Matrix Functions

        #region Row Reduction

        private void ApplyColumnOperation(MatrixOperation<T> operation)
        {
            switch (operation.Kind)
            {
                case MatrixOperationKind.Swap:
                    for (int i = 0; i < _rows; i++)
                    {
                        T t;

                        t = _m[i, operation.I];
                        _m[i, operation.I] = _m[i, operation.J];
                        _m[i, operation.J] = t;
                    }

                    break;
                case MatrixOperationKind.Multiply:
                    for (int i = 0; i < _rows; i++)
                    {
                        _m[i, operation.I] = _field.Multiply(operation.K, _m[i, operation.I]);
                    }

                    break;
                case MatrixOperationKind.Add:
                    for (int i = 0; i < _rows; i++)
                    {
                        _m[i, operation.I] = _field.Add(_m[i, operation.I], _field.Multiply(operation.K, _m[i, operation.J]));
                    }

                    break;
            }
        }

        private void ApplyRowOperation(MatrixOperation<T> operation)
        {
            switch (operation.Kind)
            {
                case MatrixOperationKind.Swap:
                    for (int i = 0; i < _columns; i++)
                    {
                        T t;

                        t = _m[operation.I, i];
                        _m[operation.I, i] = _m[operation.J, i];
                        _m[operation.J, i] = t;
                    }

                    break;
                case MatrixOperationKind.Multiply:
                    for (int i = 0; i < _columns; i++)
                    {
                        _m[operation.I, i] = _field.Multiply(operation.K, _m[operation.I, i]);
                    }

                    break;
                case MatrixOperationKind.Add:
                    for (int i = 0; i < _columns; i++)
                    {
                        _m[operation.I, i] = _field.Add(_m[operation.I, i], _field.Multiply(operation.K, _m[operation.J, i]));
                    }

                    break;
            }
        }

        private MatrixOperation<T> InverseOperation(MatrixOperation<T> operation)
        {
            switch (operation.Kind)
            {
                case MatrixOperationKind.Swap:
                    return operation;
                case MatrixOperationKind.Multiply:
                    return new MatrixOperation<T>(operation.Kind, operation.I, 0, _field.Inverse(operation.K));
                case MatrixOperationKind.Add:
                    return new MatrixOperation<T>(operation.Kind, operation.I, operation.J, _field.Negate(operation.K));
                default:
                    throw new ArgumentException("operation");
            }
        }

        public Matrix<T, TSpace> Reduce(ReduceOptions options)
        {
            int rank;

            return this.Reduce(options, null, null, out rank);
        }

        public Matrix<T, TSpace> Reduce(ReduceOptions options, ICollection<MatrixOperation<T>> operations, int[] pivotColumns, out int rank)
        {
            Matrix<T, TSpace> m = new Matrix<T, TSpace>(this);
            int row = 0;
            int column = 0;
            MatrixOperation<T> operation;

            if (pivotColumns != null)
            {
                if (pivotColumns.Length != m.Rows)
                    throw new ArgumentException();
            }
            else if ((options & ReduceOptions.Rref) != 0)
            {
                pivotColumns = new int[m.Rows];
            }

            while (row < m.Rows && column < m.Columns)
            {
                int pivotRow = row;
                T pivotEntry = m.M[row, column];
                T inversePivotEntry = _field.One;
                bool allZero = _field.IsApproxZero(pivotEntry);

                if (allZero || !_field.IsUnit(pivotEntry))
                {
                    // We need to find another pivot.

                    for (int i = row + 1; i < m.Rows; i++)
                    {
                        T entry = m.M[i, column];

                        if (!_field.IsApproxZero(entry))
                            allZero = false;

                        if (!allZero && _field.IsUnit(entry))
                        {
                            pivotRow = i;
                            break;
                        }
                    }

                    if (pivotRow != row)
                    {
                        operation = new MatrixOperation<T>(MatrixOperationKind.Swap, row, pivotRow);

                        if (operations != null)
                            operations.Add(operation);

                        m.ApplyRowOperation(operation);
                        pivotRow = row;
                        pivotEntry = m.M[row, column];
                    }
                    else
                    {
                        if (allZero)
                        {
                            // Column is already reduced.
                            column++;
                            continue;
                        }
                        else
                        {
                            IEuclideanDomain<T> euclideanDomain = _field as IEuclideanDomain<T>;

                            if ((options & ReduceOptions.Rref) == 0 && euclideanDomain != null)
                            {
                                // We can't find a unit pivot entry, so we'll have to take the LCM of the entries in the column.

                                T[] entries = new T[m.Rows - row];
                                T lcm;

                                for (int i = 0; i < m.Rows - row; i++)
                                {
                                    entries[i] = m.M[row + i, column];

                                    if (_field.IsApproxZero(entries[i]))
                                        entries[i] = _field.One;
                                }

                                lcm = euclideanDomain.Lcm(entries, entries);

                                // Make each non-zero entry equal to the LCM and then reduce.

                                for (int i = row; i < m.Rows; i++)
                                {
                                    if (!_field.IsApproxZero(m.M[i, column]))
                                    {
                                        operation = new MatrixOperation<T>(MatrixOperationKind.Multiply, i, 0, entries[i - row]);

                                        if (operations != null)
                                            operations.Add(operation);

                                        m.ApplyRowOperation(operation);
                                    }
                                }

                                for (int i = row + 1; i < m.Rows; i++)
                                {
                                    if (!_field.IsApproxZero(m.M[i, column]))
                                    {
                                        operation = new MatrixOperation<T>(MatrixOperationKind.Add, i, row, _field.Negate(_field.One));

                                        if (operations != null)
                                            operations.Add(operation);

                                        m.ApplyRowOperation(operation);
                                        // Numerical stability fix.
                                        m.M[i, column] = _field.Zero;
                                    }
                                }

                                if (pivotColumns != null)
                                    pivotColumns[row] = column;

                                row++;
                                column++;
                                continue;
                            }
                            else
                            {
                                throw new ArgumentException("Unable to find a pivot entry in column " + (column + 1).ToString() + ".");
                            }
                        }
                    }
                }

                if (pivotColumns != null)
                    pivotColumns[row] = column;

                if ((options & ReduceOptions.Rref) != 0)
                {
                    if (!_field.IsOne(pivotEntry))
                    {
                        operation = new MatrixOperation<T>(MatrixOperationKind.Multiply, row, 0, _field.Inverse(pivotEntry));

                        if (operations != null)
                            operations.Add(operation);

                        m.ApplyRowOperation(operation);
                        pivotEntry = m.M[row, column];
                    }
                }
                else if (!_field.IsOne(pivotEntry))
                {
                    inversePivotEntry = _field.Inverse(pivotEntry);
                }

                // Reduce the remaining rows.

                for (int i = row + 1; i < m.Rows; i++)
                {
                    T entry = m.M[i, column];

                    if (_field.IsZero(entry))
                        continue;

                    operation = new MatrixOperation<T>(MatrixOperationKind.Add, i, row, _field.Multiply(_field.Negate(entry), inversePivotEntry));

                    if (operations != null)
                        operations.Add(operation);

                    m.ApplyRowOperation(operation);
                    // Numerical stability fix.
                    m.M[i, column] = _field.Zero;
                }

                row++;
                column++;
            }

            rank = row;

            if ((options & ReduceOptions.Rref) != 0)
            {
                // Perform the remaining Gauss-Jordan elimination operations.

                for (int i = row - 1; i >= 0; i--)
                {
                    int pivotColumn = pivotColumns[i];

                    for (int j = i - 1; j >= 0; j--)
                    {
                        T entry = m.M[j, pivotColumn];

                        if (!_field.IsZero(entry))
                        {
                            operation = new MatrixOperation<T>(MatrixOperationKind.Add, j, i, _field.Negate(entry));

                            if (operations != null)
                                operations.Add(operation);

                            m.ApplyRowOperation(operation);
                            // Numerical stability fix.
                            m.M[j, pivotColumn] = _field.Zero;
                        }
                    }
                }
            }

            return m;
        }

        #endregion

        #region Basic

        public Polynomial<T, TField> CharacteristicPolynomial<TField>() where TField : IField<T>, new()
        {
            return this.LambdaMatrix<TField>().Determinant();
        }

        public Matrix<T, TSpace>[] ColumnSpace()
        {
            List<Matrix<T, TSpace>> pivotVectors = new List<Matrix<T, TSpace>>();
            int[] pivotColumns = new int[_rows];
            int rank;

            this.Reduce(0, null, pivotColumns, out rank);

            for (int i = 0; i < rank; i++)
            {
                pivotVectors.Add(this.GetColumn(pivotColumns[i]));
            }

            return pivotVectors.ToArray();
        }

        public T Determinant()
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();

            switch (_rows)
            {
                case 1:
                    return _m[0, 0];
                case 2:
                    return _field.Subtract(_field.Multiply(_m[0, 0], _m[1, 1]), _field.Multiply(_m[0, 1], _m[1, 0]));
            }

            List<MatrixOperation<T>> operations = new List<MatrixOperation<T>>();
            int rank;
            Matrix<T, TSpace> m = this.Reduce(0, operations, null, out rank);
            T product = _field.One;
            bool negate = false;

            for (int i = 0; i < operations.Count; i++)
            {
                switch (operations[i].Kind)
                {
                    case MatrixOperationKind.Swap:
                        negate = !negate;
                        break;
                }
            }

            for (int i = 0; i < _rows; i++)
                product = _field.Multiply(product, m.M[i, i]);

            for (int i = 0; i < operations.Count; i++)
            {
                switch (operations[i].Kind)
                {
                    case MatrixOperationKind.Multiply:
                        if (_field.IsUnit(operations[i].K))
                        {
                            product = _field.Divide(product, operations[i].K);
                        }
                        else
                        {
                            IEuclideanDomain<T> euclideanDomain = _field as IEuclideanDomain<T>;
                            T remainder;

                            if (euclideanDomain == null)
                                throw new InvalidOperationException("Non-unit row operation in row reduction.");

                            product = euclideanDomain.Divide(product, operations[i].K, out remainder);
                        }
                        break;
                }
            }

            if (negate)
                product = _field.Negate(product);

            return product;
        }

        public Matrix<T, TSpace>[] Eigenspace(T eigenvalue)
        {
            return this.Eigenspace(eigenvalue, 1);
        }

        public Matrix<T, TSpace>[] Eigenspace(T eigenvalue, int multiplicity)
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();

            return (this - Matrix<T, TSpace>.Scalar(_rows, eigenvalue)).Pow(multiplicity).NullSpace();
        }

        public Matrix<T, TSpace> Inverse()
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();

            List<MatrixOperation<T>> operations = new List<MatrixOperation<T>>();
            int rank;

            this.Reduce(ReduceOptions.Rref, operations, null, out rank);

            if (rank != _rows)
                return null;

            Matrix<T, TSpace> m = Matrix<T, TSpace>.Identity(_rows);

            for (int i = 0; i < operations.Count; i++)
                m.ApplyRowOperation(operations[i]);

            return m;
        }

        private Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>> LambdaMatrix<TField>() where TField : IField<T>, new()
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();

            var polynomialMatrix = Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>>.FromMatrix(this, t => t);
            var lambdaMatrix = Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>>.Scalar(_rows, Polynomial<T, TField>.X);

            return polynomialMatrix - lambdaMatrix;
        }

        public Polynomial<T, TField> MinimalPolynomial<TField>() where TField : IField<T>, new()
        {
            Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>> left;
            Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>> right;

            return this.LambdaMatrix<TField>().SmithNormalForm(out left, out right)[_rows - 1, _rows - 1].MakeMonic();
        }

        public int Nullity()
        {
            return _columns - this.Rank();
        }

        public Matrix<T, TSpace>[] NullSpace()
        {
            List<Matrix<T, TSpace>> nullSpace = new List<Matrix<T, TSpace>>();

            Solve(this, null, nullSpace);

            return nullSpace.ToArray();
        }

        public Matrix<T, TSpace> Pow(int n)
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();
            if (n == 0)
                return Matrix<T, TSpace>.Identity(_rows);

            Matrix<T, TSpace> b = this;

            if (n < 0)
            {
                b = b.Inverse();
                n = -n;
            }

            Matrix<T, TSpace> result = b;

            n--;

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

        public int Rank()
        {
            int rank;

            this.Reduce(0, null, null, out rank);

            return rank;
        }

        public T Trace()
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();

            T sum = _field.Zero;

            for (int i = 0; i < _rows; i++)
                sum = _field.Add(sum, _m[i, i]);

            return sum;
        }

        public Matrix<T, TSpace> Transpose()
        {
            T[,] matrix = new T[_columns, _rows];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    matrix[j, i] = _m[i, j];
                }
            }

            return new Matrix<T, TSpace>(matrix);
        }

        #endregion

        #region Factorizations

        public Matrix<T, TSpace>[] GeneralizedEigenspace(T eigenvalue)
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();

            Matrix<T, TSpace> baseMatrix = this - Matrix<T, TSpace>.Scalar(_rows, eigenvalue);
            Matrix<T, TSpace>[] eigenspace = baseMatrix.NullSpace();

            for (int i = 0; i < eigenspace.Length; i++)
            {
                Matrix<T, TSpace> eigenvector = eigenspace[i];

                while (true)
                {
                    eigenvector = Matrix<T, TSpace>.Solve(baseMatrix, eigenvector, null);

                    if (eigenvector == null)
                        break;

                    eigenspace[i] = Matrix<T, TSpace>.FromColumns(new Matrix<T, TSpace>[] { eigenspace[i], eigenvector });
                }
            }

            return eigenspace;
        }

        public Matrix<T, TSpace> JordanNormalForm(T[] eigenvalues, out Matrix<T, TSpace> p, ICollection<KeyValuePair<T, int>> blockIndicies)
        {
            List<KeyValuePair<T, Matrix<T, TSpace>>> list = new List<KeyValuePair<T, Matrix<T, TSpace>>>();
            Matrix<T, TSpace>[] blocks;

            for (int i = 0; i < eigenvalues.Length; i++)
            {
                Matrix<T, TSpace>[] eigenspace = this.GeneralizedEigenspace(eigenvalues[i]);

                for (int j = 0; j < eigenspace.Length; j++)
                {
                    KeyValuePair<T, Matrix<T, TSpace>> pair = new KeyValuePair<T, Matrix<T, TSpace>>(eigenvalues[i], eigenspace[j]);

                    if (blockIndicies != null)
                        blockIndicies.Add(new KeyValuePair<T, int>(pair.Key, list.Count));

                    list.Add(pair);
                }
            }

            blocks = new Matrix<T, TSpace>[list.Count];

            for (int i = 0; i < list.Count; i++)
                blocks[i] = Matrix<T, TSpace>.JordanBlock(list[i].Value.Columns, list[i].Key);

            p = Matrix<T, TSpace>.FromColumns(list.ConvertAll(pair => pair.Value).ToArray());

            return Matrix<T, TSpace>.Diagonal(blocks);
        }

        public void LupDecomposition(out Matrix<T, TSpace> l, out Matrix<T, TSpace> u, out Matrix<T, TSpace> p)
        {
            if (!this.IsSquare)
                throw new InvalidOperationException();

            List<MatrixOperation<T>> operations = new List<MatrixOperation<T>>();
            int rank;
            Matrix<T, TSpace> upper;
            Matrix<T, TSpace> lower;
            Matrix<T, TSpace> permutation;

            upper = this.Reduce(0, operations, null, out rank);
            lower = Matrix<T, TSpace>.Identity(_rows);
            permutation = Matrix<T,TSpace>.Identity(_rows);

            for (int i = operations.Count - 1; i >= 0; i--)
            {
                switch (operations[i].Kind)
                {
                    case MatrixOperationKind.Swap:
                        permutation.ApplyRowOperation(operations[i]);
                        break;
                    case MatrixOperationKind.Multiply:
                    case MatrixOperationKind.Add:
                        lower.ApplyRowOperation(lower.InverseOperation(operations[i]));
                        break;
                }
            }

            l = lower;
            u = upper;
            p = permutation;
        }

        public Matrix<T, TSpace> RationalCanonicalForm<TField>(out Matrix<T, TSpace> p, ICollection<KeyValuePair<Polynomial<T, TField>, int>> companionIndicies) where TField : IField<T>, new()
        {
            var lambdaMatrix = this.LambdaMatrix<TField>();
            Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>> diagonal;
            Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>> left;
            Matrix<Polynomial<T, TField>, PolynomialSpace<T, TField>> right;
            int numberOfBlocks = 0;
            Matrix<T, TSpace>[] blocks;
            int index = 0;

            diagonal = lambdaMatrix.SmithNormalForm(out left, out right);

            for (int i = 0; i < _rows; i++)
            {
                if (!lambdaMatrix.Field.IsUnit(diagonal[i, i]))
                    numberOfBlocks++;
            }

            blocks = new Matrix<T, TSpace>[numberOfBlocks];

            // Make the invariant factors monic.
            for (int i = _rows - numberOfBlocks; i < _rows; i++)
            {
                Polynomial<T, TField> invariantFactor;

                invariantFactor = diagonal.M[i, i].MakeMonic();
                diagonal.M[i, i] = invariantFactor;

                if (companionIndicies != null)
                    companionIndicies.Add(new KeyValuePair<Polynomial<T, TField>, int>(invariantFactor, index));

                blocks[index++] = Matrix<T, TSpace>.Companion(invariantFactor);
            }

            var leftInverse = left.Inverse();
            Matrix<T, TSpace>[] basis = new Matrix<T, TSpace>[_columns];

            index = 0;

            for (int i = _rows - numberOfBlocks; i < _rows; i++)
            {
                int degree = diagonal[i, i].Degree;
                Matrix<T, TSpace> basisVector = new Matrix<T, TSpace>(_rows, 1);

                for (int j = 0; j < _rows; j++)
                {
                    basisVector += leftInverse[j, i].Evaluate(this).GetColumn(j);
                }

                basis[index++] = basisVector;

                for (int j = 0; j < degree - 1; j++)
                {
                    basisVector = this * basisVector;
                    basis[index++] = basisVector;
                }
            }

            p = Matrix<T, TSpace>.FromColumns(basis);

            return Matrix<T, TSpace>.Diagonal(blocks);
        }

        public Matrix<T, TSpace> SmithNormalForm(out Matrix<T, TSpace> left, out Matrix<T, TSpace> right)
        {
            IEuclideanDomain<T> euclideanDomain = _field as IEuclideanDomain<T>;

            if (euclideanDomain == null)
                throw new InvalidOperationException();

            Matrix<T, TSpace> m = new Matrix<T, TSpace>(this);
            int size = _rows < _columns ? _rows : _columns;
            MatrixOperation<T> operation;
            List<MatrixOperation<T>> rowOperations = new List<MatrixOperation<T>>();
            Matrix<T, TSpace> rowMatrix = Matrix<T, TSpace>.Identity(_rows);
            Matrix<T, TSpace> columnMatrix = Matrix<T, TSpace>.Identity(_columns);

            for (int index = 0; index < size; index++)
            {
                bool nonZero;

                while (true)
                {
                    BigInteger minimumEuclidean = 0;
                    int minimumEuclideanRow = -1;
                    int minimumEuclideanColumn = -1;
                    bool nonZeroInCross = false;

                    nonZero = false;

                    // Find the entry with minimum Euclidean value.

                    for (int i = index; i < _rows; i++)
                    {
                        for (int j = index; j < _columns; j++)
                        {
                            T entry = m.M[i, j];

                            if (_field.IsZero(entry))
                                continue;

                            BigInteger euclidean = euclideanDomain.Euclidean(entry);

                            if (!nonZero || minimumEuclidean > euclidean)
                            {
                                minimumEuclidean = euclidean;
                                minimumEuclideanRow = i;
                                minimumEuclideanColumn = j;
                            }

                            nonZero = true;

                            if ((i == index && j != index) || (i != index && j == index))
                                nonZeroInCross = true;
                        }
                    }

                    if (!nonZero)
                    {
                        // Off-diagonal entries are all zero; we are done.
                        break;
                    }

                    if (minimumEuclideanRow == index && minimumEuclideanColumn == index && !nonZeroInCross)
                    {
                        // No minimum and all other entries in the row/column are zero; we are done for this stage.
                        break;
                    }

                    // Bring the selected entry to the diagonal and reduce.

                    if (minimumEuclideanRow != index)
                    {
                        operation = new MatrixOperation<T>(MatrixOperationKind.Swap, index, minimumEuclideanRow);
                        rowMatrix.ApplyRowOperation(operation);
                        m.ApplyRowOperation(operation);
                    }

                    if (minimumEuclideanColumn != index)
                    {
                        operation = new MatrixOperation<T>(MatrixOperationKind.Swap, index, minimumEuclideanColumn);
                        columnMatrix.ApplyColumnOperation(operation);
                        m.ApplyColumnOperation(operation);
                    }

                    T minimumEntry = m.M[index, index];

                    for (int i = index + 1; i < _rows; i++)
                    {
                        T entry = m.M[i, index];

                        if (_field.IsZero(entry))
                            continue;

                        T quotient;
                        T remainder;

                        quotient = euclideanDomain.Divide(entry, minimumEntry, out remainder);

                        if (!_field.IsZero(quotient))
                        {
                            operation = new MatrixOperation<T>(MatrixOperationKind.Add, i, index, _field.Negate(quotient));
                            rowMatrix.ApplyRowOperation(operation);
                            m.ApplyRowOperation(operation);
                        }
                    }

                    for (int i = index + 1; i < _columns; i++)
                    {
                        T entry = m.M[index, i];

                        if (_field.IsZero(entry))
                            continue;

                        T quotient;
                        T remainder;

                        quotient = euclideanDomain.Divide(entry, minimumEntry, out remainder);

                        if (!_field.IsZero(quotient))
                        {
                            operation = new MatrixOperation<T>(MatrixOperationKind.Add, i, index, _field.Negate(quotient));
                            columnMatrix.ApplyColumnOperation(operation);
                            m.ApplyColumnOperation(operation);
                        }
                    }
                }

                if (!nonZero)
                    break;
            }

            left = rowMatrix;
            right = columnMatrix;

            return m;
        }

        #endregion

        #endregion

        #region Vector Functions

        public double Norm()
        {
            if (!_space.HasNorm || !this.IsVector)
                throw new InvalidOperationException();

            return _space.Norm(this);
        }

        #endregion
    }
}
