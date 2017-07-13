namespace BeatTheComputer.GUI
{
    partial class MCTSSettings
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
            this.tryToWinRadio = new System.Windows.Forms.RadioButton();
            this.tryToLoseRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.thinkingTimeField = new System.Windows.Forms.TextBox();
            this.rolloutLimitField = new System.Windows.Forms.TextBox();
            this.exploreFactorField = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.threadsField = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tryToLoseRadio);
            this.groupBox1.Controls.Add(this.tryToWinRadio);
            this.groupBox1.Location = new System.Drawing.Point(46, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(126, 82);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Goal";
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(110, 208);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 8;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(29, 208);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Thinking Time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Rollout Limit:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(184, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "s";
            // 
            // thinkingTimeField
            // 
            this.thinkingTimeField.Location = new System.Drawing.Point(126, 121);
            this.thinkingTimeField.MaxLength = 9;
            this.thinkingTimeField.Name = "thinkingTimeField";
            this.thinkingTimeField.Size = new System.Drawing.Size(58, 20);
            this.thinkingTimeField.TabIndex = 2;
            // 
            // rolloutLimitField
            // 
            this.rolloutLimitField.Location = new System.Drawing.Point(126, 145);
            this.rolloutLimitField.MaxLength = 9;
            this.rolloutLimitField.Name = "rolloutLimitField";
            this.rolloutLimitField.Size = new System.Drawing.Size(58, 20);
            this.rolloutLimitField.TabIndex = 3;
            // 
            // exploreFactorField
            // 
            this.exploreFactorField.Location = new System.Drawing.Point(126, 169);
            this.exploreFactorField.MaxLength = 9;
            this.exploreFactorField.Name = "exploreFactorField";
            this.exploreFactorField.Size = new System.Drawing.Size(58, 20);
            this.exploreFactorField.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Explore Factor:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Threads:";
            // 
            // threadsField
            // 
            this.threadsField.Location = new System.Drawing.Point(126, 97);
            this.threadsField.MaxLength = 9;
            this.threadsField.Name = "threadsField";
            this.threadsField.Size = new System.Drawing.Size(58, 20);
            this.threadsField.TabIndex = 17;
            // 
            // MCTSSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 252);
            this.Controls.Add(this.threadsField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.exploreFactorField);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.rolloutLimitField);
            this.Controls.Add(this.thinkingTimeField);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MCTSSettings";
            this.Text = "AI Settings";
            this.Load += new System.EventHandler(this.MCTSSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton tryToWinRadio;
        private System.Windows.Forms.RadioButton tryToLoseRadio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox thinkingTimeField;
        private System.Windows.Forms.TextBox rolloutLimitField;
        private System.Windows.Forms.TextBox exploreFactorField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox threadsField;
    }
}