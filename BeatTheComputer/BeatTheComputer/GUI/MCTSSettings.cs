using BeatTheComputer.AI.MCTS;
using BeatTheComputer.AI;

using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class MCTSSettings : Form
    {
        private ObjectWrapper<IBehavior> mctsWrapper;

        public MCTSSettings(ObjectWrapper<IBehavior> behaviorWrapper)
        {
            InitializeComponent();
            
            mctsWrapper = behaviorWrapper;
        }

        private void MCTSSettings_Load(object sender, System.EventArgs e)
        {
            MCTS mcts = (MCTS) mctsWrapper.Reference;
            tryToWinRadio.Checked = mcts.TryingToWin;
            tryToLoseRadio.Checked = !mcts.TryingToWin;
            threadsField.Text = mcts.Threads.ToString();
            thinkingTimeField.Text = (mcts.TimeLimit / 1000).ToString();
            rolloutLimitField.Text = mcts.RolloutLimit.ToString();
            exploreFactorField.Text = mcts.ExploreFactor.ToString();
        } 

        private void cancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void apply_Click(object sender, System.EventArgs e)
        {
            string errors = "";

            int threads;
            if (!int.TryParse(threadsField.Text, out threads) || threads <= 0) {
                errors += "Threads must be a positive integer.\n";
            }

            double thinkingTime;
            if (!double.TryParse(thinkingTimeField.Text, out thinkingTime) || thinkingTime <= 0) {
                errors += "Thinking Time must be a positive number.\n";
            }

            int rolloutLimit;
            if (!int.TryParse(rolloutLimitField.Text, out rolloutLimit) || rolloutLimit <= 0) {
                errors += "Iteration Limit must be a positive integer.\n";
            }

            double exploreFactor;
            if (!double.TryParse(exploreFactorField.Text, out exploreFactor) || exploreFactor < 0) {
                errors += "Explore Factor must be a non-negative number.\n";
            }

            bool tryToWin = tryToWinRadio.Checked;

            if (errors.Length == 0) {
                MCTS mcts = (MCTS) mctsWrapper.Reference;
                mctsWrapper.Reference = new MCTS(mcts.RolloutBehavior, threads, thinkingTime * 1000, rolloutLimit, exploreFactor, tryToWin);
                this.Close();
            } else {
                MessageBox.Show(errors);
            }
        }
    }
}
