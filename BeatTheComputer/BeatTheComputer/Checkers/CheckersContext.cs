using BeatTheComputer.Core;
using BeatTheComputer.Utils;

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

        public override double heuristicEval()
        {
            throw new NotImplementedException();

            /*const double PROMOTE_BONUS = 4.0;

            double MAX_EVAL = Math.Max(p1Pieces.Count, p2Pieces.Count) * 2.0 * PROMOTE_BONUS;
            double eval = 0.5;

            foreach (KeyValuePair<Position, Piece> entry in p1Pieces) {
                eval += (middleDist(entry.Key) * ((entry.Value.Promoted) ? 1 : PROMOTE_BONUS)) / (2.0 * MAX_EVAL);
            }

            foreach (KeyValuePair<Position, Piece> entry in p2Pieces) {
                eval += (middleDist(entry.Key) * ((entry.Value.Promoted) ? -1 : -PROMOTE_BONUS)) / (2.0 * MAX_EVAL);
            }

            return eval;*/
        }

        /*creates a weighting based on proximity to the edge of the board like the following:
                3  3  3  3  3
                3  2  2  2  3
                3  2  1  2  3
                3  2  2  2  3
                3  3  3  3  3
            */
        private double middleDist(Position pos)
        {
            throw new NotImplementedException();
            //double rowDist = Math.Abs(Math.Abs(pos.Row - ((Rows - 1)) / 2.0));
            //double colDist = Math.Abs(Math.Abs(pos.Col - ((Cols - 1)) / 2.0));
            //return 1.0 + Math.Ceiling(Math.Max(rowDist, colDist)) / Math.Ceiling(Math.Max(Rows / 2.0, Cols / 2.0));
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
