using BeatTheComputer.Core;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    class CheckersContext : GameContext
    {
        private CheckersBoard board;
        private CheckersSettings settings;

        public CheckersContext(int rows, int cols, int pieceRows, int moveLimit)
        {
            settings = new CheckersSettings(rows, cols, pieceRows, moveLimit);

            if (CheckersBitboard.fits(rows, cols)) {
                board = new CheckersBitboard(rows, cols, pieceRows);
            } else {
                board = new CheckersScalableBoard(rows, cols, pieceRows);
            }

            activePlayer = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        public CheckersContext(CheckersSettings settings) : this(settings.Rows, settings.Cols, settings.PieceRows, settings.MoveLimit) { }

        private CheckersContext() { }

        public override IList<IAction> getValidActions()
        {
            return board.getValidActions(activePlayer);
        }

        public override IGameContext applyAction(IAction action)
        {
            if (!GameDecided) {
                if (!action.isValid(this)) {
                    throw new ArgumentException("Can't apply invalid action", "action");
                }

                CheckersAction cAction = (CheckersAction) action;
                board.applyAction(cAction);

                activePlayer = activePlayer.Opponent;
                moves++;

                winner = board.currentWinner(activePlayer);
            }

            return this;
        }

        public override double[] featurize()
        {
            double[] features = new double[board.Rows * board.Cols];
            for (int row = 0; row < board.Rows; row++) {
                for (int col = 0; col < board.Cols; col++) {
                    double value = 0;
                    if (board[row, col].Player != Player.NONE) {
                        value = 0.5;
                    }
                    if (board[row, col].Promoted) {
                        value *= 2;
                    }
                    if (board[row, col].Player == Player.TWO) {
                        value *= -1;
                    }
                    features[row * board.Cols + col] = value;
                }
            }
            return features;
        }

        public override bool GameDecided { get { return winner != Player.NONE || moves >= settings.MoveLimit; } }

        public override bool equalTo(object obj)
        {
            CheckersContext other = obj as CheckersContext;
            return other != null && moves == other.moves && settings.MoveLimit == other.MoveLimit && board.equalTo(other.board);
        }

        public override string ToString()
        {
            return "Checkers";
        }

        public override IGameContext clone()
        {
            CheckersContext clone = new CheckersContext();
            clone.settings = settings;
            clone.board = board.clone();

            clone.activePlayer = activePlayer;
            clone.winner = winner;
            clone.moves = moves;

            return clone;
        }

        public CheckersBoard Board {
            get { return board; }
        }

        public int PieceRows {
            get { return settings.PieceRows; }
        }

        public int MoveLimit {
            get { return settings.MoveLimit; }
        }
    }
}
