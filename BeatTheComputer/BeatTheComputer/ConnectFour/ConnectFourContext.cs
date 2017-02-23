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

        public bool actionIsValid(IAction action)
        {
            ConnectFourAction c4Action = (ConnectFourAction) action;
            return c4Action.Position.inBounds(Rows, Cols)
                && playerAt(c4Action.Position) == Player.NONE
                && c4Action.Player == activePlayer;
        }

        public override IList<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int col = 0; col < Cols; col++) {
                ConnectFourAction action = new ConnectFourAction(col, activePlayer, this);
                if (actionIsValid(action)) {
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

        public override IGameContext applyAction(IAction action)
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

            return this;
        }

        public override double heuristicEval() //not working
        {
            double activeSign = (activePlayer == Player.ONE) ? 1 : -1;
            double eval = 0.5;
            double maxEval = 0.5 * (Rows * Cols - (double) moves / Rows * Cols);
            for (int col = 0; col < topRows.Length; col++) {
                for (int row = topRows[col]; row < Rows; row++) {
                    Position pos = new Position(row, col);
                    double threat = threatOwnerAt(pos);
                    double heightRatio = (double) (row - topRows[col]) / (Rows - topRows[col]);
                    eval += (threat * (1.0 - heightRatio)) / (2 * maxEval);
                }
            }
            return eval;
        }

        private double threatOwnerAt(Position pos)
        {
            int ownerCount = 0;
            foreach (Position dir in Position.POSITIVE_DIRECTIONS) {
                int groupSize = 0;
                for (int sign = -1; sign <= 1; sign += 2) {
                    Position signedDir = dir * sign;
                    Position curr = pos + 2 * signedDir;
                    Player owner = Player.NONE;
                    int ownerSign = 0;

                    if (curr.inBounds(Rows, Cols) && playerAt(curr - signedDir) == playerAt(curr) && playerAt(curr) != Player.NONE) {
                        owner = playerAt(curr);
                        ownerSign = (owner == Player.ONE) ? 1 : -1;
                        groupSize = 2 * ownerSign;
                    }

                    do {
                        groupSize += ownerSign;
                        if (Math.Abs(groupSize) >= 4) break;
                        curr += signedDir;
                    } while (curr.inBounds(Rows, Cols) && playerAt(curr) == owner);

                    if (Math.Abs(groupSize) >= 4) {
                        ownerCount += Math.Sign(groupSize);
                        groupSize = 0;
                    }
                }
            }

            return Math.Sign(ownerCount);
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
