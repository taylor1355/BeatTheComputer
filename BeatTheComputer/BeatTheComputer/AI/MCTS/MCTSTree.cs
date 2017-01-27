using BeatTheComputer.Shared;

using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace BeatTheComputer.AI.MCTS
{
    class MCTSTree
    {
        private MCTSNode root;
        private IGameContext rootContext;

        public MCTSTree(IGameContext rootContext, IBehavior rolloutBehavior, double exploreFactor, bool tryToWin)
        {
            root = new MCTSNode(rootContext.clone(), rolloutBehavior.clone(), exploreFactor, tryToWin);
            this.rootContext = rootContext;
        }

        public Dictionary<IAction, double> run(int threads, double maxTime, int maxRollouts, IGameContext context, IAction myAction, IAction opponentAction, CancellationToken interrupt)
        {
            Stopwatch timer = Stopwatch.StartNew();

            if (root.IsTerminal) {
                return new Dictionary<IAction, double>();
            }

            if (context.Moves > rootContext.Moves) {
                if (myAction != null) {
                    generateRootChildrenIfLeaf();
                    advanceRoot(myAction);
                }

                generateRootChildrenIfLeaf();
                advanceRoot(opponentAction);

                if (!rootContext.Equals(context)) {
                    throw new InvalidOperationException("Game states passed to MCTS out of order");
                }
            }

            int iterations = 0;
            while (root.Visits < 1 || (timer.ElapsedMilliseconds < maxTime && root.Visits < maxRollouts && !interrupt.IsCancellationRequested)) {
                root.step(rootContext, threads, 0.01, true);
                iterations++;
            };

            return root.getActionScores();
        }

        private void advanceRoot(IAction action)
        {
            if (root.Children.ContainsKey(action)) {
                root = root.Children[action];
                rootContext.applyAction(action);
            } else {
                throw new ArgumentException("Action is not valid from the current game state");
            }
        }

        private void generateRootChildrenIfLeaf()
        {
            if (root.IsLeaf) {
                root.step(rootContext);
            }
        }
    }
}