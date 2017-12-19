using BeatTheComputer.Core;

using System;

namespace BeatTheComputer.TicTacToe
{
    class TicTacToeSettings : GameSettings
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public int InARow { get; private set; }

        public TicTacToeSettings(int rows, int cols, int inARow) : base(typeof(TicTacToeContext))
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

            Rows = rows;
            Cols = cols;
            InARow = inARow;
        }

        public override bool equalTo(object obj)
        {
            TicTacToeSettings other = obj as TicTacToeSettings;
            if (other == null) {
                return false;
            }

            return Rows == other.Rows && Cols == other.Cols && InARow == other.InARow;
        }
    }
}
