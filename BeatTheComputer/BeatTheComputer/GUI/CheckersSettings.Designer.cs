namespace BeatTheComputer.GUI
{
    partial class CheckersSettings
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
            this.moveLimitField = new System.Windows.Forms.TextBox();
            this.pieceRowsField = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(38, 163);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 23;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(119, 163);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 24;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // colsField
            // 
            this.colsField.Location = new System.Drawing.Point(119, 59);
            this.colsField.MaxLength = 9;
            this.colsField.Name = "colsField";
            this.colsField.Size = new System.Drawing.Size(58, 20);
            this.colsField.TabIndex = 20;
            // 
            // rowsField
            // 
            this.rowsField.Location = new System.Drawing.Point(119, 33);
            this.rowsField.MaxLength = 9;
            this.rowsField.Name = "rowsField";
            this.rowsField.Size = new System.Drawing.Size(58, 20);
            this.rowsField.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Columns:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Rows:";
            // 
            // moveLimitField
            // 
            this.moveLimitField.Location = new System.Drawing.Point(119, 111);
            this.moveLimitField.MaxLength = 9;
            this.moveLimitField.Name = "moveLimitField";
            this.moveLimitField.Size = new System.Drawing.Size(58, 20);
            this.moveLimitField.TabIndex = 26;
            // 
            // pieceRowsField
            // 
            this.pieceRowsField.Location = new System.Drawing.Point(119, 85);
            this.pieceRowsField.MaxLength = 9;
            this.pieceRowsField.Name = "pieceRowsField";
            this.pieceRowsField.Size = new System.Drawing.Size(58, 20);
            this.pieceRowsField.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Move Limit:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Piece Rows:";
            // 
            // CheckersSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 212);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.moveLimitField);
            this.Controls.Add(this.pieceRowsField);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.colsField);
            this.Controls.Add(this.rowsField);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "CheckersSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.CheckersSettings_Load);
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
        private System.Windows.Forms.TextBox moveLimitField;
        private System.Windows.Forms.TextBox pieceRowsField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}