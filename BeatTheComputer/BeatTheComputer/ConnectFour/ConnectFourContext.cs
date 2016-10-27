using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourContext : GameContext
    {
        private Player[,] board;

        public ConnectFourContext(int rows, int cols)
        {
            validateArguments(rows, cols);

            board = new Player[rows, cols];
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int col = 0; col < board.GetLength(1); col++) {
                    board[row, col] = Player.NONE;
                }
            }
            activePlayer = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        private void validateArguments(int rows, int cols)
        {
            if (rows < 1) {
                throw new ArgumentException("Must have at least 1 row", "rows");
            }
            if (cols < 1) {
                throw new ArgumentException("Must have at least 1 column", "cols");
            }
        }

        public override ICollection<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int col = 0; col < board.GetLength(1); col++) {
                ConnectFourAction action = new ConnectFourAction(col, activePlayer, this);
                if (action.isValid(this)) {
                    validActions.Add(action);
                }
            }
            return validActions;
        }

        private Player currentWinner(int changedRow, int changedCol)
        {
            bool winTriggered = identicalNeighborsOnLine(changedRow, changedCol, 1, 0) >= 4
                || identicalNeighborsOnLine(changedRow, changedCol, 0, 1) >= 4
                || identicalNeighborsOnLine(changedRow, changedCol, 1, 1) >= 4
                || identicalNeighborsOnLine(changedRow, changedCol, -1, 1) >= 4;

            if (winTriggered) return board[changedRow, changedCol];
            else return Player.NONE;
        }

        private int identicalNeighborsOnLine(int startRow, int startCol, int rowSlope, int colSlope)
        {
            int neighborCount = 1;
            int[] signs = { -1, 1 };
            foreach(int sign in signs) {
                int row = startRow + rowSlope * sign;
                int col = startCol + colSlope * sign;
                while (BoardUtils.inBounds(board, row, col) && board[row, col] == board[startRow, startCol]) {
                    neighborCount++;
                    row += rowSlope * sign;
                    col += colSlope * sign;
                }
            }
            return neighborCount;
        }

        public override void applyAction(IAction action)
        {
            if (!gameDecided()) {
                if (!action.isValid(this)) {
                    throw new ArgumentException("Can't apply invalid action", "action");
                }

                ConnectFourAction c4Action = (ConnectFourAction) action;
                board[c4Action.Row, c4Action.Col] = c4Action.Player;
                activePlayer = 1 - activePlayer;
                moves++;
                if (moves >= 7) {
                    winner = currentWinner(c4Action.Row, c4Action.Col);
                }
            }
        }

        public override bool gameDecided() { return winner != Player.NONE || moves == board.Length; }

        public override bool equalTo(object obj)
        {
            ConnectFourContext other = obj as ConnectFourContext;
            if (other == null) {
                return false;
            }

            for (int row = 0; row < board.GetLength(0); row++) {
                for (int col = 0; col < board.GetLength(1); col++) {
                    if (board[row, col] != other.board[row, col]) return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            return "Connect Four";
        }

        public override IGameContext clone()
        {
            ConnectFourContext clone = new ConnectFourContext(board.GetLength(0), board.GetLength(1));
            clone.board = (Player[,]) board.Clone();
            clone.activePlayer = activePlayer;
            clone.winner = winner;
            clone.moves = moves;
            return clone;
        }

        public Player[,] Board {
            get { return board; }
        }
    }
}
