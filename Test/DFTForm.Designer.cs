namespace Test
{
    partial class DFTForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelOutput2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelOutput1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.upDownInputMaximum = new System.Windows.Forms.NumericUpDown();
            this.upDownSequenceLength = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.checkOutputAbsArg = new System.Windows.Forms.CheckBox();
            this.upDownOutputMaximum = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonPresets = new System.Windows.Forms.Button();
            this.sequenceOutputIm = new Test.SequenceControl();
            this.sequenceInputIm = new Test.SequenceControl();
            this.sequenceOutputRe = new Test.SequenceControl();
            this.sequenceInputRe = new Test.SequenceControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownInputMaximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSequenceLength)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownOutputMaximum)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1035, 553);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.sequenceOutputIm);
            this.panel4.Controls.Add(this.labelOutput2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(520, 264);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(512, 255);
            this.panel4.TabIndex = 3;
            // 
            // labelOutput2
            // 
            this.labelOutput2.AutoSize = true;
            this.labelOutput2.Location = new System.Drawing.Point(9, 6);
            this.labelOutput2.Name = "labelOutput2";
            this.labelOutput2.Size = new System.Drawing.Size(90, 13);
            this.labelOutput2.TabIndex = 0;
            this.labelOutput2.Text = "Output Imaginary:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.sequenceInputIm);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 264);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(511, 255);
            this.panel3.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Input Imaginary:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sequenceOutputRe);
            this.panel2.Controls.Add(this.labelOutput1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(520, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(512, 255);
            this.panel2.TabIndex = 1;
            // 
            // labelOutput1
            // 
            this.labelOutput1.AutoSize = true;
            this.labelOutput1.Location = new System.Drawing.Point(9, 6);
            this.labelOutput1.Name = "labelOutput1";
            this.labelOutput1.Size = new System.Drawing.Size(67, 13);
            this.labelOutput1.TabIndex = 0;
            this.labelOutput1.Text = "Output Real:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sequenceInputRe);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(511, 255);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input Real:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.buttonPresets);
            this.panel5.Controls.Add(this.upDownInputMaximum);
            this.panel5.Controls.Add(this.upDownSequenceLength);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 525);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(511, 25);
            this.panel5.TabIndex = 4;
            // 
            // upDownInputMaximum
            // 
            this.upDownInputMaximum.Location = new System.Drawing.Point(63, 3);
            this.upDownInputMaximum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownInputMaximum.Name = "upDownInputMaximum";
            this.upDownInputMaximum.Size = new System.Drawing.Size(120, 20);
            this.upDownInputMaximum.TabIndex = 1;
            this.upDownInputMaximum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownInputMaximum.ValueChanged += new System.EventHandler(this.upDownInputMaximum_ValueChanged);
            // 
            // upDownSequenceLength
            // 
            this.upDownSequenceLength.Location = new System.Drawing.Point(388, 4);
            this.upDownSequenceLength.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.upDownSequenceLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownSequenceLength.Name = "upDownSequenceLength";
            this.upDownSequenceLength.Size = new System.Drawing.Size(120, 20);
            this.upDownSequenceLength.TabIndex = 1;
            this.upDownSequenceLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownSequenceLength.ValueChanged += new System.EventHandler(this.upDownSequenceLength_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Maximum:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(287, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Sequence Length:";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.checkOutputAbsArg);
            this.panel6.Controls.Add(this.upDownOutputMaximum);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(520, 525);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(512, 25);
            this.panel6.TabIndex = 5;
            // 
            // checkOutputAbsArg
            // 
            this.checkOutputAbsArg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkOutputAbsArg.AutoSize = true;
            this.checkOutputAbsArg.Location = new System.Drawing.Point(397, 5);
            this.checkOutputAbsArg.Name = "checkOutputAbsArg";
            this.checkOutputAbsArg.Size = new System.Drawing.Size(111, 17);
            this.checkOutputAbsArg.TabIndex = 2;
            this.checkOutputAbsArg.Text = "Magnitude/Phase";
            this.checkOutputAbsArg.UseVisualStyleBackColor = true;
            this.checkOutputAbsArg.CheckedChanged += new System.EventHandler(this.checkOutputAbsArg_CheckedChanged);
            // 
            // upDownOutputMaximum
            // 
            this.upDownOutputMaximum.Location = new System.Drawing.Point(63, 3);
            this.upDownOutputMaximum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownOutputMaximum.Name = "upDownOutputMaximum";
            this.upDownOutputMaximum.Size = new System.Drawing.Size(120, 20);
            this.upDownOutputMaximum.TabIndex = 1;
            this.upDownOutputMaximum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownOutputMaximum.ValueChanged += new System.EventHandler(this.upDownOutputMaximum_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Maximum:";
            // 
            // buttonPresets
            // 
            this.buttonPresets.Location = new System.Drawing.Point(202, 1);
            this.buttonPresets.Name = "buttonPresets";
            this.buttonPresets.Size = new System.Drawing.Size(75, 23);
            this.buttonPresets.TabIndex = 2;
            this.buttonPresets.Text = "Presets";
            this.buttonPresets.UseVisualStyleBackColor = true;
            this.buttonPresets.Click += new System.EventHandler(this.buttonPresets_Click);
            // 
            // sequenceOutputIm
            // 
            this.sequenceOutputIm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sequenceOutputIm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sequenceOutputIm.Location = new System.Drawing.Point(3, 22);
            this.sequenceOutputIm.Name = "sequenceOutputIm";
            this.sequenceOutputIm.ReadOnly = true;
            this.sequenceOutputIm.SequenceLength = 16;
            this.sequenceOutputIm.Size = new System.Drawing.Size(505, 230);
            this.sequenceOutputIm.TabIndex = 2;
            this.sequenceOutputIm.VerticalMaximum = 1D;
            this.sequenceOutputIm.VerticalScaleFixed = true;
            this.sequenceOutputIm.VerticalTwoSided = true;
            // 
            // sequenceInputIm
            // 
            this.sequenceInputIm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sequenceInputIm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sequenceInputIm.Location = new System.Drawing.Point(3, 22);
            this.sequenceInputIm.Name = "sequenceInputIm";
            this.sequenceInputIm.ReadOnly = false;
            this.sequenceInputIm.SequenceLength = 16;
            this.sequenceInputIm.Size = new System.Drawing.Size(505, 230);
            this.sequenceInputIm.TabIndex = 2;
            this.sequenceInputIm.VerticalMaximum = 1D;
            this.sequenceInputIm.VerticalScaleFixed = true;
            this.sequenceInputIm.VerticalTwoSided = true;
            this.sequenceInputIm.ValueChanged += new System.EventHandler(this.sequenceInputIm_ValueChanged);
            // 
            // sequenceOutputRe
            // 
            this.sequenceOutputRe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sequenceOutputRe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sequenceOutputRe.Location = new System.Drawing.Point(3, 22);
            this.sequenceOutputRe.Name = "sequenceOutputRe";
            this.sequenceOutputRe.ReadOnly = true;
            this.sequenceOutputRe.SequenceLength = 16;
            this.sequenceOutputRe.Size = new System.Drawing.Size(505, 230);
            this.sequenceOutputRe.TabIndex = 2;
            this.sequenceOutputRe.VerticalMaximum = 1D;
            this.sequenceOutputRe.VerticalScaleFixed = true;
            this.sequenceOutputRe.VerticalTwoSided = true;
            // 
            // sequenceInputRe
            // 
            this.sequenceInputRe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sequenceInputRe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sequenceInputRe.Location = new System.Drawing.Point(3, 22);
            this.sequenceInputRe.Name = "sequenceInputRe";
            this.sequenceInputRe.ReadOnly = false;
            this.sequenceInputRe.SequenceLength = 16;
            this.sequenceInputRe.Size = new System.Drawing.Size(505, 230);
            this.sequenceInputRe.TabIndex = 1;
            this.sequenceInputRe.VerticalMaximum = 1D;
            this.sequenceInputRe.VerticalScaleFixed = true;
            this.sequenceInputRe.VerticalTwoSided = true;
            this.sequenceInputRe.ValueChanged += new System.EventHandler(this.sequenceInputRe_ValueChanged);
            // 
            // DFTForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 553);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DFTForm";
            this.Text = "DFT";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownInputMaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSequenceLength)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownOutputMaximum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labelOutput2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelOutput1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.NumericUpDown upDownSequenceLength;
        private System.Windows.Forms.Label label5;
        private SequenceControl sequenceOutputIm;
        private SequenceControl sequenceInputIm;
        private SequenceControl sequenceOutputRe;
        private SequenceControl sequenceInputRe;
        private System.Windows.Forms.NumericUpDown upDownInputMaximum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.NumericUpDown upDownOutputMaximum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkOutputAbsArg;
        private System.Windows.Forms.Button buttonPresets;

    }
}