using System;

namespace BeatTheComputer.Utils
{
    class BoardUtils
    {
        private BoardUtils() { }

        public static int rowCount<T>(T[,] board, T item, int row)
        {
            int count = 0;
            for (int col = 0; col < board.GetLength(1); col++) {
                if (board[row, col].Equals(item)) {
                    count++;
                }
            }
            return count;
        }

        public static int colCount<T>(T[,] board, T item, int col)
        {
            int count = 0;
            for (int row = 0; row < board.GetLength(0); row++) {
                if (board[row, col].Equals(item)) {
                    count++;
                }
            }
            return count;
        }

        public static int diagonalCount<T>(T[,] board, T item, int diagonal)
        {
            if (board.GetLength(0) != board.GetLength(1)) {
                throw new ArgumentException("Error: board must be square");
            }

            int[] cur;
            int dRow, dCol;
            if (diagonal == 0) {
                cur = new int[2] { 0, 0 };
                dRow = 1;
                dCol = 1;
            } else if (diagonal == 1) {
                cur = new int[2] { 0, board.GetLength(1) - 1 };
                dRow = 1;
                dCol = -1;
            } else {
                throw new ArgumentException("Error: " + diagonal + " is not a valid diagonal");
            }

            int count = 0;
            while (cur[0] >= 0 && cur[0] < board.GetLength(0)) {
                if (board[cur[0], cur[1]].Equals(item)) {
                    count++;
                }

                cur[0] += dRow;
                cur[1] += dCol;
            }

            return count;
        }
    }
}
