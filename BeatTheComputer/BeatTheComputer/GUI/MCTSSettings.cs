using BeatTheComputer.AI.MCTS;
using BeatTheComputer.AI;

using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class MCTSSettings : Form
    {
        MCTS modified;

        public MCTSSettings(IBehavior mcts)
        {
            InitializeComponent();
            
            modified = (MCTS) mcts;
        }

        private void MCTSSettings_Load(object sender, System.EventArgs e)
        {
            tryToWinRadio.Checked = modified.TryingToWin;
            tryToLoseRadio.Checked = !modified.TryingToWin;
            thinkingTimeField.Text = (modified.TimeLimit / 1000).ToString();
            iterationLimitField.Text = modified.IterationLimit.ToString();
            rolloutsPerNodeField.Text = modified.RolloutsPerNode.ToString();
            parallelTreesField.Text = modified.NumTrees.ToString();
            exploreFactorField.Text = modified.ExploreFactor.ToString();
        } 

        private void cancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void apply_Click(object sender, System.EventArgs e)
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

            int rolloutsPerNode;
            if (!int.TryParse(rolloutsPerNodeField.Text, out rolloutsPerNode) || rolloutsPerNode <= 0) {
                errors += "Rollouts / Node must be a positive integer.\n";
            }

            int parallelTrees;
            if (!int.TryParse(parallelTreesField.Text, out parallelTrees) || parallelTrees <= 0) {
                errors += "Parallel Trees must be a positive integer.\n";
            }

            double exploreFactor;
            if (!double.TryParse(exploreFactorField.Text, out exploreFactor) || exploreFactor < 0) {
                errors += "Explore Factor must be a non-negative number.\n";
            }

            bool tryToWin = tryToWinRadio.Checked;

            if (errors.Length == 0) {
                modified.reset(modified.RolloutBehavior, parallelTrees, rolloutsPerNode, thinkingTime * 1000, iterationLimit, exploreFactor, tryToWin);
                this.Close();
            } else {
                MessageBox.Show(errors);
            }
        }

        public IBehavior Modified {
            get { return modified; }
        }
    }
}
