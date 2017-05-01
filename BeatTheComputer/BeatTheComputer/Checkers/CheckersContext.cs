using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    class CheckersContext : GameContext
    {
        private int pieceRows;
        private int moveLimit;

        private CheckersBoard board;

        public CheckersContext(int rows, int cols, int pieceRows, int moveLimit)
        {
            validateArguments(rows, cols, pieceRows, moveLimit);

            this.pieceRows = pieceRows;
            this.moveLimit = moveLimit;

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

        private void validateArguments(int rows, int cols, int pieceRows, int moveLimit)
        {
            int minRows = Math.Max(3, 2 * pieceRows + 1);
            if (rows < minRows) {
                throw new ArgumentException("Must have at least " + minRows.ToString() + " rows", "rows");
            }
            if (cols < 2) {
                throw new ArgumentException("Must have at least 2 columns", "cols");
            }
            if (pieceRows < 1) {
                throw new ArgumentException("Must have at least 1 row of pieces", "pieceRows");
            }
            if (moveLimit < 1) {
                throw new ArgumentException("Move limit must be at least 1", "moveLimit");
            }
        }

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

        public override bool GameDecided { get { return winner != Player.NONE || moves >= moveLimit; } }

        public override bool equalTo(object obj)
        {
            CheckersContext other = obj as CheckersContext;
            return other != null && moves == other.moves && moveLimit == other.moveLimit && board.equalTo(other.board);
        }

        public override string ToString()
        {
            return "Checkers";
        }

        public override IGameContext clone()
        {
            CheckersContext clone = new CheckersContext();
            clone.moveLimit = moveLimit;
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
            get { return pieceRows; }
        }

        public int MoveLimit {
            get { return moveLimit; }
        }
    }
}
