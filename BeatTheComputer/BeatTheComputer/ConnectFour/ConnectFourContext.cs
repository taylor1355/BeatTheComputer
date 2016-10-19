using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourContext : GameContext
    {
        private PlayerID[,] board;
        private int moves;

        public ConnectFourContext(int rows, int cols)
        {
            board = new PlayerID[rows, cols];
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int col = 0; col < board.GetLength(1); col++) {
                    board[row, col] = PlayerID.NONE;
                }
            }
            turn = PlayerID.ONE;
            winner = PlayerID.NONE;
            moves = 0;
        }

        public override List<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int col = 0; col < board.GetLength(1); col++) {
                ConnectFourAction action = new ConnectFourAction(col, turn, this);
                if (action.isValid(this)) {
                    validActions.Add(action);
                }
            }
            return validActions;
        }

        private PlayerID currentWinner(int changedRow, int changedCol)
        {
            bool winTriggered = identicalNeighborsOnLine(changedRow, changedCol, 1, 0) >= 4
                || identicalNeighborsOnLine(changedRow, changedCol, 0, 1) >= 4
                || identicalNeighborsOnLine(changedRow, changedCol, 1, 1) >= 4
                || identicalNeighborsOnLine(changedRow, changedCol, -1, 1) >= 4;

            if (winTriggered) return board[changedRow, changedCol];
            else return PlayerID.NONE;
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
                ConnectFourAction c4Action = (ConnectFourAction) action;
                if (!c4Action.isValid(this)) throw new ArgumentException("Can't apply invalid action", "action");

                board[c4Action.Row, c4Action.Col] = c4Action.PlayerID;
                turn = 1 - turn;
                moves++;
                if (moves >= 7) {
                    winner = currentWinner(c4Action.Row, c4Action.Col);
                }
            }
        }

        public override bool gameDecided() { return winner != PlayerID.NONE || moves >= board.Length; }

        public override int getMoves() { return moves; }

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

        public override IGameContext clone()
        {
            ConnectFourContext clone = new ConnectFourContext(board.GetLength(0), board.GetLength(1));
            clone.board = (PlayerID[,]) board.Clone();
            clone.turn = turn;
            clone.winner = winner;
            clone.moves = moves;
            return clone;
        }

        public PlayerID[,] Board {
            get { return board; }
        }
    }
}
