using BeatTheComputer.Shared;

using System;
using System.Diagnostics;

namespace BeatTheComputer.AI.MCTS
{
    class MCTSTree
    {
        private MCTSNode root;

        public MCTSTree(IGameContext rootContext, IBehavior rolloutBehavior, int rolloutsPerNode, double exploreFactor)
        {
            root = new MCTSNode(rootContext.clone(), rolloutBehavior.clone(), rolloutsPerNode, exploreFactor);
        }

        public IAction run(double maxTime, int maxIterations, IGameContext context, IAction opponentAction)
        {
            IAction bestAction = null;
            if (!root.IsTerminal) {
                Stopwatch timer = Stopwatch.StartNew();

                if (context.getMoves() > root.Context.getMoves()) {
                    if (root.IsLeaf) {
                        root.step();
                    }
                    advanceRoot(opponentAction);

                    if (!root.Context.Equals(context)) {
                        throw new InvalidOperationException("Game states passed to MCTS out of order");
                    }
                }

                int iterations = 0;
                while (timer.ElapsedMilliseconds < maxTime && iterations < maxIterations) {
                    root.step();
                    iterations++;
                };

                bestAction = root.bestAction();

                advanceRoot(bestAction);
            }
            return bestAction;
        }

        private void advanceRoot(IAction action)
        {
            if (root.Children.ContainsKey(action)) {
                root = root.Children[action];
            } else {
                throw new ArgumentException("Action is not valid from the current game state");
            }
        }
    }
}