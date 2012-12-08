using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wj.Math;

namespace Test
{
    public partial class DFTForm : Form
    {
        private int _sequenceLength = 16;
        Complex[] _inputBuffer;
        Complex[] _outputBuffer;
        bool _outputAbsArg = false;
        FrequencyForm _frequencyForm;

        public DFTForm()
        {
            InitializeComponent();
            UpdateSequenceSettings();
            upDownSequenceLength.Value = _sequenceLength;

            _frequencyForm = new FrequencyForm();
            _frequencyForm.DataChanged += new EventHandler(frequencyForm_DataChanged);
        }

        private void frequencyForm_DataChanged(object sender, EventArgs e)
        {
            double frequency = _frequencyForm.Frequency * Math.PI / _sequenceLength;
            double phase = _frequencyForm.Phase;

            Array.Clear(sequenceInputIm.Sequence, 0, _sequenceLength);

            for (int i = 0; i < _sequenceLength; i++)
            {
                sequenceInputRe.Sequence[i] = Math.Sin(phase + frequency * i);
            }

            sequenceInputRe.Invalidate();
            sequenceInputIm.Invalidate();
            UpdateDFT();
        }

        private void upDownSequenceLength_ValueChanged(object sender, EventArgs e)
        {
            _sequenceLength = (int)upDownSequenceLength.Value;
            UpdateSequenceSettings();
            UpdateDFT();
        }

        private void sequenceInputRe_ValueChanged(object sender, EventArgs e)
        {
            UpdateDFT();
        }

        private void sequenceInputIm_ValueChanged(object sender, EventArgs e)
        {
            UpdateDFT();
        }

        private void UpdateSequenceSettings()
        {
            _inputBuffer = new Complex[_sequenceLength];
            _outputBuffer = new Complex[_sequenceLength];
            sequenceInputRe.SequenceLength = _sequenceLength;
            sequenceInputIm.SequenceLength = _sequenceLength;
            sequenceOutputRe.SequenceLength = _sequenceLength;
            sequenceOutputIm.SequenceLength = _sequenceLength;
            sequenceInputRe.VerticalMaximum = (double)upDownInputMaximum.Value;
            sequenceInputIm.VerticalMaximum = (double)upDownInputMaximum.Value;

            sequenceOutputRe.VerticalMaximum = (double)upDownOutputMaximum.Value;

            if (_outputAbsArg)
            {
                labelOutput1.Text = "Output Magnitude:";
                labelOutput2.Text = "Output Phase:";
                sequenceOutputRe.VerticalTwoSided = false;
                sequenceOutputIm.VerticalMaximum = Math.PI;
            }
            else
            {
                labelOutput1.Text = "Output Real:";
                labelOutput2.Text = "Output Imaginary:";
                sequenceOutputRe.VerticalTwoSided = true;
                sequenceOutputIm.VerticalMaximum = (double)upDownOutputMaximum.Value;
            }
        }

        private void UpdateDFT()
        {
            for (int i = 0; i < _sequenceLength; i++)
                _inputBuffer[i] = new Complex(sequenceInputRe.Sequence[i], sequenceInputIm.Sequence[i]);

            DiscreteFourierTransform(_inputBuffer, _outputBuffer);

            if (_outputAbsArg)
            {
                for (int i = 0; i < _sequenceLength; i++)
                {
                    sequenceOutputRe.Sequence[i] = _outputBuffer[i].Abs;
                    sequenceOutputIm.Sequence[i] = _outputBuffer[i].Arg;
                }
            }
            else
            {
                for (int i = 0; i < _sequenceLength; i++)
                {
                    sequenceOutputRe.Sequence[i] = _outputBuffer[i].Re;
                    sequenceOutputIm.Sequence[i] = _outputBuffer[i].Im;
                }
            }

            sequenceOutputRe.Invalidate();
            sequenceOutputIm.Invalidate();
        }

        private void DiscreteFourierTransform(Complex[] input, Complex[] output)
        {
            for (int k = 0; k < input.Length; k++)
            {
                Complex sum = 0;

                for (int n = 0; n < input.Length; n++)
                {
                    sum += input[n] * Complex.FromPolar(1, -2 * Math.PI * k / input.Length * n);
                }

                output[k] = sum;
            }
        }

        private void upDownInputMaximum_ValueChanged(object sender, EventArgs e)
        {
            UpdateSequenceSettings();
        }

        private void upDownOutputMaximum_ValueChanged(object sender, EventArgs e)
        {
            UpdateSequenceSettings();
        }

        private void checkOutputAbsArg_CheckedChanged(object sender, EventArgs e)
        {
            _outputAbsArg = checkOutputAbsArg.Checked;
            UpdateSequenceSettings();
            UpdateDFT();
        }

        private void buttonPresets_Click(object sender, EventArgs e)
        {
            ContextMenu menu = new ContextMenu();

            menu.MenuItems.Add("Zero", (a, b) =>
            {
                Array.Clear(sequenceInputRe.Sequence, 0, _sequenceLength);
                Array.Clear(sequenceInputIm.Sequence, 0, _sequenceLength);
                sequenceInputRe.Invalidate();
                sequenceInputIm.Invalidate();
                UpdateDFT();
            });
            menu.MenuItems.Add("Set Frequency", (a, b) =>
            {
                _frequencyForm.Show();
            });

            menu.Show(buttonPresets, new Point(0, buttonPresets.Height));
        }
    }
}
