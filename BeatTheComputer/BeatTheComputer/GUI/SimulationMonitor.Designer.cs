namespace BeatTheComputer.GUI
{
    partial class SimulationMonitor
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
            this.components = new System.ComponentModel.Container();
            this.simulationsLbl = new System.Windows.Forms.Label();
            this.p1WinsLbl = new System.Windows.Forms.Label();
            this.p2WinsLbl = new System.Windows.Forms.Label();
            this.tiesLbl = new System.Windows.Forms.Label();
            this.timeElapsedLbl = new System.Windows.Forms.Label();
            this.simulationTimeLbl = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // simulationsLbl
            // 
            this.simulationsLbl.AutoSize = true;
            this.simulationsLbl.Location = new System.Drawing.Point(75, 23);
            this.simulationsLbl.Name = "simulationsLbl";
            this.simulationsLbl.Size = new System.Drawing.Size(63, 13);
            this.simulationsLbl.TabIndex = 0;
            this.simulationsLbl.Text = "Simulations:";
            // 
            // p1WinsLbl
            // 
            this.p1WinsLbl.AutoSize = true;
            this.p1WinsLbl.Location = new System.Drawing.Point(63, 47);
            this.p1WinsLbl.Name = "p1WinsLbl";
            this.p1WinsLbl.Size = new System.Drawing.Size(75, 13);
            this.p1WinsLbl.TabIndex = 1;
            this.p1WinsLbl.Text = "Player 1 Wins:";
            // 
            // p2WinsLbl
            // 
            this.p2WinsLbl.AutoSize = true;
            this.p2WinsLbl.Location = new System.Drawing.Point(63, 71);
            this.p2WinsLbl.Name = "p2WinsLbl";
            this.p2WinsLbl.Size = new System.Drawing.Size(75, 13);
            this.p2WinsLbl.TabIndex = 2;
            this.p2WinsLbl.Text = "Player 2 Wins:";
            // 
            // tiesLbl
            // 
            this.tiesLbl.AutoSize = true;
            this.tiesLbl.Location = new System.Drawing.Point(108, 95);
            this.tiesLbl.Name = "tiesLbl";
            this.tiesLbl.Size = new System.Drawing.Size(30, 13);
            this.tiesLbl.TabIndex = 3;
            this.tiesLbl.Text = "Ties:";
            // 
            // timeElapsedLbl
            // 
            this.timeElapsedLbl.AutoSize = true;
            this.timeElapsedLbl.Location = new System.Drawing.Point(46, 152);
            this.timeElapsedLbl.Name = "timeElapsedLbl";
            this.timeElapsedLbl.Size = new System.Drawing.Size(74, 13);
            this.timeElapsedLbl.TabIndex = 5;
            this.timeElapsedLbl.Text = "Time Elapsed:";
            // 
            // simulationTimeLbl
            // 
            this.simulationTimeLbl.AutoSize = true;
            this.simulationTimeLbl.Location = new System.Drawing.Point(17, 166);
            this.simulationTimeLbl.Name = "simulationTimeLbl";
            this.simulationTimeLbl.Size = new System.Drawing.Size(103, 13);
            this.simulationTimeLbl.TabIndex = 6;
            this.simulationTimeLbl.Text = "Time Per Simulation:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(27, 124);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(263, 23);
            this.progressBar.TabIndex = 7;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(215, 155);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // SimulationMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 197);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.simulationTimeLbl);
            this.Controls.Add(this.timeElapsedLbl);
            this.Controls.Add(this.tiesLbl);
            this.Controls.Add(this.p2WinsLbl);
            this.Controls.Add(this.p1WinsLbl);
            this.Controls.Add(this.simulationsLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SimulationMonitor";
            this.Text = "Simulation Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimulationMonitor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label simulationsLbl;
        private System.Windows.Forms.Label p1WinsLbl;
        private System.Windows.Forms.Label p2WinsLbl;
        private System.Windows.Forms.Label tiesLbl;
        private System.Windows.Forms.Label timeElapsedLbl;
        private System.Windows.Forms.Label simulationTimeLbl;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Timer timer;
    }
}