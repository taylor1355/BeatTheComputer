using BeatTheComputer.Core;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeatTheComputer.AI.Minimax
{
    class Minimax : Behavior
    {
        private IHeuristic heuristic;
        private double timeLimit;
        private int iterationLimit;
        private bool tryToWin;

        private Random rand;

        private MinimaxTree tree;
        private IAction myLastAction;

        public Minimax(IHeuristic heuristic, double timeLimit, int iterationLimit, bool tryToWin)
        {
            this.heuristic = heuristic;
            this.timeLimit = timeLimit;
            this.iterationLimit = iterationLimit;
            this.tryToWin = tryToWin;

            rand = new Random();

            myLastAction = null;
        }

        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            if (tree == null) {
                tree = new MinimaxTree(heuristic.clone(), context.clone(), tryToWin);
            }

            Dictionary<IAction, double> actionScores = tree.run(timeLimit, iterationLimit, context.clone(), myLastAction, opponentAction, interrupt);

            IAction bestAction = null;
            double bestScore = Double.NaN;
            foreach (IAction action in actionScores.Keys) {
                double score = actionScores[action] * (1 + (rand.NextDouble() - 0.5) / 1e10);
                if (bestAction == null || score > bestScore) {
                    bestAction = action;
                    bestScore = score;
                }
            }

            myLastAction = bestAction;
            return bestAction;
        }

        public override string ToString()
        {
            return "Minimax";
        }

        //does not save game tree
        public override IBehavior clone()
        {
            return new Minimax(heuristic.clone(), timeLimit, iterationLimit, tryToWin);
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
