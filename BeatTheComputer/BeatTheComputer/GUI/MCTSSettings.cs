using BeatTheComputer.AI.MCTS;
using BeatTheComputer.AI;

using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class MCTSSettings : Form
    {
        ObjectWrapper<IBehavior> mctsWrapper;

        public MCTSSettings(ObjectWrapper<IBehavior> behaviorWrapper)
        {
            InitializeComponent();
            
            this.mctsWrapper = behaviorWrapper;
        }

        private void MCTSSettings_Load(object sender, System.EventArgs e)
        {
            MCTS mcts = (MCTS) mctsWrapper.Reference;
            tryToWinRadio.Checked = mcts.TryingToWin;
            tryToLoseRadio.Checked = !mcts.TryingToWin;
            thinkingTimeField.Text = (mcts.TimeLimit / 1000).ToString();
            iterationLimitField.Text = mcts.IterationLimit.ToString();
            parallelTreesField.Text = mcts.NumTrees.ToString();
            exploreFactorField.Text = mcts.ExploreFactor.ToString();
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
                MCTS mcts = (MCTS) mctsWrapper.Reference;
                mctsWrapper.Reference = new MCTS(mcts.RolloutBehavior, parallelTrees, thinkingTime * 1000, iterationLimit, exploreFactor, tryToWin);
                this.Close();
            } else {
                MessageBox.Show(errors);
            }
        }

        public IBehavior mcts {
            get { return mcts; }
        }
    }
}
