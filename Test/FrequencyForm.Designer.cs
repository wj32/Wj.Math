namespace Test
{
    partial class FrequencyForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackFrequency = new System.Windows.Forms.TrackBar();
            this.trackPhase = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPhase)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Phase:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Frequency:";
            // 
            // trackFrequency
            // 
            this.trackFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackFrequency.Location = new System.Drawing.Point(78, 9);
            this.trackFrequency.Maximum = 100;
            this.trackFrequency.Name = "trackFrequency";
            this.trackFrequency.Size = new System.Drawing.Size(304, 45);
            this.trackFrequency.TabIndex = 2;
            this.trackFrequency.Scroll += new System.EventHandler(this.trackFrequency_Scroll);
            // 
            // trackPhase
            // 
            this.trackPhase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPhase.Location = new System.Drawing.Point(78, 60);
            this.trackPhase.Maximum = 100;
            this.trackPhase.Name = "trackPhase";
            this.trackPhase.Size = new System.Drawing.Size(304, 45);
            this.trackPhase.TabIndex = 3;
            this.trackPhase.Scroll += new System.EventHandler(this.trackPhase_Scroll);
            // 
            // FrequencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 110);
            this.Controls.Add(this.trackPhase);
            this.Controls.Add(this.trackFrequency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrequencyForm";
            this.Text = "Frequency";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrequencyForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPhase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackFrequency;
        private System.Windows.Forms.TrackBar trackPhase;
    }
}