﻿using BeatTheComputer.Core;
using BeatTheComputer.Utils;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourScalableBoard : ConnectFourBoard
    {
        private Player[,] board;
        private int[] topRows;

        private Position lastChanged;

        public ConnectFourScalableBoard(int rows, int cols)
        {
            board = new Player[rows, cols];
            topRows = new int[cols];

            lastChanged = new Position(-1, -1);
        }

        private ConnectFourScalableBoard() { }

        public override int topRowOf(int col)
        {
            return topRows[col];
        }

        public override Player currentWinner()
        {
            // if nobody has played a move
            if (!lastChanged.inBounds(Rows, Cols)) return Player.NONE;

            bool winTriggered = identicalNeighborsOnLine(lastChanged, new Position(1, 0)) >= 4
                || identicalNeighborsOnLine(lastChanged, new Position(0, 1)) >= 4
                || identicalNeighborsOnLine(lastChanged, new Position(1, 1)) >= 4
                || identicalNeighborsOnLine(lastChanged, new Position(-1, 1)) >= 4;

            if (winTriggered) return this[lastChanged];
            else return Player.NONE;
        }

        // returns the number of pieces in a line passing through start belonging to the same player
        private int identicalNeighborsOnLine(Position start, Position slope)
        {
            int neighborCount = 1;
            for (int sign = -1; sign <= 1; sign += 2) {
                Position pos = start + slope * sign;
                while (pos.inBounds(Rows, Cols) && this[pos] == this[start]) {
                    neighborCount++;
                    pos += slope * sign;
                }
            }
            return neighborCount;
        }

        public override Player this[int row, int col] {
            get { return board[row, col]; }
        }

        public override Player this[Position pos] {
            get { return board[pos.Row, pos.Col]; }
        }

        public override void applyAction(ConnectFourAction action)
        {
            board[action.Position.Row, action.Position.Col] = action.Player;
            lastChanged = action.Position;
            topRows[action.Position.Col]++;
        }

        public override int Rows {
            get { return board.GetLength(0); }
        }

        public override int Cols {
            get { return board.GetLength(1); }
        }

        public override ConnectFourBoard clone()
        {
            ConnectFourScalableBoard clone = new ConnectFourScalableBoard();
            clone.board = (Player[,]) board.Clone();
            clone.topRows = (int[]) topRows.Clone();
            clone.lastChanged = lastChanged;
            return clone;
        }
    }
}
