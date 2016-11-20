using BeatTheComputer.Shared;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeatTheComputer.AI.Minimax
{
    class Minimax : Behavior
    {
        private double timeLimit;
        private int iterationLimit;
        private bool tryToWin;

        private MinimaxTree[] trees;
        private IAction myLastAction;

        public Minimax(int numTrees, double timeLimit, int iterationLimit, bool tryToWin)
        {
            this.timeLimit = timeLimit;
            this.iterationLimit = iterationLimit;
            this.tryToWin = tryToWin;

            trees = new MinimaxTree[numTrees];
            myLastAction = null;
        }

        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            if (trees[0] == null) {
                for (int i = 0; i < trees.Length; i++) {
                    trees[i] = new MinimaxTree(context.clone(), tryToWin);
                }
            }

            Dictionary<IAction, double>[] actionScoresList = new Dictionary<IAction, double>[trees.Length];

            Parallel.For(0, trees.Length, i => {
                IGameContext contextClone;
                lock (context) {
                    contextClone = context.clone();
                }

                IAction opponentActionClone = null;
                if (opponentAction != null) {
                    lock (opponentAction) {
                        opponentActionClone = opponentAction.clone();
                    }
                }

                IAction myActionClone = null;
                if (myLastAction != null) {
                    lock (myLastAction) {
                        myActionClone = myLastAction.clone();
                    }
                }

                actionScoresList[i] = trees[i].run(timeLimit, iterationLimit, contextClone, myActionClone, opponentActionClone, interrupt);
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
            return "Minimax";
        }

        public override IBehavior clone()
        {
            return new Minimax(trees.Length, timeLimit, iterationLimit, tryToWin);
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

        public bool TryingToWin {
            get { return tryToWin; }
        }
    }
}
