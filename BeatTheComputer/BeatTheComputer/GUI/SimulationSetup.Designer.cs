namespace BeatTheComputer.GUI
{
    partial class SimulationSetup
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
            if (disposing && (components != null)) {
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
            this.run = new System.Windows.Forms.Button();
            this.alternateToggle = new System.Windows.Forms.CheckBox();
            this.simulationsField = new System.Windows.Forms.TextBox();
            this.parallelToggle = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Simulations:";
            // 
            // run
            // 
            this.run.Location = new System.Drawing.Point(101, 143);
            this.run.Name = "run";
            this.run.Size = new System.Drawing.Size(75, 23);
            this.run.TabIndex = 26;
            this.run.Text = "Run";
            this.run.UseVisualStyleBackColor = true;
            this.run.Click += new System.EventHandler(this.run_Click);
            // 
            // alternateToggle
            // 
            this.alternateToggle.Location = new System.Drawing.Point(92, 92);
            this.alternateToggle.Name = "alternateToggle";
            this.alternateToggle.Size = new System.Drawing.Size(104, 34);
            this.alternateToggle.TabIndex = 28;
            this.alternateToggle.Text = "Alternate Who Goes First";
            this.alternateToggle.UseVisualStyleBackColor = true;
            // 
            // simulationsField
            // 
            this.simulationsField.Location = new System.Drawing.Point(149, 34);
            this.simulationsField.MaxLength = 9;
            this.simulationsField.Name = "simulationsField";
            this.simulationsField.Size = new System.Drawing.Size(58, 20);
            this.simulationsField.TabIndex = 29;
            // 
            // parallelToggle
            // 
            this.parallelToggle.AutoSize = true;
            this.parallelToggle.Location = new System.Drawing.Point(92, 69);
            this.parallelToggle.Name = "parallelToggle";
            this.parallelToggle.Size = new System.Drawing.Size(94, 17);
            this.parallelToggle.TabIndex = 30;
            this.parallelToggle.Text = "Run in Parallel";
            this.parallelToggle.UseVisualStyleBackColor = true;
            // 
            // SimulationSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 195);
            this.Controls.Add(this.parallelToggle);
            this.Controls.Add(this.simulationsField);
            this.Controls.Add(this.alternateToggle);
            this.Controls.Add(this.run);
            this.Controls.Add(this.label1);
            this.Name = "SimulationSetup";
            this.Text = "Simulation Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button run;
        private System.Windows.Forms.CheckBox alternateToggle;
        private System.Windows.Forms.TextBox simulationsField;
        private System.Windows.Forms.CheckBox parallelToggle;
    }
}