using BeatTheComputer.Shared;

using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeatTheComputer.AI.MCTS
{
    class MCTS : IBehavior
    {
        private double EXPLORE_FACTOR = 1.41;

        private IBehavior rolloutBehavior;
        private int rolloutsPerNode;
        private double timeLimit;
        private int iterationLimit;

        private MCTSTree[] trees;
        private IAction myLastAction;

        public MCTS(IBehavior rolloutBehavior, int numTrees, int rolloutsPerNode, double timeLimit, int iterationLimit = int.MaxValue)
        {
            this.rolloutBehavior = rolloutBehavior;
            this.rolloutsPerNode = rolloutsPerNode;
            this.timeLimit = timeLimit;
            this.iterationLimit = iterationLimit;

            trees = new MCTSTree[numTrees];
            myLastAction = null;
        }

        public IAction requestAction(IGameContext context, IAction opponentAction)
        {
            if (trees[0] == null) {
                for (int i = 0; i < trees.Length; i++) {
                    trees[i] = new MCTSTree(context.clone(), rolloutBehavior.clone(), rolloutsPerNode, EXPLORE_FACTOR);
                }
            }

            Dictionary<IAction, double>[] actionScoresList = new Dictionary<IAction, double>[trees.Length];

            IGameContext contextClone = context.clone();
            IAction opponentActionClone = null;
            if (opponentAction != null) {
                opponentActionClone = opponentAction.clone();
            }
            
            Parallel.For(0, trees.Length, i => {
                IGameContext myContextClone;
                lock (contextClone) {
                    myContextClone = contextClone.clone();
                }

                IAction myOpponentActionClone = null;
                if (opponentActionClone != null) {
                    lock (opponentActionClone) {
                        myOpponentActionClone = opponentAction.clone();
                    }
                }

                IAction myLastActionClone = null;
                if (myLastAction != null) {
                    lock (myLastAction) {
                        myLastActionClone = myLastAction.clone();
                    }
                }

                actionScoresList[i] = trees[i].run(timeLimit, iterationLimit, myContextClone, myLastActionClone, myOpponentActionClone);
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

        //does not save game tree
        public IBehavior clone()
        {
            return new MCTS(rolloutBehavior.clone(), trees.Length, rolloutsPerNode, timeLimit, iterationLimit);
        }
    }
}