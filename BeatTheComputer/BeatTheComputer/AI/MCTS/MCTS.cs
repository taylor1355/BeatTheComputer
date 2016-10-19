using BeatTheComputer.Shared;

namespace BeatTheComputer.AI.MCTS
{
    class MCTS : IBehavior
    {
        private double EXPLORE_FACTOR = 1.41;

        private IBehavior rolloutBehavior;
        private int rolloutsPerNode;
        private double timeLimit;
        private int iterationLimit;

        private MCTSTree tree;

        public MCTS(IBehavior rolloutBehavior, int rolloutsPerNode, double timeLimit, int iterationLimit = int.MaxValue)
        {
            this.rolloutBehavior = rolloutBehavior;
            this.rolloutsPerNode = rolloutsPerNode;
            this.timeLimit = timeLimit;
            this.iterationLimit = iterationLimit;

            tree = null;
        }

        public IAction requestAction(IGameContext context, IAction opponentAction)
        {
            if (tree == null) {
                tree = new MCTSTree(context.clone(), rolloutBehavior.clone(), rolloutsPerNode, EXPLORE_FACTOR);
            }

            return tree.run(timeLimit, iterationLimit, context, opponentAction);
        }

        //does not save game tree
        public IBehavior clone()
        {
            return new MCTS(rolloutBehavior.clone(), rolloutsPerNode, timeLimit, iterationLimit);
        }
    }
}