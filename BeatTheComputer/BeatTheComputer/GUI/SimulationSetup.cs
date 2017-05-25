﻿using BeatTheComputer.Shared;
using BeatTheComputer.AI;

using System;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class SimulationSetup : Form
    {
        private IGameContext game;
        private IBehavior player1;
        private IBehavior player2;

        private int simulations;
        private bool parallel;
        private bool alternateFirst;

        public SimulationSetup(IGameContext game, IBehavior player1, IBehavior player2)
        {
            InitializeComponent();

            this.game = game.clone();
            this.player1 = player1.clone();
            this.player2 = player2.clone();

            simulations = 10;
            simulationsField.Text = simulations.ToString();

            parallel = true;
            parallelToggle.Checked = parallel;

            alternateFirst = true;
            alternateToggle.Checked = alternateFirst;
        }

        private void run_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(simulationsField.Text, out simulations) || simulations <= 0) {
                MessageBox.Show("Simulations must be a positive integer");
                return;
            }
            parallel = parallelToggle.Checked;
            alternateFirst = alternateToggle.Checked;

            SimulationMonitor monitor = new SimulationMonitor(game.clone(), player1.clone(), player2.clone(), simulations, parallel, alternateFirst);
            monitor.Show();
        }
    }
}