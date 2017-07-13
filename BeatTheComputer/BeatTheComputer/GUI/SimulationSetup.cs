using BeatTheComputer.Core;
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

        public void setGame(IGameContext game)
        {
            this.game = game.clone();
        }

        public void setPlayer1(IBehavior player1)
        {
            this.player1 = player1.clone();
        }

        public void setPlayer2(IBehavior player2)
        {
            this.player2 = player2.clone();
        }

        private void run_Click(object sender, EventArgs e)
        {
            if (player1 is DummyBehavior || player2 is DummyBehavior) {
                MessageBox.Show("Can't run simulations with a human");
                return;
            } else if (!int.TryParse(simulationsField.Text, out simulations) || simulations <= 0) {
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
