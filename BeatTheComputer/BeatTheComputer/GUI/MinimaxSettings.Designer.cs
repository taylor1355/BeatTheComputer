namespace BeatTheComputer.GUI
{
    partial class MinimaxSettings
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
            this.iterationLimitField = new System.Windows.Forms.TextBox();
            this.thinkingTimeField = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.apply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tryToLoseRadio = new System.Windows.Forms.RadioButton();
            this.tryToWinRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // iterationLimitField
            // 
            this.iterationLimitField.Location = new System.Drawing.Point(128, 124);
            this.iterationLimitField.MaxLength = 9;
            this.iterationLimitField.Name = "iterationLimitField";
            this.iterationLimitField.Size = new System.Drawing.Size(58, 20);
            this.iterationLimitField.TabIndex = 18;
            // 
            // thinkingTimeField
            // 
            this.thinkingTimeField.Location = new System.Drawing.Point(128, 100);
            this.thinkingTimeField.MaxLength = 9;
            this.thinkingTimeField.Name = "thinkingTimeField";
            this.thinkingTimeField.Size = new System.Drawing.Size(58, 20);
            this.thinkingTimeField.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(187, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "s";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Iteration Limit:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Thinking Time:";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(30, 155);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 21;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(111, 155);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 22;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tryToLoseRadio);
            this.groupBox1.Controls.Add(this.tryToWinRadio);
            this.groupBox1.Location = new System.Drawing.Point(48, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(126, 82);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Goal";
            // 
            // tryToLoseRadio
            // 
            this.tryToLoseRadio.AutoSize = true;
            this.tryToLoseRadio.Location = new System.Drawing.Point(13, 43);
            this.tryToLoseRadio.Name = "tryToLoseRadio";
            this.tryToLoseRadio.Size = new System.Drawing.Size(78, 17);
            this.tryToLoseRadio.TabIndex = 1;
            this.tryToLoseRadio.TabStop = true;
            this.tryToLoseRadio.Text = "Try to Lose";
            this.tryToLoseRadio.UseVisualStyleBackColor = true;
            // 
            // tryToWinRadio
            // 
            this.tryToWinRadio.AutoSize = true;
            this.tryToWinRadio.Location = new System.Drawing.Point(13, 19);
            this.tryToWinRadio.Name = "tryToWinRadio";
            this.tryToWinRadio.Size = new System.Drawing.Size(74, 17);
            this.tryToWinRadio.TabIndex = 0;
            this.tryToWinRadio.TabStop = true;
            this.tryToWinRadio.Text = "Try to Win";
            this.tryToWinRadio.UseVisualStyleBackColor = true;
            // 
            // MinimaxSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 194);
            this.Controls.Add(this.iterationLimitField);
            this.Controls.Add(this.thinkingTimeField);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.groupBox1);
            this.Name = "MinimaxSettings";
            this.Text = "AI Settings";
            this.Load += new System.EventHandler(this.MinimaxSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox iterationLimitField;
        private System.Windows.Forms.TextBox thinkingTimeField;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton tryToLoseRadio;
        private System.Windows.Forms.RadioButton tryToWinRadio;
    }
}