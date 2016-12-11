using BeatTheComputer.Shared;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeatTheComputer.AI.MCTS
{
    class MCTS : Behavior
    {
        private IBehavior rolloutBehavior;
        private double timeLimit;
        private int iterationLimit;
        private double exploreFactor;
        private bool tryToWin;

        private MCTSTree[] trees;
        private IAction myLastAction;

        public MCTS(IBehavior rolloutBehavior, int numTrees, double timeLimit, int iterationLimit, double exploreFactor, bool tryToWin)
        {
            this.rolloutBehavior = rolloutBehavior;
            this.timeLimit = timeLimit;
            this.iterationLimit = iterationLimit;
            this.exploreFactor = exploreFactor;
            this.tryToWin = tryToWin;

            trees = new MCTSTree[numTrees];
            myLastAction = null;
        }

        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            if (trees[0] == null) {
                for (int i = 0; i < trees.Length; i++) {
                    trees[i] = new MCTSTree(context.clone(), rolloutBehavior.clone(), exploreFactor, tryToWin);
                }
            }

            Dictionary<IAction, double>[] actionScoresList = new Dictionary<IAction, double>[trees.Length];
            
            Parallel.For(0, trees.Length, i => {
                IGameContext contextClone;
                lock (context) {
                    contextClone = context.clone();
                }

                actionScoresList[i] = trees[i].run(timeLimit, iterationLimit, contextClone, myLastAction, opponentAction, interrupt);
            });

            Dictionary<IAction, double> averageActionScores = new Dictionary<IAction, double>();
            for (int i = 0; i < actionScoresList.Length; i++) {
                foreach (IAction action in actionScoresList[i].Keys) {
                    if (averageActionScores.ContainsKey(action)) {
                        averageActionScores[action] += actionScoresList[i][action] / trees.Length;
                    } else {
                        averageActionScores.Add(action, actionScoresList[i][action] / trees.Length);
                    }
                }
            }

            IAction bestAction = null;
            foreach (IAction action in averageActionScores.Keys) {
                if (bestAction == null || averageActionScores[action] > averageActionScores[bestAction]) {
                    bestAction = action;
                }
            }
            myLastAction = bestAction;
            return bestAction;
        }

        public override string ToString()
        {
            return "Monte Carlo Tree Search";
        }

        //does not save game tree
        public override IBehavior clone()
        {
            return new MCTS(rolloutBehavior.clone(), trees.Length, timeLimit, iterationLimit, exploreFactor, tryToWin);
        }

        public IBehavior RolloutBehavior {
            get { return rolloutBehavior; }
        }

        public int NumTrees {
            get { return trees.Length; }
        }

        public double TimeLimit {
            get { return timeLimit; }
        }

        public int IterationLimit {
            get { return iterationLimit; }
        }

        public double ExploreFactor {
            get { return exploreFactor; }
        }

        public bool TryingToWin {
            get { return tryToWin; }
        }
    }
}