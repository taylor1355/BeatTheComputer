using BeatTheComputer.Core;

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

        private MCTS(MCTS cloneFrom)
        {
            rolloutBehavior = cloneFrom.rolloutBehavior.clone();
            threads = cloneFrom.threads;
            timeLimit = cloneFrom.timeLimit;
            rolloutLimit = cloneFrom.rolloutLimit;
            exploreFactor = cloneFrom.exploreFactor;
            tryToWin = cloneFrom.tryToWin;

            if (cloneFrom.tree == null) {
                tree = null;
            } else {
                tree = cloneFrom.tree.clone();
            }

            if (cloneFrom.myLastAction == null) {
                cloneFrom = null;
            } else {
                myLastAction = cloneFrom.myLastAction.clone();
            }
        }

        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            if (tree == null) {
                tree = new MCTSTree(context.clone(), rolloutBehavior.clone(), exploreFactor, tryToWin);
            }

            Dictionary<IAction, double> actionScores = tree.run(threads, timeLimit, rolloutLimit, context, myLastAction, opponentAction, interrupt);
            if (actionScores == null) return null;

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

        public override IBehavior clone()
        {
            return new MCTS(this);
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