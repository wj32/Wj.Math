using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class SequenceControl : UserControl
    {
        [Browsable(true)]
        public event EventHandler ValueChanged;

        private const int DefaultSequenceLength = 16;
        private const int LeftMargin = 40;
        private const int RightMargin = 20;
        private const int TopMargin = 30;
        private const int BottomMargin = 30;
        private const int CircleRadius = 4;

        private bool _readOnly = false;
        private int _sequenceLength = DefaultSequenceLength;
        private double[] _sequence = new double[DefaultSequenceLength];
        private bool _verticalScaleFixed = true;
        private double _verticalMaximum = 1;
        private bool _verticalTwoSided = true;
        private bool _editing = false;

        public SequenceControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public int SequenceLength
        {
            get { return _sequenceLength; }
            set
            {
                _sequenceLength = value;
                Array.Resize(ref _sequence, _sequenceLength);
                this.Invalidate();
            }
        }

        public double[] Sequence
        {
            get { return _sequence; }
        }

        public bool VerticalScaleFixed
        {
            get { return _verticalScaleFixed; }
            set { _verticalScaleFixed = value; this.Invalidate(); }
        }

        public double VerticalMaximum
        {
            get { return _verticalMaximum; }
            set { _verticalMaximum = value; this.Invalidate(); }
        }

        public bool VerticalTwoSided
        {
            get { return _verticalTwoSided; }
            set { _verticalTwoSided = value; this.Invalidate(); }
        }

        private double CalculateMaximum()
        {
            double maximum;

            if (_verticalScaleFixed)
            {
                maximum = _verticalMaximum;
            }
            else
            {
                maximum = 0;

                for (int i = 0; i < _sequenceLength; i++)
                {
                    if (maximum < _sequence[i])
                        maximum = _sequence[i];
                }

                if (maximum == 0)
                    maximum = 1;
            }

            return maximum;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = this.Width;
            int height = this.Height;
            double maximum;
            Pen thickPen = new Pen(Color.Black, 2);

            maximum = CalculateMaximum();
            g.Clear(this.BackColor);

            int availableWidth = width - LeftMargin - RightMargin;
            int availableHeight;
            double sequenceSpacing = (double)availableWidth / _sequenceLength;
            int middle;

            // y-axis

            if (_verticalTwoSided)
            {
                availableHeight = (height - TopMargin - BottomMargin) / 2;
                middle = TopMargin + (height - TopMargin - BottomMargin) / 2;
            }
            else
            {
                availableHeight = height - TopMargin - BottomMargin;
                middle = height - BottomMargin;
            }

            g.DrawLine(thickPen, LeftMargin, height - BottomMargin, LeftMargin, TopMargin);
            g.DrawString(maximum.ToString("F2"), this.Font, Brushes.Black, 0, TopMargin);
            g.DrawString("0", this.Font, Brushes.Black, 0, middle);

            if (_verticalTwoSided)
                g.DrawString((-maximum).ToString("F2"), this.Font, Brushes.Black, 0, height - BottomMargin);

            // x-axis
            g.DrawLine(thickPen, LeftMargin, middle, width - RightMargin, middle);

            for (int i = 0; i < _sequenceLength; i++)
            {
                int endPoint = middle - (int)Math.Round(availableHeight * _sequence[i] / maximum);
                int x = LeftMargin + (int)(i * sequenceSpacing);

                g.DrawLine(Pens.Black, new Point(x, middle), new Point(x, endPoint));
                g.FillEllipse(Brushes.Black, x - CircleRadius, endPoint - CircleRadius, CircleRadius * 2, CircleRadius * 2);
            }

            thickPen.Dispose();
        }

        private void ChangeValueAtMouse(int x, int y)
        {
            int width = this.Width;
            int height = this.Height;
            double maximum = CalculateMaximum();
            int availableWidth = width - LeftMargin - RightMargin;
            int availableHeight;
            double sequenceSpacing = (double)availableWidth / _sequenceLength;
            int middle;

            if (sequenceSpacing == 0)
                return;

            if (_verticalTwoSided)
            {
                availableHeight = (height - TopMargin - BottomMargin) / 2;
                middle = TopMargin + (height - TopMargin - BottomMargin) / 2;
            }
            else
            {
                availableHeight = height - TopMargin - BottomMargin;
                middle = height - BottomMargin;
            }

            double approxIndex = (double)(x - LeftMargin) / sequenceSpacing;
            int index = (int)Math.Round(approxIndex);

            if (index < 0 || index >= _sequenceLength)
                return;

            double newValue = (double)(middle - y) / availableHeight * maximum;

            if (_verticalTwoSided)
            {
                if (newValue > maximum)
                    newValue = maximum;
                else if (newValue < -maximum)
                    newValue = -maximum;
            }
            else
            {
                if (newValue > maximum)
                    newValue = maximum;
                else if (newValue < 0)
                    newValue = 0;
            }

            _sequence[index] = newValue;
            this.Invalidate();

            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _editing = true;

            if (!_readOnly)
                ChangeValueAtMouse(e.X, e.Y);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _editing = false;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _editing = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_editing && !_readOnly)
                ChangeValueAtMouse(e.X, e.Y);
        }
    }
}
