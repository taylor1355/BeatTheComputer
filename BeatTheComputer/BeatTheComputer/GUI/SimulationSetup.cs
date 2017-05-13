using BeatTheComputer.Shared;
using BeatTheComputer.AI;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class SimulationSetup : Form
    {
        private IGameContext game;
        private IBehavior player1;
        private IBehavior player2;

        private int numTrials;
        private bool parallel;
        private bool alternateFirst;

        public SimulationSetup()
        {
            InitializeComponent();

            numTrials = 10;
            trialsField.Text = numTrials.ToString();

            parallel = true;
            parallelToggle.Checked = parallel;

            alternateFirst = true;
            alternateToggle.Checked = alternateFirst;
        }

        public void setPlayer(Player player, IBehavior newBehavior)
        {
            if (player == Player.ONE) {
                player1 = newBehavior;
            } else if (player == Player.TWO) {
                player2 = newBehavior;
            } else {
                throw new ArgumentException("Invalid player");
            }
        }

        private void SimulationSetup_Load(object sender, EventArgs e)
        {

        }

        private void run_Click(object sender, EventArgs e)
        {
            
        }
    }
}
