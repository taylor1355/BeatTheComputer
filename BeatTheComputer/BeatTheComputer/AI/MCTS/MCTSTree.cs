using BeatTheComputer.Shared;

using System.Collections.Generic;
using System;


namespace BeatTheComputer.AI.MCTS
{
    class MCTSTree
    {
        private MCTSNode root;
        private IGameContext rootContext;

        public MCTSTree(IGameContext rootContext, IBehavior rolloutBehavior, double exploreFactor)
        {
            root = new MCTSNode(rootContext, rolloutBehavior, exploreFactor);
            this.rootContext = rootContext;
        }

        public IDictionary<IAction, double> run(int iterations)
        {
            for (int i = 0; i < iterations; i++) {
                root.step();
            }
            IDictionary<IAction, double> actionValues = root.getActionValues(rootContext.getActivePlayerID());
            return root.getActionValues(rootContext.getActivePlayerID());
        }
    }
}