using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System.Collections.Generic;

namespace BeatTheComputer.TicTacToeGame
{
    class TicTacToeContext : GameContext
    {
        private Player[,] board;
        private int moves;

        public TicTacToeContext()
        {
            board = new Player[3, 3];
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int col = 0; col < board.GetLength(1); col++) {
                    board[row, col] = Player.NONE;
                }
            }
            turn = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        public override List<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int col = 0; col < board.GetLength(1); col++) {
                    if (board[row, col] == Player.NONE) {
                        validActions.Add(new TicTacToeAction(row, col, turn));
                    }
                }
            }
            return validActions;
        }

        public override void applyAction(IAction action)
        {
            if (!gameDecided()) {
                TicTacToeAction tttAction = (TicTacToeAction) action;
                board[tttAction.Row, tttAction.Col] = tttAction.PlayerID;
                turn = 1 - turn;
                moves++;
                if (moves >= 5) {
                    winner = getWinner();
                }
            }
        }

        private Player getWinner()
        {
            for (int i = 0; i < board.GetLength(0); i++) {
                if (BoardUtils.rowCount(board, Player.ONE, i) == board.GetLength(0)) {
                    return Player.ONE;
                } else if (BoardUtils.rowCount(board, Player.TWO, i) == board.GetLength(0)) {
                    return Player.TWO;
                }

                if (BoardUtils.colCount(board, Player.ONE, i) == board.GetLength(0)) {
                    return Player.ONE;
                } else if (BoardUtils.colCount(board, Player.TWO, i) == board.GetLength(0)) {
                    return Player.TWO;
                }

                if (i <= 1) {
                    if (BoardUtils.diagonalCount(board, Player.ONE, i) == board.GetLength(0)) {
                        return Player.ONE;
                    } else if (BoardUtils.diagonalCount(board, Player.TWO, i) == board.GetLength(0)) {
                        return Player.TWO;
                    }
                }
            }
            return Player.NONE;
        }

        public override bool gameDecided() { return winner != Player.NONE || moves >= board.Length; }

        public override int getMoves() { return moves; }

        public override bool equalTo(object obj)
        {
            TicTacToeContext other = obj as TicTacToeContext;
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
            TicTacToeContext clone = new TicTacToeContext();
            clone.board = (Player[,]) board.Clone();
            clone.turn = turn;
            clone.winner = winner;
            clone.moves = moves;
            return clone;
        }

        public Player[,] Board {
            get { return board; }
        }
    }
}
