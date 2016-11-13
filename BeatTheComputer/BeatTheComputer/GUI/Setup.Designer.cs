namespace BeatTheComputer.GUI
{
    partial class Setup
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
            this.p1List = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.p2List = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gameList = new System.Windows.Forms.ListBox();
            this.p1Settings = new System.Windows.Forms.Button();
            this.p2Settings = new System.Windows.Forms.Button();
            this.gameSettings = new System.Windows.Forms.Button();
            this.playGame = new System.Windows.Forms.Button();
            this.runSimulations = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // p1List
            // 
            this.p1List.FormattingEnabled = true;
            this.p1List.Location = new System.Drawing.Point(38, 45);
            this.p1List.Name = "p1List";
            this.p1List.Size = new System.Drawing.Size(158, 134);
            this.p1List.TabIndex = 3;
            this.p1List.SelectedIndexChanged += new System.EventHandler(this.p1List_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(85, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Player 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(279, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Player 2";
            // 
            // p2List
            // 
            this.p2List.FormattingEnabled = true;
            this.p2List.Location = new System.Drawing.Point(232, 45);
            this.p2List.Name = "p2List";
            this.p2List.Size = new System.Drawing.Size(158, 134);
            this.p2List.TabIndex = 5;
            this.p2List.SelectedIndexChanged += new System.EventHandler(this.p2List_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(481, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Game";
            // 
            // gameList
            // 
            this.gameList.FormattingEnabled = true;
            this.gameList.Location = new System.Drawing.Point(426, 45);
            this.gameList.Name = "gameList";
            this.gameList.Size = new System.Drawing.Size(158, 134);
            this.gameList.TabIndex = 7;
            this.gameList.SelectedIndexChanged += new System.EventHandler(this.gameList_SelectedIndexChanged);
            // 
            // p1Settings
            // 
            this.p1Settings.Location = new System.Drawing.Point(38, 185);
            this.p1Settings.Name = "p1Settings";
            this.p1Settings.Size = new System.Drawing.Size(158, 23);
            this.p1Settings.TabIndex = 9;
            this.p1Settings.Text = "Settings";
            this.p1Settings.UseVisualStyleBackColor = true;
            this.p1Settings.Click += new System.EventHandler(this.p1Settings_Click);
            // 
            // p2Settings
            // 
            this.p2Settings.Location = new System.Drawing.Point(232, 185);
            this.p2Settings.Name = "p2Settings";
            this.p2Settings.Size = new System.Drawing.Size(158, 23);
            this.p2Settings.TabIndex = 10;
            this.p2Settings.Text = "Settings";
            this.p2Settings.UseVisualStyleBackColor = true;
            this.p2Settings.Click += new System.EventHandler(this.p2Settings_Click);
            // 
            // gameSettings
            // 
            this.gameSettings.Location = new System.Drawing.Point(426, 185);
            this.gameSettings.Name = "gameSettings";
            this.gameSettings.Size = new System.Drawing.Size(158, 23);
            this.gameSettings.TabIndex = 11;
            this.gameSettings.Text = "Settings";
            this.gameSettings.UseVisualStyleBackColor = true;
            // 
            // playGame
            // 
            this.playGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playGame.Location = new System.Drawing.Point(154, 253);
            this.playGame.Name = "playGame";
            this.playGame.Size = new System.Drawing.Size(118, 81);
            this.playGame.TabIndex = 12;
            this.playGame.Text = "Play Game";
            this.playGame.UseVisualStyleBackColor = true;
            this.playGame.Click += new System.EventHandler(this.playGame_Click);
            // 
            // runSimulations
            // 
            this.runSimulations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runSimulations.Location = new System.Drawing.Point(354, 253);
            this.runSimulations.Name = "runSimulations";
            this.runSimulations.Size = new System.Drawing.Size(118, 81);
            this.runSimulations.TabIndex = 13;
            this.runSimulations.Text = "Run Simulations";
            this.runSimulations.UseVisualStyleBackColor = true;
            this.runSimulations.Click += new System.EventHandler(this.runSimulations_Click);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 371);
            this.Controls.Add(this.p1List);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.p1Settings);
            this.Controls.Add(this.runSimulations);
            this.Controls.Add(this.playGame);
            this.Controls.Add(this.gameSettings);
            this.Controls.Add(this.p2Settings);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gameList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.p2List);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Setup";
            this.Text = "Setup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setup_FormClosed);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox p1List;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox p2List;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox gameList;
        private System.Windows.Forms.Button p1Settings;
        private System.Windows.Forms.Button p2Settings;
        private System.Windows.Forms.Button gameSettings;
        private System.Windows.Forms.Button playGame;
        private System.Windows.Forms.Button runSimulations;
    }
}

