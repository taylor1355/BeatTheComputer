namespace BeatTheComputer.GUI
{
    partial class TicTacToeSettings
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
            this.cancel = new System.Windows.Forms.Button();
            this.apply = new System.Windows.Forms.Button();
            this.colsField = new System.Windows.Forms.TextBox();
            this.rowsField = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inARowField = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(47, 106);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 23;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(128, 106);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 24;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // colsField
            // 
            this.colsField.Location = new System.Drawing.Point(118, 44);
            this.colsField.MaxLength = 9;
            this.colsField.Name = "colsField";
            this.colsField.Size = new System.Drawing.Size(58, 20);
            this.colsField.TabIndex = 20;
            // 
            // rowsField
            // 
            this.rowsField.Location = new System.Drawing.Point(118, 18);
            this.rowsField.MaxLength = 9;
            this.rowsField.Name = "rowsField";
            this.rowsField.Size = new System.Drawing.Size(58, 20);
            this.rowsField.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Columns:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Rows:";
            // 
            // inARowField
            // 
            this.inARowField.Location = new System.Drawing.Point(118, 70);
            this.inARowField.MaxLength = 9;
            this.inARowField.Name = "inARowField";
            this.inARowField.Size = new System.Drawing.Size(58, 20);
            this.inARowField.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "X in a Row:";
            // 
            // TicTacToeSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 149);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inARowField);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.colsField);
            this.Controls.Add(this.rowsField);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.Name = "TicTacToeSettings";
            this.Text = "Game Settings";
            this.Load += new System.EventHandler(this.TicTacToeSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.TextBox colsField;
        private System.Windows.Forms.TextBox rowsField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inARowField;
    }
}