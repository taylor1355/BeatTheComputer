using BeatTheComputer.Shared;

using System.Collections.Generic;

namespace BeatTheComputer.AI.MCTS
{
    class MCTS : IBehavior
    {
        private const double EXPLORE_FACTOR = 1.4;

        private IBehavior rolloutBehavior;
        private int numIterations;

        public MCTS(IBehavior rolloutBehavior, int numIterations)
        {
            this.rolloutBehavior = rolloutBehavior;
            this.numIterations = numIterations;
        }

        public IAction requestAction(IGameContext context)
        {
            MCTSTree tree = new MCTSTree(context, rolloutBehavior, EXPLORE_FACTOR);
            IDictionary<IAction, double> actionValues = tree.run(numIterations);

            IAction bestAction = null;
            foreach (IAction action in actionValues.Keys) {
                if (bestAction == null || actionValues[action] > actionValues[bestAction]) {
                    bestAction = action;
                }
            }

            return bestAction;
        }

        public IBehavior clone()
        {
            return new MCTS(rolloutBehavior.clone(), numIterations);
        }
    }
}
