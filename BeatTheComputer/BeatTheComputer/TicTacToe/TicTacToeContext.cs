using BeatTheComputer.Core;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.TicTacToe
{
    class TicTacToeContext : GameContext
    {
        private Player[,] board;
        private int inARow;

        public TicTacToeContext(int rows, int cols, int inARow)
        {
            validateArguments(rows, cols, inARow);

            board = new Player[rows, cols];
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    board[row, col] = Player.NONE;
                }
            }
            this.inARow = inARow;
            activePlayer = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        private void validateArguments(int rows, int cols, int inARow)
        {
            if (rows < 1) {
                throw new ArgumentException("Must have at least 1 row", "rows");
            }
            if (cols < 1) {
                throw new ArgumentException("Must have at least 1 column", "cols");
            }
            if (inARow < 1) {
                throw new ArgumentException("Must need at least 1 in a row to win", "inARow");
            }
            if (inARow > Math.Max(rows, cols)) {
                throw new ArgumentException("X in a row to win cannot exceed both rows and columns", "inARow");
            }
        }

        public override IList<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Cols; col++) {
                    Position pos = new Position(row, col);
                    if (playerAt(pos) == Player.NONE) {
                        validActions.Add(new TicTacToeAction(pos, activePlayer));
                    }
                }
            }
            return validActions;
        }

        public override IGameContext applyAction(IAction action)
        {
            if (!GameDecided) {
                if (!action.isValid(this)) {
                    throw new ArgumentException("Can't apply invalid action", "action");
                }

                TicTacToeAction tttAction = (TicTacToeAction) action;
                setPlayer(tttAction.Position, tttAction.Player);
                activePlayer = activePlayer.Opponent;
                moves++;
                if (moves >= 2 * inARow - 1) {
                    winner = getWinner(tttAction.Position);
                }
            }

            return this;
        }

        private Player getWinner(Position changed)
        {
            foreach (Position dir in Position.POSITIVE_DIRECTIONS) {
                int count = 1;

                Position curr = changed + dir;
                while (curr.inBounds(Rows, Cols) && playerAt(curr) == playerAt(changed)) {
                    count++;
                    curr += dir;
                }

                curr = changed - dir;
                while (curr.inBounds(Rows, Cols) && playerAt(curr) == playerAt(changed)) {
                    count++;
                    curr -= dir;
                }

                if (count >= inARow) {
                    return playerAt(changed);
                }
            }
            return Player.NONE;
        }

        public override double heuristicEval()
        {
            /*const double WEIGHT_SUM = 12;
            double eval = 0.5;
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Cols; col++) {
                    if (board[row, col] == Player.ONE) {
                        eval += squareWeight(row, col) / WEIGHT_SUM;
                    } else if (board[row, col] == Player.TWO) {
                        eval -= squareWeight(row, col) / WEIGHT_SUM;
                    }
                }
            }
            return eval;*/
            throw new NotImplementedException();
        }

        /*public double squareWeight(int row, int col)
        {
            double weight = 0;
            if (row + col == 2) weight++;
            if (row == col) weight++;
            return weight;
        }*/

        public override bool GameDecided { get { return winner != Player.NONE || moves >= board.Length; } }

        private void setPlayer(Position pos, Player player)
        {
            board[pos.Row, pos.Col] = player;
        }

        public Player playerAt(Position pos)
        {
            return board[pos.Row, pos.Col];
        }

        public override bool equalTo(object obj)
        {
            TicTacToeContext other = obj as TicTacToeContext;
            if (other == null) {
                return false;
            }

            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Cols; col++) {
                    if (board[row, col] != other.board[row, col]) return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return "Tic Tac Toe";
        }

        public override IGameContext clone()
        {
            TicTacToeContext clone = new TicTacToeContext(Rows, Cols, inARow);
            clone.board = (Player[,]) board.Clone();
            clone.activePlayer = activePlayer;
            clone.winner = winner;
            clone.moves = moves;
            return clone;
        }

        public int Rows {
            get { return board.GetLength(0); }
        }

        public int Cols {
            get { return board.GetLength(1); }
        }

        public int InARow {
            get { return inARow; }
        }
    }
}
