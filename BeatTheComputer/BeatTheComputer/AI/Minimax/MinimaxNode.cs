﻿using BeatTheComputer.Core;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.AI.Minimax
{
    class MinimaxNode
    {
        private bool tryToWin;
        private int move;
        private Player activePlayer;
        private double p1Score;
        private GameOutcome outcome;
        private Dictionary<IAction, MinimaxNode> children;

        public MinimaxNode(IGameContext context, bool tryToWin)
        {
            move = context.Moves;
            this.tryToWin = tryToWin;

            activePlayer = context.ActivePlayer;
            outcome = context.GameOutcome;
            if (outcome == GameOutcome.WIN) {
                p1Score = Double.PositiveInfinity;
            } else if (outcome == GameOutcome.LOSS) {
                p1Score = Double.NegativeInfinity;
            } else if (outcome == GameOutcome.TIE) {
                p1Score = 0.5;
            } else {
                p1Score = 0;
            }
            children = null;
        }

        public double updateScore(IGameContext context, double alpha, double beta, int depthLimit)
        {
            if (!IsTerminal) {
                if (depthLimit > 0) {
                    if (IsLeaf) {
                        generateChildren(context);
                    }

                    p1Score = Double.NaN;
                    foreach (KeyValuePair<IAction, MinimaxNode> entry in children) {
                        IList<IAction> validActions = context.getValidActions();
                        double childScore = entry.Value.updateScore(context.clone().applyAction(entry.Key), alpha, beta, depthLimit - 1);
                        if (Double.IsNaN(p1Score) || childScore > 1 - Score) {
                            p1Score = entry.Value.p1Score;

                            if (IsMaximizer) {
                                alpha = Math.Max(alpha, p1Score);
                            } else {
                                beta = Math.Min(beta, p1Score);
                            }

                            if (alpha >= beta) {
                                return Score;
                            }
                        }
                    }
                } else {
                    p1Score = context.heuristicEval();
                }
            }
            return Score;
        }

        public void generateChildren(IGameContext context)
        {
            if (IsLeaf && !IsTerminal) {
                children = new Dictionary<IAction, MinimaxNode>();
                ICollection<IAction> validActions = context.getValidActions();
                foreach (IAction action in validActions) {
                    IGameContext successor = context.clone();
                    successor.applyAction(action);
                    children.Add(action, new MinimaxNode(successor, tryToWin));
                }
            }
        }

        public Dictionary<IAction, double> getActionScores()
        {
            if (children == null) {
                throw new InvalidOperationException("Node has no children to compare");
            }

            Dictionary<IAction, double> actionScores = new Dictionary<IAction, double>();
            foreach (IAction action in children.Keys) {
                actionScores.Add(action, children[action].Score);
            }
            return actionScores;
        }

        public double Score {
            get {
                if (IsMaximizer) return 1 - p1Score;
                else return p1Score;
            }
        }

        public Dictionary<IAction, MinimaxNode> Children {
            get { return children; }
        }

        public bool IsMaximizer {
            get { return tryToWin == (activePlayer == Player.ONE); }
        }

        public bool IsLeaf {
            get { return children == null; }
        }

        public bool IsTerminal {
            get { return outcome != GameOutcome.UNDECIDED; }
        }
    }
}
