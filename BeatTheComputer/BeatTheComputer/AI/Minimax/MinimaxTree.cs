﻿using BeatTheComputer.Core;

using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace BeatTheComputer.AI.Minimax
{
    class MinimaxTree
    {
        private MinimaxNode root;
        private IGameContext rootContext;

        public MinimaxTree(IGameContext rootContext, bool tryToWin)
        {
            root = new MinimaxNode(rootContext.clone(), tryToWin);
            this.rootContext = rootContext;
        }

        public Dictionary<IAction, double> run(double maxTime, int maxIterations, IGameContext context, IAction myAction, IAction opponentAction, CancellationToken interrupt)
        {
            Stopwatch timer = Stopwatch.StartNew();

            if (root.IsTerminal) {
                return new Dictionary<IAction, double>();
            }

            if (context.Moves > rootContext.Moves) {
                if (myAction != null) {
                    if (root.IsLeaf) {
                        root.generateChildren(rootContext);
                    }
                    advanceRoot(myAction);
                }

                if (root.IsLeaf) {
                    root.generateChildren(rootContext);
                }
                advanceRoot(opponentAction);

                if (!rootContext.Equals(context)) {
                    throw new InvalidOperationException("Game states passed to Minimax out of order");
                }
            }

            int iterations = 0;
            while (iterations < 1 || (timer.ElapsedMilliseconds < maxTime && iterations < maxIterations && !interrupt.IsCancellationRequested)) {
                root.updateScore(rootContext, Double.NegativeInfinity, Double.PositiveInfinity, iterations + 1);
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
    }
}
