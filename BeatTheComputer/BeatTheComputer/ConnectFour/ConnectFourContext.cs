using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourContext : GameContext
    {
        private Player[,] board;
        private int[] topRows;

        public ConnectFourContext(int rows, int cols)
        {
            validateArguments(rows, cols);

            board = new Player[rows, cols];
            topRows = new int[cols];
            
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    board[row, col] = Player.NONE;
                }
            }
            for (int col = 0; col < cols; col++) {
                topRows[col] = 0;
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

        public override IList<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int col = 0; col < Cols; col++) {
                ConnectFourAction action = new ConnectFourAction(col, activePlayer, this);
                if (action.isValid(this)) {
                    validActions.Add(action);
                }
            }
            return validActions;
        }

        public int topRowOf(int col)
        {
            return topRows[col];
        }

        private Player currentWinner(Position changed)
        {
            bool winTriggered = identicalNeighborsOnLine(changed, new Position(1, 0)) >= 4
                || identicalNeighborsOnLine(changed, new Position(0, 1)) >= 4
                || identicalNeighborsOnLine(changed, new Position(1, 1)) >= 4
                || identicalNeighborsOnLine(changed, new Position(-1, 1)) >= 4;

            if (winTriggered) return playerAt(changed);
            else return Player.NONE;
        }

        private int identicalNeighborsOnLine(Position start, Position slope)
        {
            int neighborCount = 1;
            for(int sign = -1; sign <= 1; sign += 2) {
                Position pos = start + slope * sign;
                while (pos.inBounds(Rows, Cols) && playerAt(pos) == playerAt(start)) {
                    neighborCount++;
                    pos += slope * sign;
                }
            }
            return neighborCount;
        }

        public override void applyAction(IAction action)
        {
            if (!GameDecided) {
                if (!action.isValid(this)) {
                    throw new ArgumentException("Can't apply invalid action", "action");
                }

                ConnectFourAction c4Action = (ConnectFourAction) action;
                setPlayerAt(c4Action.Position, c4Action.Player);
                topRows[c4Action.Position.Col]++;

                activePlayer = activePlayer.Opponent;
                moves++;
                if (moves >= 7) {
                    winner = currentWinner(c4Action.Position);
                }
            }
        }

        public override bool GameDecided { get { return winner != Player.NONE || moves == board.Length; } }

        private void setPlayerAt(Position pos, Player player)
        {
            board[pos.Row, pos.Col] = player;
        }

        public Player playerAt(Position pos)
        {
            return board[pos.Row, pos.Col];
        }

        public override bool equalTo(object obj)
        {
            ConnectFourContext other = obj as ConnectFourContext;
            if (other == null || Rows != other.Rows || Cols != other.Cols || moves != other.moves) {
                return false;
            }

            for (int col = 0; col < Cols; col++) {
                if (topRowOf(col) != other.topRowOf(col)) {
                    return false;
                }

                for (int row = 0; row < topRowOf(col); row++) {
                    if (board[row, col] != other.board[row, col]) {
                        return false;
                    }
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
            ConnectFourContext clone = new ConnectFourContext(Rows, Cols);
            clone.board = (Player[,]) board.Clone();
            clone.topRows = (int[]) topRows.Clone();
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
    }
}
