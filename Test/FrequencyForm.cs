using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class FrequencyForm : Form
    {
        public event EventHandler DataChanged;

        public FrequencyForm()
        {
            InitializeComponent();
        }

        public double Frequency
        {
            get { return (double)trackFrequency.Value; }
        }

        public double Phase
        {
            get { return 2 * Math.PI * (double)trackPhase.Value / trackPhase.Maximum; }
        }

        private void trackFrequency_Scroll(object sender, EventArgs e)
        {
            if (this.DataChanged != null)
                this.DataChanged(this, null);
        }

        private void trackPhase_Scroll(object sender, EventArgs e)
        {
            if (this.DataChanged != null)
                this.DataChanged(this, null);
        }

        private void FrequencyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
    }
}
