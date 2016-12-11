using BeatTheComputer.AI.Minimax;
using BeatTheComputer.AI;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class MinimaxSettings : Form
    {
        ObjectWrapper<IBehavior> minimaxWrapper;

        public MinimaxSettings(ObjectWrapper<IBehavior> behaviorWrapper)
        {
            InitializeComponent();

            minimaxWrapper = behaviorWrapper;
        }

        private void MinimaxSettings_Load(object sender, EventArgs e)
        {
            Minimax minimax = (Minimax) minimaxWrapper.Reference;
            tryToWinRadio.Checked = minimax.TryingToWin;
            tryToLoseRadio.Checked = !minimax.TryingToWin;
            thinkingTimeField.Text = (minimax.TimeLimit / 1000).ToString();
            iterationLimitField.Text = minimax.IterationLimit.ToString();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void apply_Click(object sender, EventArgs e)
        {
            string errors = "";

            double thinkingTime;
            if (!double.TryParse(thinkingTimeField.Text, out thinkingTime) || thinkingTime <= 0) {
                errors += "Thinking Time must be a positive number.\n";
            }

            int iterationLimit;
            if (!int.TryParse(iterationLimitField.Text, out iterationLimit) || iterationLimit <= 0) {
                errors += "Iteration Limit must be a positive integer.\n";
            }

            bool tryToWin = tryToWinRadio.Checked;

            if (errors.Length == 0) {
                Minimax minimax = (Minimax) minimaxWrapper.Reference;
                minimaxWrapper.Reference = new Minimax(thinkingTime * 1000, iterationLimit, tryToWin);
                this.Close();
            } else {
                MessageBox.Show(errors);
            }
        }
    }
}
