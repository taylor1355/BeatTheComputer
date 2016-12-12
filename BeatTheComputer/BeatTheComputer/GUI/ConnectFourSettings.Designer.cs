namespace BeatTheComputer.GUI
{
    partial class ConnectFourSettings
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
            this.colsField = new System.Windows.Forms.TextBox();
            this.rowsField = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.apply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // colsField
            // 
            this.colsField.Location = new System.Drawing.Point(97, 49);
            this.colsField.MaxLength = 9;
            this.colsField.Name = "colsField";
            this.colsField.Size = new System.Drawing.Size(58, 20);
            this.colsField.TabIndex = 14;
            // 
            // rowsField
            // 
            this.rowsField.Location = new System.Drawing.Point(97, 23);
            this.rowsField.MaxLength = 9;
            this.rowsField.Name = "rowsField";
            this.rowsField.Size = new System.Drawing.Size(58, 20);
            this.rowsField.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Columns:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Rows:";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(26, 93);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 17;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(107, 93);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 18;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // ConnectFourSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 133);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.colsField);
            this.Controls.Add(this.rowsField);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "ConnectFourSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.ConnectFourSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox colsField;
        private System.Windows.Forms.TextBox rowsField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button apply;
    }
}