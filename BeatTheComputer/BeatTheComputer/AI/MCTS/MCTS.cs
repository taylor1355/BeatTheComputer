using BeatTheComputer.Shared;

using System.Threading;
using System.Collections.Generic;

namespace BeatTheComputer.AI.MCTS
{
    class MCTS : Behavior
    {
        private IBehavior rolloutBehavior;
        private int threads;
        private double timeLimit;
        private int rolloutLimit;
        private double exploreFactor;
        private bool tryToWin;

        private MCTSTree tree;
        private IAction myLastAction;

        public MCTS(IBehavior rolloutBehavior, int threads, double timeLimit, int rolloutLimit, double exploreFactor, bool tryToWin)
        {
            this.rolloutBehavior = rolloutBehavior;
            this.threads = threads;
            this.timeLimit = timeLimit;
            this.rolloutLimit = rolloutLimit;
            this.exploreFactor = exploreFactor;
            this.tryToWin = tryToWin;

            myLastAction = null;
        }

        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            if (tree == null) {
                tree = new MCTSTree(context.clone(), rolloutBehavior.clone(), exploreFactor, tryToWin);
            }

            Dictionary<IAction, double> actionScores = tree.run(threads, timeLimit, iterationLimit, context, myLastAction, opponentAction, interrupt);

            IAction bestAction = null;
            foreach (IAction action in actionScores.Keys) {
                if (bestAction == null || actionScores[action] > actionScores[bestAction]) {
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
            return new MCTS(rolloutBehavior.clone(), threads, timeLimit, iterationLimit, exploreFactor, tryToWin);
        }

        public IBehavior RolloutBehavior {
            get { return rolloutBehavior; }
        }

        public int Threads {
            get { return threads; }
        }

        public double TimeLimit {
            get { return timeLimit; }
        }

        public int RolloutLimit {
            get { return rolloutLimit; }
        }

        public double ExploreFactor {
            get { return exploreFactor; }
        }

        public bool TryingToWin {
            get { return tryToWin; }
        }
    }
}